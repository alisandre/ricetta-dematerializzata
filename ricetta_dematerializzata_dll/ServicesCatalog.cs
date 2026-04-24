using System;
using System.Collections.Generic;
using ricetta_dematerializzata_dll.Models;

namespace ricetta_dematerializzata_dll.Core
{
    /// <summary>
    /// Catalogo interno: mappa ogni ServizioSanita all'URL di endpoint
    /// e alla SOAPAction corretta, per entrambi gli ambienti.
    /// </summary>
    internal static class ServicesCatalog
    {
        // ── URL base ──────────────────────────────────────────────────────────────
        private const string BaseTest  = "https://demservicetest.sanita.finanze.it";
        private const string BaseProd  = "https://demservice.sanita.finanze.it";
        private const string BaseA2f   = "https://servizits.sanita.finanze.it";

        // Namespace SOAP comune (il server usa questo per il SOAPAction)
        private const string NsSoap = "http://invioprescritto.wsdl.dem.sanita.finanze.it";
        private const string NsA2f  = "http://wsdl.auth.a2f.sts.sanita.finanze.it";

        // ── Descrittore di un singolo endpoint ───────────────────────────────────
        internal class EndpointInfo
        {
            /// <summary>Path relativo rispetto alla base (uguale per test e prod)</summary>
            public string PathRelativo { get; }

            /// <summary>SOAPAction da usare nell'header HTTP</summary>
            public string SoapAction { get; }

            /// <summary>Nome dell'operazione SOAP (local part)</summary>
            public string OperazioneWsdl { get; }

            public bool UsaBaseA2f { get; }

            public EndpointInfo(string pathRelativo, string soapAction, string operazioneWsdl, bool usaBaseA2f = false)
            {
                PathRelativo   = pathRelativo;
                SoapAction     = soapAction;
                OperazioneWsdl = operazioneWsdl;
                UsaBaseA2f     = usaBaseA2f;
            }

            public string GetUrl(AmbienteSanita ambiente)
                => UsaBaseA2f
                    ? BaseA2f + PathRelativo
                    : (ambiente == AmbienteSanita.Test ? BaseTest : BaseProd) + PathRelativo;
        }

        // ── Dizionario principale ─────────────────────────────────────────────────
        private static readonly Dictionary<DigitalPrescriptionService, EndpointInfo> _catalogo
            = new Dictionary<DigitalPrescriptionService, EndpointInfo>
        {
            // ── PRESCRITTORE ─────────────────────────────────────────────────────
            [DigitalPrescriptionService.VisualizzaPrescritto] = new EndpointInfo(
                "/DemRicettaPrescrittoServicesWeb/services/demVisualizzaPrescritto",
                $"{NsSoap}/VisualizzaPrescritto",
                "VisualizzaPrescritto"),

            [DigitalPrescriptionService.InvioPrescritto] = new EndpointInfo(
                "/DemRicettaPrescrittoServicesWeb/services/demInvioPrescritto",
                $"{NsSoap}/InvioPrescritto",
                "InvioPrescritto"),

            [DigitalPrescriptionService.AnnullaPrescritto] = new EndpointInfo(
                "/DemRicettaPrescrittoServicesWeb/services/demAnnullaPrescritto",
                $"{NsSoap}/AnnullaPrescritto",
                "AnnullaPrescritto"),

            [DigitalPrescriptionService.InterrogaNreUtilizzati] = new EndpointInfo(
                "/DemRicettaInterrogazioniServicesWeb/services/demInterrogaNreUtilizzati",
                $"{NsSoap}/InterrogaNreUtilizzati",
                "InterrogaNreUtilizzati"),

            [DigitalPrescriptionService.ServiceAnagPrescrittore] = new EndpointInfo(
                "/DemRicettaServiceAnagServicesWeb/services/demServiceAnag",
                $"{NsSoap}/ServiceAnag",
                "ServiceAnag"),

            [DigitalPrescriptionService.InvioDichiarazioneSostituzioneMedico] = new EndpointInfo(
                "/DemRicettaServiceMedicoWeb/services/invioDichiarazioneSostituzioneMedico",
                $"{NsSoap}/InvioDichiarazioneSostituzioneMedico",
                "InvioDichiarazioneSostituzioneMedico"),

            // ── EROGATORE ────────────────────────────────────────────────────────
            [DigitalPrescriptionService.InvioErogato] = new EndpointInfo(
                "/DemRicettaErogatoServicesWeb/services/demInvioErogato",
                $"{NsSoap}/InvioErogato",
                "InvioErogato"),

            [DigitalPrescriptionService.VisualizzaErogato] = new EndpointInfo(
                "/DemRicettaErogatoServicesWeb/services/demVisualizzaErogato",
                $"{NsSoap}/VisualizzaErogato",
                "VisualizzaErogato"),

            [DigitalPrescriptionService.SospendiErogato] = new EndpointInfo(
                "/DemRicettaErogatoServicesWeb/services/demSospendiErogato",
                $"{NsSoap}/SospendiErogato",
                "SospendiErogato"),

            [DigitalPrescriptionService.AnnullaErogato] = new EndpointInfo(
                "/DemRicettaErogatoServicesWeb/services/demAnnullaErogato",
                $"{NsSoap}/AnnullaErogato",
                "AnnullaErogato"),

            [DigitalPrescriptionService.RicercaErogatore] = new EndpointInfo(
                "/DemRicettaRicercaErogatoreServicesWeb/services/ricercaErogatore",
                $"{NsSoap}/RicercaErogatore",
                "RicercaErogatore"),

            [DigitalPrescriptionService.ReportErogatoMensile] = new EndpointInfo(
                "/DemRicettaReportServicesWeb/services/demReportErogatoMensile",
                $"{NsSoap}/ReportErogatoMensile",
                "ReportErogatoMensile"),

            [DigitalPrescriptionService.ServiceAnagErogatore] = new EndpointInfo(
                "/DemRicettaServiceAnagServicesWeb/services/demServiceAnag",
                $"{NsSoap}/ServiceAnag",
                "ServiceAnag"),

            [DigitalPrescriptionService.RicettaDifferita] = new EndpointInfo(
                "/DemRicettaDifferitaServicesWeb/services/ricettaDifferita",
                $"{NsSoap}/RicettaDifferita",
                "RicettaDifferita"),

            [DigitalPrescriptionService.AnnullaErogatoDiff] = new EndpointInfo(
                "/DemRicettaDifferitaServicesWeb/services/annullaErogatoDiff",
                $"{NsSoap}/AnnullaErogatoDiff",
                "AnnullaErogatoDiff"),

            [DigitalPrescriptionService.RicevuteSac] = new EndpointInfo(
                "/DemRicettaServiceRicevuteWeb/services/ricevuteSac",
                $"{NsSoap}/RicevuteSac",
                "RicevuteSac"),

            // ── AUTHORIZATION 2F (A2F) ─────────────────────────────────────────────
            [DigitalPrescriptionService.CreateAuth] = new EndpointInfo(
                "/a2f-auth-ws/soap/v1/authentication-service",
                $"{NsA2f}/create",
                "create",
                usaBaseA2f: true),

            [DigitalPrescriptionService.RevokeAuth] = new EndpointInfo(
                "/a2f-auth-ws/soap/v1/authentication-service",
                $"{NsA2f}/revoke",
                "revoke",
                usaBaseA2f: true),

            [DigitalPrescriptionService.CheckToken] = new EndpointInfo(
                "/a2f-auth-ws/soap/v1/authentication-service",
                $"{NsA2f}/checkToken",
                "checkToken",
                usaBaseA2f: true),
        };

        // ── API pubblica ──────────────────────────────────────────────────────────

        public static EndpointInfo Ottieni(DigitalPrescriptionService servizio)
        {
            if (!_catalogo.TryGetValue(servizio, out var info))
                throw new ArgumentException(
                    $"Servizio non mappato nel catalogo: {servizio}",
                    nameof(servizio));
            return info;
        }

        public static string OttieniUrl(DigitalPrescriptionService servizio, AmbienteSanita ambiente)
            => Ottieni(servizio).GetUrl(ambiente);

        public static string OttieniSoapAction(DigitalPrescriptionService servizio)
            => Ottieni(servizio).SoapAction;

        public static string OttieniNamespaceSoap(DigitalPrescriptionService servizio)
            => servizio switch
            {
                DigitalPrescriptionService.CreateAuth => NsA2f,
                DigitalPrescriptionService.RevokeAuth => NsA2f,
                DigitalPrescriptionService.CheckToken => NsA2f,
                _ => NsSoap
            };
    }
}
