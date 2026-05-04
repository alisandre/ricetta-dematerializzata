using ricetta_dematerializzata.Core;
using ricetta_dematerializzata.Models;
using ricetta_dematerializzata.Services;
using System;
using System.Windows.Forms;

namespace ricetta_dematerializzata_test_ui
{
    public partial class MainForm : Form
    {
        // Credenziali di test
        private const string DefaultUsername = "PROVAX00X00X000Y";
        private const string DefaultPassword = "Salve123";
        private const string DefaultCfAssistito = "GLLGNN37B51C286O";

        // Credenziali dedicate ai servizi EROGATORE
        private const string DefaultUsernameErogatore = "TSTSIC00B01H501E";
        private const string DefaultPasswordErogatore = "Salve123";
        private const string DefaultPinCodeErogatore = "TSTSIC00B01H501E";
        private const string DefaultCodRegioneErogatore = "190";
        private const string DefaultCodAslErogatore = "201";
        private const string DefaultCodSsaErogatore = "888888";

        // Input default A2F
        private const string DefaultUserId = "PROVAX00X00X000Y";
        private const string DefaultCfUtente = "PROVAX00X00X000Y";
        private const string DefaultIdentBase64 = "LsQiYtf7FcpMYVKvf+51V6t1BSUk+E/dGOB2vmwNl0DhirZ8QzvTI2Ay04p6+t+eH+DjzkJpXrlEEZvKRz6wKVNOt7uYSQUYKBIFcbcEQJnqT7zTgtz7jV3BK+QaEphfKRsOP1Iejv+vKvJ/3te2xNMHPkNYZIAjxEQHftw9Swk=";
        private const string DefaultCodRegioneAuth = "130";
        private const string DefaultCodAslAoAuth = "202";
        private const string DefaultCodSsaAuth = "000000";
        private const string DefaultCodStrutturaToken = "";
        private const string DefaultCodStrutturaServizi = "201600104";
        private const string DefaultCodRegioneServizi = "190";
        private const string DefaultCodAslAoServizi = "201";

        public MainForm()
        {
            InitializeComponent();

            _txtUsername.Text = DefaultUsername;
            _txtPassword.Text = DefaultPassword;
            _txtUsernameE.Text = DefaultUsernameErogatore;
            _txtPasswordE.Text = DefaultPasswordErogatore;

            // Combo servizi prescrittore
            _cmbServizioP.DataSource = new[]
            {
                DigitalPrescriptionService.VisualizzaPrescritto,
                DigitalPrescriptionService.InvioPrescritto,
                DigitalPrescriptionService.AnnullaPrescritto,
                DigitalPrescriptionService.InterrogaNreUtilizzati,
                DigitalPrescriptionService.ServiceAnagPrescrittore,
                DigitalPrescriptionService.InvioDichiarazioneSostituzioneMedico,
            };
            _cmbServizioP.SelectedIndexChanged += (s, e) => _txtInputP.Text = GetDefaultInput((DigitalPrescriptionService)_cmbServizioP.SelectedItem!);
            _txtInputP.Text = GetDefaultInput(DigitalPrescriptionService.VisualizzaPrescritto);

            // Combo servizi erogatore
            _cmbServizioE.DataSource = new[]
            {
                DigitalPrescriptionService.InvioErogato,
                DigitalPrescriptionService.VisualizzaErogato,
                DigitalPrescriptionService.SospendiErogato,
                DigitalPrescriptionService.AnnullaErogato,
                DigitalPrescriptionService.RicercaErogatore,
                DigitalPrescriptionService.ReportErogatoMensile,
                DigitalPrescriptionService.ServiceAnagErogatore,
                DigitalPrescriptionService.RicettaDifferita,
                DigitalPrescriptionService.AnnullaErogatoDiff,
                DigitalPrescriptionService.RicevuteSac,
            };
            _cmbServizioE.SelectedIndexChanged += (s, e) => _txtInputE.Text = GetDefaultInput((DigitalPrescriptionService)_cmbServizioE.SelectedItem!);
            _txtInputE.Text = GetDefaultInput(DigitalPrescriptionService.InvioErogato);

            LoadTokensFromStorage();
        }

        // ── Configurazione ────────────────────────────────────────────────────────

        private ServiceConfiguration BuildConfig(string applicazione)
        {
            var ambiente = _chkProduzione.Checked ? ServiceEnvironment.Produzione : ServiceEnvironment.Test;

            // Scegli il campo Auth2F corretto in base al ruolo
            var auth2FField = applicazione == "EROGATORE" ? _txtAuth2FE.Text.Trim() : _txtAuth2FP.Text.Trim();
            var auth2F = auth2FField;

            var username = applicazione == "EROGATORE" ? _txtUsernameE.Text.Trim() : _txtUsername.Text.Trim();
            var password = applicazione == "EROGATORE" ? _txtPasswordE.Text : _txtPassword.Text;

            // In test genera automaticamente con la formula documentata
            if (ambiente == ServiceEnvironment.Test)
            {
                var now = DateTime.Now;
                auth2F = $"Bearer {username}-{now.Year}-{now.Month:D2}-RICETTA-DEM-{applicazione}";
            }

            return new ServiceConfiguration
            {
                Username        = username,
                Password        = password,
                Ambiente        = ambiente,
                Authorization2F = auth2F
            };
        }

        private bool ValidaCredenziali()
        {
            if (string.IsNullOrWhiteSpace(_txtUsername.Text) || string.IsNullOrWhiteSpace(_txtPassword.Text))
            {
                MessageBox.Show("Inserire username e password.", "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(_txtUsernameE.Text) || string.IsNullOrWhiteSpace(_txtPasswordE.Text))
            {
                MessageBox.Show("Inserire username e password Erogatore.", "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        // ── A2F Input builder ─────────────────────────────────────────────────────

        private string BuildA2FInput(string applicazione, string token = "", bool skipToken = false)
        {
            var t = skipToken ? "" : token;
            var isErogatore = string.Equals(applicazione, "EROGATORE", StringComparison.OrdinalIgnoreCase);
            var userId = isErogatore ? DefaultUsernameErogatore : DefaultUserId;
            var cfUtente = isErogatore ? DefaultUsernameErogatore : DefaultCfUtente;
            return $"userId={userId};identificativo={DefaultIdentBase64};cfUtente={cfUtente}"
                 + $";codRegione={DefaultCodRegioneAuth};codAslAo={DefaultCodAslAoAuth};codSsa={DefaultCodSsaAuth}"
                 + $";codiceStruttura={DefaultCodStrutturaToken};contesto=RICETTA-DEM;applicazione={applicazione}"
                 + (string.IsNullOrWhiteSpace(t) ? "" : $";token={t}");
        }

        // ── TAB TOKEN – PRESCRITTORE ──────────────────────────────────────────────

        private void BtnCreateTokenP_Click(object sender, EventArgs e)
        {
            if (!ValidaCredenziali()) return;
            try
            {
                var config = BuildConfig("PRESCRITTORE");
                var client = new Auth2FClient(config);
                var input = BuildA2FInput("PRESCRITTORE", skipToken: true);
                var result = client.Create(input);

                if (_chkProduzione.Checked)
                {
                    // In produzione il token non è nella risposta: viene inviato via email all'utente
                    _txtA2FOutput.Text = "INPUT PARAMETRI:\r\n" + input + "\r\n\r\n" + $"✅ [PRESCRITTORE] Richiesta token inviata.\r\nIl token verrà recapitato via email. Inserirlo manualmente.\r\n\r\n{result}";
                    var token = ChiediTokenManuale("PRESCRITTORE");
                    if (!string.IsNullOrWhiteSpace(token))
                    {
                        _txtTokenP.Text = token;
                        TokenManager.SaveToken(token, "PRESCRITTORE");
                        _txtA2FOutput.Text += $"\r\n\r\n🔑 Token inserito manualmente e salvato.";
                    }
                }
                else
                {
                    var token = EstraiValoreCodice(result, "token");
                    if (!string.IsNullOrWhiteSpace(token))
                    {
                        _txtTokenP.Text = token;
                        TokenManager.SaveToken(token, "PRESCRITTORE");
                        _txtA2FOutput.Text = "INPUT PARAMETRI:\r\n" + input + "\r\n\r\n" + $"✅ [PRESCRITTORE] Token creato:\r\n{token}\r\n\r\n{result}";
                    }
                    else
                    {
                        _txtA2FOutput.Text = "INPUT PARAMETRI:\r\n" + input + "\r\n\r\n" + $"⚠️ [PRESCRITTORE] Token non trovato:\r\n{result}";
                    }
                }
            }
            catch (Exception ex) { _txtA2FOutput.Text = $"❌ {ex.Message}"; }
        }

        private void BtnCheckTokenP_Click(object sender, EventArgs e)
        {
            if (!ValidaCredenziali()) return;
            if (string.IsNullOrWhiteSpace(_txtTokenP.Text)) { _txtA2FOutput.Text = "❌ Nessun token PRESCRITTORE."; return; }
            try
            {
                var client = new Auth2FClient(BuildConfig("PRESCRITTORE"));
                var input = BuildA2FInput("PRESCRITTORE", _txtTokenP.Text);
                var result = client.CheckToken(input);
                if (IsA2FError(result))
                {
                    AzzeraInfoValiditaP();
                    _txtA2FOutput.Text = "INPUT PARAMETRI:\r\n" + input + "\r\n\r\n" + $"❌ [PRESCRITTORE] Token non valido:\r\n{result}";
                }
                else
                {
                    PopolaInfoValidita(result, _txtStatoP, _txtInizioP, _txtFineP);
                    _txtA2FOutput.Text = "INPUT PARAMETRI:\r\n" + input + "\r\n\r\n" + $"✅ [PRESCRITTORE] Token valido:\r\n{result}";
                }
            }
            catch (Exception ex) { _txtA2FOutput.Text = $"❌ {ex.Message}"; }
        }

        private void BtnRevokeTokenP_Click(object sender, EventArgs e)
        {
            if (!ValidaCredenziali()) return;
            if (string.IsNullOrWhiteSpace(_txtTokenP.Text)) { _txtA2FOutput.Text = "❌ Nessun token PRESCRITTORE."; return; }
            try
            {
                var client = new Auth2FClient(BuildConfig("PRESCRITTORE"));
                var input = BuildA2FInput("PRESCRITTORE", _txtTokenP.Text);
                var result = client.Revoke(input);
                if (!IsA2FError(result))
                {
                    var d = ParserKV.Parse(result);
                    var desc = d.TryGetValue("VALORE", out var v) ? v : "Revoca eseguita";
                    _txtTokenP.Text = string.Empty;
                    TokenManager.ClearToken("PRESCRITTORE");
                    AzzeraInfoValiditaP();
                    _txtA2FOutput.Text = "INPUT PARAMETRI:\r\n" + input + "\r\n\r\n" + $"✅ [PRESCRITTORE] Token revocato:\r\n{desc}\r\n\r\n{result}";
                }
                else
                {
                    _txtA2FOutput.Text = "INPUT PARAMETRI:\r\n" + input + "\r\n\r\n" + $"❌ [PRESCRITTORE] Revoca fallita:\r\n{result}";
                }
            }
            catch (Exception ex) { _txtA2FOutput.Text = $"❌ {ex.Message}"; }
        }

        private void BtnInsertTokenP_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_txtTokenP.Text))
            {
                MessageBox.Show("Nessun token PRESCRITTORE. Prima esegui Create.", "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            _txtAuth2FP.Text = _txtTokenP.Text;
            _tabControl.SelectedTab = _tabPrescrittore;
        }

        // ── TAB TOKEN – EROGATORE ─────────────────────────────────────────────────

        private void BtnCreateTokenE_Click(object sender, EventArgs e)
        {
            if (!ValidaCredenziali()) return;
            try
            {
                var client = new Auth2FClient(BuildConfig("EROGATORE"));
                var input = BuildA2FInput("EROGATORE", skipToken: true);
                var result = client.Create(input);

                if (_chkProduzione.Checked)
                {
                    // In produzione il token non è nella risposta: viene inviato via email all'utente
                    _txtA2FOutput.Text = "INPUT PARAMETRI:\r\n" + input + "\r\n\r\n" + $"✅ [EROGATORE] Richiesta token inviata.\r\nIl token verrà recapitato via email. Inserirlo manualmente.\r\n\r\n{result}";
                    var token = ChiediTokenManuale("EROGATORE");
                    if (!string.IsNullOrWhiteSpace(token))
                    {
                        _txtTokenE.Text = token;
                        TokenManager.SaveToken(token, "EROGATORE");
                        _txtA2FOutput.Text += $"\r\n\r\n🔑 Token inserito manualmente e salvato.";
                    }
                }
                else
                {
                    var token = EstraiValoreCodice(result, "token");
                    if (!string.IsNullOrWhiteSpace(token))
                    {
                        _txtTokenE.Text = token;
                        TokenManager.SaveToken(token, "EROGATORE");
                        _txtA2FOutput.Text = "INPUT PARAMETRI:\r\n" + input + "\r\n\r\n" + $"✅ [EROGATORE] Token creato:\r\n{token}\r\n\r\n{result}";
                    }
                    else
                    {
                        _txtA2FOutput.Text = "INPUT PARAMETRI:\r\n" + input + "\r\n\r\n" + $"⚠️ [EROGATORE] Token non trovato:\r\n{result}";
                    }
                }
            }
            catch (Exception ex) { _txtA2FOutput.Text = $"❌ {ex.Message}"; }
        }

        private void BtnCheckTokenE_Click(object sender, EventArgs e)
        {
            if (!ValidaCredenziali()) return;
            if (string.IsNullOrWhiteSpace(_txtTokenE.Text)) { _txtA2FOutput.Text = "❌ Nessun token EROGATORE."; return; }
            try
            {
                var client = new Auth2FClient(BuildConfig("EROGATORE"));
                var input = BuildA2FInput("EROGATORE", _txtTokenE.Text);
                var result = client.CheckToken(input);
                if (IsA2FError(result))
                {
                    AzzeraInfoValiditaE();
                    _txtA2FOutput.Text = "INPUT PARAMETRI:\r\n" + input + "\r\n\r\n" + $"❌ [EROGATORE] Token non valido:\r\n{result}";
                }
                else
                {
                    PopolaInfoValidita(result, _txtStatoE, _txtInizioE, _txtFineE);
                    _txtA2FOutput.Text = "INPUT PARAMETRI:\r\n" + input + "\r\n\r\n" + $"✅ [EROGATORE] Token valido:\r\n{result}";
                }
            }
            catch (Exception ex) { _txtA2FOutput.Text = $"❌ {ex.Message}"; }
        }

        private void BtnRevokeTokenE_Click(object sender, EventArgs e)
        {
            if (!ValidaCredenziali()) return;
            if (string.IsNullOrWhiteSpace(_txtTokenE.Text)) { _txtA2FOutput.Text = "❌ Nessun token EROGATORE."; return; }
            try
            {
                var client = new Auth2FClient(BuildConfig("EROGATORE"));
                var input = BuildA2FInput("EROGATORE", _txtTokenE.Text);
                var result = client.Revoke(input);
                if (!IsA2FError(result))
                {
                    var d = ParserKV.Parse(result);
                    var desc = d.TryGetValue("VALORE", out var v) ? v : "Revoca eseguita";
                    _txtTokenE.Text = string.Empty;
                    TokenManager.ClearToken("EROGATORE");
                    AzzeraInfoValiditaE();
                    _txtA2FOutput.Text = "INPUT PARAMETRI:\r\n" + input + "\r\n\r\n" + $"✅ [EROGATORE] Token revocato:\r\n{desc}\r\n\r\n{result}";
                }
                else
                {
                    _txtA2FOutput.Text = "INPUT PARAMETRI:\r\n" + input + "\r\n\r\n" + $"❌ [EROGATORE] Revoca fallita:\r\n{result}";
                }
            }
            catch (Exception ex) { _txtA2FOutput.Text = $"❌ {ex.Message}"; }
        }

        private void BtnInsertTokenE_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_txtTokenE.Text))
            {
                MessageBox.Show("Nessun token EROGATORE. Prima esegui Create.", "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            _txtAuth2FE.Text = _txtTokenE.Text;
            _tabControl.SelectedTab = _tabErogatore;
        }

        // ── TAB SERVIZI PRESCRITTORE ──────────────────────────────────────────────

        private void BtnChiamaP_Click(object sender, EventArgs e)
        {
            if (!ValidaCredenziali()) return;
            try
            {
                var config = BuildConfig("PRESCRITTORE");
                var client = new RicettaDematerializzataBaseClient(config);
                var input = _txtInputP.Text;
                var result = client.Chiama((DigitalPrescriptionService)_cmbServizioP.SelectedItem!, input);
                _txtOutputP.Text = "INPUT PARAMETRI:\r\n" + input + "\r\n\r\nRISPOSTA:\r\n" + result;
            }
            catch (Exception ex) { _txtOutputP.Text = $"❌ {ex.Message}"; }
        }

        private void BtnDebugSoapP_Click(object sender, EventArgs e)
            => MostraDebug();

        // ── TAB SERVIZI EROGATORE ─────────────────────────────────────────────────

        private void BtnChiamaE_Click(object sender, EventArgs e)
        {
            if (!ValidaCredenziali()) return;
            try
            {
                var config = BuildConfig("EROGATORE");
                var client = new RicettaDematerializzataBaseClient(config);
                var input = _txtInputE.Text;
                var result = client.Chiama((DigitalPrescriptionService)_cmbServizioE.SelectedItem!, input);
                _txtOutputE.Text = "INPUT PARAMETRI:\r\n" + input + "\r\n\r\nRISPOSTA:\r\n" + result;
            }
            catch (Exception ex) { _txtOutputE.Text = $"❌ {ex.Message}"; }
        }

        private void BtnDebugSoapE_Click(object sender, EventArgs e)
            => MostraDebug();

        private void BtnDebugSoapHeadersA2F_Click(object sender, EventArgs e)
            => MostraDebug();

        // ── Helpers ───────────────────────────────────────────────────────────────

        private void PopolaInfoValidita(string result,
            System.Windows.Forms.TextBox txtStato,
            System.Windows.Forms.TextBox txtInizio,
            System.Windows.Forms.TextBox txtFine)
        {
            var d = ParserKV.Parse(result);
            d.TryGetValue("STATO", out var stato);
            d.TryGetValue("DESCRIZIONE", out var desc);
            d.TryGetValue("DATAINIZIOVALIDITA", out var dal);
            d.TryGetValue("DATAFINEVALIDITA", out var al);

            txtStato.Text = string.IsNullOrWhiteSpace(desc) ? stato ?? "" : $"{stato} – {desc}";
            txtInizio.Text = FormatDataA2F(dal);
            txtFine.Text = FormatDataA2F(al);
            txtFine.BackColor = IsScaduto(al) ? System.Drawing.Color.LightCoral : System.Drawing.Color.LightGreen;
            txtInizio.BackColor = System.Drawing.Color.WhiteSmoke;
        }

        private void AzzeraInfoValiditaP()
        {
            _txtStatoP.Text = string.Empty;
            _txtInizioP.Text = string.Empty;
            _txtFineP.Text = string.Empty;
            _txtFineP.BackColor = System.Drawing.Color.WhiteSmoke;
            _txtInizioP.BackColor = System.Drawing.Color.WhiteSmoke;
        }

        private void AzzeraInfoValiditaE()
        {
            _txtStatoE.Text = string.Empty;
            _txtInizioE.Text = string.Empty;
            _txtFineE.Text = string.Empty;
            _txtFineE.BackColor = System.Drawing.Color.WhiteSmoke;
            _txtInizioE.BackColor = System.Drawing.Color.WhiteSmoke;
        }

        private static string FormatDataA2F(string? iso)
        {
            if (string.IsNullOrWhiteSpace(iso)) return string.Empty;
            return DateTimeOffset.TryParse(iso, out var dto)
                ? dto.ToLocalTime().ToString("dd/MM/yyyy HH:mm:ss")
                : iso;
        }

        private static bool IsScaduto(string? iso)
            => DateTimeOffset.TryParse(iso, out var dto) && dto < DateTimeOffset.Now;

        private static bool IsA2FError(string result)
        {
            if (string.IsNullOrWhiteSpace(result)) return true;
            var d = ParserKV.Parse(result);
            if (d.ContainsKey("ERRORE_NUMERO")) return true;
            if (d.TryGetValue("CODESITO", out var esito) && esito != "0") return true;
            return false;
        }

        private static string? EstraiValoreCodice(string kv, string codice)
        {
            var dict = ParserKV.Parse(kv);
            var suffissi = new[] { "", "_1", "_2", "_3", "_4", "_5", "_6", "_7", "_8", "_9" };
            foreach (var s in suffissi)
            {
                var kc = ("CODICE" + s).ToUpperInvariant();
                var km = ("MESSAGGIO" + s).ToUpperInvariant();
                if (dict.TryGetValue(kc, out var val) &&
                    string.Equals(val, codice, StringComparison.OrdinalIgnoreCase) &&
                    dict.TryGetValue(km, out var msg))
                    return msg;
            }
            return null;
        }

        private static string GetDefaultInput(DigitalPrescriptionService servizio)
            => servizio switch
            {
                DigitalPrescriptionService.VisualizzaPrescritto =>
                    "pinCode=1234567890;nre=120000000000;cfMedico=PROVAX00X00X000Y",
                DigitalPrescriptionService.InvioPrescritto =>
                    $"pinCode=1234567890;cfMedico1=PROVAX00X00X000Y;codRegione={DefaultCodRegioneServizi};codASLAo={DefaultCodAslAoServizi};codStruttura={DefaultCodStrutturaServizi};codSpecializzazione=P;codiceAss={DefaultCfAssistito};tipoPrescrizione=P;nonEsente=1;codDiagnosi=401.9;descrizioneDiagnosi=IPERTENSIONE ARTERIOSA ESSENZIALE;dataCompilazione={DateTime.Now:yyyy-MM-dd HH:mm:ss};tipoVisita=A;classePriorita=P;provAssistito=AG;aslAssistito=201;ElencoDettagliPrescrizioni=ARRAY;ElencoDettagliPrescrizioni_1_codProdPrest=89.7;ElencoDettagliPrescrizioni_1_descrProdPrest=VISITA CARDIOLOGICA;ElencoDettagliPrescrizioni_1_quantita=1;ElencoDettagliPrescrizioni_1_tipoAccesso=1;ElencoDettagliPrescrizioni_2_codProdPrest=89.52;ElencoDettagliPrescrizioni_2_descrProdPrest=ELETTROCARDIOGRAMMA (ECG) BASALE;ElencoDettagliPrescrizioni_2_quantita=1;ElencoDettagliPrescrizioni_2_tipoAccesso=1",
                DigitalPrescriptionService.AnnullaPrescritto =>
                    "pinCode=1234567890;nre=120000000000;cfMedico=PROVAX00X00X000Y",
                DigitalPrescriptionService.InterrogaNreUtilizzati =>
                    $"pinCode=1234567890;codRegione={DefaultCodRegioneServizi};cfMedico=PROVAX00X00X000Y",
                DigitalPrescriptionService.ServiceAnagPrescrittore =>
                    "pinCode=1234567890;tipoOperazione=1",
                DigitalPrescriptionService.InvioDichiarazioneSostituzioneMedico =>
                    $"pinCode=1234567890;pwd=UTENTE;cfMedicoTitolare=PROVAX00X00X000Y;codRegione={DefaultCodRegioneServizi};codASLAo={DefaultCodAslAoServizi};codSpecializzazione=P;cfMedicoSostituto=RSSMRA80A01H501T;dataInizioSostituzione=2026-01-01;dataFineSostituzione=2026-01-31",
                DigitalPrescriptionService.InvioErogato =>
                    $"pinCode={DefaultPinCodeErogatore};codiceRegioneErogatore={DefaultCodRegioneErogatore};codiceAslErogatore={DefaultCodAslErogatore};codiceSsaErogatore={DefaultCodSsaErogatore};pwd={DefaultUsernameErogatore};nre=120000000000;cfAssistito={DefaultCfAssistito};tipoOperazione=1;prescrizioneFruita=1;dataSpedizione={DateTime.Now:yyyy-MM-dd HH:mm:ss};ElencoDettagliPrescrInviiErogato=ARRAY;ElencoDettagliPrescrInviiErogato_1_codProdPrest=89.7;ElencoDettagliPrescrInviiErogato_1_codProdPrestErog=89.7;ElencoDettagliPrescrInviiErogato_1_descrProdPrestErog=VISITA CARDIOLOGICA;ElencoDettagliPrescrInviiErogato_1_prezzo=36.15;ElencoDettagliPrescrInviiErogato_1_quantitaErogata=1;ElencoDettagliPrescrInviiErogato_1_dataIniErog={DateTime.Now:yyyy-MM-dd HH:mm:ss};ElencoDettagliPrescrInviiErogato_1_dataFineErog={DateTime.Now:yyyy-MM-dd HH:mm:ss}",
                DigitalPrescriptionService.VisualizzaErogato =>
                    $"pinCode={DefaultPinCodeErogatore};codiceRegioneErogatore={DefaultCodRegioneErogatore};codiceAslErogatore={DefaultCodAslErogatore};codiceSsaErogatore={DefaultCodSsaErogatore};nre=1900A4005322015;tipoOperazione=1;cfAssistito={DefaultCfAssistito}",
                DigitalPrescriptionService.SospendiErogato =>
                    $"pinCode={DefaultPinCodeErogatore};codiceRegioneErogatore={DefaultCodRegioneErogatore};codiceAslErogatore={DefaultCodAslErogatore};codiceSsaErogatore={DefaultCodSsaErogatore};nre=120000000000;tipoOperazione=S",
                DigitalPrescriptionService.AnnullaErogato =>
                    $"pinCode={DefaultPinCodeErogatore};codiceRegioneErogatore={DefaultCodRegioneErogatore};codiceAslErogatore={DefaultCodAslErogatore};codiceSsaErogatore={DefaultCodSsaErogatore};nre=120000000000;codAnnullamento=1",
                DigitalPrescriptionService.RicercaErogatore =>
                    $"pinCode={DefaultPinCodeErogatore};codiceRegioneErogatore={DefaultCodRegioneErogatore};codiceAslErogatore={DefaultCodAslErogatore};codiceSsaErogatore={DefaultCodSsaErogatore}",
                DigitalPrescriptionService.ReportErogatoMensile =>
                    $"pinCode={DefaultPinCodeErogatore};codiceRegioneErogatore={DefaultCodRegioneErogatore};codiceAslErogatore={DefaultCodAslErogatore};codiceSsaErogatore={DefaultCodSsaErogatore};annoMese=202601",
                DigitalPrescriptionService.ServiceAnagErogatore =>
                    $"pinCode={DefaultPinCodeErogatore};tipoOperazione=1",
                DigitalPrescriptionService.RicettaDifferita =>
                    $"pinCode={DefaultPinCodeErogatore};codiceRegioneErogatore={DefaultCodRegioneErogatore};codiceAslErogatore={DefaultCodAslErogatore};codiceSsaErogatore={DefaultCodSsaErogatore};codMotivazione=1;dataDal=2026-01-01",
                DigitalPrescriptionService.AnnullaErogatoDiff =>
                    $"pinCode={DefaultPinCodeErogatore};codiceRegioneErogatore={DefaultCodRegioneErogatore};codiceAslErogatore={DefaultCodAslErogatore};codiceSsaErogatore={DefaultCodSsaErogatore};idRicetta=1;nre=120000000000",
                DigitalPrescriptionService.RicevuteSac =>
                    $"pinCode={DefaultPinCodeErogatore}",
                _ => string.Empty
            };

        private void LoadTokensFromStorage()
        {
            try { var t = TokenManager.LoadToken("PRESCRITTORE"); if (!string.IsNullOrWhiteSpace(t)) _txtTokenP.Text = t; } catch { }
            try { var t = TokenManager.LoadToken("EROGATORE"); if (!string.IsNullOrWhiteSpace(t)) _txtTokenE.Text = t; } catch { }
        }

        private static string? ChiediTokenManuale(string ruolo)
        {
            using var form = new Form
            {
                Text = $"Inserisci token {ruolo}",
                Width = 500,
                Height = 160,
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };
            var lbl = new Label { Text = $"Il token è stato inviato via email. Incollarlo qui:", Left = 12, Top = 12, Width = 460 };
            var txt = new TextBox { Left = 12, Top = 34, Width = 460, Font = new System.Drawing.Font("Courier New", 9f) };
            var btnOk     = new Button { Text = "OK",     Left = 308, Top = 68, Width = 80, DialogResult = DialogResult.OK };
            var btnAnnulla = new Button { Text = "Annulla", Left = 394, Top = 68, Width = 80, DialogResult = DialogResult.Cancel };
            form.AcceptButton = btnOk;
            form.CancelButton = btnAnnulla;
            form.Controls.AddRange(new Control[] { lbl, txt, btnOk, btnAnnulla });
            return form.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(txt.Text)
                ? txt.Text.Trim()
                : null;
        }

        private void MostraDebug()
        {
            var debugInfo = SoapDebugCapture.GetLastDebugInfo();
            if (debugInfo == null)
            {
                MessageBox.Show("Nessuna richiesta SOAP catturata.", "Debug SOAP", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            var content = SoapDebugCapture.FormatDebugInfo(debugInfo);
            var form = new Form
            {
                Text = "SOAP Debug",
                Width = 1000,
                Height = 700,
                StartPosition = FormStartPosition.CenterParent
            };
            var tb = new TextBox { Multiline = true, ReadOnly = true, Dock = DockStyle.Fill, Font = new System.Drawing.Font("Courier New", 9f), Text = content, ScrollBars = ScrollBars.Both };
            var btn = new Button { Text = "📋 Copia", Dock = DockStyle.Bottom, Height = 35 };
            btn.Click += (s, e2) => { try { Clipboard.SetText(content); } catch { } };
            form.Controls.Add(tb);
            form.Controls.Add(btn);
            form.ShowDialog(this);
        }
    }
}








