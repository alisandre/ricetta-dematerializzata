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
        /// I parametri vengono inseriti come elementi figli dell'operazione:
        ///   &lt;ns:InvioPrescritto xmlns:ns="http://..."&gt;
        ///     &lt;nre&gt;12345&lt;/nre&gt;
        ///   &lt;/ns:InvioPrescritto&gt;
        ///
        /// NOTA: Il mapping chiave→nome-elemento SOAP dipende dal WSDL specifico.
        ///       Qui viene usato il nome chiave in lowercase come nome di elemento.
        ///       Personalizzare per servizi che richiedono nomi diversi.
        /// </summary>
        public static string BuildSoapEnvelope(
            string operazione,
            string namespaceSoap,
            Dictionary<string, string> parametri)
        {
            var sb = new StringBuilder();
            sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            sb.Append("<soapenv:Envelope");
            sb.Append(" xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\"");
            sb.AppendFormat(" xmlns:tns=\"{0}\">", namespaceSoap);
            sb.Append("<soapenv:Header/>");
            sb.Append("<soapenv:Body>");
            sb.AppendFormat("<tns:{0}>", operazione);

            foreach (var kv in parametri)
            {
                var elName = kv.Key;
                sb.AppendFormat("<{0}>{1}</{0}>", elName, SecurityElement.Escape(kv.Value));
            }

            sb.AppendFormat("</tns:{0}>", operazione);
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
