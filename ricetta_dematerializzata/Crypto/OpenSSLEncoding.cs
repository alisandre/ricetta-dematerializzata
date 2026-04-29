using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace ricetta_dematerializzata.Crypto
{
    /// <summary>
    /// Gestisce la cifratura con OpenSSL/CMS richiesta dal servizio SAC.
    ///
    /// Dalla documentazione:
    ///   "Il codice fiscale e il pincode devono essere cifrati con la chiave pubblica
    ///    RSA contenuta nel certificato X.509 fornito dal MEF, applicando il padding
    ///    PKCS#1 v1.5, e codificati in Base64 (RFC 1521)."
    /// </summary>
    public static class OpenSSLEncoding
    {
        // ── Cifra con RSA PKCS#1 v1.5 (metodo richiesto dalla specifica MEF) ─────

        /// <summary>
        /// Estrae la chiave pubblica RSA dal certificato X.509, cifra il testo in chiaro
        /// con padding PKCS#1 v1.5 e restituisce il risultato codificato in Base64.
        /// </summary>
        public static string CifraConCertificato(string testoPianoUTF8, string pathCertificato)
        {
            if (!File.Exists(pathCertificato))
                throw new FileNotFoundException(
                    $"Certificato Sanitel non trovato: {pathCertificato}", pathCertificato);

            var certBytes = File.ReadAllBytes(pathCertificato);
            return CifraConCertificato(testoPianoUTF8, certBytes);
        }

        /// <summary>
        /// Overload che accetta i byte del certificato.
        /// </summary>
        public static string CifraConCertificato(string testoPianoUTF8, byte[] certificatoBytes)
        {
            var cert = new X509Certificate2(certificatoBytes);
            return CifraConRsaPkcs1(Encoding.UTF8.GetBytes(testoPianoUTF8), cert);
        }

        // ── Implementazione interna ───────────────────────────────────────────────

        private static string CifraConRsaPkcs1(byte[] datiChiari, X509Certificate2 cert)
        {
            using (var rsa = cert.PublicKey.Key as RSACryptoServiceProvider)
            {
                if (rsa == null)
                    throw new InvalidOperationException(
                        "Il certificato non contiene una chiave pubblica RSA.");

                // fOAEP = false → PKCS#1 v1.5 padding
                var cifrato = rsa.Encrypt(datiChiari, fOAEP: false);
                return Convert.ToBase64String(cifrato);
            }
        }

        // ── Verifica disponibilità certificato ────────────────────────────────────

        /// <summary>
        /// Verifica che il file del certificato esista e sia leggibile.
        /// </summary>
        public static bool VerificaCertificato(string pathCertificato, out string messaggio)
        {
            try
            {
                if (!File.Exists(pathCertificato))
                {
                    messaggio = $"File non trovato: {pathCertificato}";
                    return false;
                }

                var cert = new X509Certificate2(File.ReadAllBytes(pathCertificato));
                messaggio = $"Certificato valido. Subject: {cert.Subject}, Scadenza: {cert.GetExpirationDateString()}";
                return true;
            }
            catch (Exception ex)
            {
                messaggio = $"Errore lettura certificato: {ex.Message}";
                return false;
            }
        }
    }
}
