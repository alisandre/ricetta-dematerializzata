using System;
using System.Runtime.InteropServices;
using ricetta_dematerializzata.Core;
using ricetta_dematerializzata.Models;

namespace ricetta_dematerializzata.Services
{
    /// <summary>
    /// Classe COM-visible. Decorata con attributi necessari per la registrazione
    /// nel registro di sistema via regasm.
    ///
    /// Da Delphi (dopo regasm /tlb):
    ///   uses ricetta_dematerializzata_TLB;
    ///   var c: IRicettaDematerializzataClient;
    ///   c := CoRicettaDematerializzataClient.Create;
    ///   c.Configura('utente', 'password', 0, True);
    ///   ShowMessage(c.Chiama(2, 'NRE=123456789;CF=RSSMRA80A01H501T'));
    /// </summary>
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    [Guid("B2C3D4E5-F6A7-8901-BCDE-F12345678901")]
    [ProgId("RicettaDematerializzata.Client")]
    public class RicettaDematerializzataClient : RicettaDematerializzataBaseClient, IRicettaDematerializzataClient
    {
        // Costruttore vuoto obbligatorio per COM
        public RicettaDematerializzataClient() : base() { }

        // ── Registrazione/deregistrazione COM ──────────────────────────────────────

        [ComRegisterFunction]
        public static void RegisterClass(string key)
        {
            // Eventuale logica di registrazione custom
        }

        [ComUnregisterFunction]
        public static void UnregisterClass(string key)
        {
            // Eventuale logica di deregistrazione custom
        }
    }

    /// <summary>
    /// Factory statica per uso da Delphi senza COM (via LoadLibrary + GetProcAddress).
    /// Espone metodi statici con signature semplici.
    ///
    /// Alternativa a COM: compilare come DLL standard e chiamare via P/Invoke da Delphi.
    /// </summary>
    public static class RicettaDematerializzataFactory
    {
        private static RicettaDematerializzataBaseClient? _istanza;

        /// <summary>
        /// Inizializza il client con configurazione unificata. Metodo principale.
        /// Chiamare una sola volta all'avvio.
        /// ambiente: 0=Test, 1=Produzione
        /// Il certificato Sanitel è hardcodato come "certificates/SanitelCF.cer".
        /// </summary>
        public static string Inizializza(
            string username, string password, string seriale,
            int ambiente = 0)
        {
            try
            {
                var config = new ServiceConfiguration
                {
                    Username              = username,
                    Password              = password,
                    SerialeCertificatoSsl = seriale,
                    Ambiente              = (ServiceEnvironment)ambiente,
                };
                _istanza = new RicettaDematerializzataBaseClient(config);
                return "OK";
            }
            catch (Exception ex)
            {
                return ParserKV.BuildErrore(9999, ex.Message);
            }
        }

        /// <summary>
        /// Inizializza il client (metodo legacy). Usare Inizializza(username, password, seriale) preferibilmente.
        /// ambiente: 0=Test, 1=Produzione
        /// Il certificato Sanitel è hardcodato come "certificates/SanitelCF.cer".
        /// </summary>
        [Obsolete("Usare Inizializza(username, password, seriale, ...) invece.", false)]
        public static string InizialiazzaLegacy(
            string username, string password,
            int ambiente = 0)
        {
            try
            {
                var config = new ServiceConfiguration
                {
                    Username = username,
                    Password = password,
                    Ambiente = (ServiceEnvironment)ambiente,
                };
                _istanza = new RicettaDematerializzataBaseClient(config);
                return "OK";
            }
            catch (Exception ex)
            {
                return ParserKV.BuildErrore(9999, ex.Message);
            }
        }

        /// <summary>
        /// Chiama un servizio. L'istanza deve essere già inizializzata con Inizializza().
        /// </summary>
        public static string Chiama(int servizio, string parametriInput)
        {
            if (_istanza == null)
                return ParserKV.BuildErrore(1, "Client non inizializzato. Chiamare Inizializza().");
            return _istanza.Chiama(servizio, parametriInput);
        }

        /// <summary>Restituisce la lista di tutti i servizi disponibili.</summary>
        public static string ElencaServizi()
        {
            var sb = new System.Text.StringBuilder();
            foreach (DigitalPrescriptionService s in Enum.GetValues(typeof(DigitalPrescriptionService)))
            {
                if (sb.Length > 0) sb.Append(';');
                sb.AppendFormat("SERVIZIO_{0}={1}", (int)s, s.ToString());
            }
            return sb.ToString();
        }
    }
}
