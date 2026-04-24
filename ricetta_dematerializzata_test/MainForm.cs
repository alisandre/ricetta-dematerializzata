using ricetta_dematerializzata_dll.Core;
using ricetta_dematerializzata_dll.Models;
using ricetta_dematerializzata_dll.Services;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ricetta_dematerializzata_test_ui
{
    public partial class MainForm : Form
    {
        private TextBox _txtUsername = null!;
        private TextBox _txtPassword = null!;
        private ComboBox _cmbServizio = null!;
        private TextBox _txtInput = null!;
        private TextBox _txtOutput = null!;
        private TextBox _txtAuthorization2F = null!;
        private CheckBox _chkIgnoraSsl = null!;
        private CheckBox _chkProduzione = null!;
        private Button _btnChiama = null!;
        private const string NomeCertificatoSanitel = "SanitelCF-2024-2027.cer";

        public MainForm()
        {
            InitializeComponent();
            _cmbServizio.DataSource = Enum.GetValues(typeof(DigitalPrescriptionService));
            AggiornaUiPerServizio();
        }

        private void BtnChiama_Click(object? sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(_txtUsername.Text) || string.IsNullOrWhiteSpace(_txtPassword.Text))
                {
                    _txtOutput.Text = "Inserire username e password.";
                    return;
                }

                var config = new ServiceConfiguration
                {
                    Username = _txtUsername.Text.Trim(),
                    Password = _txtPassword.Text,
                    Ambiente = _chkProduzione.Checked ? AmbienteSanita.Produzione : AmbienteSanita.Test,
                    IgnoraErroriSsl = _chkIgnoraSsl.Checked,
                    Authorization2F = _txtAuthorization2F.Text?.Trim(),
                    PathCertificatoSanitel = TrovaPathCertificatoSanitel()
                };

                var client = new PrescriptionClient(config);
                var servizio = (DigitalPrescriptionService)_cmbServizio.SelectedItem;
                var output = client.Chiama(servizio, _txtInput.Text ?? string.Empty);

                _txtOutput.Text = output;
            }
            catch (Exception ex)
            {
                _txtOutput.Text = $"ERRORE_NUMERO=9999;ERRORE_DESCRIZIONE={ex.Message}";
            }
        }

        private void CmbServizio_SelectedIndexChanged(object? sender, EventArgs e)
        {
            AggiornaUiPerServizio();
        }

        private void AggiornaUiPerServizio()
        {
            if (_cmbServizio.SelectedItem is not DigitalPrescriptionService servizio)
            {
                _txtInput.Text = string.Empty;
                return;
            }

            _txtInput.Text = GetDefaultInput(servizio);
        }

        private static string GetDefaultInput(DigitalPrescriptionService servizio)
            => servizio switch
            {
                DigitalPrescriptionService.VisualizzaPrescritto =>
                    "pinCode=1234567890;nre=120000000000;cfMedico=PROVAX00X00X000Y",

                DigitalPrescriptionService.InvioPrescritto =>
                    "pinCode=1234567890;cfMedico1=PROVAX00X00X000Y;codRegione=190;codASLAo=201;codSpecializzazione=P;tipoPrescrizione=F;dataCompilazione=2026-01-01 10:00:00;tipoVisita=A;ElencoDettagliPrescrizioni=DETTAGLIO_MINIMO",

                DigitalPrescriptionService.AnnullaPrescritto =>
                    "pinCode=1234567890;nre=120000000000;cfMedico=PROVAX00X00X000Y",

                DigitalPrescriptionService.InterrogaNreUtilizzati =>
                    "pinCode=1234567890;codRegione=190;cfMedico=PROVAX00X00X000Y",

                DigitalPrescriptionService.ServiceAnagPrescrittore =>
                    "pinCode=1234567890;tipoOperazione=1",

                DigitalPrescriptionService.InvioDichiarazioneSostituzioneMedico =>
                    "pinCode=1234567890;pwd=UTENTE;cfMedicoTitolare=PROVAX00X00X000Y;codRegione=190;codASLAo=201;codSpecializzazione=P;cfMedicoSostituto=RSSMRA80A01H501T;dataInizioSostituzione=2026-01-01;dataFineSostituzione=2026-01-31",

                DigitalPrescriptionService.InvioErogato =>
                    "pinCode=1234567890;codiceRegioneErogatore=190;codiceAslErogatore=201;codiceSsaErogatore=201600104;nre=120000000000;tipoOperazione=I;dataSpedizione=2026-01-01",

                DigitalPrescriptionService.VisualizzaErogato =>
                    "pinCode=1234567890;codiceRegioneErogatore=190;codiceAslErogatore=201;codiceSsaErogatore=201600104;nre=120000000000;tipoOperazione=V",

                DigitalPrescriptionService.SospendiErogato =>
                    "pinCode=1234567890;codiceRegioneErogatore=190;codiceAslErogatore=201;codiceSsaErogatore=201600104;nre=120000000000;tipoOperazione=S",

                DigitalPrescriptionService.AnnullaErogato =>
                    "pinCode=1234567890;codiceRegioneErogatore=190;codiceAslErogatore=201;codiceSsaErogatore=201600104;nre=120000000000;codAnnullamento=1",

                DigitalPrescriptionService.RicercaErogatore =>
                    "pinCode=1234567890;codiceRegioneErogatore=190;codiceAslErogatore=201;codiceSsaErogatore=201600104",

                DigitalPrescriptionService.ReportErogatoMensile =>
                    "pinCode=1234567890;codiceRegioneErogatore=190;codiceAslErogatore=201;codiceSsaErogatore=201600104;annoMese=202601",

                DigitalPrescriptionService.ServiceAnagErogatore =>
                    "pinCode=1234567890;tipoOperazione=1",

                DigitalPrescriptionService.RicettaDifferita =>
                    "pinCode=1234567890;codiceRegioneErogatore=190;codiceAslErogatore=201;codiceSsaErogatore=201600104;codMotivazione=01;dataDal=2026-01-01",

                DigitalPrescriptionService.AnnullaErogatoDiff =>
                    "pinCode=1234567890;codiceRegioneErogatore=190;codiceAslErogatore=201;codiceSsaErogatore=201600104;idRicetta=ID001;nre=120000000000",

                DigitalPrescriptionService.RicevuteSac =>
                    "pinCode=1234567890",

                DigitalPrescriptionService.CreateAuth =>
                    "userId=PROVAX00X00X000Y;cfUtente=PROVAX00X00X000Y;codRegione=190;codAslAo=201;codSsa=201600;codiceStruttura=;contesto=DEM;applicazione=PRESCRITTO",

                DigitalPrescriptionService.RevokeAuth =>
                    "userId=PROVAX00X00X000Y;cfUtente=PROVAX00X00X000Y;token=00000000-0000-0000-0000-000000000000;contesto=DEM;applicazione=PRESCRITTO",

                DigitalPrescriptionService.CheckToken =>
                    "userId=PROVAX00X00X000Y;cfUtente=PROVAX00X00X000Y;token=00000000-0000-0000-0000-000000000000;contesto=DEM;applicazione=PRESCRITTO",

                _ => string.Empty
            };

        private static bool IsServizioErogatore(DigitalPrescriptionService servizio)
            => servizio switch
            {
                DigitalPrescriptionService.InvioErogato => true,
                DigitalPrescriptionService.VisualizzaErogato => true,
                DigitalPrescriptionService.SospendiErogato => true,
                DigitalPrescriptionService.AnnullaErogato => true,
                DigitalPrescriptionService.RicercaErogatore => true,
                DigitalPrescriptionService.ReportErogatoMensile => true,
                DigitalPrescriptionService.ServiceAnagErogatore => true,
                DigitalPrescriptionService.RicettaDifferita => true,
                DigitalPrescriptionService.AnnullaErogatoDiff => true,
                DigitalPrescriptionService.RicevuteSac => true,
                _ => false
            };

        private static string? TrovaPathCertificatoSanitel()
        {
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;

            var candidati = new[]
            {
                Path.Combine(baseDir, "certificates", NomeCertificatoSanitel),
                Path.GetFullPath(Path.Combine(baseDir, "..", "..", "..", "..", "ricetta_dematerializzata_dll", "certificates", NomeCertificatoSanitel)),
                Path.GetFullPath(Path.Combine(baseDir, "..", "..", "..", "..", "..", "ricetta_dematerializzata_dll", "certificates", NomeCertificatoSanitel))
            };

            foreach (var path in candidati)
            {
                if (File.Exists(path))
                    return path;
            }

            return null;
        }
    }
}
