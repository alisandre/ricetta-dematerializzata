using System;
using System.Collections.Generic;
using System.Security;
using System.Text;
using System.Xml;

namespace ricetta_dematerializzata.Core
{
    /// <summary>
    /// Costruisce messaggi SOAP 1.1 e analizza le risposte XML.
    /// </summary>
    internal static class SoapHelper
    {
        private const string NsSoapEnv = "http://schemas.xmlsoap.org/soap/envelope/";

        /// <summary>
        /// Ordine canonico dei parametri semplici per ciascuna operazione WSDL.
        /// I campi non presenti nell'ordine vengono emessi dopo quelli elencati.
        /// </summary>
        private static readonly Dictionary<string, string[]> OrdineParametri =
            new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase)
        {
            // ── PRESCRITTORE ─────────────────────────────────────────────────────
            ["VisualizzaPrescrittoRichiesta"] = new[]
            {
                "pinCode", "nre", "cfMedico"
            },
            ["InvioPrescrittoRichiesta"] = new[]
            {
                "pinCode", "cfMedico1", "cfMedico2",
                "codRegione", "codASLAo", "codStruttura",
                "codSpecializzazione",
                "testata1", "testata2",
                "nre", "tipoRic",
                "codiceAss",
                "cognNome", "indirizzo", "oscuramDati", "numTessSasn", "socNavigaz",
                "tipoPrescrizione",
                "ricettaInterna", "codEsenzione", "nonEsente", "reddito",
                "codDiagnosi", "descrizioneDiagnosi",
                "dataCompilazione", "tipoVisita",
                "dispReg", "provAssistito", "aslAssistito",
                "indicazionePrescr", "altro", "classePriorita",
                "statoEstero", "istituzCompetente", "numIdentPers", "numIdentTess",
                "dataNascitaEstero", "dataScadTessera"
            },
            ["AnnullaPrescritto"] = new[]
            {
                "pinCode", "nre", "cfMedico"
            },
            ["InterrogaNreUtilizzati"] = new[]
            {
                "pinCode", "codRegione", "nre", "codLotto", "cfMedico", "cfAssistito",
                "tipoPrescr", "dataCompilazioneRicettaDal", "dataCompilazioneRicettaAl"
            },
            ["ServiceAnag"] = new[]
            {
                "pinCode", "tipoOperazione",
                "codiceRegione", "nomeSWH", "mailSWH",
                "opzione1", "opzione2", "opzione3", "opzione4", "opzione5"
            },
            ["InvioDichiarazioneSostituzioneMedico"] = new[]
            {
                "pinCode", "pwd", "cfMedicoTitolare",
                "codRegione", "codASLAo", "codStruttura",
                "codSpecializzazione", "cfMedicoSostituto",
                "comunicazioneAsl",
                "dataInizioSostituzione", "dataFineSostituzione", "nota"
            },
            // ── EROGATORE ────────────────────────────────────────────────────────
            ["InvioErogato"] = new[]
            {
                "pinCode", "codiceRegioneErogatore", "codiceAslErogatore", "codiceSsaErogatore",
                "pwd", "nre", "cfAssistito", "tipoOperazione",
                "prescrizioneFruita", "tipoErogazioneSpec",
                "ticket", "quotaFissa", "franchigia", "galDirChiamAltro", "reddito",
                "dataSpedizione",
                "dispRic1", "dispRic2", "dispRic3"
            },
            ["VisualizzaErogato"] = new[]
            {
                "pinCode", "codiceRegioneErogatore", "codiceAslErogatore", "codiceSsaErogatore",
                "pwd", "nre", "cfAssistito", "tipoOperazione"
            },
            ["VisualizzaErogatoRichiesta"] = new[]
            {
                "pinCode", "codiceRegioneErogatore", "codiceAslErogatore", "codiceSsaErogatore",
                "pwd", "nre", "cfAssistito", "tipoOperazione"
            },
            ["SospendiErogato"] = new[]
            {
                "pinCode", "codiceRegioneErogatore", "codiceAslErogatore", "codiceSsaErogatore",
                "pwd", "nre", "cfAssistito", "tipoOperazione"
            },
            ["AnnullaErogato"] = new[]
            {
                "pinCode", "codiceRegioneErogatore", "codiceAslErogatore", "codiceSsaErogatore",
                "pwd", "nre", "cfAssistito", "codAnnullamento"
            },
            ["RicercaErogatore"] = new[]
            {
                "pinCode", "codiceRegioneErogatore", "codiceAslErogatore", "codiceSsaErogatore",
                "pwd", "nre", "cfAssistito",
                "disp1", "disp2", "disp3"
            },
            ["ReportErogatoMensile"] = new[]
            {
                "pinCode", "codiceRegioneErogatore", "codiceAslErogatore", "codiceSsaErogatore",
                "annoMese"
            },
            ["RicettaDifferita"] = new[]
            {
                "pinCode", "codiceRegioneErogatore", "codiceAslErogatore", "codiceSsaErogatore",
                "pwd", "codMotivazione", "dataDal", "note",
                "dispRic1", "dispRic2", "dispRic3"
            },
            ["AnnullaErogatoDiff"] = new[]
            {
                "pinCode", "codiceRegioneErogatore", "codiceAslErogatore", "codiceSsaErogatore",
                "pwd", "idRicetta", "nre", "cfAssistito",
                "codAnnullamentoDiff",
                "disp1", "disp2", "disp3"
            },
            ["RicevuteSac"] = new[]
            {
                "pinCode", "cfAssistito"
            },
        };

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
            Dictionary<string, string> parametri,
            string? prefissoNsOverride = null,
            bool usaDatNamespace = true,
            string? namespaceTipiDatiOverride = null,
            string? prefissoTipiDatiOverride = null)
        {
            bool isA2f = namespaceSoap.Contains("a2f") || namespaceSoap.Contains("auth");

            // Usa il prefisso fornito dall'endpoint; fallback euristico per compatibilità
            string prefissoNs = prefissoNsOverride ?? (isA2f ? "aut" : "tns");
            string prefissoTipiDati = prefissoTipiDatiOverride ?? (isA2f ? "dt" : "tip");
            string namespaceTipiDati = namespaceTipiDatiOverride
                                        ?? (isA2f
                                            ? "http://datatype.xsd.wsdl.auth.a2f.sts.sanita.finanze.it"
                                            : "http://tipodati.xsd.dem.sanita.finanze.it");

            // Raggruppa i parametri per elemento
            var elementiAnnidati = new Dictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase);
            var elementiSemplici = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            var dettagliPrescrizioniRaw = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            string? dettagliPrescrizioniLegacy = null;
            var dettagliInvioErogatoRaw = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            string? dettagliInvioErogatoLegacy = null;

            foreach (var kv in parametri)
            {
                var chiave = kv.Key;
                var valore = kv.Value;

                if (chiave.StartsWith("ElencoDettagliPrescrizioni_", StringComparison.OrdinalIgnoreCase))
                {
                    dettagliPrescrizioniRaw[chiave] = valore;
                    continue;
                }

                if (string.Equals(chiave, "ElencoDettagliPrescrizioni", StringComparison.OrdinalIgnoreCase))
                {
                    dettagliPrescrizioniLegacy = valore;
                    continue;
                }

                if (chiave.StartsWith("ElencoDettagliPrescrInviiErogato_", StringComparison.OrdinalIgnoreCase))
                {
                    dettagliInvioErogatoRaw[chiave] = valore;
                    continue;
                }

                if (string.Equals(chiave, "ElencoDettagliPrescrInviiErogato", StringComparison.OrdinalIgnoreCase))
                {
                    dettagliInvioErogatoLegacy = valore;
                    continue;
                }

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

            bool includeTipiDatiNs = elementiAnnidati.Count > 0;
            bool hasDettagliPrescrizioni =
                dettagliPrescrizioniRaw.Count > 0 ||
                string.Equals(dettagliPrescrizioniLegacy?.Trim(), "DETTAGLIO_MINIMO", StringComparison.OrdinalIgnoreCase);
            bool hasDettagliInvioErogato =
                dettagliInvioErogatoRaw.Count > 0 ||
                string.Equals(dettagliInvioErogatoLegacy?.Trim(), "DETTAGLIO_MINIMO", StringComparison.OrdinalIgnoreCase);

            if (string.Equals(operazione, "InvioPrescritto", StringComparison.OrdinalIgnoreCase)
                || string.Equals(operazione, "InvioPrescrittoRichiesta", StringComparison.OrdinalIgnoreCase))
            {
                includeTipiDatiNs |= hasDettagliPrescrizioni;
            }

            if (string.Equals(operazione, "InvioErogato", StringComparison.OrdinalIgnoreCase)
                || string.Equals(operazione, "InvioErogatoRichiesta", StringComparison.OrdinalIgnoreCase))
            {
                includeTipiDatiNs |= hasDettagliInvioErogato;
            }

            if (!usaDatNamespace)
            {
                includeTipiDatiNs = includeTipiDatiNs && (elementiAnnidati.Count > 0 || hasDettagliPrescrizioni || hasDettagliInvioErogato);
            }

            var sb = new StringBuilder();
            sb.Append("<soapenv:Envelope");
            sb.Append(" xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\"");
            sb.AppendFormat(" xmlns:{0}=\"{1}\"", prefissoNs, namespaceSoap);
            if (includeTipiDatiNs)
                sb.AppendFormat(" xmlns:{0}=\"{1}\"", prefissoTipiDati, namespaceTipiDati);
            sb.Append(">");

            sb.Append("<soapenv:Header/>");
            sb.Append("<soapenv:Body>");
            sb.AppendFormat("<{0}:{1}>", prefissoNs, operazione);

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
                    sb.AppendFormat("<{0}:{1}>{2}</{0}:{1}>", prefissoTipiDati, childName, SecurityElement.Escape(kvChild.Value ?? string.Empty));
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

                foreach (var kv in elementiSemplici)
                    sb.AppendFormat("<{0}:{1}>{2}</{0}:{1}>", prefissoNs, kv.Key, SecurityElement.Escape(kv.Value ?? string.Empty));
            }
            else
            {
                string[] ordine = OrdineParametri.TryGetValue(operazione, out var o) ? o : Array.Empty<string>();

                foreach (var key in ordine)
                {
                    if (elementiSemplici.TryGetValue(key, out var val))
                    {
                        sb.AppendFormat("<{0}:{1}>{2}</{0}:{1}>", prefissoNs, key, SecurityElement.Escape(val ?? string.Empty));
                        elementiSemplici.Remove(key);
                    }
                }

                // Eventuali campi non presenti nell'ordine esplicito
                foreach (var kv in elementiSemplici)
                    sb.AppendFormat("<{0}:{1}>{2}</{0}:{1}>", prefissoNs, kv.Key, SecurityElement.Escape(kv.Value ?? string.Empty));
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
                    sb.AppendFormat("<{0}:{1}>{2}</{0}:{1}>", prefissoTipiDati, childName, SecurityElement.Escape(kvChild.Value ?? string.Empty));
                }
                sb.AppendFormat("</{0}:{1}>", prefissoNs, parentName);
            }

            // Supporto struttura annidata di demInvioPrescritto:
            // <ElencoDettagliPrescrizioni><DettaglioPrescrizione>...</DettaglioPrescrizione></ElencoDettagliPrescrizioni>
            if (string.Equals(operazione, "InvioPrescritto", StringComparison.OrdinalIgnoreCase)
                || string.Equals(operazione, "InvioPrescrittoRichiesta", StringComparison.OrdinalIgnoreCase))
            {
                AppendElencoDettagliPrescrizioni(sb, prefissoNs, prefissoTipiDati, dettagliPrescrizioniRaw, dettagliPrescrizioniLegacy);
            }

            // Supporto struttura annidata di demInvioErogato:
            // <ElencoDettagliPrescrInviiErogato><DettaglioPrescrizioneInvioErogato>...</DettaglioPrescrizioneInvioErogato></ElencoDettagliPrescrInviiErogato>
            if (string.Equals(operazione, "InvioErogato", StringComparison.OrdinalIgnoreCase)
                || string.Equals(operazione, "InvioErogatoRichiesta", StringComparison.OrdinalIgnoreCase))
            {
                AppendElencoDettagliPrescrInviiErogato(sb, prefissoNs, prefissoTipiDati, dettagliInvioErogatoRaw, dettagliInvioErogatoLegacy);
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

        private static void AppendElencoDettagliPrescrizioni(
            StringBuilder sb,
            string prefissoNs,
            string prefissoTipiDati,
            Dictionary<string, string> dettagliRaw,
            string? dettagliLegacy)
        {
            var dettagli = new SortedDictionary<int, Dictionary<string, string>>();

            foreach (var kv in dettagliRaw)
            {
                var remainder = kv.Key.Substring("ElencoDettagliPrescrizioni_".Length);
                if (string.IsNullOrWhiteSpace(remainder)) continue;

                int indice = 1;
                string campo;

                var parti = remainder.Split(new[] { '_' }, 2);
                if (parti.Length == 2 && int.TryParse(parti[0], out var idx) && idx > 0)
                {
                    indice = idx;
                    campo = parti[1];
                }
                else
                {
                    campo = remainder;
                }

                if (string.IsNullOrWhiteSpace(campo)) continue;

                if (!dettagli.TryGetValue(indice, out var dict))
                {
                    dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                    dettagli[indice] = dict;
                }

                dict[campo] = kv.Value;
            }

            // Compatibilità con input storico: ElencoDettagliPrescrizioni=DETTAGLIO_MINIMO
            if (dettagli.Count == 0 &&
                string.Equals(dettagliLegacy?.Trim(), "DETTAGLIO_MINIMO", StringComparison.OrdinalIgnoreCase))
            {
                dettagli[1] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    ["quantita"] = "1"
                };
            }

            if (dettagli.Count == 0) return;

            var ordineCampi = new[]
            {
                "codProdPrest", "descrProdPrest", "codGruppoEquival", "descrGruppoEquival",
                "testoLibero", "descrTestoLiberoNote", "nonSost", "motivazNote", "codMotivazione",
                "notaProd", "quantita", "prescrizione1", "prescrizione2", "codCatalogoPrescr",
                "tipoAccesso", "numeroNota", "condErogabilita", "approprPrescrittiva", "patologia", "numsedute"
            };

            sb.AppendFormat("<{0}:ElencoDettagliPrescrizioni>", prefissoNs);

            foreach (var dettaglio in dettagli.Values)
            {
                if (!dettaglio.ContainsKey("quantita") || string.IsNullOrWhiteSpace(dettaglio["quantita"]))
                    dettaglio["quantita"] = "1";

                sb.AppendFormat("<{0}:DettaglioPrescrizione>", prefissoTipiDati);

                foreach (var campo in ordineCampi)
                {
                    if (!dettaglio.TryGetValue(campo, out var valore) || string.IsNullOrWhiteSpace(valore))
                        continue;

                    sb.AppendFormat("<{0}:{1}>{2}</{0}:{1}>", prefissoTipiDati, campo, SecurityElement.Escape(valore));
                }

                sb.AppendFormat("</{0}:DettaglioPrescrizione>", prefissoTipiDati);
            }

            sb.AppendFormat("</{0}:ElencoDettagliPrescrizioni>", prefissoNs);
        }

        private static void AppendElencoDettagliPrescrInviiErogato(
            StringBuilder sb,
            string prefissoNs,
            string prefissoTipiDati,
            Dictionary<string, string> dettagliRaw,
            string? dettagliLegacy)
        {
            var dettagli = new SortedDictionary<int, Dictionary<string, string>>();

            foreach (var kv in dettagliRaw)
            {
                var remainder = kv.Key.Substring("ElencoDettagliPrescrInviiErogato_".Length);
                if (string.IsNullOrWhiteSpace(remainder)) continue;

                int indice = 1;
                string campo;

                var parti = remainder.Split(new[] { '_' }, 2);
                if (parti.Length == 2 && int.TryParse(parti[0], out var idx) && idx > 0)
                {
                    indice = idx;
                    campo = parti[1];
                }
                else
                {
                    campo = remainder;
                }

                if (string.IsNullOrWhiteSpace(campo)) continue;

                if (!dettagli.TryGetValue(indice, out var dict))
                {
                    dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                    dettagli[indice] = dict;
                }

                dict[campo] = kv.Value;
            }

            // Compatibilità con eventuale input minimale legacy
            if (dettagli.Count == 0 &&
                string.Equals(dettagliLegacy?.Trim(), "DETTAGLIO_MINIMO", StringComparison.OrdinalIgnoreCase))
            {
                dettagli[1] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    ["codProdPrestErog"] = "0",
                    ["prezzo"] = "0",
                    ["quantitaErogata"] = "1",
                    ["dataIniErog"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    ["dataFineErog"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                };
            }

            if (dettagli.Count == 0) return;

            var ordineCampi = new[]
            {
                "codProdPrest", "codGruppoEquival", "descrTestoLiberoNote", "codProdPrestErog", "descrProdPrestErog",
                "flagErog", "motivazSostProd", "targa", "dichTargaDoppia", "codBranca", "tipoErogazioneFarm",
                "prezzo", "ticketConfezione", "diffGenerico", "quantitaErogata", "dataIniErog", "dataFineErog",
                "prezzoRimborso", "onereProd", "scontoSSN", "extraScontoIndustria", "extraScontoPayback",
                "extraScontoDL31052010", "codPresidio", "codReparto", "dispFust1", "dispFust2", "dispFust3",
                "codCatalogoPrescr", "codCatalogoErog", "garanziaTempiMax", "dataPrenotazione"
            };

            sb.AppendFormat("<{0}:ElencoDettagliPrescrInviiErogato>", prefissoNs);

            foreach (var dettaglio in dettagli.Values)
            {
                sb.AppendFormat("<{0}:DettaglioPrescrizioneInvioErogato>", prefissoTipiDati);

                foreach (string campo in ordineCampi)
                {
                    if (!dettaglio.TryGetValue(campo, out var valore) || string.IsNullOrWhiteSpace(valore))
                        continue;

                    sb.AppendFormat("<{0}:{1}>{2}</{0}:{1}>", prefissoTipiDati, campo, SecurityElement.Escape(valore));
                }

                sb.AppendFormat("</{0}:DettaglioPrescrizioneInvioErogato>", prefissoTipiDati);
            }

            sb.AppendFormat("</{0}:ElencoDettagliPrescrInviiErogato>", prefissoNs);
        }
     }
 }
