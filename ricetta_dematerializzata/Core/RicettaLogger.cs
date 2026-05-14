using System;
using System.IO;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace ricetta_dematerializzata.Core
{
    /// <summary>
    /// Logger centralizzato per tutte le chiamate della libreria ricetta_dematerializzata.
    /// Scrive in Log\Serilog\RicettaDematerializzata_yyyyMMdd.log con rotazione giornaliera.
    /// Livello minimo configurabile via Configura(..., logLevel).
    /// </summary>
    internal static class RicettaLogger
    {
        private static ILogger? _log;
        private static readonly object _initLock = new object();
        private static readonly LoggingLevelSwitch _levelSwitch = new LoggingLevelSwitch(LogEventLevel.Error);

        /// <summary>
        /// Restituisce l'istanza del logger, inizializzandola al primo accesso.
        /// </summary>
        private static ILogger Log
        {
            get
            {
                if (_log != null) return _log;
                lock (_initLock)
                {
                    if (_log != null) return _log;
                    _log = BuildLogger();
                }
                return _log;
            }
        }

        private static ILogger BuildLogger()
        {
            var logDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log", "Serilog");

            return new LoggerConfiguration()
                .MinimumLevel.ControlledBy(_levelSwitch)
                .WriteTo.File(
                    path: Path.Combine(logDir, "RicettaDematerializzata_.log"),
                    rollingInterval: RollingInterval.Day,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                    encoding: System.Text.Encoding.UTF8,
                    shared: true,
                    retainedFileCountLimit: 90)
                .CreateLogger();
        }

        public static void ConfiguraLivello(string? logLevel)
        {
            var livello = ParseLogLevel(logLevel);
            _levelSwitch.MinimumLevel = livello;
            Log.Information("LOG LEVEL impostato a {LogLevel}", livello);
        }

        private static LogEventLevel ParseLogLevel(string? logLevel)
        {
            if (string.IsNullOrWhiteSpace(logLevel))
                return LogEventLevel.Error;

            return Enum.TryParse(logLevel.Trim(), ignoreCase: true, out LogEventLevel level)
                ? level
                : LogEventLevel.Error;
        }

        // ── API di logging ────────────────────────────────────────────────────────

        /// <summary>
        /// Logga l'inizio di una chiamata al servizio con i parametri in input.
        /// </summary>
        public static void LogChiamata(string servizio, string applicazione, string ambiente, string inputCamel, string inputUpperCase)
        {
            Log.Information(
                "CHIAMATA | Servizio={Servizio} Applicazione={Applicazione} Ambiente={Ambiente}",
                servizio, applicazione, ambiente);

            Log.Debug(
                "INPUT camelCase  : {Input}", inputCamel);

            Log.Debug(
                "INPUT MAIUSCOLO  : {Input}", inputUpperCase);
        }

        /// <summary>
        /// Logga la risposta ricevuta dal servizio.
        /// </summary>
        public static void LogRisposta(string servizio, string applicazione, string outputCamel, string outputUpperCase, long elapsedMs)
        {
            Log.Information(
                "RISPOSTA | Servizio={Servizio} Applicazione={Applicazione} DurataMs={DurataMs}",
                servizio, applicazione, elapsedMs);

            Log.Debug(
                "OUTPUT camelCase : {Output}", outputCamel);

            Log.Debug(
                "OUTPUT MAIUSCOLO : {Output}", outputUpperCase);
        }

        /// <summary>
        /// Logga la richiesta SOAP grezza (envelope XML + URL + SOAPAction).
        /// </summary>
        public static void LogSoapRequest(string operazione, string url, string soapAction, string envelope)
        {
            Log.Debug(
                "SOAP REQUEST | Operazione={Operazione} Url={Url} SoapAction={SoapAction}",
                operazione, url, soapAction);

            Log.Debug("SOAP ENVELOPE REQUEST:{NewLine}{Envelope}", Environment.NewLine, envelope);
        }

        /// <summary>
        /// Logga la risposta SOAP grezza.
        /// </summary>
        public static void LogSoapResponse(string operazione, int httpStatus, long elapsedMs, string envelope)
        {
            Log.Debug(
                "SOAP RESPONSE | Operazione={Operazione} HttpStatus={HttpStatus} DurataMs={DurataMs}",
                operazione, httpStatus, elapsedMs);

            Log.Debug("SOAP ENVELOPE RESPONSE:{NewLine}{Envelope}", Environment.NewLine, envelope);
        }

        /// <summary>
        /// Logga un errore durante una chiamata.
        /// </summary>
        public static void LogErrore(string servizio, string applicazione, Exception ex, string? inputCamel = null)
        {
            Log.Error(ex,
                "ERRORE | Servizio={Servizio} Applicazione={Applicazione} Eccezione={Eccezione}",
                servizio, applicazione, ex.GetType().Name);

            if (!string.IsNullOrEmpty(inputCamel))
                Log.Debug("INPUT al momento dell'errore: {Input}", inputCamel);
        }

        /// <summary>
        /// Logga un warning generico.
        /// </summary>
        public static void LogWarning(string messaggio)
        {
            Log.Warning(messaggio);
        }

        /// <summary>
        /// Logga un messaggio informativo generico.
        /// </summary>
        public static void LogInfo(string messaggio)
        {
            Log.Information(messaggio);
        }
    }
}
