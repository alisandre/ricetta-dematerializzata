using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace ricetta_dematerializzata_dll.Crypto
{
    /// <summary>
    /// Gestisce la cifratura con OpenSSL/CMS richiesta dal servizio SAC.
    ///
    /// Dalla documentazione:
    ///   "Nella request il pincode e il CF dell'assistito devono essere cifrati
    ///    con OPEN SSL utilizzando il certificato Sanitel.cer"
    ///
    /// Implementazione: CMS EnvelopedData (equivalente a:
    ///   openssl cms -encrypt -in plain.txt -out enc.b64 -outform PEM
    ///               -recip Sanitel.cer -aes256
    /// )
    /// </summary>
    public static class OpenSSLEncoding
    {
        // ── Cifra con certificato (CMS/PKCS#7 EnvelopedData) ─────────────────────

        /// <summary>
        /// Cifra il testo in chiaro con il certificato Sanitel.cer e restituisce
        /// il risultato in Base64 (formato PEM senza header/footer).
        ///
        /// Il file del certificato può essere passato come path o come bytes.
        /// </summary>
        public static string CifraConCertificato(string testoPianoUTF8, byte[] certificatoBytes)
        {
            var certX509 = new X509Certificate2(certificatoBytes);
            return CifraInternally(Encoding.UTF8.GetBytes(testoPianoUTF8), certX509);
        }

        /// <summary>
        /// Overload che accetta il path del file .cer
        /// </summary>
        public static string CifraConCertificato(string testoPianoUTF8, string pathCertificato)
        {
            if (!File.Exists(pathCertificato))
                throw new FileNotFoundException(
                    $"Certificato Sanitel non trovato: {pathCertificato}", pathCertificato);

            var certBytes = File.ReadAllBytes(pathCertificato);
            return CifraConCertificato(testoPianoUTF8, certBytes);
        }

        // ── Implementazione interna ───────────────────────────────────────────────

        private static string CifraInternally(byte[] datiChiari, X509Certificate2 cert)
        {
            // Usa EnvelopedCms (PKCS#7/CMS) nativo .NET — equivalente a OpenSSL CMS
            var contenuto = new System.Security.Cryptography.Pkcs.ContentInfo(datiChiari);
            var cms = new System.Security.Cryptography.Pkcs.EnvelopedCms(contenuto);

            var destinatari = new System.Security.Cryptography.Pkcs.CmsRecipientCollection(
                new System.Security.Cryptography.Pkcs.CmsRecipient(cert));

            cms.Encrypt(destinatari);

            // Restituisce Base64 (DER encoded EnvelopedData)
            return Convert.ToBase64String(cms.Encode());
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
