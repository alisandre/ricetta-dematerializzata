using System;
using System.Collections.Generic;
using System.Linq;
using ricetta_dematerializzata_dll.Models;

namespace ricetta_dematerializzata_dll.Core
{
    internal static class InputMapperService
    {
        private static readonly Dictionary<DigitalPrescriptionService, string[]> RequiredCanonicalKeys = new()
        {
            [DigitalPrescriptionService.VisualizzaPrescritto] = new[] { "pinCode", "nre", "cfMedico" },
            [DigitalPrescriptionService.InvioPrescritto] = new[] { "pinCode", "cfMedico1", "codRegione", "codASLAo", "codSpecializzazione", "tipoPrescrizione", "dataCompilazione", "tipoVisita", "ElencoDettagliPrescrizioni" },
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
                ["TIPOPRESCRIZIONE"] = "tipoPrescrizione",
                ["DATACOMPILAZIONE"] = "dataCompilazione",
                ["TIPOVISITA"] = "tipoVisita",
                ["ELENCODETTAGLIPRESCRIZIONI"] = "ElencoDettagliPrescrizioni",
                ["NRE"] = "nre",
                ["CODICEASS"] = "codiceAss"
            },
            [DigitalPrescriptionService.AnnullaPrescritto] = new(StringComparer.OrdinalIgnoreCase)
            {
                ["PINCODE"] = "pinCode",
                ["NRE"] = "nre",
                ["CFMEDICO"] = "cfMedico"
            },
            [DigitalPrescriptionService.InterrogaNreUtilizzati] = new(StringComparer.OrdinalIgnoreCase)
            {
                ["PINCODE"] = "pinCode",
                ["CODREGIONE"] = "codRegione",
                ["CFMEDICO"] = "cfMedico",
                ["NRE"] = "nre"
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
                ["CODSPECIALIZZAZIONE"] = "codSpecializzazione",
                ["CFMEDICOSOSTITUTO"] = "cfMedicoSostituto",
                ["DATAINIZIOSOSTITUZIONE"] = "dataInizioSostituzione",
                ["DATAFINESOSTITUZIONE"] = "dataFineSostituzione"
            },
            [DigitalPrescriptionService.InvioErogato] = ErogatoreAliasBase(new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["TIPOOPERAZIONE"] = "tipoOperazione",
                ["DATASPEDIZIONE"] = "dataSpedizione",
                ["NRE"] = "nre"
            }),
            [DigitalPrescriptionService.VisualizzaErogato] = ErogatoreAliasBase(new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["TIPOOPERAZIONE"] = "tipoOperazione",
                ["NRE"] = "nre",
                ["CFASSISTITO"] = "cfAssistito"
            }),
            [DigitalPrescriptionService.SospendiErogato] = ErogatoreAliasBase(new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["TIPOOPERAZIONE"] = "tipoOperazione",
                ["NRE"] = "nre"
            }),
            [DigitalPrescriptionService.AnnullaErogato] = ErogatoreAliasBase(new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["CODANNULLAMENTO"] = "codAnnullamento",
                ["NRE"] = "nre"
            }),
            [DigitalPrescriptionService.RicercaErogatore] = ErogatoreAliasBase(new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)),
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
                ["DATADAL"] = "dataDal"
            }),
            [DigitalPrescriptionService.AnnullaErogatoDiff] = ErogatoreAliasBase(new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["IDRICETTA"] = "idRicetta",
                ["NRE"] = "nre"
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

            ValidateRequired(servizio, normalized);
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
