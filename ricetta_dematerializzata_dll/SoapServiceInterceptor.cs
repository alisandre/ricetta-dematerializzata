using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;

namespace ricetta_dematerializzata_dll.Core
{
    /// <summary>
    /// Interceptor per loggare le richieste e risposte SOAP con timestamp e timing.
    /// Utile per debug e test su SoapUI.
    /// </summary>
    internal static class SoapServiceInterceptor
    {
        private static readonly object _syncLock = new object();

        /// <summary>
        /// Crea la directory Logs/SOAP se non esiste e ritorna il percorso
        /// </summary>
        private static string GetSoapLogDirectory()
        {
            var logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs", "SOAP");
            if (!Directory.Exists(logPath))
            {
                try
                {
                    Directory.CreateDirectory(logPath);
                }
                catch { /* Ignora errori di creazione */ }
            }
            return logPath;
        }

        /// <summary>
        /// Log della richiesta SOAP prima dell'invio
        /// </summary>
        public static void LogSoapRequest(string methodName, string soapEnvelope, string url, string soapAction)
        {
            lock (_syncLock)
            {
                try
                {
                    var timestamp = DateTime.UtcNow;
                    var separator = new string('=', 80);

                    var logText = new StringBuilder();
                    logText.AppendLine();
                    logText.AppendLine(separator);
                    logText.AppendLine($"[{timestamp:yyyy-MM-dd HH:mm:ss.fff}] INIZIO RICHIESTA SOAP: {methodName}");
                    logText.AppendLine(separator);
                    logText.AppendLine($"URL: {url}");
                    logText.AppendLine($"SOAPAction: {soapAction}");
                    logText.AppendLine();
                    logText.AppendLine("ENVELOPE SOAP:");
                    logText.AppendLine("---------");
                    logText.AppendLine(FormatXml(soapEnvelope));
                    logText.AppendLine("---------");
                    logText.AppendLine();

                    System.Diagnostics.Debug.WriteLine(logText.ToString());
                    Console.WriteLine(logText.ToString());

                    SaveSoapEnvelopeToFile(
                        $"01_Request_{methodName}_{timestamp:yyyyMMdd_HHmmss_fff}",
                        soapEnvelope);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Errore nel logging SOAP Request: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Log della risposta SOAP dopo la ricezione
        /// </summary>
        public static void LogSoapResponse(
            string methodName,
            string soapEnvelope,
            long elapsedMilliseconds,
            int httpStatusCode)
        {
            lock (_syncLock)
            {
                try
                {
                    var timestamp = DateTime.UtcNow;
                    var separator = new string('=', 80);

                    var logText = new StringBuilder();
                    logText.AppendLine();
                    logText.AppendLine(separator);
                    logText.AppendLine($"[{timestamp:yyyy-MM-dd HH:mm:ss.fff}] RISPOSTA RICEVUTA: {methodName}");
                    logText.AppendLine(separator);
                    logText.AppendLine($"HTTP Status Code: {httpStatusCode}");
                    logText.AppendLine($"Tempo elaborazione: {elapsedMilliseconds} ms");
                    logText.AppendLine();
                    logText.AppendLine("ENVELOPE SOAP:");
                    logText.AppendLine("---------");
                    logText.AppendLine(FormatXml(soapEnvelope));
                    logText.AppendLine("---------");
                    logText.AppendLine();

                    System.Diagnostics.Debug.WriteLine(logText.ToString());
                    Console.WriteLine(logText.ToString());

                    SaveSoapEnvelopeToFile(
                        $"02_Response_{methodName}_{timestamp:yyyyMMdd_HHmmss_fff}",
                        soapEnvelope);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Errore nel logging SOAP Response: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Log degli errori SOAP
        /// </summary>
        public static void LogSoapError(
            string methodName,
            Exception exception,
            string? soapEnvelope = null,
            int? httpStatusCode = null)
        {
            lock (_syncLock)
            {
                try
                {
                    var timestamp = DateTime.UtcNow;
                    var separator = new string('=', 80);

                    var logText = new StringBuilder();
                    logText.AppendLine();
                    logText.AppendLine(separator);
                    logText.AppendLine($"[{timestamp:yyyy-MM-dd HH:mm:ss.fff}] ERRORE NELLA RICHIESTA SOAP: {methodName}");
                    logText.AppendLine(separator);

                    if (httpStatusCode.HasValue)
                        logText.AppendLine($"HTTP Status Code: {httpStatusCode}");

                    logText.AppendLine($"Eccezione: {exception.GetType().Name}");
                    logText.AppendLine($"Messaggio: {exception.Message}");
                    logText.AppendLine();
                    logText.AppendLine("Stack Trace:");
                    logText.AppendLine(exception.StackTrace);
                    logText.AppendLine();

                    if (!string.IsNullOrEmpty(soapEnvelope))
                    {
                        logText.AppendLine("ENVELOPE RICHIESTA CHE HA CAUSATO L'ERRORE:");
                        logText.AppendLine("---------");
                        logText.AppendLine(FormatXml(soapEnvelope));
                        logText.AppendLine("---------");
                        logText.AppendLine();
                    }

                    logText.AppendLine(separator);

                    System.Diagnostics.Debug.WriteLine(logText.ToString());
                    Console.WriteLine(logText.ToString());

                    if (!string.IsNullOrEmpty(soapEnvelope))
                    {
                        SaveSoapEnvelopeToFile(
                            $"03_Error_{methodName}_{timestamp:yyyyMMdd_HHmmss_fff}",
                            soapEnvelope);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Errore nel logging SOAP Error: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Formatta XML con indentazione per migliorare la leggibilità
        /// </summary>
        private static string FormatXml(string xml)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(xml))
                    return xml;

                var doc = new XmlDocument();
                doc.LoadXml(xml);

                using var stringWriter = new StringWriter();
                using var xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings
                {
                    Indent = true,
                    IndentChars = "  ",
                    NewLineChars = Environment.NewLine,
                    Encoding = Encoding.UTF8
                });

                doc.WriteTo(xmlWriter);
                xmlWriter.Flush();

                return stringWriter.ToString();
            }
            catch
            {
                // Se il formatting fallisce, ritorna l'XML originale
                return xml;
            }
        }

        /// <summary>
        /// Salva l'XML su file per consultazione successiva su SoapUI
        /// </summary>
        private static void SaveSoapEnvelopeToFile(string filePrefix, string soapEnvelope)
        {
            try
            {
                var logPath = GetSoapLogDirectory();
                var fileName = $"{filePrefix}.xml";
                var filePath = Path.Combine(logPath, fileName);

                var formattedXml = FormatXml(soapEnvelope);
                File.WriteAllText(filePath, formattedXml, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Impossibile salvare SOAP envelope su file: {ex.Message}");
            }
        }
    }
}
