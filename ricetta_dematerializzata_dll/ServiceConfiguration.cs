using System.IO;
using ricetta_dematerializzata_dll.Models;

namespace ricetta_dematerializzata_dll.Core
{
    /// <summary>
    /// Configurazione per l'accesso ai servizi SAC / DEM Ricetta.
    /// Istanziare una volta sola e passare a SanitaServiceClient.
    /// </summary>
    public class ServiceConfiguration
    {
        // ── Credenziali ───────────────────────────────────────────────────────────

        /// <summary>Username per l'autenticazione Basic HTTP</summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>Password per l'autenticazione Basic HTTP</summary>
        public string Password { get; set; } = string.Empty;

        // ── Ambiente ──────────────────────────────────────────────────────────────

        /// <summary>Test o Produzione (default: Test)</summary>
        public ServiceEnvironment Ambiente { get; set; } = ServiceEnvironment.Test;

        // ── Certificati SSL ───────────────────────────────────────────────────────

        /// <summary>
        /// Path del certificato SSL di trasporto.
        /// Test:       demservicetest.pem
        /// Produzione: demservice.pem
        /// </summary>
        public string? PathCertificatoSsl { get; set; }

        /// <summary>
        /// Path del certificato CA (Certification Authority).
        /// Test:       CA Agenzia delle Entrate Test.pem
        /// Produzione: CAEntrate.pem
        /// </summary>
        public string? PathCertificatoCA { get; set; }

        // ── Certificato Sanitel (cifratura CF e pincode) ──────────────────────────

        /// <summary>
        /// Path del file Sanitel.cer usato per cifrare CF e pincode dell'assistito.
        /// Presente nel pacchetto demPrescritto.zip → SanitelCF.rar
        /// </summary>
        public string? PathCertificatoSanitel { get; set; }

        // ── Opzioni avanzate ──────────────────────────────────────────────────────

        /// <summary>
        /// Se true, ignora gli errori SSL (solo per sviluppo/test locale!).
        /// Non usare mai in produzione.
        /// </summary>
        public bool IgnoraErroriSsl { get; set; } = false;

        /// <summary>Timeout in millisecondi per ogni chiamata (default: 30.000 ms)</summary>
        public int TimeoutMs { get; set; } = 30_000;

        // ── Namespace SOAP ────────────────────────────────────────────────────────

        /// <summary>
        /// Namespace SOAP usato per le operazioni (default da documentazione).
        /// Modificare solo se il WSDL specifica un namespace diverso.
        /// </summary>
        public string NamespaceSoap { get; set; } =
            "http://invioprescritto.wsdl.dem.sanita.finanze.it";

        // ── Factory methods ───────────────────────────────────────────────────────

        /// <summary>
        /// Crea una configurazione di test rapida con SSL ignorato.
        /// SOLO per sviluppo locale, mai in produzione.
        /// </summary>
        public static ServiceConfiguration CreaTest(string username, string password)
            => new ServiceConfiguration
            {
                Username       = username,
                Password       = password,
                Ambiente       = ServiceEnvironment.Test,
                IgnoraErroriSsl = true
            };

        /// <summary>
        /// Crea una configurazione di produzione con i certificati specificati.
        /// </summary>
        public static ServiceConfiguration CreaProduzione(
            string username,
            string password,
            string pathCertSsl,
            string pathCertCA,
            string pathSanitel)
            => new ServiceConfiguration
            {
                Username               = username,
                Password               = password,
                Ambiente               = ServiceEnvironment.Produzione,
                PathCertificatoSsl     = pathCertSsl,
                PathCertificatoCA      = pathCertCA,
                PathCertificatoSanitel = pathSanitel,
                IgnoraErroriSsl        = false
            };

        // ── Validazione ───────────────────────────────────────────────────────────

        /// <summary>
        /// Verifica la coerenza della configurazione.
        /// Lancia ArgumentException se mancano parametri obbligatori.
        /// </summary>
        public void Valida()
        {
            if (string.IsNullOrWhiteSpace(Username))
                throw new System.ArgumentException("Username obbligatorio.", nameof(Username));
            if (string.IsNullOrWhiteSpace(Password))
                throw new System.ArgumentException("Password obbligatoria.", nameof(Password));

            if (Ambiente == ServiceEnvironment.Produzione && !IgnoraErroriSsl)
            {
                if (string.IsNullOrEmpty(PathCertificatoSsl) || !File.Exists(PathCertificatoSsl))
                    throw new System.ArgumentException(
                        "In produzione è richiesto il certificato SSL.", nameof(PathCertificatoSsl));

                if (string.IsNullOrEmpty(PathCertificatoCA) || !File.Exists(PathCertificatoCA))
                    throw new System.ArgumentException(
                        "In produzione è richiesto il certificato CA.", nameof(PathCertificatoCA));
            }
        }

        // ── Authorization2F ─────────────────────────────────────────────────────────

        /// <summary>
        /// Valore dell'header Authorization2F.
        /// Accetta sia "Bearer <ID-SESSIONE>" sia solo "<ID-SESSIONE>".
        /// </summary>
        public string? Authorization2F { get; set; }
    }
}
