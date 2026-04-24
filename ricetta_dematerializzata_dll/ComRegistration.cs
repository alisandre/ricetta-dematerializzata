using System;
using System.Runtime.InteropServices;
using ricetta_dematerializzata_dll.Core;
using ricetta_dematerializzata_dll.Models;

namespace ricetta_dematerializzata_dll.Services
{
    /// <summary>
    /// Classe COM-visible. Decorata con attributi necessari per la registrazione
    /// nel registro di sistema via regasm.
    ///
    /// Da Delphi (dopo regasm /tlb):
    ///   uses SanitaServiceLib_TLB;
    ///   var c: ISanitaServiceClient;
    ///   c := CoSanitaServiceClient.Create;
    ///   c.Configura('utente', 'password', 0, True);
    ///   ShowMessage(c.Chiama(2, 'NRE=123456789;CF=RSSMRA80A01H501T'));
    /// </summary>
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    [Guid("B2C3D4E5-F6A7-8901-BCDE-F12345678901")]
    [ProgId("SanitaServiceLib.Client")]
    public class SanitaServiceClientCom : PrescriptionClient, ISanitaServiceClient
    {
        // Costruttore vuoto obbligatorio per COM
        public SanitaServiceClientCom() : base() { }

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
    public static class SanitaServiceFactory
    {
        private static PrescriptionClient? _istanza;

        /// <summary>
        /// Inizializza il client. Chiamare una sola volta all'avvio.
        /// ambiente: 0=Test, 1=Produzione
        /// </summary>
        public static string Inizializza(
            string username, string password,
            int ambiente = 0, bool ignoraSsl = false,
            string? pathSanitel = null)
        {
            try
            {
                var config = new ServiceConfiguration
                {
                    Username               = username,
                    Password               = password,
                    Ambiente               = (AmbienteSanita)ambiente,
                    IgnoraErroriSsl        = ignoraSsl,
                    PathCertificatoSanitel = pathSanitel
                };
                _istanza = new PrescriptionClient(config);
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
