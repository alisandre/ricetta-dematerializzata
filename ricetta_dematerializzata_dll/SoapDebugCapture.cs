using System;
using System.Collections.Generic;

namespace ricetta_dematerializzata_dll.Core
{
    /// <summary>
    /// Classe statica per catturare e memorizzare l'ultima richiesta/risposta SOAP
    /// e i relativi header per scopi di debug dalla UI.
    /// </summary>
    public static class SoapDebugCapture
    {
        public class SoapDebugInfo
        {
            public string? RequestSoapRaw { get; set; }
            public string? ResponseSoapRaw { get; set; }
            public Dictionary<string, string> RequestHeaders { get; set; } = new();
            public Dictionary<string, string> ResponseHeaders { get; set; } = new();
            public int? HttpStatusCode { get; set; }
            public long? ElapsedMilliseconds { get; set; }
            public string? RequestUrl { get; set; }
            public string? RequestSoapAction { get; set; }
            public DateTime? CaptureTime { get; set; }
            public string? ErrorMessage { get; set; }
        }

        private static SoapDebugInfo _lastDebugInfo = new();
        private static readonly object _lockObject = new();

        /// <summary>
        /// Cattura i dettagli della richiesta SOAP
        /// </summary>
        public static void CaptureRequest(
            string url,
            string soapAction,
            string soapEnvelope,
            Dictionary<string, string> headers,
            string? authorization2F = null)
        {
            lock (_lockObject)
            {
                _lastDebugInfo = new SoapDebugInfo
                {
                    RequestUrl = url,
                    RequestSoapAction = soapAction,
                    RequestSoapRaw = soapEnvelope,
                    RequestHeaders = new Dictionary<string, string>(headers),
                    CaptureTime = DateTime.Now
                };

                if (!string.IsNullOrWhiteSpace(authorization2F))
                {
                    _lastDebugInfo.RequestHeaders["Authorization2F"] = authorization2F;
                }
            }
        }

        /// <summary>
        /// Cattura i dettagli della risposta SOAP
        /// </summary>
        public static void CaptureResponse(
            string soapResponse,
            Dictionary<string, string> headers,
            int httpStatusCode,
            long elapsedMilliseconds)
        {
            lock (_lockObject)
            {
                _lastDebugInfo.ResponseSoapRaw = soapResponse;
                _lastDebugInfo.ResponseHeaders = new Dictionary<string, string>(headers);
                _lastDebugInfo.HttpStatusCode = httpStatusCode;
                _lastDebugInfo.ElapsedMilliseconds = elapsedMilliseconds;
            }
        }

        /// <summary>
        /// Cattura un errore durante la richiesta SOAP
        /// </summary>
        public static void CaptureError(string errorMessage)
        {
            lock (_lockObject)
            {
                _lastDebugInfo.ErrorMessage = errorMessage;
            }
        }

        /// <summary>
        /// Ottiene l'ultima informazione di debug catturata
        /// </summary>
        public static SoapDebugInfo GetLastDebugInfo()
        {
            lock (_lockObject)
            {
                return _lastDebugInfo;
            }
        }

        /// <summary>
        /// Formatta l'informazione di debug come testo leggibile
        /// </summary>
        public static string FormatDebugInfo(SoapDebugInfo info)
        {
            var sb = new System.Text.StringBuilder();

            sb.AppendLine("═════════════════════════════════════════════════════════════════════");
            sb.AppendLine("                    SOAP DEBUG CAPTURE");
            sb.AppendLine("═════════════════════════════════════════════════════════════════════");
            sb.AppendLine();

            // Timestamp
            if (info.CaptureTime.HasValue)
            {
                sb.AppendLine($"📅 Capture Time: {info.CaptureTime:yyyy-MM-dd HH:mm:ss.fff}");
                sb.AppendLine();
            }

            // URL e SOAPAction
            if (!string.IsNullOrWhiteSpace(info.RequestUrl))
            {
                sb.AppendLine($"🔗 Request URL: {info.RequestUrl}");
                sb.AppendLine();
            }

            if (!string.IsNullOrWhiteSpace(info.RequestSoapAction))
            {
                sb.AppendLine($"⚙️  SOAPAction: {info.RequestSoapAction}");
                sb.AppendLine();
            }

            // REQUEST HEADERS
            if (info.RequestHeaders.Count > 0)
            {
                sb.AppendLine("─────────────────────────────────────────────────────────────────────");
                sb.AppendLine("📤 REQUEST HEADERS");
                sb.AppendLine("─────────────────────────────────────────────────────────────────────");
                foreach (var header in info.RequestHeaders)
                {
                    // Maschera dati sensibili
                    var value = MaskSensitiveData(header.Key, header.Value);
                    sb.AppendLine($"{header.Key}: {value}");
                }
                sb.AppendLine();
            }

            // REQUEST SOAP
            if (!string.IsNullOrWhiteSpace(info.RequestSoapRaw))
            {
                sb.AppendLine("─────────────────────────────────────────────────────────────────────");
                sb.AppendLine("📤 REQUEST SOAP (RAW)");
                sb.AppendLine("─────────────────────────────────────────────────────────────────────");
                sb.AppendLine(PrettyPrintXml(info.RequestSoapRaw));
                sb.AppendLine();
            }

            // HTTP STATUS
            if (info.HttpStatusCode.HasValue)
            {
                sb.AppendLine("─────────────────────────────────────────────────────────────────────");
                sb.AppendLine($"📊 HTTP Status: {info.HttpStatusCode}");
                if (info.ElapsedMilliseconds.HasValue)
                {
                    sb.AppendLine($"⏱️  Duration: {info.ElapsedMilliseconds} ms");
                }
                sb.AppendLine();
            }

            // RESPONSE HEADERS
            if (info.ResponseHeaders.Count > 0)
            {
                sb.AppendLine("─────────────────────────────────────────────────────────────────────");
                sb.AppendLine("📥 RESPONSE HEADERS");
                sb.AppendLine("─────────────────────────────────────────────────────────────────────");
                foreach (var header in info.ResponseHeaders)
                {
                    var value = MaskSensitiveData(header.Key, header.Value);
                    sb.AppendLine($"{header.Key}: {value}");
                }
                sb.AppendLine();
            }

            // RESPONSE SOAP
            if (!string.IsNullOrWhiteSpace(info.ResponseSoapRaw))
            {
                sb.AppendLine("─────────────────────────────────────────────────────────────────────");
                sb.AppendLine("📥 RESPONSE SOAP (RAW)");
                sb.AppendLine("─────────────────────────────────────────────────────────────────────");
                sb.AppendLine(PrettyPrintXml(info.ResponseSoapRaw));
                sb.AppendLine();
            }

            // ERROR
            if (!string.IsNullOrWhiteSpace(info.ErrorMessage))
            {
                sb.AppendLine("─────────────────────────────────────────────────────────────────────");
                sb.AppendLine("❌ ERROR");
                sb.AppendLine("─────────────────────────────────────────────────────────────────────");
                sb.AppendLine(info.ErrorMessage);
                sb.AppendLine();
            }

            sb.AppendLine("═════════════════════════════════════════════════════════════════════");

            return sb.ToString();
        }

        private static string MaskSensitiveData(string headerName, string value)
        {
            var lowerName = headerName.ToLower();
            if (lowerName.Contains("authorization") || 
                lowerName.Contains("password") || 
                lowerName.Contains("token"))
            {
                if (value.Length > 10)
                    return value.Substring(0, 10) + "*** [MASKED] ***";
                return "*** [MASKED] ***";
            }
            return value;
        }

        private static string PrettyPrintXml(string xml)
        {
            try
            {
                var doc = new System.Xml.XmlDocument();
                doc.LoadXml(xml);
                var settings = new System.Xml.XmlWriterSettings
                {
                    Indent = true,
                    IndentChars = "  ",
                    NewLineOnAttributes = false,
                    OmitXmlDeclaration = true  // ← IMPORTANTE: Rimuove <?xml version="1.0" encoding="utf-16"?>
                };
                var sb = new System.Text.StringBuilder();
                using (var writer = System.Xml.XmlWriter.Create(sb, settings))
                {
                    doc.WriteTo(writer);
                }
                return sb.ToString();
            }
            catch
            {
                // Se il pretty print fallisce, restituisce il raw XML
                return xml;
            }
        }
    }
}
