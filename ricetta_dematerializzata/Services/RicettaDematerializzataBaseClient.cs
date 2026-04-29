using System;
using System.Collections.Generic;
using System.IO;
using ricetta_dematerializzata.Core;
using ricetta_dematerializzata.Crypto;
using ricetta_dematerializzata.Models;

namespace ricetta_dematerializzata.Services
{
    /// <summary>
    /// Client principale per i servizi SAC / DEM Ricetta.
    ///
    /// Uso da C#:
    ///   var cfg = ConfigurazioneServizio.CreaTest("user", "pass");
    ///   var client = new RicettaDematerializzataBaseClient(cfg);
    ///   string output = client.Chiama((int)DigitalPrescriptionService.InvioPrescritto,
    ///       "NRE=12345;CODICE_FISCALE_MEDICO=RSSMRA80A01H501T;...");
    ///
    /// Uso da Delphi (dopo registrazione COM):
    ///   var client: IRicettaDematerializzataClient;
    ///   client := CoRicettaDematerializzataClient.Create;
    ///   client.Configura('user', 'pass', 0 {Test});
    ///   output := client.Chiama(2 {InvioPrescritto}, 'NRE=12345;...');
    /// </summary>
    public class RicettaDematerializzataBaseClient : IRicettaDematerializzataClient
    {
        private ServiceConfiguration? _config;
        private SoapHttpClient? _httpClient;

        private static readonly string[] CampiSensibili = { "cfAssistito", "pinCode", "codiceAss" };
        private static readonly string[] CampiDaCifrare = { "cfAssistito", "pinCode", "codiceAss" };

        // ── Costruttori ───────────────────────────────────────────────────────────

        /// <summary>Costruttore per uso diretto da C# con configurazione completa.</summary>
        public RicettaDematerializzataBaseClient(ServiceConfiguration configurazione)
        {
            _config = configurazione;
            _config.Valida();
            _httpClient = new SoapHttpClient(
                _config.Username,
                _config.Password,
                _config.SerialeCertificatoSsl,
                _config.RisolviPathCertificatoCA(),
                _config.IgnoraErroriSsl,
                _config.Ambiente);
        }

        /// <summary>
        /// Costruttore senza parametri richiesti per COM Interop (Delphi).
        /// Chiamare Configura() prima di Chiama().
        /// </summary>
        public RicettaDematerializzataBaseClient() { }

        /// <summary>
        /// Configura il client. Da chiamare dopo il costruttore vuoto (uso Delphi).
        /// </summary>
        /// <param name="username">Username autenticazione Basic</param>
        /// <param name="password">Password autenticazione Basic</param>
        /// <param name="ambiente">0=Test, 1=Produzione</param>
        [Obsolete("Usare ConfiguraUnificata() con (username, password, seriale) invece.", false)]
        public void Configura(string username, string password, int ambiente = 0)
        {
            _config = new ServiceConfiguration
            {
                Username = username,
                Password = password,
                Ambiente = (ServiceEnvironment)ambiente,
            };
            _httpClient = new SoapHttpClient(
                username, password, null, null, _config.IgnoraErroriSsl, (ServiceEnvironment)ambiente);
        }

        /// <summary>
        /// Configura il client con username, password e seriale certificato.
        /// Questo è il metodo di configurazione principale.
        /// Il certificato SSL verrà usato automaticamente per i servizi che lo richiedono (prescrittore/erogatore),
        /// mentre i servizi di gestione token (Create/Revoke/CheckToken) useranno sempre Basic Auth.
        /// </summary>
        /// <param name="username">Username per autenticazione Basic</param>
        /// <param name="password">Password per autenticazione Basic</param>
        /// <param name="seriale">Seriale del certificato client nel Windows Certificate Store (LOCAL_MACHINE\Personal)</param>
        /// <param name="ambiente">0=Test, 1=Produzione</param>
        public void Configura(string username, string password, string seriale, int ambiente = 0)
        {
            _config = new ServiceConfiguration
            {
                Username              = username,
                Password              = password,
                SerialeCertificatoSsl = seriale,
                Ambiente              = (ServiceEnvironment)ambiente,
            };
            _httpClient = new SoapHttpClient(
                username, password, seriale, _config.RisolviPathCertificatoCA(), _config.IgnoraErroriSsl, (ServiceEnvironment)ambiente);
        }

        /// <summary>
        /// Metodo precedente mantenuto per compatibilità. Usare Configura() preferibilmente.
        /// </summary>
        /// <param name="username">Username autenticazione Basic</param>
        /// <param name="password">Password autenticazione Basic</param>
        /// <param name="ambiente">0=Test, 1=Produzione</param>
        [Obsolete("Usare Configura(username, password, seriale) invece.", false)]
        public void ConfiguraLegacy(string username, string password, int ambiente = 0)
        {
            _config = new ServiceConfiguration
            {
                Username = username,
                Password = password,
                Ambiente = (ServiceEnvironment)ambiente,
            };
            _httpClient = new SoapHttpClient(
                username, password, null, null, _config.IgnoraErroriSsl, (ServiceEnvironment)ambiente);
        }

        public void ConfiguraAuthorization2F(string? authorization2F)
        {
            if (_config == null)
                throw new InvalidOperationException("Chiamare prima Configura().");

            _config.Authorization2F = authorization2F;
        }

        // ── Metodo principale ─────────────────────────────────────────────────────

        /// <summary>
        /// Chiama un servizio SAC e restituisce il risultato nel formato chiave=valore.
        ///
        /// Input:  "CHIAVE1=VALORE1;CHIAVE2=VALORE2"
        /// Output: "CHIAVE1=VALORE1;CHIAVE2=VALORE2"
        ///         oppure "ERRORE_NUMERO=9999;ERRORE_DESCRIZIONE=messaggio"
        ///
        /// Codici esito standard:
        ///   CODICE_ESITO=0000 → Transazione avvenuta con successo
        ///   CODICE_ESITO=0001 → Successo con avvisi non bloccanti
        ///   CODICE_ESITO=9999 → Errore bloccante
        /// </summary>
        /// <param name="servizio">Servizio da chiamare (valore intero dell'enum DigitalPrescriptionService)</param>
        /// <param name="parametriInput">Stringa key=value con i parametri di input</param>
        public string Chiama(int servizio, string parametriInput)
            => Chiama((DigitalPrescriptionService)servizio, parametriInput);

        /// <summary>Overload tipizzato per uso da C#.</summary>
        public string Chiama(DigitalPrescriptionService servizio, string parametriInput)
        {
            // I servizi A2F non devono passare per PrescriptionClient
            if (servizio == DigitalPrescriptionService.CreateAuth ||
                servizio == DigitalPrescriptionService.RevokeAuth ||
                servizio == DigitalPrescriptionService.CheckToken)
                return ParserKV.BuildErrore(9999, "Usare Auth2FClient per i servizi di autenticazione A2F (Create/Revoke/CheckToken).");

            try
            {
                ValidaConfigurazione();
                var config     = _config!;
                var httpClient = _httpClient!;

                // 1. Parse input
                var dictInput    = ParserKV.Parse(parametriInput);
                var dictCanonico = InputMapperService.NormalizeAndValidate(servizio, dictInput);

                // 2. Cifratura automatica CF e pincode
                CifraParametriSensibili(dictCanonico);

                // 3. Recupera info endpoint
                var endpoint      = ServicesCatalog.Ottieni(servizio);
                var url           = endpoint.GetUrl(config.Ambiente);
                var soapAction    = endpoint.SoapAction;
                var operazione    = endpoint.OperazioneWsdl;
                var namespaceSoap = ServicesCatalog.OttieniNamespaceSoap(servizio);

                // 4. Costruisce SOAP envelope
                var envelope = SoapHelper.BuildSoapEnvelope(operazione, namespaceSoap, dictCanonico, endpoint.PrefissoNs, endpoint.UsaDatNamespace);

                // 5. Chiamata HTTP con Authorization2F
                var authorization2F = CreaAuthorization2F(servizio);
                var xmlRisposta = httpClient.ChiamaServizio(url, soapAction, envelope, authorization2F, endpoint.UsaSslClientCert, endpoint.UsaValidazioneCA);

                // 6. Parse risposta
                var dictOutput = SoapHelper.ParseSoapResponse(xmlRisposta);
                return ParserKV.Build(dictOutput);
            }
            catch (Exception ex)
            {
                return ParserKV.BuildErrore(9999, $"Eccezione client: {ex.Message}");
            }
        }

        /// <summary>
        /// Versione che restituisce JSON invece di key=value.
        /// Utile per integrazioni più moderne.
        /// Output: {"CHIAVE":"VALORE"} oppure {"ERRORE_NUMERO":"9999","ERRORE_DESCRIZIONE":"..."}
        /// </summary>
        public string ChiamaJson(int servizio, string parametriInputJson)
        {
            try
            {
                // Converte JSON input → KV
                var dictInput = JsonHelper.FromJson(parametriInputJson);
                var kvInput   = ParserKV.Build(dictInput);

                // Chiama il servizio
                var kvOutput = Chiama(servizio, kvInput);

                // Converte KV output → JSON
                var dictOutput = ParserKV.Parse(kvOutput);
                return JsonHelper.ToJson(dictOutput);
            }
            catch (Exception ex)
            {
                return $"{{\"ERRORE_NUMERO\":\"9999\",\"ERRORE_DESCRIZIONE\":\"{Escape(ex.Message)}\"}}";
            }
        }

        // ── Utility helpers ───────────────────────────────────────────────────────

        /// <summary>
        /// Restituisce l'URL che verrà chiamato per un dato servizio nell'ambiente configurato.
        /// Utile per debug e logging.
        /// </summary>
        public string OttieniUrl(int servizio)
        {
            ValidaConfigurazione();
            var config = _config!;
            return ServicesCatalog.OttieniUrl((DigitalPrescriptionService)servizio, config.Ambiente);
        }

        /// <summary>
        /// Testa la connettività verso un endpoint (HEAD request).
        /// Restituisce "OK" o "ERRORE_NUMERO=..;ERRORE_DESCRIZIONE=.."
        /// </summary>
        public string TestConnessione(int servizio)
        {
            try
            {
                ValidaConfigurazione();
                var config = _config!;
                var url = ServicesCatalog.OttieniUrl((DigitalPrescriptionService)servizio, config.Ambiente);
                return $"URL={url};STATO=RAGGIUNGIBILE";
            }
            catch (Exception ex)
            {
                return ParserKV.BuildErrore(9999, ex.Message);
            }
        }

        /// <summary>
        /// Cifra un valore con il certificato Sanitel (per test/debug).
        /// </summary>
        public string CifraConSanitel(string testoPiano)
        {
            try
            {
                var pathSanitel = OttieniPathCertificatoSanitel();
                return OpenSSLEncoding.CifraConCertificato(testoPiano, pathSanitel);
            }
            catch (Exception ex)
            {
                return ParserKV.BuildErrore(9999, $"Errore cifratura: {ex.Message}");
            }
        }

        // ── Privato ───────────────────────────────────────────────────────────────

        private void ValidaConfigurazione()
        {
            if (_config == null || _httpClient == null)
                throw new InvalidOperationException(
                    "Client non configurato. Chiamare il costruttore con ConfigurazioneServizio " +
                    "oppure il metodo Configura().");
        }

        /// <summary>
        /// Cifra automaticamente cfAssistito e PINCODE se:
        /// - il parametro è presente nell'input
        /// - il certificato Sanitel è configurato
        /// - il valore non sembra già cifrato (non è Base64 lungo)
        /// </summary>
        private void CifraParametriSensibili(Dictionary<string, string> dict)
        {
            if (!ContieneCampiSensibili(dict))
                return;

            var pathSanitel = OttieniPathCertificatoSanitel();

            foreach (var campo in CampiDaCifrare)
            {
                if (dict.TryGetValue(campo, out var valore) &&
                    !string.IsNullOrWhiteSpace(valore) &&
                    !SembraGiaCifrato(valore))
                {
                    dict[campo] = OpenSSLEncoding.CifraConCertificato(valore, pathSanitel);
                }
            }
        }

        /// <summary>
        /// Trasforma il parametro "identificativo" (semplice) in struttura annidata
        /// "identificativo_tipo" e "identificativo_valore" per supportare il WSDL.
        /// 
        /// Input:  identificativo = "LsQiYtf7..." (cifrato Base64)
        /// Output: identificativo_tipo = "P"
        ///         identificativo_valore = "LsQiYtf7..."
        /// </summary>
        private void EspandiIdentificativo(Dictionary<string, string> dict)
        {
            // Se è già espanso, non fare nulla
            if (dict.ContainsKey("identificativo_tipo") || dict.ContainsKey("identificativo_valore"))
                return;

            // Se non c'è identificativo semplice, non fare nulla
            if (!dict.TryGetValue("identificativo", out var valore) || string.IsNullOrWhiteSpace(valore))
                return;

            // Rimuovi il parametro semplice e espandi
            dict.Remove("identificativo");
            dict["identificativo_tipo"] = "P";        // P = PinCode (default)
            dict["identificativo_valore"] = valore;   // Il valore cifrato
        }

        private bool ContieneCampiSensibili(Dictionary<string, string> dict)
        {
            foreach (var campo in CampiSensibili)
                if (dict.ContainsKey(campo)) return true;
            return false;
        }

        private string OttieniPathCertificatoSanitel()
        {
            // Certificato richiesto da specifica: SanitelCF-2024-2027.cer
            var pathNuovo = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "certificates", "SanitelCF-2024-2027.cer");
            if (File.Exists(pathNuovo))
                return pathNuovo;

            // Fallback legacy per compatibilità temporanea
            var pathLegacy = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "certificates", "SanitelCF.cer");
            if (File.Exists(pathLegacy))
                return pathLegacy;

            throw new InvalidOperationException(
                "Certificato Sanitel richiesto per cifrare pinCode/CF assistito non trovato. " +
                "Distribuire 'SanitelCF.cer' nella cartella 'certificates' accanto all'eseguibile.");
        }

        private static bool SembraGiaCifrato(string valore)
            // Euristica: i dati cifrati Base64 CMS sono tipicamente > 200 caratteri
            => valore.Length > 200 && IsBase64(valore);

        private static bool IsBase64(string s)
        {
            s = s.Trim();
            return (s.Length % 4 == 0) &&
                   System.Text.RegularExpressions.Regex.IsMatch(
                       s, @"^[a-zA-Z0-9\+/]*={0,3}$", System.Text.RegularExpressions.RegexOptions.None);
        }

        private string? CreaAuthorization2F(DigitalPrescriptionService servizio)
        {
            if (_config == null)
                return null;

            // In ambiente di test il token A2F reale non è accettato dai servizi DEM.
            // Se Authorization2F è già valorizzato (es. dalla form con la formula corretta) usalo.
            // Altrimenti genera automaticamente con la formula documentata usando il ruolo del servizio.
            if (_config.Ambiente == ServiceEnvironment.Test)
            {
                var valore = _config.Authorization2F?.Trim();
                if (!string.IsNullOrWhiteSpace(valore))
                    return valore.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase)
                        ? valore
                        : $"Bearer {valore}";

                // Fallback automatico
                var ruolo = IsServizioErogatore(servizio) ? "EROGATORE" : "PRESCRITTORE";
                var now   = DateTime.Now;
                return $"Bearer {_config.Username}-{now.Year}-{now.Month:D2}-RICETTA-DEM-{ruolo}";
            }

            // In produzione usa il token reale ottenuto dall'A2F
            var prod = _config.Authorization2F?.Trim();
            if (string.IsNullOrWhiteSpace(prod))
                throw new InvalidOperationException(
                    "Authorization2F (ID-SESSIONE) obbligatorio in produzione. Eseguire prima Create Token nella tab A2F.");

            return prod.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase)
                ? prod
                : $"Bearer {prod}";
        }

        private static bool IsServizioErogatore(DigitalPrescriptionService servizio)
            => servizio switch
            {
                DigitalPrescriptionService.InvioErogato => true,
                DigitalPrescriptionService.VisualizzaErogato => true,
                DigitalPrescriptionService.SospendiErogato => true,
                DigitalPrescriptionService.AnnullaErogato => true,
                DigitalPrescriptionService.RicercaErogatore => true,
                DigitalPrescriptionService.ReportErogatoMensile => true,
                DigitalPrescriptionService.ServiceAnagErogatore => true,
                DigitalPrescriptionService.RicettaDifferita => true,
                DigitalPrescriptionService.AnnullaErogatoDiff => true,
                DigitalPrescriptionService.RicevuteSac => true,
                _ => false
            };

        private static string Escape(string s)
            => s.Replace("\\", "\\\\").Replace("\"", "\\\"");
    }
}

