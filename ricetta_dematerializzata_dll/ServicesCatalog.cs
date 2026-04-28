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
        private const string BaseTest    = "https://demservicetest.sanita.finanze.it";
        private const string BaseProd    = "https://demservice.sanita.finanze.it";
        private const string BaseA2fTest = "https://servizitstest.sanita.finanze.it";
        private const string BaseA2fProd = "https://servizits.sanita.finanze.it";

        // Namespace SOAP
        private const string NsA2fSoapAction = "http://wsdl.auth.a2f.sts.sanita.finanze.it";       // per SOAPAction HTTP
        private const string NsA2fEnvelope  = "http://authservice.xsd.wsdl.auth.a2f.sts.sanita.finanze.it"; // per xmlns:aut nell'XML

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

            public string? NamespaceSoap { get; }

            public EndpointInfo(string pathRelativo, string soapAction, string operazioneWsdl, bool usaBaseA2f = false, string? namespaceSoap = null)
            {
                PathRelativo   = pathRelativo;
                SoapAction     = soapAction;
                OperazioneWsdl = operazioneWsdl;
                UsaBaseA2f     = usaBaseA2f;
                NamespaceSoap  = namespaceSoap;
            }

            public string GetUrl(ServiceEnvironment ambiente)
            {
                if (UsaBaseA2f)
                    return (ambiente == ServiceEnvironment.Test ? BaseA2fTest : BaseA2fProd) + PathRelativo;
                return (ambiente == ServiceEnvironment.Test ? BaseTest : BaseProd) + PathRelativo;
            }
        }

        // ── Dizionario principale ─────────────────────────────────────────────────
        private static readonly Dictionary<DigitalPrescriptionService, EndpointInfo> _catalogo
            = new Dictionary<DigitalPrescriptionService, EndpointInfo>
        {
            // ── PRESCRITTORE ─────────────────────────────────────────────────────
            [DigitalPrescriptionService.VisualizzaPrescritto] = new EndpointInfo(
                "/DemRicettaPrescrittoServicesWeb/services/demVisualizzaPrescritto",
                "http://visualizzaprescritto.wsdl.dem.sanita.finanze.it/VisualizzaPrescritto",
                "VisualizzaPrescrittoRichiesta",
                namespaceSoap: "http://visualizzaprescrittorichiesta.xsd.dem.sanita.finanze.it"),

            [DigitalPrescriptionService.InvioPrescritto] = new EndpointInfo(
                "/DemRicettaPrescrittoServicesWeb/services/demInvioPrescritto",
                "http://invioprescritto.wsdl.dem.sanita.finanze.it/InvioPrescritto",
                "InvioPrescritto",
                namespaceSoap: "http://invioprescrittorichiesta.xsd.dem.sanita.finanze.it"),

            [DigitalPrescriptionService.AnnullaPrescritto] = new EndpointInfo(
                "/DemRicettaPrescrittoServicesWeb/services/demAnnullaPrescritto",
                "http://annullaprescritto.wsdl.dem.sanita.finanze.it/AnnullaPrescritto",
                "AnnullaPrescritto",
                namespaceSoap: "http://annullaprescrittorichiesta.xsd.dem.sanita.finanze.it"),

            [DigitalPrescriptionService.InterrogaNreUtilizzati] = new EndpointInfo(
                "/DemRicettaInterrogazioniServicesWeb/services/demInterrogaNreUtilizzati",
                "http://interroganreassociati.wsdl.dem.sanita.finanze.it/InterrogaNreUtilizzati",
                "InterrogaNreUtilizzati",
                namespaceSoap: "http://interroganreutilrichiesta.xsd.dem.sanita.finanze.it"),

            [DigitalPrescriptionService.ServiceAnagPrescrittore] = new EndpointInfo(
                "/DemRicettaServiceAnagServicesWeb/services/demServiceAnag",
                "http://serviceanag.wsdl.dem.sanita.finanze.it/ServiceAnag",
                "ServiceAnag",
                namespaceSoap: "http://serviceanagrichiesta.xsd.dem.sanita.finanze.it"),

            [DigitalPrescriptionService.InvioDichiarazioneSostituzioneMedico] = new EndpointInfo(
                "/DemRicettaServiceMedicoWeb/services/invioDichiarazioneSostituzioneMedico",
                "http://dichiarazionesostituzionemedico.wsdl.dem.sanita.finanze.it/DichiarazioneSostituzioneMedico",
                "InvioDichiarazioneSostituzioneMedico",
                namespaceSoap: "http://dichiarazionesostituzionemedicorichiesta.xsd.dem.sanita.finanze.it"),

            // ── EROGATORE ────────────────────────────────────────────────────────
            [DigitalPrescriptionService.InvioErogato] = new EndpointInfo(
                "/DemRicettaErogatoServicesWeb/services/demInvioErogato",
                "http://invioerogato.wsdl.dem.sanita.finanze.it/InvioErogato",
                "InvioErogato",
                namespaceSoap: "http://invioerogatorichiesta.xsd.dem.sanita.finanze.it"),

            [DigitalPrescriptionService.VisualizzaErogato] = new EndpointInfo(
                "/DemRicettaErogatoServicesWeb/services/demVisualizzaErogato",
                "http://visualizzaerogato.wsdl.dem.sanita.finanze.it/VisualizzaErogato",
                "VisualizzaErogato",
                namespaceSoap: "http://visualizzaerogatorichiesta.xsd.dem.sanita.finanze.it"),

            [DigitalPrescriptionService.SospendiErogato] = new EndpointInfo(
                "/DemRicettaErogatoServicesWeb/services/demSospendiErogato",
                "http://visualizzaerogato.wsdl.dem.sanita.finanze.it/SospendiErogato",
                "SospendiErogato",
                namespaceSoap: "http://sospendierogatorichiesta.xsd.dem.sanita.finanze.it"),

            [DigitalPrescriptionService.AnnullaErogato] = new EndpointInfo(
                "/DemRicettaErogatoServicesWeb/services/demAnnullaErogato",
                "http://annullaerogato.wsdl.dem.sanita.finanze.it/AnnullaErogato",
                "AnnullaErogato",
                namespaceSoap: "http://annullaerogatorichiesta.xsd.dem.sanita.finanze.it"),

            [DigitalPrescriptionService.RicercaErogatore] = new EndpointInfo(
                "/DemRicettaRicercaErogatoreServicesWeb/services/ricercaErogatore",
                "http://demricettaricercaerogatore.wsdl.dem.sanita.finanze.it/elencoRicette",
                "RicercaErogatore",
                namespaceSoap: "http://elencoricetterichiesta.xsd.dem.sanita.finanze.it"),

            [DigitalPrescriptionService.ReportErogatoMensile] = new EndpointInfo(
                "/DemRicettaReportServicesWeb/services/demReportErogatoMensile",
                "http://reporterogatomensile.wsdl.dem.sanita.finanze.it/ReportErogatoMensile",
                "ReportErogatoMensile",
                namespaceSoap: "http://reporterogatomensilerichiesta.xsd.dem.sanita.finanze.it"),

            [DigitalPrescriptionService.ServiceAnagErogatore] = new EndpointInfo(
                "/DemRicettaServiceAnagServicesWeb/services/demServiceAnag",
                "http://serviceanag.wsdl.dem.sanita.finanze.it/ServiceAnag",
                "ServiceAnag",
                namespaceSoap: "http://serviceanagrichiesta.xsd.dem.sanita.finanze.it"),

            [DigitalPrescriptionService.RicettaDifferita] = new EndpointInfo(
                "/DemRicettaDifferitaServicesWeb/services/ricettaDifferita",
                "http://demricettadifferita.wsdl.dem.sanita.finanze.it/invioSegnalazione",
                "RicettaDifferita",
                namespaceSoap: "http://inviosegnalazionerichiesta.xsd.dem.sanita.finanze.it"),

            [DigitalPrescriptionService.AnnullaErogatoDiff] = new EndpointInfo(
                "/DemRicettaDifferitaServicesWeb/services/annullaErogatoDiff",
                "http://demannullaerogatodiff.wsdl.dem.sanita.finanze.it/annullaErogatoDiff",
                "AnnullaErogatoDiff",
                namespaceSoap: "http://annullaerogatodiffrichiesta.xsd.dem.sanita.finanze.it"),

            [DigitalPrescriptionService.RicevuteSac] = new EndpointInfo(
                "/DemRicettaServiceRicevuteWeb/services/ricevuteSac",
                "http://invioprescritto.wsdl.dem.sanita.finanze.it/RicevuteSac",
                "RicevuteSac"),

            // ── AUTHORIZATION 2F (A2F) ─────────────────────────────────────────────
            [DigitalPrescriptionService.CreateAuth] = new EndpointInfo(
                "/a2f-auth-ws/soap/v1/authentication-service",
                $"{NsA2fSoapAction}/create",
                "CreateAuthReq",
                usaBaseA2f: true),

            [DigitalPrescriptionService.RevokeAuth] = new EndpointInfo(
                "/a2f-auth-ws/soap/v1/authentication-service",
                $"{NsA2fSoapAction}/revoke",
                "RevokeAuthReq",
                usaBaseA2f: true),

            [DigitalPrescriptionService.CheckToken] = new EndpointInfo(
                "/a2f-auth-ws/soap/v1/authentication-service",
                $"{NsA2fSoapAction}/checkToken",
                "CheckTokenReq",
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

        public static string OttieniUrl(DigitalPrescriptionService servizio, ServiceEnvironment ambiente)
            => Ottieni(servizio).GetUrl(ambiente);

        public static string OttieniSoapAction(DigitalPrescriptionService servizio)
            => Ottieni(servizio).SoapAction;

        public static string OttieniNamespaceSoap(DigitalPrescriptionService servizio)
        {
            var endpoint = Ottieni(servizio);
            if (!string.IsNullOrWhiteSpace(endpoint.NamespaceSoap))
                return endpoint.NamespaceSoap;

            return servizio switch
            {
                DigitalPrescriptionService.CreateAuth => NsA2fEnvelope,
                DigitalPrescriptionService.RevokeAuth => NsA2fEnvelope,
                DigitalPrescriptionService.CheckToken => NsA2fEnvelope,
                _ => "http://invioprescritto.wsdl.dem.sanita.finanze.it"
            };
        }
    }
}




