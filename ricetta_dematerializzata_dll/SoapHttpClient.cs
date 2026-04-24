using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using ricetta_dematerializzata_dll.Models;

namespace ricetta_dematerializzata_dll.Core
{
    /// <summary>
    /// Client HTTP specializzato per i servizi SAC / DEM Ricetta.
    ///
    /// Gestisce:
    /// - Autenticazione Basic (username:password in Base64) come da documentazione
    /// - Certificati SSL di trasporto (demservicetest.pem / demservice.pem)
    /// - Certificato CA (CA Agenzia delle Entrate Test.pem / CAEntrate.pem)
    /// - Header SOAP 1.1 (Content-Type, SOAPAction, Accept-Encoding)
    /// - Keep-Alive e User-Agent come da esempio documentazione
    ///
    /// Come da documentazione immagine:
    ///   POST /DemRicettaPrescrittoServicesWeb/services/demInvioPrescritto HTTP/1.1
    ///   Accept-Encoding: gzip,deflate
    ///   Content-Type: text/xml;charset=UTF-8
    ///   SOAPAction: "http://invioprescritto.wsdl.dem.sanita.finanze.it/InvioPrescritto"
    ///   Authorization: Basic VTFMQXaWT16UDdUV1VLSDQ=
    ///   Connection: Keep-Alive
    ///   User-Agent: Apache-HttpClient/4.1.1 (java 1.5)
    /// </summary>
    internal class SoapHttpClient
    {
        private readonly string _username;
        private readonly string _password;
        private readonly string? _pathCertificatoSsl;
        private readonly string? _pathCertificatoCA;
        private readonly bool _ignoraErroriSsl;

        public SoapHttpClient(
            string username,
            string password,
            string? pathCertificatoSsl = null,
            string? pathCertificatoCA  = null,
            bool ignoraErroriSsl       = false)
        {
            _username          = username;
            _password          = password;
            _pathCertificatoSsl = pathCertificatoSsl;
            _pathCertificatoCA  = pathCertificatoCA;
            _ignoraErroriSsl    = ignoraErroriSsl;
        }

        // ── Esecuzione chiamata SOAP ───────────────────────────────────────────────

        /// <summary>
        /// Invia una richiesta SOAP e restituisce la risposta XML come stringa.
        /// Lancia eccezione in caso di errore HTTP non gestito.
        /// </summary>
        public string ChiamaServizio(string url, string soapAction, string soapEnvelope, string? authorization2F = null)
        {
            ConfiguraSSL();

            var richiesta = CreateHttpWebRequest(url, soapAction, soapEnvelope, authorization2F);

            try
            {
                using var risposta   = (HttpWebResponse)richiesta.GetResponse();
                using var stream     = risposta.GetResponseStream()!;
                using var reader     = new StreamReader(stream, Encoding.UTF8);
                return reader.ReadToEnd();
            }
            catch (WebException ex) when (ex.Response is HttpWebResponse httpErr)
            {
                // Legge il corpo dell'errore (es. SOAP Fault con codice 500)
                using var stream = httpErr.GetResponseStream()!;
                using var reader = new StreamReader(stream, Encoding.UTF8);
                var body = reader.ReadToEnd();

                if (!string.IsNullOrWhiteSpace(body))
                    return body;   // Il chiamante analizza il SOAP Fault

                throw new InvalidOperationException(
                    $"Errore HTTP {(int)httpErr.StatusCode}: {httpErr.StatusDescription}", ex);
            }
        }

        // ── Creazione request con autenticazione preventiva ───────────────────────

        /// <summary>
        /// Crea l'HttpWebRequest con tutti gli header richiesti dalla documentazione,
        /// inclusa l'autenticazione Basic PREVENTIVA (senza attendere il challenge 401).
        ///
        /// Come da immagine documentazione:
        ///   "Per creare un WSclient .NET con autenticazione preventiva si deve usare
        ///    l'oggetto SoapHttpClientProtocol sul quale si deve però assolutamente
        ///    fare un override del metodo GetWebRequest dove viene forzata appunto
        ///    l'autenticazione"
        ///
        /// Qui implementiamo lo stesso concetto su HttpWebRequest direttamente.
        /// </summary>
        private HttpWebRequest CreateHttpWebRequest(string url, string soapAction, string soapEnvelope, string? authorization2F = null)
        {
            var req = (HttpWebRequest)WebRequest.Create(url);

            // ── Metodo e protocollo ──────────────────────────────────────────────
            req.Method      = "POST";
            req.KeepAlive   = true;

            // ── Autenticazione Basic PREVENTIVA ──────────────────────────────────
            // Non usare req.Credentials (attende 401 challenge)
            // Impostare direttamente l'header Authorization come da documentazione
            var credenziali = Convert.ToBase64String(
                Encoding.UTF8.GetBytes($"{_username}:{_password}"));
            req.Headers["Authorization"] = $"Basic {credenziali}";

            if (!string.IsNullOrWhiteSpace(authorization2F))
                req.Headers["Authorization2F"] = authorization2F;

            // ── Header SOAP obbligatori (da documentazione) ───────────────────────
            req.ContentType = "text/xml;charset=UTF-8";
            req.Headers["SOAPAction"] = $"\"{soapAction}\"";
            req.Headers["Accept-Encoding"] = "gzip,deflate";

            // ── User-Agent come da esempio documentazione ─────────────────────────
            req.UserAgent = "Apache-HttpClient/4.1.1 (java 1.5)";

            // ── Gestione decompressione automatica (gzip/deflate) ─────────────────
            req.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            // ── Timeout (30 secondi, configurabile) ───────────────────────────────
            req.Timeout      = 30_000;
            req.ReadWriteTimeout = 30_000;

            // ── Certificato client SSL (se configurato) ────────────────────────────
            if (!string.IsNullOrEmpty(_pathCertificatoSsl) && File.Exists(_pathCertificatoSsl))
            {
                var cert = new X509Certificate2(_pathCertificatoSsl);
                req.ClientCertificates.Add(cert);
            }

            // ── Body SOAP ────────────────────────────────────────────────────────
            var bodyBytes = Encoding.UTF8.GetBytes(soapEnvelope);
            req.ContentLength = bodyBytes.Length;

            using var stream = req.GetRequestStream();
            stream.Write(bodyBytes, 0, bodyBytes.Length);

            return req;
        }

        // ── Configurazione SSL ────────────────────────────────────────────────────

        private void ConfiguraSSL()
        {
            if (_ignoraErroriSsl)
            {
                // Solo per ambienti di test/sviluppo!
                ServicePointManager.ServerCertificateValidationCallback =
                    (sender, cert, chain, errors) => true;
                return;
            }

            if (!string.IsNullOrEmpty(_pathCertificatoCA) && File.Exists(_pathCertificatoCA))
            {
                ServicePointManager.ServerCertificateValidationCallback =
                    ValidaConCA;
            }

            // TLS 1.2 minimo (richiesto da ambienti sanitari)
            ServicePointManager.SecurityProtocol =
                SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;
        }

        private bool ValidaConCA(
            object sender,
            X509Certificate? certificate,
            X509Chain? chain,
            SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors == SslPolicyErrors.None) return true;

            try
            {
                var ca = new X509Certificate2(_pathCertificatoCA!);
                var chainPersonalizzata = new X509Chain();
                chainPersonalizzata.ChainPolicy.ExtraStore.Add(ca);
                chainPersonalizzata.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck;
                chainPersonalizzata.ChainPolicy.VerificationFlags =
                    X509VerificationFlags.AllowUnknownCertificateAuthority;

                if (certificate is X509Certificate2 cert2)
                    return chainPersonalizzata.Build(cert2);

                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
