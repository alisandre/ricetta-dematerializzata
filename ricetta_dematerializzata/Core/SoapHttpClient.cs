using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using ricetta_dematerializzata.Models;

namespace ricetta_dematerializzata.Core
{
    /// <summary>
    /// Client HTTP base per i servizi SAC / DEM Ricetta.
    /// Invia envelope SOAP raw via HttpWebRequest con autenticazione Basic pre-emptive.
    /// Header sul wire nell'ordine esatto del Raw SoapUI (Apache HttpClient 4.5.5).
    /// </summary>
    internal class SoapHttpClient
    {
        private readonly string _username;
        private readonly string _password;
        private readonly string? _serialeCertificatoSsl;
        private readonly string? _pathCertificatoCA;
        private readonly bool _ignoraErroriSsl;
        private readonly ServiceEnvironment _ambiente;

        public SoapHttpClient(
            string username,
            string password,
            string? serialeCertificatoSsl = null,
            string? pathCertificatoCA  = null,
            bool ignoraErroriSsl       = false,
            ServiceEnvironment ambiente = ServiceEnvironment.Test)
        {
            _username              = username;
            _password              = password;
            _serialeCertificatoSsl = serialeCertificatoSsl;
            _pathCertificatoCA     = pathCertificatoCA;
            _ignoraErroriSsl       = ignoraErroriSsl;
            _ambiente              = ambiente;
        }

        public string ChiamaServizio(string url, string soapAction, string soapEnvelope, string? authorization2F = null, bool usaSslClientCert = false, bool usaValidazioneCA = false)
        {
            // TLS 1.2 + disabilita Expect:100-continue globalmente
            ServicePointManager.SecurityProtocol =
                SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            ServicePointManager.Expect100Continue = false;

            if (_ignoraErroriSsl)
                ServicePointManager.ServerCertificateValidationCallback = (s, c, ch, e) => true;
            else if (usaValidazioneCA && _ambiente == ServiceEnvironment.Produzione &&
                     !string.IsNullOrEmpty(_pathCertificatoCA) && File.Exists(_pathCertificatoCA))
                ServicePointManager.ServerCertificateValidationCallback = ValidaConCA;

            SoapServiceInterceptor.LogSoapRequest("ChiamaServizio", soapEnvelope, url, soapAction);
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            var utf8NoBom  = new UTF8Encoding(false);
            var bodyBytes  = utf8NoBom.GetBytes(soapEnvelope);
            var credenziali = Convert.ToBase64String(utf8NoBom.GetBytes($"{_username}:{_password}"));

            var req = (HttpWebRequest)WebRequest.Create(url);
            req.Method        = "POST";
            req.KeepAlive     = true;
            req.ContentType   = "text/xml;charset=UTF-8";
            req.ContentLength = bodyBytes.Length;
            req.UserAgent     = "Apache-HttpClient/4.5.5 (Java/17.0.12)";
            req.ServicePoint.Expect100Continue = false;

            // Ordine header identico al Raw SoapUI:
            // Accept-Encoding, Content-Type*, SOAPAction, Authorization, User-Agent*, Host*, Content-Length*, Connection*
            // (* gestiti automaticamente da HttpWebRequest)
            req.Headers["Accept-Encoding"] = "gzip,deflate";
            req.Headers["SOAPAction"]      = $"\"{soapAction}\"";
            req.Headers["Authorization"]   = $"Basic {credenziali}";

            if (!string.IsNullOrWhiteSpace(authorization2F))
                req.Headers["Authorization2F"] = authorization2F;

            // Certificato client SSL solo se il servizio lo richiede (usaSslClientCert = true)
            if (usaSslClientCert)
            {
                var certClienteSsl = CaricaCertificatoClient();
                if (certClienteSsl != null)
                    req.ClientCertificates.Add(certClienteSsl);
            }

            using (var s = req.GetRequestStream())
                s.Write(bodyBytes, 0, bodyBytes.Length);

            // Debug capture con header reali
            var requestHeaders = new Dictionary<string, string>();
            foreach (string h in req.Headers.Keys)
                requestHeaders[h] = req.Headers[h] ?? string.Empty;
            requestHeaders["Content-Type"]   = req.ContentType ?? string.Empty;
            requestHeaders["Content-Length"] = req.ContentLength.ToString();
            requestHeaders["User-Agent"]     = req.UserAgent ?? string.Empty;
            requestHeaders["Method"]         = req.Method;
            SoapDebugCapture.CaptureRequest(url, soapAction, soapEnvelope, requestHeaders, authorization2F);

            try
            {
                using var risposta = (HttpWebResponse)req.GetResponse();
                var body = LeggiStream(risposta.GetResponseStream()!, risposta.ContentEncoding, utf8NoBom);
                stopwatch.Stop();

                var responseHeaders = new Dictionary<string, string>();
                foreach (string h in risposta.Headers.Keys)
                    responseHeaders[h] = risposta.Headers[h] ?? string.Empty;

                SoapDebugCapture.CaptureResponse(body, responseHeaders, (int)risposta.StatusCode, stopwatch.ElapsedMilliseconds);
                SoapServiceInterceptor.LogSoapResponse("ChiamaServizio", body, stopwatch.ElapsedMilliseconds, (int)risposta.StatusCode);
                return body;
            }
            catch (WebException ex) when (ex.Response is HttpWebResponse httpErr)
            {
                stopwatch.Stop();
                var body = LeggiStream(httpErr.GetResponseStream()!, httpErr.ContentEncoding, utf8NoBom);

                var responseHeaders = new Dictionary<string, string>();
                foreach (string h in httpErr.Headers.Keys)
                    responseHeaders[h] = httpErr.Headers[h] ?? string.Empty;

                SoapDebugCapture.CaptureResponse(body, responseHeaders, (int)httpErr.StatusCode, stopwatch.ElapsedMilliseconds);
                SoapDebugCapture.CaptureError($"HTTP {(int)httpErr.StatusCode}: {httpErr.StatusDescription}");
                SoapServiceInterceptor.LogSoapError("ChiamaServizio", ex, soapEnvelope, (int)httpErr.StatusCode);

                if (!string.IsNullOrWhiteSpace(body)) return body;
                throw new InvalidOperationException($"Errore HTTP {(int)httpErr.StatusCode}: {httpErr.StatusDescription}", ex);
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                SoapDebugCapture.CaptureError($"Exception: {ex.Message}");
                SoapServiceInterceptor.LogSoapError("ChiamaServizio", ex, soapEnvelope);
                throw;
            }
        }

        private static string LeggiStream(Stream baseStream, string? contentEncoding, UTF8Encoding enc)
        {
            var encoding = contentEncoding?.ToLowerInvariant() ?? "";
            Stream readStream;
            if (encoding.Contains("gzip"))
                readStream = new System.IO.Compression.GZipStream(baseStream, System.IO.Compression.CompressionMode.Decompress);
            else if (encoding.Contains("deflate"))
                readStream = new System.IO.Compression.DeflateStream(baseStream, System.IO.Compression.CompressionMode.Decompress);
            else
                readStream = baseStream;

            using (readStream)
            using (var reader = new StreamReader(readStream, enc))
                return reader.ReadToEnd();
        }

        private X509Certificate2? CaricaCertificatoClient()
        {
            try
            {
                if (!string.IsNullOrEmpty(_serialeCertificatoSsl))
                {
                    var store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
                    store.Open(OpenFlags.ReadOnly);
                    try
                    {
                        var certs = store.Certificates.Find(X509FindType.FindBySerialNumber, _serialeCertificatoSsl, false);
                        if (certs.Count > 0)
                            return certs[0];
                    }
                    finally
                    {
                        store.Close();
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                SoapDebugCapture.CaptureError($"Errore caricamento certificato client: {ex.Message}");
                return null;
            }
        }

        private bool ValidaConCA(object sender, X509Certificate? certificate, X509Chain? chain, SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors == SslPolicyErrors.None) return true;
            try
            {
                var ca = new X509Certificate2(_pathCertificatoCA!);
                var chainCustom = new X509Chain();
                chainCustom.ChainPolicy.ExtraStore.Add(ca);
                chainCustom.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck;
                chainCustom.ChainPolicy.VerificationFlags = X509VerificationFlags.AllowUnknownCertificateAuthority;
                return certificate is X509Certificate2 cert2 && chainCustom.Build(cert2);
            }
            catch { return false; }
        }
    }
}
