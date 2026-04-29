using System;
using System.Collections.Generic;
using System.Linq;
using ricetta_dematerializzata.Models;

namespace ricetta_dematerializzata.Core
{
    internal static class InputMapperService
    {
        private static readonly Dictionary<DigitalPrescriptionService, string[]> RequiredCanonicalKeys = new()
        {
            [DigitalPrescriptionService.VisualizzaPrescritto] = new[] { "pinCode", "nre", "cfMedico" },
            [DigitalPrescriptionService.InvioPrescritto] = new[] { "pinCode", "cfMedico1", "codRegione", "codASLAo", "codSpecializzazione", "codiceAss", "tipoPrescrizione", "dataCompilazione", "tipoVisita", "ElencoDettagliPrescrizioni" },
            [DigitalPrescriptionService.AnnullaPrescritto] = new[] { "pinCode", "nre", "cfMedico" },
            [DigitalPrescriptionService.InterrogaNreUtilizzati] = new[] { "pinCode", "codRegione", "cfMedico" },
            [DigitalPrescriptionService.ServiceAnagPrescrittore] = new[] { "pinCode", "tipoOperazione" },
            [DigitalPrescriptionService.InvioDichiarazioneSostituzioneMedico] = new[] { "pinCode", "pwd", "cfMedicoTitolare", "codRegione", "codASLAo", "codSpecializzazione", "cfMedicoSostituto", "dataInizioSostituzione", "dataFineSostituzione" },
            [DigitalPrescriptionService.InvioErogato] = new[] { "pinCode", "codiceRegioneErogatore", "codiceAslErogatore", "codiceSsaErogatore", "nre", "tipoOperazione", "dataSpedizione" },
            [DigitalPrescriptionService.VisualizzaErogato] = new[] { "pinCode", "codiceRegioneErogatore", "codiceAslErogatore", "codiceSsaErogatore", "nre", "tipoOperazione" },
            [DigitalPrescriptionService.SospendiErogato] = new[] { "pinCode", "codiceRegioneErogatore", "codiceAslErogatore", "codiceSsaErogatore", "nre", "tipoOperazione" },
            [DigitalPrescriptionService.AnnullaErogato] = new[] { "pinCode", "codiceRegioneErogatore", "codiceAslErogatore", "codiceSsaErogatore", "nre", "codAnnullamento" },
            [DigitalPrescriptionService.RicercaErogatore] = new[] { "pinCode", "codiceRegioneErogatore", "codiceAslErogatore", "codiceSsaErogatore" },
            [DigitalPrescriptionService.ReportErogatoMensile] = new[] { "pinCode", "codiceRegioneErogatore", "codiceAslErogatore", "codiceSsaErogatore", "annoMese" },
            [DigitalPrescriptionService.ServiceAnagErogatore] = new[] { "pinCode", "tipoOperazione" },
            [DigitalPrescriptionService.RicettaDifferita] = new[] { "pinCode", "codiceRegioneErogatore", "codiceAslErogatore", "codiceSsaErogatore", "codMotivazione", "dataDal" },
            [DigitalPrescriptionService.AnnullaErogatoDiff] = new[] { "pinCode", "codiceRegioneErogatore", "codiceAslErogatore", "codiceSsaErogatore", "idRicetta", "nre" },

            [DigitalPrescriptionService.CreateAuth] = new[] { "contesto" },
            [DigitalPrescriptionService.RevokeAuth] = new[] { "token" },
            [DigitalPrescriptionService.CheckToken] = new[] { "token" }
        };

        private static readonly Dictionary<DigitalPrescriptionService, Dictionary<string, string>> AliasToCanonical = new()
        {
            [DigitalPrescriptionService.VisualizzaPrescritto] = new(StringComparer.OrdinalIgnoreCase)
            {
                ["PINCODE"] = "pinCode",
                ["NRE"] = "nre",
                ["CFMEDICO"] = "cfMedico"
            },
            [DigitalPrescriptionService.InvioPrescritto] = new(StringComparer.OrdinalIgnoreCase)
            {
                ["PINCODE"] = "pinCode",
                ["CFMEDICO1"] = "cfMedico1",
                ["CFMEDICO2"] = "cfMedico2",
                ["CODREGIONE"] = "codRegione",
                ["CODASLAO"] = "codASLAo",
                ["CODSTRUTTURA"] = "codStruttura",
                ["CODSPECIALIZZAZIONE"] = "codSpecializzazione",
                ["CODICEASS"] = "codiceAss",
                ["TIPOPRESCRIZIONE"] = "tipoPrescrizione",
                ["RICETTAINTERNA"] = "ricettaInterna",
                ["CODESENZIONE"] = "codEsenzione",
                ["NONESENTE"] = "nonEsente",
                ["REDDITO"] = "reddito",
                ["CODDIAGNOSI"] = "codDiagnosi",
                ["DESCRDIAGNOSI"] = "descrizioneDiagnosi",
                ["DESCRIZIONEDIAGNOSI"] = "descrizioneDiagnosi",
                ["DATACOMPILAZIONE"] = "dataCompilazione",
                ["TIPOVISITA"] = "tipoVisita",
                ["CLASSEPRIORITA"] = "classePriorita",
                ["PRIORITA"] = "classePriorita",
                ["PROVASSISTITO"] = "provAssistito",
                ["PROVINCIAASSISTITO"] = "provAssistito",
                ["ASLASSISTITO"] = "aslAssistito",
                ["ELENCODETTAGLIPRESCRIZIONI"] = "ElencoDettagliPrescrizioni",
                ["NRE"] = "nre"
            },
            [DigitalPrescriptionService.AnnullaPrescritto] = new(StringComparer.OrdinalIgnoreCase)
            {
                ["PINCODE"] = "pinCode",
                ["NRE"] = "nre",
                ["CFMEDICO"] = "cfMedico",
                ["CFASSISTITO"] = "cfAssistito"
            },
            [DigitalPrescriptionService.InterrogaNreUtilizzati] = new(StringComparer.OrdinalIgnoreCase)
            {
                ["PINCODE"] = "pinCode",
                ["CODREGIONE"] = "codRegione",
                ["NRE"] = "nre",
                ["CFMEDICO"] = "cfMedico",
                ["CFASSISTITO"] = "cfAssistito"
            },
            [DigitalPrescriptionService.ServiceAnagPrescrittore] = new(StringComparer.OrdinalIgnoreCase)
            {
                ["PINCODE"] = "pinCode",
                ["TIPOOPERAZIONE"] = "tipoOperazione"
            },
            [DigitalPrescriptionService.InvioDichiarazioneSostituzioneMedico] = new(StringComparer.OrdinalIgnoreCase)
            {
                ["PINCODE"] = "pinCode",
                ["PWD"] = "pwd",
                ["CFMEDICOTITOLARE"] = "cfMedicoTitolare",
                ["CODREGIONE"] = "codRegione",
                ["CODASLAO"] = "codASLAo",
                ["CODSTRUTTURA"] = "codStruttura",
                ["CODSPECIALIZZAZIONE"] = "codSpecializzazione",
                ["CFMEDICOSOSTITUTO"] = "cfMedicoSostituto",
                ["DATAINIZIOSOSTITUZIONE"] = "dataInizioSostituzione",
                ["DATAFINESOSTITUZIONE"] = "dataFineSostituzione"
            },
            [DigitalPrescriptionService.InvioErogato] = ErogatoreAliasBase(new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["NRE"] = "nre",
                ["CFASSISTITO"] = "cfAssistito",
                ["TIPOOPERAZIONE"] = "tipoOperazione",
                ["DATASPEDIZIONE"] = "dataSpedizione"
            }),
            [DigitalPrescriptionService.VisualizzaErogato] = ErogatoreAliasBase(new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["NRE"] = "nre",
                ["CFASSISTITO"] = "cfAssistito",
                ["TIPOOPERAZIONE"] = "tipoOperazione"
            }),
            [DigitalPrescriptionService.SospendiErogato] = ErogatoreAliasBase(new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["NRE"] = "nre",
                ["CFASSISTITO"] = "cfAssistito",
                ["TIPOOPERAZIONE"] = "tipoOperazione"
            }),
            [DigitalPrescriptionService.AnnullaErogato] = ErogatoreAliasBase(new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["NRE"] = "nre",
                ["CFASSISTITO"] = "cfAssistito",
                ["CODANNULLAMENTO"] = "codAnnullamento"
            }),
            [DigitalPrescriptionService.RicercaErogatore] = ErogatoreAliasBase(new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["CFASSISTITO"] = "cfAssistito"
            }),
            [DigitalPrescriptionService.ReportErogatoMensile] = ErogatoreAliasBase(new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["ANNOMESE"] = "annoMese"
            }),
            [DigitalPrescriptionService.ServiceAnagErogatore] = new(StringComparer.OrdinalIgnoreCase)
            {
                ["PINCODE"] = "pinCode",
                ["TIPOOPERAZIONE"] = "tipoOperazione"
            },
            [DigitalPrescriptionService.RicettaDifferita] = ErogatoreAliasBase(new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["CODMOTIVAZIONE"] = "codMotivazione",
                ["DATADAL"] = "dataDal",
                ["CFASSISTITO"] = "cfAssistito"
            }),
            [DigitalPrescriptionService.AnnullaErogatoDiff] = ErogatoreAliasBase(new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["IDRICETTA"] = "idRicetta",
                ["NRE"] = "nre",
                ["CFASSISTITO"] = "cfAssistito"
            }),

            [DigitalPrescriptionService.CreateAuth] = new(StringComparer.OrdinalIgnoreCase)
            {
                ["USERID"] = "userId",
                ["IDENTIFICATIVO"] = "identificativo",
                ["IDENTIFICATIVO_TIPO"] = "identificativo_tipo",
                ["IDENTIFICATIVO_VALORE"] = "identificativo_valore",
                ["CFUTENTE"] = "cfUtente",
                ["CODREGIONE"] = "codRegione",
                ["CODASLAO"] = "codAslAo",
                ["CODSSA"] = "codSsa",
                ["CODICESTRUTTURA"] = "codiceStruttura",
                ["CONTESTO"] = "contesto",
                ["APPLICAZIONE"] = "applicazione"
            },

            [DigitalPrescriptionService.RevokeAuth] = new(StringComparer.OrdinalIgnoreCase)
            {
                ["USERID"] = "userId",
                ["IDENTIFICATIVO"] = "identificativo",
                ["IDENTIFICATIVO_TIPO"] = "identificativo_tipo",
                ["IDENTIFICATIVO_VALORE"] = "identificativo_valore",
                ["CFUTENTE"] = "cfUtente",
                ["CODREGIONE"] = "codRegione",
                ["CODASLAO"] = "codAslAo",
                ["CODSSA"] = "codSsa",
                ["CODICESTRUTTURA"] = "codiceStruttura",
                ["TOKEN"] = "token",
                ["CONTESTO"] = "contesto",
                ["APPLICAZIONE"] = "applicazione"
            },

            [DigitalPrescriptionService.CheckToken] = new(StringComparer.OrdinalIgnoreCase)
            {
                ["USERID"] = "userId",
                ["IDENTIFICATIVO"] = "identificativo",
                ["IDENTIFICATIVO_TIPO"] = "identificativo_tipo",
                ["IDENTIFICATIVO_VALORE"] = "identificativo_valore",
                ["CFUTENTE"] = "cfUtente",
                ["CODREGIONE"] = "codRegione",
                ["CODASLAO"] = "codAslAo",
                ["CODSSA"] = "codSsa",
                ["CODICESTRUTTURA"] = "codiceStruttura",
                ["TOKEN"] = "token",
                ["CONTESTO"] = "contesto",
                ["APPLICAZIONE"] = "applicazione"
            }
        };

        // Campi che richiedono il formato data-ora: "yyyy-MM-dd HH:mm:ss" (19 char, come da XSD dataOraType)
        private static readonly string[] CampiDataOra =
        {
            "dataCompilazione", "dataInizioSostituzione", "dataFineSostituzione",
            "dataDal", "dataSpedizione",
            "dataIniErog", "dataFineErog",
            "dataCompilazioneRicettaDal", "dataCompilazioneRicettaAl",
            "dataNascitaEstero", "dataScadTessera"
        };

        private const string FormatoDataOra = "yyyy-MM-dd HH:mm:ss";

        public static Dictionary<string, string> NormalizeAndValidate(DigitalPrescriptionService servizio, Dictionary<string, string> input)
        {
            var normalized = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            var aliases = AliasToCanonical.TryGetValue(servizio, out var map)
                ? map
                : new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            foreach (var kv in input)
            {
                var sourceKey = kv.Key;
                var canonical = aliases.TryGetValue(sourceKey, out var mapped)
                    ? mapped
                    : sourceKey;

                normalized[canonical] = kv.Value;
            }

            if (servizio == DigitalPrescriptionService.InvioPrescritto)
            {
                var hasStructuredDettagli = normalized.Keys.Any(k =>
                    k.StartsWith("ElencoDettagliPrescrizioni_", StringComparison.OrdinalIgnoreCase));

                if (hasStructuredDettagli && !normalized.ContainsKey("ElencoDettagliPrescrizioni"))
                    normalized["ElencoDettagliPrescrizioni"] = "STRUCTURED";
            }

            if (servizio == DigitalPrescriptionService.InvioErogato)
            {
                var hasStructuredDettagliErogato = normalized.Keys.Any(k =>
                    k.StartsWith("ElencoDettagliPrescrInviiErogato_", StringComparison.OrdinalIgnoreCase));

                if (hasStructuredDettagliErogato && !normalized.ContainsKey("ElencoDettagliPrescrInviiErogato"))
                    normalized["ElencoDettagliPrescrInviiErogato"] = "STRUCTURED";
            }

            ValidateRequired(servizio, normalized);
            ValidateDataOra(normalized);
            return normalized;
        }

        private static void ValidateRequired(DigitalPrescriptionService servizio, Dictionary<string, string> normalized)
        {
            if (!RequiredCanonicalKeys.TryGetValue(servizio, out var required) || required.Length == 0)
                return;

            var missing = required.Where(k => !normalized.TryGetValue(k, out var v) || string.IsNullOrWhiteSpace(v)).ToArray();
            if (missing.Length == 0)
                return;

            throw new ArgumentException($"Parametri obbligatori mancanti per {servizio}: {string.Join(", ", missing)}");
        }

        private static void ValidateDataOra(Dictionary<string, string> normalized)
        {
            foreach (var campo in CampiDataOra)
            {
                if (!normalized.TryGetValue(campo, out var valore) || string.IsNullOrWhiteSpace(valore))
                    continue;

                if (!DateTime.TryParseExact(valore, FormatoDataOra,
                        System.Globalization.CultureInfo.InvariantCulture,
                        System.Globalization.DateTimeStyles.None, out _))
                {
                    throw new ArgumentException(
                        $"Il campo '{campo}' ha formato data non valido: '{valore}'. " +
                        $"Il formato atteso è '{FormatoDataOra}' (es. 2026-01-01 10:00:00).");
                }
            }
        }

        private static Dictionary<string, string> ErogatoreAliasBase(Dictionary<string, string> specific)
        {
            specific["PINCODE"] = "pinCode";
            specific["CODICEREGIONEEROGATORE"] = "codiceRegioneErogatore";
            specific["CODICEASLEROGATORE"] = "codiceAslErogatore";
            specific["CODICESSAEROGATORE"] = "codiceSsaErogatore";
            return specific;
        }
    }
}
