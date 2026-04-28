using System;
using ricetta_dematerializzata_dll.Core;
using ricetta_dematerializzata_dll.Models;

namespace ricetta_dematerializzata_dll.Services
{
    /// <summary>
    /// Client specializzato per i servizi di autenticazione A2F (Create, Revoke, CheckToken).
    /// Usa SoapHttpClient come trasporto HTTP.
    /// </summary>
    public class Auth2FClient
    {
        private ServiceConfiguration _config;
        private SoapHttpClient _httpClient;

        public Auth2FClient(ServiceConfiguration configurazione)
        {
            _config = configurazione;
            _httpClient = CreaHttpClient(configurazione);
        }

        /// <summary>
        /// Crea un token di sessione A2F.
        /// </summary>
        public string Create(string parametriInput)
            => Chiama(DigitalPrescriptionService.CreateAuth, parametriInput);

        /// <summary>
        /// Revoca un token di sessione A2F.
        /// </summary>
        public string Revoke(string parametriInput)
            => Chiama(DigitalPrescriptionService.RevokeAuth, parametriInput);

        /// <summary>
        /// Verifica la validità di un token A2F.
        /// </summary>
        public string CheckToken(string parametriInput)
            => Chiama(DigitalPrescriptionService.CheckToken, parametriInput);

        private string Chiama(DigitalPrescriptionService servizio, string parametriInput)
        {
            try
            {
                var dictInput = ParserKV.Parse(parametriInput);
                var dictCanonico = InputMapperService.NormalizeAndValidate(servizio, dictInput);

                EspandiIdentificativo(dictCanonico);
                NormalizzaCampiPerOperazione(servizio, dictCanonico);

                var endpoint = ServicesCatalog.Ottieni(servizio);
                var url = endpoint.GetUrl(_config.Ambiente);
                var soapAction = endpoint.SoapAction;
                var operazione = endpoint.OperazioneWsdl;
                var namespaceSoap = ServicesCatalog.OttieniNamespaceSoap(servizio);

                var envelope = SoapHelper.BuildSoapEnvelope(operazione, namespaceSoap, dictCanonico);
                var xmlRisposta = _httpClient.ChiamaServizio(url, soapAction, envelope, null);

                var dictOutput = SoapHelper.ParseSoapResponse(xmlRisposta);
                return ParserKV.Build(dictOutput);
            }
            catch (Exception ex)
            {
                return ParserKV.BuildErrore(9999, $"Eccezione client: {ex.Message}");
            }
        }

        private static void EspandiIdentificativo(System.Collections.Generic.Dictionary<string, string> dict)
        {
            if (dict.ContainsKey("identificativo_tipo") || dict.ContainsKey("identificativo_valore"))
                return;
            if (!dict.TryGetValue("identificativo", out var valore) || string.IsNullOrWhiteSpace(valore))
                return;

            dict.Remove("identificativo");
            dict["identificativo_tipo"] = "P";
            dict["identificativo_valore"] = valore;
        }

        private static void NormalizzaCampiPerOperazione(
            DigitalPrescriptionService servizio,
            System.Collections.Generic.Dictionary<string, string> dict)
        {
            if (servizio == DigitalPrescriptionService.RevokeAuth ||
                servizio == DigitalPrescriptionService.CheckToken)
            {
                dict.Remove("codRegione");
                dict.Remove("codAslAo");
                dict.Remove("codSsa");
                dict.Remove("codiceStruttura");
            }
        }

        private static SoapHttpClient CreaHttpClient(ServiceConfiguration cfg)
            => new SoapHttpClient(
                cfg.Username,
                cfg.Password,
                cfg.PathCertificatoSsl,
                cfg.PathCertificatoCA,
                cfg.IgnoraErroriSsl,
                cfg.Ambiente);
    }
}


