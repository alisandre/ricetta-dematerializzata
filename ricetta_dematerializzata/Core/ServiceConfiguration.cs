using System;
using System.IO;
using ricetta_dematerializzata.Models;

namespace ricetta_dematerializzata.Core
{
    /// <summary>
    /// Configurazione per l'accesso ai servizi SAC / DEM Ricetta.
    /// Istanziare una volta sola e passare a RicettaDematerializzataBaseClient o RicettaDematerializzataClient.
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
        /// Seriale del certificato SSL di trasporto nel Windows Certificate Store (LOCAL_MACHINE\Personal).
        /// Se impostato, viene usato per autenticazione SSL client.
        /// Se non impostato, viene usata solo la Basic Auth.
        /// Formato: stringa esadecimale (es. "F0B8C2D1E5A3F9B6C2D1E5A3F9B6C2D")
        /// </summary>
        public string? SerialeCertificatoSsl { get; set; }

        // ── Opzioni avanzate ──────────────────────────────────────────────────────

        /// <summary>Timeout in millisecondi per ogni chiamata (default: 30.000 ms)</summary>
        public int TimeoutMs { get; set; } = 30_000;

        // ── Namespace SOAP ────────────────────────────────────────────────────────

        /// <summary>
        /// Namespace SOAP usato per le operazioni (default da documentazione).
        /// Modificare solo se il WSDL specifica un namespace diverso.
        /// </summary>
        public string NamespaceSoap { get; set; } =
            "http://invioprescritto.wsdl.dem.sanita.finanze.it";

        /// <summary>
        /// Restituisce true se la validazione SSL deve essere disabilitata.
        /// In ambiente Test è automaticamente true; in Produzione è sempre false.
        /// </summary>
        internal bool IgnoraErroriSsl => Ambiente == ServiceEnvironment.Test;

        // ── Factory methods ───────────────────────────────────────────────────────

        /// <summary>
        /// Crea una configurazione di test.
        /// </summary>
        public static ServiceConfiguration CreaTest(string username, string password)
            => new ServiceConfiguration
            {
                Username = username,
                Password = password,
                Ambiente = ServiceEnvironment.Test,
            };

        /// <summary>
        /// Crea una configurazione di produzione con il certificato SSL specificato tramite seriale.
        /// Se il seriale non è impostato, viene usata solo la Basic Auth.
        /// I certificati CA sono risolti automaticamente dalla cartella certificates.
        /// </summary>
        public static ServiceConfiguration CreaProduzione(
            string username,
            string password,
            string? serialeCertSsl = null)
            => new ServiceConfiguration
            {
                Username              = username,
                Password              = password,
                Ambiente              = ServiceEnvironment.Produzione,
                SerialeCertificatoSsl = serialeCertSsl,
            };

        // ── Validazione ───────────────────────────────────────────────────────────

        /// <summary>
        /// Verifica la coerenza della configurazione.
        /// Lancia ArgumentException se mancano parametri obbligatori.
        /// </summary>
        public void Valida()
        {
            if (string.IsNullOrWhiteSpace(Username))
                throw new ArgumentException("Username obbligatorio.", nameof(Username));
            if (string.IsNullOrWhiteSpace(Password))
                throw new ArgumentException("Password obbligatoria.", nameof(Password));

            if (Ambiente == ServiceEnvironment.Produzione)
            {
                var pathCa = RisolviPathCertificatoCA();
                if (!File.Exists(pathCa))
                    throw new ArgumentException(
                        $"In produzione è richiesto il certificato CA nella cartella certificates: {pathCa}");
            }
        }

        public string RisolviPathCertificatoCA()
        {
            var fileName = Ambiente == ServiceEnvironment.Produzione
                ? "ActalisCA.cert"
                : "demservicetest.cert";

            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "certificates", fileName);
        }

        // ── Authorization2F ─────────────────────────────────────────────────────────

        /// <summary>
        /// Valore dell'header Authorization2F.
        /// Accetta sia "Bearer <ID-SESSIONE>" sia solo "<ID-SESSIONE>".
        /// </summary>
        public string? Authorization2F { get; set; }
    }
}
