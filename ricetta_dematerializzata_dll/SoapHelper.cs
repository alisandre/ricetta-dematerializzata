using System;
using System.Collections.Generic;
using System.Security;
using System.Text;
using System.Xml;

namespace ricetta_dematerializzata_dll.Core
{
    /// <summary>
    /// Costruisce messaggi SOAP 1.1 e analizza le risposte XML.
    /// </summary>
    internal static class SoapHelper
    {
        private const string NsSoapEnv = "http://schemas.xmlsoap.org/soap/envelope/";

        // ── Build Request ─────────────────────────────────────────────────────────

        /// <summary>
        /// Crea il body SOAP come stringa XML.
        /// Supporta parametri semplici e strutture annidate (es. identificativo con tipo/valore).
        /// 
        /// Per elementi semplici: chiave → valore
        /// Per elementi annidati: chiave_sottoelemento → valore
        /// 
        /// Esempio:
        ///   "userId" → "PROVAX00X00X000Y"
        ///   "identificativo_tipo" → "P"
        ///   "identificativo_valore" → "LsQiYtf7..."
        ///   
        /// Risultato XML:
        ///   &lt;tns:CreateAuthReq&gt;
        ///     &lt;userId&gt;PROVAX00X00X000Y&lt;/userId&gt;
        ///     &lt;identificativo&gt;
        ///       &lt;tipo&gt;P&lt;/tipo&gt;
        ///       &lt;valore&gt;LsQiYtf7...&lt;/valore&gt;
        ///     &lt;/identificativo&gt;
        ///   &lt;/tns:CreateAuthReq&gt;
        /// </summary>
        public static string BuildSoapEnvelope(
            string operazione,
            string namespaceSoap,
            Dictionary<string, string> parametri)
        {
            var sb = new StringBuilder();
            // IMPORTANTE: Rimuovere la dichiarazione XML <?xml version="1.0" encoding="UTF-8"?>
            // Il servizio non la vuole e ritorna errori se presente
            sb.Append("<soapenv:Envelope");
            sb.Append(" xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\"");
            
            // Determina il prefisso da usare in base al namespace
            // Per A2F usiamo "aut:", per gli altri usiamo "tns:"
            string prefissoNs;
            string xmlnsNs;
            bool isA2f = namespaceSoap.Contains("a2f") || namespaceSoap.Contains("auth");
            
            if (isA2f)
            {
                prefissoNs = "aut";
                xmlnsNs = "xmlns:aut";
            }
            else
            {
                prefissoNs = "tns";
                xmlnsNs = "xmlns:tns";
            }
            
            sb.AppendFormat(" {0}=\"{1}\"", xmlnsNs, namespaceSoap);
            
            // Aggiungi namespace per gli elementi annidati (dat:)
            if (isA2f)
            {
                // Per A2F il namespace dat è specifico
                sb.Append(" xmlns:dat=\"http://datatype.xsd.wsdl.auth.a2f.sts.sanita.finanze.it\"");
            }
            else
            {
                // Per i servizi normali il namespace dat è diverso
                sb.Append(" xmlns:dat=\"http://datischema.wsdl.dem.sanita.finanze.it\"");
            }
            
            sb.Append(">");
            
            sb.Append("<soapenv:Header/>");
            sb.Append("<soapenv:Body>");
            sb.AppendFormat("<{0}:{1}>", prefissoNs, operazione);

            // Raggruppa i parametri per elemento
            var elementiAnnidati = new Dictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase);
            var elementiSemplici = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            foreach (var kv in parametri)
            {
                var chiave = kv.Key;
                var valore = kv.Value;

                // Verifica se è un elemento annidato (formato: parent_child)
                if (chiave.Contains("_"))
                {
                    var parti = chiave.Split(new[] { '_' }, 2);
                    var parent = parti[0];
                    var child = parti[1];

                    if (!elementiAnnidati.TryGetValue(parent, out var dict))
                    {
                        dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                        elementiAnnidati[parent] = dict;
                    }

                    dict[child] = valore;
                }
                else
                {
                    elementiSemplici[chiave] = valore;
                }
            }

            // IMPORTANTE: userId e identificativo DEVONO stare per primi
            // Se presenti, scrivili subito all'inizio
            if (elementiSemplici.TryGetValue("userId", out var userIdVal) && !string.IsNullOrEmpty(userIdVal))
            {
                sb.AppendFormat("<{0}:userId>{1}</{0}:userId>", prefissoNs, SecurityElement.Escape(userIdVal));
                elementiSemplici.Remove("userId");
            }

            if (elementiAnnidati.TryGetValue("identificativo", out var identDict))
            {
                sb.AppendFormat("<{0}:identificativo>", prefissoNs);
                foreach (var kvChild in identDict)
                {
                    var childName = kvChild.Key;
                    sb.AppendFormat("<dat:{0}>{1}</dat:{0}>", childName, SecurityElement.Escape(kvChild.Value ?? string.Empty));
                }
                sb.AppendFormat("</{0}:identificativo>", prefissoNs);
                elementiAnnidati.Remove("identificativo");
            }

            // Scrivi elementi semplici rimanenti
            if (isA2f)
            {
                string[] ordineA2f = operazione switch
                {
                    "CreateAuthReq" => new[] { "cfUtente", "codRegione", "codAslAo", "codSsa", "codiceStruttura", "contesto", "applicazione" },
                    "RevokeAuthReq" => new[] { "cfUtente", "token", "contesto", "applicazione" },
                    "CheckTokenReq" => new[] { "cfUtente", "token", "contesto", "applicazione" },
                    _ => Array.Empty<string>()
                };

                foreach (var key in ordineA2f)
                {
                    if (elementiSemplici.TryGetValue(key, out var val))
                    {
                        sb.AppendFormat("<{0}:{1}>{2}</{0}:{1}>", prefissoNs, key, SecurityElement.Escape(val ?? string.Empty));
                        elementiSemplici.Remove(key);
                    }
                }
            }

            foreach (var kv in elementiSemplici)
            {
                var elName = kv.Key;
                sb.AppendFormat("<{0}:{1}>{2}</{0}:{1}>", prefissoNs, elName, SecurityElement.Escape(kv.Value ?? string.Empty));
            }

            // Scrivi elementi annidati rimanenti
            foreach (var kvParent in elementiAnnidati)
            {
                var parentName = kvParent.Key;
                var children = kvParent.Value;

                sb.AppendFormat("<{0}:{1}>", prefissoNs, parentName);
                foreach (var kvChild in children)
                {
                    var childName = kvChild.Key;
                    sb.AppendFormat("<dat:{0}>{1}</dat:{0}>", childName, SecurityElement.Escape(kvChild.Value ?? string.Empty));
                }
                sb.AppendFormat("</{0}:{1}>", prefissoNs, parentName);
            }

            sb.AppendFormat("</{0}:{1}>", prefissoNs, operazione);
            sb.Append("</soapenv:Body>");
            sb.Append("</soapenv:Envelope>");
            return sb.ToString();
        }

        // ── Parse Response ────────────────────────────────────────────────────────

        /// <summary>
        /// Estrae tutti i nodi foglia dal Body della risposta SOAP come dizionario.
        /// La chiave è il LocalName dell'elemento (uppercase), il valore è il testo.
        /// I nodi SOAP Fault vengono mappati a ERRORE_NUMERO / ERRORE_DESCRIZIONE.
        /// </summary>
        public static Dictionary<string, string> ParseSoapResponse(string xml)
        {
            var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            var doc = new XmlDocument();
            doc.LoadXml(xml);

            var nsMgr = new XmlNamespaceManager(doc.NameTable);
            nsMgr.AddNamespace("soap", NsSoapEnv);

            // Verifica SOAP Fault
            var fault = doc.SelectSingleNode("//soap:Fault", nsMgr)
                       ?? doc.SelectSingleNode("//Fault");
            if (fault != null)
            {
                result["ERRORE_NUMERO"]      = GetInnerText(fault, "faultcode") ?? "9999";
                result["ERRORE_DESCRIZIONE"] = GetInnerText(fault, "faultstring") ?? "Errore SOAP sconosciuto";
                return result;
            }

            // Naviga fino al Body
            var body = doc.SelectSingleNode("//soap:Body", nsMgr)
                    ?? doc.SelectSingleNode("//*[local-name()='Body']");

            if (body == null)
            {
                result["ERRORE_NUMERO"]      = "9999";
                result["ERRORE_DESCRIZIONE"] = "Body SOAP non trovato nella risposta";
                return result;
            }

            // Estrae ricorsivamente tutti i nodi foglia
            EstraiNodiFoglia(body, result);
            return result;
        }

        private static void EstraiNodiFoglia(XmlNode nodo, Dictionary<string, string> target)
        {
            bool haFigliElemento = false;
            foreach (XmlNode figlio in nodo.ChildNodes)
            {
                if (figlio.NodeType == XmlNodeType.Element)
                {
                    haFigliElemento = true;
                    EstraiNodiFoglia(figlio, target);
                }
            }

            if (!haFigliElemento && nodo.NodeType == XmlNodeType.Element)
            {
                var chiave = nodo.LocalName.ToUpperInvariant();
                // Evita duplicati: in presenza di array aggiungo indice
                if (target.ContainsKey(chiave))
                {
                    int i = 1;
                    while (target.ContainsKey($"{chiave}_{i}")) i++;
                    chiave = $"{chiave}_{i}";
                }
                target[chiave] = nodo.InnerText;
            }
        }

        private static string? GetInnerText(XmlNode parent, string localName)
        {
            foreach (XmlNode child in parent.ChildNodes)
                if (string.Equals(child.LocalName, localName, StringComparison.OrdinalIgnoreCase))
                    return child.InnerText;
            return null;
        }
    }
}
