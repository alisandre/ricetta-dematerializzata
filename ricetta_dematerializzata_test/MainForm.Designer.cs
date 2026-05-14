using System.Drawing;
using System.Windows.Forms;

namespace ricetta_dematerializzata_test_ui
{
    public partial class MainForm
    {

        // ── TabControl principale ─────────────────────────────────────────────────
        private System.Windows.Forms.TabControl _tabControl;
        private System.Windows.Forms.TabPage    _tabToken;
        private System.Windows.Forms.TabPage    _tabTokenE;
        private System.Windows.Forms.TabPage    _tabPrescrittore;
        private System.Windows.Forms.TabPage    _tabErogatore;
        private System.Windows.Forms.TabPage    _tabCifra;

        // ── Tab Token A2F – GroupBox Prescrittore ─────────────────────────────────
        private System.Windows.Forms.GroupBox   _grpTokenP;
        private System.Windows.Forms.Label      lblTokenP;
        private System.Windows.Forms.TextBox    _txtTokenP;
        private System.Windows.Forms.Button     _btnCreateTokenP;
        private System.Windows.Forms.Button     _btnCheckTokenP;
        private System.Windows.Forms.Button     _btnRevokeTokenP;
        private System.Windows.Forms.Button     _btnInsertTokenP;
        // Parametri A2F Prescrittore
        private System.Windows.Forms.Label      lblA2FUserIdP;
        private System.Windows.Forms.TextBox    _txtA2FUserIdP;
        private System.Windows.Forms.Label      lblA2FCodRegioneP;
        private System.Windows.Forms.TextBox    _txtA2FCodRegioneP;
        private System.Windows.Forms.Label      lblA2FCodAslP;
        private System.Windows.Forms.TextBox    _txtA2FCodAslP;
        private System.Windows.Forms.Label      lblA2FCodSsaP;
        private System.Windows.Forms.TextBox    _txtA2FCodSsaP;
        private System.Windows.Forms.Label      lblA2FStrutturaP;
        private System.Windows.Forms.TextBox    _txtA2FStrutturaP;
        private System.Windows.Forms.Label      lblA2FIdentP;
        private System.Windows.Forms.TextBox    _txtA2FIdentP;
        private System.Windows.Forms.Label      lblStatoP;
        private System.Windows.Forms.TextBox    _txtStatoP;
        private System.Windows.Forms.Label      lblInizioP;
        private System.Windows.Forms.TextBox    _txtInizioP;
        private System.Windows.Forms.Label      lblFineP;
        private System.Windows.Forms.TextBox    _txtFineP;

        // ── Tab Token A2F – GroupBox Erogatore ────────────────────────────────────
        private System.Windows.Forms.GroupBox   _grpTokenE;
        private System.Windows.Forms.Label      lblTokenE;
        private System.Windows.Forms.TextBox    _txtTokenE;
        private System.Windows.Forms.Button     _btnCreateTokenE;
        private System.Windows.Forms.Button     _btnCheckTokenE;
        private System.Windows.Forms.Button     _btnRevokeTokenE;
        private System.Windows.Forms.Button     _btnInsertTokenE;
        // Parametri A2F Erogatore
        private System.Windows.Forms.Label      lblA2FUserIdE;
        private System.Windows.Forms.TextBox    _txtA2FUserIdE;
        private System.Windows.Forms.Label      lblA2FCodRegioneE;
        private System.Windows.Forms.TextBox    _txtA2FCodRegioneE;
        private System.Windows.Forms.Label      lblA2FCodAslE;
        private System.Windows.Forms.TextBox    _txtA2FCodAslE;
        private System.Windows.Forms.Label      lblA2FCodSsaE;
        private System.Windows.Forms.TextBox    _txtA2FCodSsaE;
        private System.Windows.Forms.Label      lblA2FStrutturaE;
        private System.Windows.Forms.TextBox    _txtA2FStrutturaE;
        private System.Windows.Forms.Label      lblA2FIdentE;
        private System.Windows.Forms.TextBox    _txtA2FIdentE;
        private System.Windows.Forms.Label      lblStatoE;
        private System.Windows.Forms.TextBox    _txtStatoE;
        private System.Windows.Forms.Label      lblInizioE;
        private System.Windows.Forms.TextBox    _txtInizioE;
        private System.Windows.Forms.Label      lblFineE;
        private System.Windows.Forms.TextBox    _txtFineE;

        // ── Tab Token A2F Prescrittore – Output ─────────────────────────────────
        private System.Windows.Forms.Button     _btnDebugSoapHeadersA2F;
        private System.Windows.Forms.TextBox    _txtA2FOutput;

        // ── Tab Token A2F Erogatore – Output ──────────────────────────────────────
        private System.Windows.Forms.Button     _btnDebugSoapHeadersA2FE;
        private System.Windows.Forms.TextBox    _txtA2FOutputE;

        // ── Tab Servizi Prescrittore ───────────────────────────────────────────────
        // Credenziali Prescrittore
        private System.Windows.Forms.Label      lblUsernameP;
        private System.Windows.Forms.TextBox    _txtUsername;
        private System.Windows.Forms.Label      lblPasswordP;
        private System.Windows.Forms.TextBox    _txtPassword;
        private System.Windows.Forms.Label      lblServizioP;
        private System.Windows.Forms.ComboBox   _cmbServizioP;
        private System.Windows.Forms.Label      lblAuth2FP;
        private System.Windows.Forms.TextBox    _txtAuth2FP;
        private System.Windows.Forms.Label      lblInputP;
        private System.Windows.Forms.TextBox    _txtInputP;
        private System.Windows.Forms.Button     _btnChiamaP;
        private System.Windows.Forms.Button     _btnDebugSoapP;
        private System.Windows.Forms.TextBox    _txtOutputP;

        // ── Tab Servizi Erogatore ─────────────────────────────────────────────────
        // Credenziali Erogatore
        private System.Windows.Forms.Label      lblUsernameE;
        private System.Windows.Forms.TextBox    _txtUsernameE;
        private System.Windows.Forms.Label      lblPasswordE;
        private System.Windows.Forms.TextBox    _txtPasswordE;
        private System.Windows.Forms.Label      lblServizioE;
        private System.Windows.Forms.ComboBox   _cmbServizioE;
        private System.Windows.Forms.Label      lblAuth2FE;
        private System.Windows.Forms.TextBox    _txtAuth2FE;
        private System.Windows.Forms.Label      lblInputE;
        private System.Windows.Forms.TextBox    _txtInputE;
        private System.Windows.Forms.Button     _btnChiamaE;
        private System.Windows.Forms.Button     _btnDebugSoapE;
        private System.Windows.Forms.TextBox    _txtOutputE;

        // ── Tab Cifra PinCode ─────────────────────────────────────────────────────
        private System.Windows.Forms.Label      lblCifraTesto;
        private System.Windows.Forms.TextBox    _txtCifraTesto;
        private System.Windows.Forms.Label      lblCifraCert;
        private System.Windows.Forms.TextBox    _txtCifraCert;
        private System.Windows.Forms.Button     _btnCifraCercaCert;
        private System.Windows.Forms.Button     _btnCifra;
        private System.Windows.Forms.Label      lblCifraRisultato;
        private System.Windows.Forms.TextBox    _txtCifraRisultato;
        private System.Windows.Forms.Button     _btnCifraCopia;
        private System.Windows.Forms.Label      lblCifraInfo;

        private void InitializeComponent()
        {
            this._tabControl = new System.Windows.Forms.TabControl();
            this._tabToken = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this._btnDebugSoapHeadersA2F = new System.Windows.Forms.Button();
            this._grpTokenP = new System.Windows.Forms.GroupBox();
            this.lblTokenP = new System.Windows.Forms.Label();
            this._txtTokenP = new System.Windows.Forms.TextBox();
            this._btnCreateTokenP = new System.Windows.Forms.Button();
            this._btnCheckTokenP = new System.Windows.Forms.Button();
            this._btnRevokeTokenP = new System.Windows.Forms.Button();
            this._btnInsertTokenP = new System.Windows.Forms.Button();
            this.lblA2FUserIdP = new System.Windows.Forms.Label();
            this._txtA2FUserIdP = new System.Windows.Forms.TextBox();
            this.lblA2FCodRegioneP = new System.Windows.Forms.Label();
            this._txtA2FCodRegioneP = new System.Windows.Forms.TextBox();
            this.lblA2FCodAslP = new System.Windows.Forms.Label();
            this._txtA2FCodAslP = new System.Windows.Forms.TextBox();
            this.lblA2FCodSsaP = new System.Windows.Forms.Label();
            this._txtA2FCodSsaP = new System.Windows.Forms.TextBox();
            this.lblA2FStrutturaP = new System.Windows.Forms.Label();
            this._txtA2FStrutturaP = new System.Windows.Forms.TextBox();
            this.lblA2FIdentP = new System.Windows.Forms.Label();
            this._txtA2FIdentP = new System.Windows.Forms.TextBox();
            this.lblStatoP = new System.Windows.Forms.Label();
            this._txtStatoP = new System.Windows.Forms.TextBox();
            this.lblInizioP = new System.Windows.Forms.Label();
            this._txtInizioP = new System.Windows.Forms.TextBox();
            this.lblFineP = new System.Windows.Forms.Label();
            this._txtFineP = new System.Windows.Forms.TextBox();
            this._grpTokenE = new System.Windows.Forms.GroupBox();
            this.lblTokenE = new System.Windows.Forms.Label();
            this._txtTokenE = new System.Windows.Forms.TextBox();
            this._btnCreateTokenE = new System.Windows.Forms.Button();
            this._btnCheckTokenE = new System.Windows.Forms.Button();
            this._btnRevokeTokenE = new System.Windows.Forms.Button();
            this._btnInsertTokenE = new System.Windows.Forms.Button();
            this.lblA2FUserIdE = new System.Windows.Forms.Label();
            this._txtA2FUserIdE = new System.Windows.Forms.TextBox();
            this.lblA2FCodRegioneE = new System.Windows.Forms.Label();
            this._txtA2FCodRegioneE = new System.Windows.Forms.TextBox();
            this.lblA2FCodAslE = new System.Windows.Forms.Label();
            this._txtA2FCodAslE = new System.Windows.Forms.TextBox();
            this.lblA2FCodSsaE = new System.Windows.Forms.Label();
            this._txtA2FCodSsaE = new System.Windows.Forms.TextBox();
            this.lblA2FStrutturaE = new System.Windows.Forms.Label();
            this._txtA2FStrutturaE = new System.Windows.Forms.TextBox();
            this.lblA2FIdentE = new System.Windows.Forms.Label();
            this._txtA2FIdentE = new System.Windows.Forms.TextBox();
            this.lblStatoE = new System.Windows.Forms.Label();
            this._txtStatoE = new System.Windows.Forms.TextBox();
            this.lblInizioE = new System.Windows.Forms.Label();
            this._txtInizioE = new System.Windows.Forms.TextBox();
            this.lblFineE = new System.Windows.Forms.Label();
            this._txtFineE = new System.Windows.Forms.TextBox();
            this._txtA2FOutput = new System.Windows.Forms.TextBox();
            this._tabTokenE = new System.Windows.Forms.TabPage();
            this.panel3 = new System.Windows.Forms.Panel();
            this._btnDebugSoapHeadersA2FE = new System.Windows.Forms.Button();
            this._txtA2FOutputE = new System.Windows.Forms.TextBox();
            this._tabPrescrittore = new System.Windows.Forms.TabPage();
            this.lblUsernameP = new System.Windows.Forms.Label();
            this._txtUsername = new System.Windows.Forms.TextBox();
            this.lblPasswordP = new System.Windows.Forms.Label();
            this._txtPassword = new System.Windows.Forms.TextBox();
            this.lblServizioP = new System.Windows.Forms.Label();
            this._cmbServizioP = new System.Windows.Forms.ComboBox();
            this.lblAuth2FP = new System.Windows.Forms.Label();
            this._txtAuth2FP = new System.Windows.Forms.TextBox();
            this.lblInputP = new System.Windows.Forms.Label();
            this._txtInputP = new System.Windows.Forms.TextBox();
            this._btnChiamaP = new System.Windows.Forms.Button();
            this._btnDebugSoapP = new System.Windows.Forms.Button();
            this._txtOutputP = new System.Windows.Forms.TextBox();
            this._tabErogatore = new System.Windows.Forms.TabPage();
            this.lblUsernameE = new System.Windows.Forms.Label();
            this._txtUsernameE = new System.Windows.Forms.TextBox();
            this.lblPasswordE = new System.Windows.Forms.Label();
            this._txtPasswordE = new System.Windows.Forms.TextBox();
            this.lblServizioE = new System.Windows.Forms.Label();
            this._cmbServizioE = new System.Windows.Forms.ComboBox();
            this.lblAuth2FE = new System.Windows.Forms.Label();
            this._txtAuth2FE = new System.Windows.Forms.TextBox();
            this.lblInputE = new System.Windows.Forms.Label();
            this._txtInputE = new System.Windows.Forms.TextBox();
            this._btnChiamaE = new System.Windows.Forms.Button();
            this._btnDebugSoapE = new System.Windows.Forms.Button();
            this._txtOutputE = new System.Windows.Forms.TextBox();
            this._tabCifra = new System.Windows.Forms.TabPage();
            this.lblCifraTesto = new System.Windows.Forms.Label();
            this._txtCifraTesto = new System.Windows.Forms.TextBox();
            this.lblCifraCert = new System.Windows.Forms.Label();
            this._txtCifraCert = new System.Windows.Forms.TextBox();
            this._btnCifraCercaCert = new System.Windows.Forms.Button();
            this._btnCifra = new System.Windows.Forms.Button();
            this.lblCifraRisultato = new System.Windows.Forms.Label();
            this._txtCifraRisultato = new System.Windows.Forms.TextBox();
            this._btnCifraCopia = new System.Windows.Forms.Button();
            this.lblCifraInfo = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this._chkProduzione = new System.Windows.Forms.CheckBox();
            this._tabControl.SuspendLayout();
            this._tabToken.SuspendLayout();
            this.panel2.SuspendLayout();
            this._grpTokenP.SuspendLayout();
            this._tabTokenE.SuspendLayout();
            this.panel3.SuspendLayout();
            this._grpTokenE.SuspendLayout();
            this._tabPrescrittore.SuspendLayout();
            this._tabErogatore.SuspendLayout();
            this._tabCifra.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // _tabControl
            // 
            this._tabControl.Controls.Add(this._tabToken);
            this._tabControl.Controls.Add(this._tabPrescrittore);
            this._tabControl.Controls.Add(this._tabTokenE);
            this._tabControl.Controls.Add(this._tabErogatore);
            this._tabControl.Controls.Add(this._tabCifra);
            this._tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tabControl.Location = new System.Drawing.Point(0, 32);
            this._tabControl.Name = "_tabControl";
            this._tabControl.SelectedIndex = 0;
            this._tabControl.Size = new System.Drawing.Size(1064, 809);
            this._tabControl.TabIndex = 2;
            // 
            // _tabToken
            // 
            this._tabToken.Controls.Add(this.panel2);
            this._tabToken.Controls.Add(this._grpTokenP);
            this._tabToken.Controls.Add(this._txtA2FOutput);
            this._tabToken.Location = new System.Drawing.Point(4, 22);
            this._tabToken.Name = "_tabToken";
            this._tabToken.Padding = new System.Windows.Forms.Padding(8);
            this._tabToken.Size = new System.Drawing.Size(1056, 783);
            this._tabToken.TabIndex = 0;
            this._tabToken.Text = "Token Prescrittore";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this._btnDebugSoapHeadersA2F);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(8, 468);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1040, 41);
            this.panel2.TabIndex = 7;
            // 
            // _btnDebugSoapHeadersA2F
            // 
            this._btnDebugSoapHeadersA2F.BackColor = System.Drawing.Color.LightYellow;
            this._btnDebugSoapHeadersA2F.Location = new System.Drawing.Point(3, 3);
            this._btnDebugSoapHeadersA2F.Name = "_btnDebugSoapHeadersA2F";
            this._btnDebugSoapHeadersA2F.Size = new System.Drawing.Size(150, 30);
            this._btnDebugSoapHeadersA2F.TabIndex = 5;
            this._btnDebugSoapHeadersA2F.Text = "🔍 Debug SOAP";
            this._btnDebugSoapHeadersA2F.UseVisualStyleBackColor = false;
            this._btnDebugSoapHeadersA2F.Click += new System.EventHandler(this.BtnDebugSoapHeadersA2F_Click);
            // 
            // _grpTokenP  (senza credenziali – solo token e parametri A2F)
            // 
            this._grpTokenP.Controls.Add(this.lblTokenP);
            this._grpTokenP.Controls.Add(this._txtTokenP);
            this._grpTokenP.Controls.Add(this._btnCreateTokenP);
            this._grpTokenP.Controls.Add(this._btnCheckTokenP);
            this._grpTokenP.Controls.Add(this._btnRevokeTokenP);
            this._grpTokenP.Controls.Add(this._btnInsertTokenP);
            this._grpTokenP.Controls.Add(this.lblA2FUserIdP);
            this._grpTokenP.Controls.Add(this._txtA2FUserIdP);
            this._grpTokenP.Controls.Add(this.lblA2FCodRegioneP);
            this._grpTokenP.Controls.Add(this._txtA2FCodRegioneP);
            this._grpTokenP.Controls.Add(this.lblA2FCodAslP);
            this._grpTokenP.Controls.Add(this._txtA2FCodAslP);
            this._grpTokenP.Controls.Add(this.lblA2FCodSsaP);
            this._grpTokenP.Controls.Add(this._txtA2FCodSsaP);
            this._grpTokenP.Controls.Add(this.lblA2FStrutturaP);
            this._grpTokenP.Controls.Add(this._txtA2FStrutturaP);
            this._grpTokenP.Controls.Add(this.lblA2FIdentP);
            this._grpTokenP.Controls.Add(this._txtA2FIdentP);
            this._grpTokenP.Controls.Add(this.lblStatoP);
            this._grpTokenP.Controls.Add(this._txtStatoP);
            this._grpTokenP.Controls.Add(this.lblInizioP);
            this._grpTokenP.Controls.Add(this._txtInizioP);
            this._grpTokenP.Controls.Add(this.lblFineP);
            this._grpTokenP.Controls.Add(this._txtFineP);
            this._grpTokenP.Dock = System.Windows.Forms.DockStyle.Top;
            this._grpTokenP.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this._grpTokenP.Location = new System.Drawing.Point(8, 238);
            this._grpTokenP.Name = "_grpTokenP";
            this._grpTokenP.Size = new System.Drawing.Size(1040, 230);
            this._grpTokenP.TabIndex = 0;
            this._grpTokenP.TabStop = false;
            this._grpTokenP.Text = "Prescrittore (PRESCRITTORE)";
            // 
            // lblTokenP
            // 
            this.lblTokenP.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblTokenP.Location = new System.Drawing.Point(10, 22);
            this.lblTokenP.Name = "lblTokenP";
            this.lblTokenP.Size = new System.Drawing.Size(85, 18);
            this.lblTokenP.TabIndex = 0;
            this.lblTokenP.Text = "Token (GUID)";
            // 
            // _txtTokenP
            // 
            this._txtTokenP.Font = new System.Drawing.Font("Courier New", 9F);
            this._txtTokenP.Location = new System.Drawing.Point(98, 19);
            this._txtTokenP.Name = "_txtTokenP";
            this._txtTokenP.Size = new System.Drawing.Size(420, 21);
            this._txtTokenP.TabIndex = 1;
            // 
            // _btnCreateTokenP
            // 
            this._btnCreateTokenP.BackColor = System.Drawing.Color.LightGreen;
            this._btnCreateTokenP.Location = new System.Drawing.Point(10, 48);
            this._btnCreateTokenP.Name = "_btnCreateTokenP";
            this._btnCreateTokenP.Size = new System.Drawing.Size(130, 30);
            this._btnCreateTokenP.TabIndex = 2;
            this._btnCreateTokenP.Text = "🔑 Create";
            this._btnCreateTokenP.UseVisualStyleBackColor = false;
            this._btnCreateTokenP.Click += new System.EventHandler(this.BtnCreateTokenP_Click);
            // 
            // _btnCheckTokenP
            // 
            this._btnCheckTokenP.BackColor = System.Drawing.Color.LightYellow;
            this._btnCheckTokenP.Location = new System.Drawing.Point(148, 48);
            this._btnCheckTokenP.Name = "_btnCheckTokenP";
            this._btnCheckTokenP.Size = new System.Drawing.Size(130, 30);
            this._btnCheckTokenP.TabIndex = 3;
            this._btnCheckTokenP.Text = "✅ Check";
            this._btnCheckTokenP.UseVisualStyleBackColor = false;
            this._btnCheckTokenP.Click += new System.EventHandler(this.BtnCheckTokenP_Click);
            // 
            // _btnRevokeTokenP
            // 
            this._btnRevokeTokenP.BackColor = System.Drawing.Color.LightCoral;
            this._btnRevokeTokenP.Location = new System.Drawing.Point(286, 48);
            this._btnRevokeTokenP.Name = "_btnRevokeTokenP";
            this._btnRevokeTokenP.Size = new System.Drawing.Size(130, 30);
            this._btnRevokeTokenP.TabIndex = 4;
            this._btnRevokeTokenP.Text = "🗑 Revoke";
            this._btnRevokeTokenP.UseVisualStyleBackColor = false;
            this._btnRevokeTokenP.Click += new System.EventHandler(this.BtnRevokeTokenP_Click);
            // 
            // _btnInsertTokenP
            // 
            this._btnInsertTokenP.BackColor = System.Drawing.Color.LightBlue;
            this._btnInsertTokenP.Location = new System.Drawing.Point(424, 48);
            this._btnInsertTokenP.Name = "_btnInsertTokenP";
            this._btnInsertTokenP.Size = new System.Drawing.Size(200, 30);
            this._btnInsertTokenP.TabIndex = 5;
            this._btnInsertTokenP.Text = "📋 Usa in Servizi Prescrittore";
            this._btnInsertTokenP.UseVisualStyleBackColor = false;
            this._btnInsertTokenP.Click += new System.EventHandler(this.BtnInsertTokenP_Click);
            // 
            // lblA2FUserIdP
            // 
            this.lblA2FUserIdP.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblA2FUserIdP.Location = new System.Drawing.Point(10, 88);
            this.lblA2FUserIdP.Name = "lblA2FUserIdP";
            this.lblA2FUserIdP.Size = new System.Drawing.Size(110, 17);
            this.lblA2FUserIdP.TabIndex = 20;
            this.lblA2FUserIdP.Text = "userId / cfUtente";
            // 
            // _txtA2FUserIdP
            // 
            this._txtA2FUserIdP.Font = new System.Drawing.Font("Courier New", 8.5F);
            this._txtA2FUserIdP.Location = new System.Drawing.Point(10, 107);
            this._txtA2FUserIdP.Name = "_txtA2FUserIdP";
            this._txtA2FUserIdP.Size = new System.Drawing.Size(165, 20);
            this._txtA2FUserIdP.TabIndex = 21;
            // 
            // lblA2FCodRegioneP
            // 
            this.lblA2FCodRegioneP.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblA2FCodRegioneP.Location = new System.Drawing.Point(185, 88);
            this.lblA2FCodRegioneP.Name = "lblA2FCodRegioneP";
            this.lblA2FCodRegioneP.Size = new System.Drawing.Size(72, 17);
            this.lblA2FCodRegioneP.TabIndex = 22;
            this.lblA2FCodRegioneP.Text = "codRegione";
            // 
            // _txtA2FCodRegioneP
            // 
            this._txtA2FCodRegioneP.Font = new System.Drawing.Font("Courier New", 8.5F);
            this._txtA2FCodRegioneP.Location = new System.Drawing.Point(185, 107);
            this._txtA2FCodRegioneP.Name = "_txtA2FCodRegioneP";
            this._txtA2FCodRegioneP.Size = new System.Drawing.Size(60, 20);
            this._txtA2FCodRegioneP.TabIndex = 23;
            // 
            // lblA2FCodAslP
            // 
            this.lblA2FCodAslP.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblA2FCodAslP.Location = new System.Drawing.Point(257, 88);
            this.lblA2FCodAslP.Name = "lblA2FCodAslP";
            this.lblA2FCodAslP.Size = new System.Drawing.Size(55, 17);
            this.lblA2FCodAslP.TabIndex = 24;
            this.lblA2FCodAslP.Text = "codAslAo";
            // 
            // _txtA2FCodAslP
            // 
            this._txtA2FCodAslP.Font = new System.Drawing.Font("Courier New", 8.5F);
            this._txtA2FCodAslP.Location = new System.Drawing.Point(257, 107);
            this._txtA2FCodAslP.Name = "_txtA2FCodAslP";
            this._txtA2FCodAslP.Size = new System.Drawing.Size(60, 20);
            this._txtA2FCodAslP.TabIndex = 25;
            // 
            // lblA2FCodSsaP
            // 
            this.lblA2FCodSsaP.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblA2FCodSsaP.Location = new System.Drawing.Point(329, 88);
            this.lblA2FCodSsaP.Name = "lblA2FCodSsaP";
            this.lblA2FCodSsaP.Size = new System.Drawing.Size(45, 17);
            this.lblA2FCodSsaP.TabIndex = 26;
            this.lblA2FCodSsaP.Text = "codSsa";
            // 
            // _txtA2FCodSsaP
            // 
            this._txtA2FCodSsaP.Font = new System.Drawing.Font("Courier New", 8.5F);
            this._txtA2FCodSsaP.Location = new System.Drawing.Point(329, 107);
            this._txtA2FCodSsaP.Name = "_txtA2FCodSsaP";
            this._txtA2FCodSsaP.Size = new System.Drawing.Size(65, 20);
            this._txtA2FCodSsaP.TabIndex = 27;
            // 
            // lblA2FStrutturaP
            // 
            this.lblA2FStrutturaP.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblA2FStrutturaP.Location = new System.Drawing.Point(406, 88);
            this.lblA2FStrutturaP.Name = "lblA2FStrutturaP";
            this.lblA2FStrutturaP.Size = new System.Drawing.Size(90, 17);
            this.lblA2FStrutturaP.TabIndex = 28;
            this.lblA2FStrutturaP.Text = "codiceStruttura";
            // 
            // _txtA2FStrutturaP
            // 
            this._txtA2FStrutturaP.Font = new System.Drawing.Font("Courier New", 8.5F);
            this._txtA2FStrutturaP.Location = new System.Drawing.Point(406, 107);
            this._txtA2FStrutturaP.Name = "_txtA2FStrutturaP";
            this._txtA2FStrutturaP.Size = new System.Drawing.Size(110, 20);
            this._txtA2FStrutturaP.TabIndex = 29;
            // 
            // lblA2FIdentP
            // 
            this.lblA2FIdentP.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblA2FIdentP.Location = new System.Drawing.Point(10, 136);
            this.lblA2FIdentP.Name = "lblA2FIdentP";
            this.lblA2FIdentP.Size = new System.Drawing.Size(130, 17);
            this.lblA2FIdentP.TabIndex = 30;
            this.lblA2FIdentP.Text = "identificativo (Base64)";
            // 
            // _txtA2FIdentP
            // 
            this._txtA2FIdentP.Font = new System.Drawing.Font("Courier New", 8.5F);
            this._txtA2FIdentP.Location = new System.Drawing.Point(10, 155);
            this._txtA2FIdentP.Name = "_txtA2FIdentP";
            this._txtA2FIdentP.Size = new System.Drawing.Size(880, 20);
            this._txtA2FIdentP.TabIndex = 31;
            // 
            // lblStatoP
            // 
            this.lblStatoP.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblStatoP.Location = new System.Drawing.Point(10, 189);
            this.lblStatoP.Name = "lblStatoP";
            this.lblStatoP.Size = new System.Drawing.Size(40, 18);
            this.lblStatoP.TabIndex = 5;
            this.lblStatoP.Text = "Stato";
            // 
            // _txtStatoP
            // 
            this._txtStatoP.BackColor = System.Drawing.Color.WhiteSmoke;
            this._txtStatoP.Location = new System.Drawing.Point(53, 186);
            this._txtStatoP.Name = "_txtStatoP";
            this._txtStatoP.ReadOnly = true;
            this._txtStatoP.Size = new System.Drawing.Size(130, 23);
            this._txtStatoP.TabIndex = 6;
            this._txtStatoP.TabStop = false;
            // 
            // lblInizioP
            // 
            this.lblInizioP.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblInizioP.Location = new System.Drawing.Point(196, 189);
            this.lblInizioP.Name = "lblInizioP";
            this.lblInizioP.Size = new System.Drawing.Size(65, 18);
            this.lblInizioP.TabIndex = 7;
            this.lblInizioP.Text = "Valido dal";
            // 
            // _txtInizioP
            // 
            this._txtInizioP.BackColor = System.Drawing.Color.WhiteSmoke;
            this._txtInizioP.Location = new System.Drawing.Point(264, 186);
            this._txtInizioP.Name = "_txtInizioP";
            this._txtInizioP.ReadOnly = true;
            this._txtInizioP.Size = new System.Drawing.Size(148, 23);
            this._txtInizioP.TabIndex = 8;
            this._txtInizioP.TabStop = false;
            // 
            // lblFineP
            // 
            this.lblFineP.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblFineP.Location = new System.Drawing.Point(424, 189);
            this.lblFineP.Name = "lblFineP";
            this.lblFineP.Size = new System.Drawing.Size(58, 18);
            this.lblFineP.TabIndex = 9;
            this.lblFineP.Text = "Valido al";
            // 
            // _txtFineP
            // 
            this._txtFineP.BackColor = System.Drawing.Color.WhiteSmoke;
            this._txtFineP.Location = new System.Drawing.Point(485, 186);
            this._txtFineP.Name = "_txtFineP";
            this._txtFineP.ReadOnly = true;
            this._txtFineP.Size = new System.Drawing.Size(148, 23);
            this._txtFineP.TabIndex = 10;
            this._txtFineP.TabStop = false;
            // 
            // _grpTokenE  (senza credenziali – solo token e parametri A2F)
            // 
            this._grpTokenE.Controls.Add(this.lblTokenE);
            this._grpTokenE.Controls.Add(this._txtTokenE);
            this._grpTokenE.Controls.Add(this._btnCreateTokenE);
            this._grpTokenE.Controls.Add(this._btnCheckTokenE);
            this._grpTokenE.Controls.Add(this._btnRevokeTokenE);
            this._grpTokenE.Controls.Add(this._btnInsertTokenE);
            this._grpTokenE.Controls.Add(this.lblA2FUserIdE);
            this._grpTokenE.Controls.Add(this._txtA2FUserIdE);
            this._grpTokenE.Controls.Add(this.lblA2FCodRegioneE);
            this._grpTokenE.Controls.Add(this._txtA2FCodRegioneE);
            this._grpTokenE.Controls.Add(this.lblA2FCodAslE);
            this._grpTokenE.Controls.Add(this._txtA2FCodAslE);
            this._grpTokenE.Controls.Add(this.lblA2FCodSsaE);
            this._grpTokenE.Controls.Add(this._txtA2FCodSsaE);
            this._grpTokenE.Controls.Add(this.lblA2FStrutturaE);
            this._grpTokenE.Controls.Add(this._txtA2FStrutturaE);
            this._grpTokenE.Controls.Add(this.lblA2FIdentE);
            this._grpTokenE.Controls.Add(this._txtA2FIdentE);
            this._grpTokenE.Controls.Add(this.lblStatoE);
            this._grpTokenE.Controls.Add(this._txtStatoE);
            this._grpTokenE.Controls.Add(this.lblInizioE);
            this._grpTokenE.Controls.Add(this._txtInizioE);
            this._grpTokenE.Controls.Add(this.lblFineE);
            this._grpTokenE.Controls.Add(this._txtFineE);
            this._grpTokenE.Dock = System.Windows.Forms.DockStyle.Top;
            this._grpTokenE.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this._grpTokenE.Location = new System.Drawing.Point(8, 8);
            this._grpTokenE.Name = "_grpTokenE";
            this._grpTokenE.Size = new System.Drawing.Size(1040, 230);
            this._grpTokenE.TabIndex = 1;
            this._grpTokenE.TabStop = false;
            this._grpTokenE.Text = "Erogatore (EROGATORE)";
            // 
            // lblTokenE
            // 
            this.lblTokenE.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblTokenE.Location = new System.Drawing.Point(10, 22);
            this.lblTokenE.Name = "lblTokenE";
            this.lblTokenE.Size = new System.Drawing.Size(85, 18);
            this.lblTokenE.TabIndex = 0;
            this.lblTokenE.Text = "Token (GUID)";
            // 
            // _txtTokenE
            // 
            this._txtTokenE.Font = new System.Drawing.Font("Courier New", 9F);
            this._txtTokenE.Location = new System.Drawing.Point(98, 19);
            this._txtTokenE.Name = "_txtTokenE";
            this._txtTokenE.Size = new System.Drawing.Size(420, 21);
            this._txtTokenE.TabIndex = 1;
            // 
            // _btnCreateTokenE
            // 
            this._btnCreateTokenE.BackColor = System.Drawing.Color.LightGreen;
            this._btnCreateTokenE.Location = new System.Drawing.Point(10, 48);
            this._btnCreateTokenE.Name = "_btnCreateTokenE";
            this._btnCreateTokenE.Size = new System.Drawing.Size(130, 30);
            this._btnCreateTokenE.TabIndex = 2;
            this._btnCreateTokenE.Text = "🔑 Create";
            this._btnCreateTokenE.UseVisualStyleBackColor = false;
            this._btnCreateTokenE.Click += new System.EventHandler(this.BtnCreateTokenE_Click);
            // 
            // _btnCheckTokenE
            // 
            this._btnCheckTokenE.BackColor = System.Drawing.Color.LightYellow;
            this._btnCheckTokenE.Location = new System.Drawing.Point(148, 48);
            this._btnCheckTokenE.Name = "_btnCheckTokenE";
            this._btnCheckTokenE.Size = new System.Drawing.Size(130, 30);
            this._btnCheckTokenE.TabIndex = 3;
            this._btnCheckTokenE.Text = "✅ Check";
            this._btnCheckTokenE.UseVisualStyleBackColor = false;
            this._btnCheckTokenE.Click += new System.EventHandler(this.BtnCheckTokenE_Click);
            // 
            // _btnRevokeTokenE
            // 
            this._btnRevokeTokenE.BackColor = System.Drawing.Color.LightCoral;
            this._btnRevokeTokenE.Location = new System.Drawing.Point(286, 48);
            this._btnRevokeTokenE.Name = "_btnRevokeTokenE";
            this._btnRevokeTokenE.Size = new System.Drawing.Size(130, 30);
            this._btnRevokeTokenE.TabIndex = 4;
            this._btnRevokeTokenE.Text = "🗑 Revoke";
            this._btnRevokeTokenE.UseVisualStyleBackColor = false;
            this._btnRevokeTokenE.Click += new System.EventHandler(this.BtnRevokeTokenE_Click);
            // 
            // _btnInsertTokenE
            // 
            this._btnInsertTokenE.BackColor = System.Drawing.Color.LightBlue;
            this._btnInsertTokenE.Location = new System.Drawing.Point(424, 48);
            this._btnInsertTokenE.Name = "_btnInsertTokenE";
            this._btnInsertTokenE.Size = new System.Drawing.Size(200, 30);
            this._btnInsertTokenE.TabIndex = 5;
            this._btnInsertTokenE.Text = "📋 Usa in Servizi Erogatore";
            this._btnInsertTokenE.UseVisualStyleBackColor = false;
            this._btnInsertTokenE.Click += new System.EventHandler(this.BtnInsertTokenE_Click);
            // 
            // lblA2FUserIdE
            // 
            this.lblA2FUserIdE.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblA2FUserIdE.Location = new System.Drawing.Point(10, 88);
            this.lblA2FUserIdE.Name = "lblA2FUserIdE";
            this.lblA2FUserIdE.Size = new System.Drawing.Size(110, 17);
            this.lblA2FUserIdE.TabIndex = 20;
            this.lblA2FUserIdE.Text = "userId / cfUtente";
            // 
            // _txtA2FUserIdE
            // 
            this._txtA2FUserIdE.Font = new System.Drawing.Font("Courier New", 8.5F);
            this._txtA2FUserIdE.Location = new System.Drawing.Point(10, 107);
            this._txtA2FUserIdE.Name = "_txtA2FUserIdE";
            this._txtA2FUserIdE.Size = new System.Drawing.Size(165, 20);
            this._txtA2FUserIdE.TabIndex = 21;
            // 
            // lblA2FCodRegioneE
            // 
            this.lblA2FCodRegioneE.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblA2FCodRegioneE.Location = new System.Drawing.Point(185, 88);
            this.lblA2FCodRegioneE.Name = "lblA2FCodRegioneE";
            this.lblA2FCodRegioneE.Size = new System.Drawing.Size(72, 17);
            this.lblA2FCodRegioneE.TabIndex = 22;
            this.lblA2FCodRegioneE.Text = "codRegione";
            // 
            // _txtA2FCodRegioneE
            // 
            this._txtA2FCodRegioneE.Font = new System.Drawing.Font("Courier New", 8.5F);
            this._txtA2FCodRegioneE.Location = new System.Drawing.Point(185, 107);
            this._txtA2FCodRegioneE.Name = "_txtA2FCodRegioneE";
            this._txtA2FCodRegioneE.Size = new System.Drawing.Size(60, 20);
            this._txtA2FCodRegioneE.TabIndex = 23;
            // 
            // lblA2FCodAslE
            // 
            this.lblA2FCodAslE.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblA2FCodAslE.Location = new System.Drawing.Point(257, 88);
            this.lblA2FCodAslE.Name = "lblA2FCodAslE";
            this.lblA2FCodAslE.Size = new System.Drawing.Size(55, 17);
            this.lblA2FCodAslE.TabIndex = 24;
            this.lblA2FCodAslE.Text = "codAslAo";
            // 
            // _txtA2FCodAslE
            // 
            this._txtA2FCodAslE.Font = new System.Drawing.Font("Courier New", 8.5F);
            this._txtA2FCodAslE.Location = new System.Drawing.Point(257, 107);
            this._txtA2FCodAslE.Name = "_txtA2FCodAslE";
            this._txtA2FCodAslE.Size = new System.Drawing.Size(60, 20);
            this._txtA2FCodAslE.TabIndex = 25;
            // 
            // lblA2FCodSsaE
            // 
            this.lblA2FCodSsaE.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblA2FCodSsaE.Location = new System.Drawing.Point(329, 88);
            this.lblA2FCodSsaE.Name = "lblA2FCodSsaE";
            this.lblA2FCodSsaE.Size = new System.Drawing.Size(45, 17);
            this.lblA2FCodSsaE.TabIndex = 26;
            this.lblA2FCodSsaE.Text = "codSsa";
            // 
            // _txtA2FCodSsaE
            // 
            this._txtA2FCodSsaE.Font = new System.Drawing.Font("Courier New", 8.5F);
            this._txtA2FCodSsaE.Location = new System.Drawing.Point(329, 107);
            this._txtA2FCodSsaE.Name = "_txtA2FCodSsaE";
            this._txtA2FCodSsaE.Size = new System.Drawing.Size(65, 20);
            this._txtA2FCodSsaE.TabIndex = 27;
            // 
            // lblA2FStrutturaE
            // 
            this.lblA2FStrutturaE.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblA2FStrutturaE.Location = new System.Drawing.Point(406, 88);
            this.lblA2FStrutturaE.Name = "lblA2FStrutturaE";
            this.lblA2FStrutturaE.Size = new System.Drawing.Size(90, 17);
            this.lblA2FStrutturaE.TabIndex = 28;
            this.lblA2FStrutturaE.Text = "codiceStruttura";
            // 
            // _txtA2FStrutturaE
            // 
            this._txtA2FStrutturaE.Font = new System.Drawing.Font("Courier New", 8.5F);
            this._txtA2FStrutturaE.Location = new System.Drawing.Point(406, 107);
            this._txtA2FStrutturaE.Name = "_txtA2FStrutturaE";
            this._txtA2FStrutturaE.Size = new System.Drawing.Size(110, 20);
            this._txtA2FStrutturaE.TabIndex = 29;
            // 
            // lblA2FIdentE
            // 
            this.lblA2FIdentE.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblA2FIdentE.Location = new System.Drawing.Point(10, 136);
            this.lblA2FIdentE.Name = "lblA2FIdentE";
            this.lblA2FIdentE.Size = new System.Drawing.Size(130, 17);
            this.lblA2FIdentE.TabIndex = 30;
            this.lblA2FIdentE.Text = "identificativo (Base64)";
            // 
            // _txtA2FIdentE
            // 
            this._txtA2FIdentE.Font = new System.Drawing.Font("Courier New", 8.5F);
            this._txtA2FIdentE.Location = new System.Drawing.Point(10, 155);
            this._txtA2FIdentE.Name = "_txtA2FIdentE";
            this._txtA2FIdentE.Size = new System.Drawing.Size(880, 20);
            this._txtA2FIdentE.TabIndex = 31;
            // 
            // lblStatoE
            // 
            this.lblStatoE.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblStatoE.Location = new System.Drawing.Point(10, 189);
            this.lblStatoE.Name = "lblStatoE";
            this.lblStatoE.Size = new System.Drawing.Size(40, 18);
            this.lblStatoE.TabIndex = 5;
            this.lblStatoE.Text = "Stato";
            // 
            // _txtStatoE
            // 
            this._txtStatoE.BackColor = System.Drawing.Color.WhiteSmoke;
            this._txtStatoE.Location = new System.Drawing.Point(53, 186);
            this._txtStatoE.Name = "_txtStatoE";
            this._txtStatoE.ReadOnly = true;
            this._txtStatoE.Size = new System.Drawing.Size(130, 23);
            this._txtStatoE.TabIndex = 6;
            this._txtStatoE.TabStop = false;
            // 
            // lblInizioE
            // 
            this.lblInizioE.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblInizioE.Location = new System.Drawing.Point(196, 189);
            this.lblInizioE.Name = "lblInizioE";
            this.lblInizioE.Size = new System.Drawing.Size(65, 18);
            this.lblInizioE.TabIndex = 7;
            this.lblInizioE.Text = "Valido dal";
            // 
            // _txtInizioE
            // 
            this._txtInizioE.BackColor = System.Drawing.Color.WhiteSmoke;
            this._txtInizioE.Location = new System.Drawing.Point(264, 186);
            this._txtInizioE.Name = "_txtInizioE";
            this._txtInizioE.ReadOnly = true;
            this._txtInizioE.Size = new System.Drawing.Size(148, 23);
            this._txtInizioE.TabIndex = 8;
            this._txtInizioE.TabStop = false;
            // 
            // lblFineE
            // 
            this.lblFineE.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblFineE.Location = new System.Drawing.Point(424, 189);
            this.lblFineE.Name = "lblFineE";
            this.lblFineE.Size = new System.Drawing.Size(58, 18);
            this.lblFineE.TabIndex = 9;
            this.lblFineE.Text = "Valido al";
            // 
            // _txtFineE
            // 
            this._txtFineE.BackColor = System.Drawing.Color.WhiteSmoke;
            this._txtFineE.Location = new System.Drawing.Point(485, 186);
            this._txtFineE.Name = "_txtFineE";
            this._txtFineE.ReadOnly = true;
            this._txtFineE.Size = new System.Drawing.Size(148, 23);
            this._txtFineE.TabIndex = 10;
            this._txtFineE.TabStop = false;
            // 
            // _txtA2FOutput
            // 
            this._txtA2FOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this._txtA2FOutput.Font = new System.Drawing.Font("Courier New", 9F);
            this._txtA2FOutput.Location = new System.Drawing.Point(13, 294);
            this._txtA2FOutput.Multiline = true;
            this._txtA2FOutput.Name = "_txtA2FOutput";
            this._txtA2FOutput.ReadOnly = true;
            this._txtA2FOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this._txtA2FOutput.Size = new System.Drawing.Size(1032, 481);
            this._txtA2FOutput.TabIndex = 6;
            this._txtA2FOutput.WordWrap = false;
            // 
            // _tabTokenE
            // 
            this._tabTokenE.Controls.Add(this.panel3);
            this._tabTokenE.Controls.Add(this._grpTokenE);
            this._tabTokenE.Controls.Add(this._txtA2FOutputE);
            this._tabTokenE.Location = new System.Drawing.Point(4, 22);
            this._tabTokenE.Name = "_tabTokenE";
            this._tabTokenE.Padding = new System.Windows.Forms.Padding(8);
            this._tabTokenE.Size = new System.Drawing.Size(1056, 783);
            this._tabTokenE.TabIndex = 4;
            this._tabTokenE.Text = "Token Erogatore";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this._btnDebugSoapHeadersA2FE);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(8, 238);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1040, 41);
            this.panel3.TabIndex = 8;
            // 
            // _btnDebugSoapHeadersA2FE
            // 
            this._btnDebugSoapHeadersA2FE.BackColor = System.Drawing.Color.LightYellow;
            this._btnDebugSoapHeadersA2FE.Location = new System.Drawing.Point(3, 3);
            this._btnDebugSoapHeadersA2FE.Name = "_btnDebugSoapHeadersA2FE";
            this._btnDebugSoapHeadersA2FE.Size = new System.Drawing.Size(150, 30);
            this._btnDebugSoapHeadersA2FE.TabIndex = 5;
            this._btnDebugSoapHeadersA2FE.Text = "🔍 Debug SOAP";
            this._btnDebugSoapHeadersA2FE.UseVisualStyleBackColor = false;
            this._btnDebugSoapHeadersA2FE.Click += new System.EventHandler(this.BtnDebugSoapHeadersA2F_Click);
            // 
            // _txtA2FOutputE
            // 
            this._txtA2FOutputE.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this._txtA2FOutputE.Font = new System.Drawing.Font("Courier New", 9F);
            this._txtA2FOutputE.Location = new System.Drawing.Point(13, 294);
            this._txtA2FOutputE.Multiline = true;
            this._txtA2FOutputE.Name = "_txtA2FOutputE";
            this._txtA2FOutputE.ReadOnly = true;
            this._txtA2FOutputE.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this._txtA2FOutputE.Size = new System.Drawing.Size(1032, 481);
            this._txtA2FOutputE.TabIndex = 7;
            this._txtA2FOutputE.WordWrap = false;
            // 
            // _tabPrescrittore  – credenziali in cima, poi servizio, auth2F, input, output
            // 
            this._tabPrescrittore.Controls.Add(this.lblUsernameP);
            this._tabPrescrittore.Controls.Add(this._txtUsername);
            this._tabPrescrittore.Controls.Add(this.lblPasswordP);
            this._tabPrescrittore.Controls.Add(this._txtPassword);
            this._tabPrescrittore.Controls.Add(this.lblServizioP);
            this._tabPrescrittore.Controls.Add(this._cmbServizioP);
            this._tabPrescrittore.Controls.Add(this.lblAuth2FP);
            this._tabPrescrittore.Controls.Add(this._txtAuth2FP);
            this._tabPrescrittore.Controls.Add(this.lblInputP);
            this._tabPrescrittore.Controls.Add(this._txtInputP);
            this._tabPrescrittore.Controls.Add(this._btnChiamaP);
            this._tabPrescrittore.Controls.Add(this._btnDebugSoapP);
            this._tabPrescrittore.Controls.Add(this._txtOutputP);
            this._tabPrescrittore.Location = new System.Drawing.Point(4, 22);
            this._tabPrescrittore.Name = "_tabPrescrittore";
            this._tabPrescrittore.Padding = new System.Windows.Forms.Padding(8);
            this._tabPrescrittore.Size = new System.Drawing.Size(1056, 783);
            this._tabPrescrittore.TabIndex = 1;
            this._tabPrescrittore.Text = "Servizi Prescrittore";
            // 
            // lblUsernameP  (R0 – credenziali)
            // 
            this.lblUsernameP.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblUsernameP.Location = new System.Drawing.Point(10, 14);
            this.lblUsernameP.Name = "lblUsernameP";
            this.lblUsernameP.Size = new System.Drawing.Size(62, 18);
            this.lblUsernameP.TabIndex = 10;
            this.lblUsernameP.Text = "Username";
            // 
            // _txtUsername
            // 
            this._txtUsername.Font = new System.Drawing.Font("Courier New", 8.5F);
            this._txtUsername.Location = new System.Drawing.Point(75, 11);
            this._txtUsername.Name = "_txtUsername";
            this._txtUsername.Size = new System.Drawing.Size(200, 20);
            this._txtUsername.TabIndex = 11;
            // 
            // lblPasswordP
            // 
            this.lblPasswordP.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblPasswordP.Location = new System.Drawing.Point(285, 14);
            this.lblPasswordP.Name = "lblPasswordP";
            this.lblPasswordP.Size = new System.Drawing.Size(62, 18);
            this.lblPasswordP.TabIndex = 12;
            this.lblPasswordP.Text = "Password";
            // 
            // _txtPassword
            // 
            this._txtPassword.Font = new System.Drawing.Font("Courier New", 8.5F);
            this._txtPassword.Location = new System.Drawing.Point(352, 11);
            this._txtPassword.Name = "_txtPassword";
            this._txtPassword.PasswordChar = '●';
            this._txtPassword.Size = new System.Drawing.Size(160, 20);
            this._txtPassword.TabIndex = 13;
            // 
            // lblServizioP
            // 
            this.lblServizioP.Location = new System.Drawing.Point(10, 44);
            this.lblServizioP.Name = "lblServizioP";
            this.lblServizioP.Size = new System.Drawing.Size(60, 20);
            this.lblServizioP.TabIndex = 0;
            this.lblServizioP.Text = "Servizio";
            // 
            // _cmbServizioP
            // 
            this._cmbServizioP.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbServizioP.Location = new System.Drawing.Point(10, 66);
            this._cmbServizioP.Name = "_cmbServizioP";
            this._cmbServizioP.Size = new System.Drawing.Size(480, 21);
            this._cmbServizioP.TabIndex = 0;
            // 
            // lblAuth2FP
            // 
            this.lblAuth2FP.Location = new System.Drawing.Point(10, 106);
            this.lblAuth2FP.Name = "lblAuth2FP";
            this.lblAuth2FP.Size = new System.Drawing.Size(340, 20);
            this.lblAuth2FP.TabIndex = 1;
            this.lblAuth2FP.Text = "Authorization2F (token sessione Prescrittore)";
            // 
            // _txtAuth2FP
            // 
            this._txtAuth2FP.Font = new System.Drawing.Font("Courier New", 9F);
            this._txtAuth2FP.Location = new System.Drawing.Point(10, 128);
            this._txtAuth2FP.Name = "_txtAuth2FP";
            this._txtAuth2FP.Size = new System.Drawing.Size(1005, 21);
            this._txtAuth2FP.TabIndex = 1;
            // 
            // lblInputP
            // 
            this.lblInputP.Location = new System.Drawing.Point(10, 166);
            this.lblInputP.Name = "lblInputP";
            this.lblInputP.Size = new System.Drawing.Size(300, 20);
            this.lblInputP.TabIndex = 2;
            this.lblInputP.Text = "Input (key=value;key2=value2)";
            // 
            // _txtInputP
            // 
            this._txtInputP.Font = new System.Drawing.Font("Courier New", 9F);
            this._txtInputP.Location = new System.Drawing.Point(10, 188);
            this._txtInputP.Name = "_txtInputP";
            this._txtInputP.Size = new System.Drawing.Size(1005, 21);
            this._txtInputP.TabIndex = 2;
            // 
            // _btnChiamaP
            // 
            this._btnChiamaP.BackColor = System.Drawing.Color.LightGreen;
            this._btnChiamaP.Location = new System.Drawing.Point(10, 222);
            this._btnChiamaP.Name = "_btnChiamaP";
            this._btnChiamaP.Size = new System.Drawing.Size(160, 32);
            this._btnChiamaP.TabIndex = 3;
            this._btnChiamaP.Text = "▶ Chiama Servizio";
            this._btnChiamaP.UseVisualStyleBackColor = false;
            this._btnChiamaP.Click += new System.EventHandler(this.BtnChiamaP_Click);
            // 
            // _btnDebugSoapP
            // 
            this._btnDebugSoapP.BackColor = System.Drawing.Color.LightYellow;
            this._btnDebugSoapP.Location = new System.Drawing.Point(178, 222);
            this._btnDebugSoapP.Name = "_btnDebugSoapP";
            this._btnDebugSoapP.Size = new System.Drawing.Size(150, 32);
            this._btnDebugSoapP.TabIndex = 4;
            this._btnDebugSoapP.Text = "🔍 Debug SOAP";
            this._btnDebugSoapP.UseVisualStyleBackColor = false;
            this._btnDebugSoapP.Click += new System.EventHandler(this.BtnDebugSoapP_Click);
            // 
            // _txtOutputP
            // 
            this._txtOutputP.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this._txtOutputP.Font = new System.Drawing.Font("Courier New", 9F);
            this._txtOutputP.Location = new System.Drawing.Point(10, 262);
            this._txtOutputP.Multiline = true;
            this._txtOutputP.Name = "_txtOutputP";
            this._txtOutputP.ReadOnly = true;
            this._txtOutputP.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this._txtOutputP.Size = new System.Drawing.Size(1035, 510);
            this._txtOutputP.TabIndex = 5;
            this._txtOutputP.WordWrap = false;
            // 
            // _tabErogatore  – credenziali in cima, poi servizio, auth2F, input, output
            // 
            this._tabErogatore.Controls.Add(this.lblUsernameE);
            this._tabErogatore.Controls.Add(this._txtUsernameE);
            this._tabErogatore.Controls.Add(this.lblPasswordE);
            this._tabErogatore.Controls.Add(this._txtPasswordE);
            this._tabErogatore.Controls.Add(this.lblServizioE);
            this._tabErogatore.Controls.Add(this._cmbServizioE);
            this._tabErogatore.Controls.Add(this.lblAuth2FE);
            this._tabErogatore.Controls.Add(this._txtAuth2FE);
            this._tabErogatore.Controls.Add(this.lblInputE);
            this._tabErogatore.Controls.Add(this._txtInputE);
            this._tabErogatore.Controls.Add(this._btnChiamaE);
            this._tabErogatore.Controls.Add(this._btnDebugSoapE);
            this._tabErogatore.Controls.Add(this._txtOutputE);
            this._tabErogatore.Location = new System.Drawing.Point(4, 22);
            this._tabErogatore.Name = "_tabErogatore";
            this._tabErogatore.Padding = new System.Windows.Forms.Padding(8);
            this._tabErogatore.Size = new System.Drawing.Size(1056, 783);
            this._tabErogatore.TabIndex = 2;
            this._tabErogatore.Text = "Servizi Erogatore";
            // 
            // lblUsernameE  (R0 – credenziali)
            // 
            this.lblUsernameE.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblUsernameE.Location = new System.Drawing.Point(10, 14);
            this.lblUsernameE.Name = "lblUsernameE";
            this.lblUsernameE.Size = new System.Drawing.Size(62, 18);
            this.lblUsernameE.TabIndex = 10;
            this.lblUsernameE.Text = "Username";
            // 
            // _txtUsernameE
            // 
            this._txtUsernameE.Font = new System.Drawing.Font("Courier New", 8.5F);
            this._txtUsernameE.Location = new System.Drawing.Point(75, 11);
            this._txtUsernameE.Name = "_txtUsernameE";
            this._txtUsernameE.Size = new System.Drawing.Size(200, 20);
            this._txtUsernameE.TabIndex = 11;
            // 
            // lblPasswordE
            // 
            this.lblPasswordE.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblPasswordE.Location = new System.Drawing.Point(285, 14);
            this.lblPasswordE.Name = "lblPasswordE";
            this.lblPasswordE.Size = new System.Drawing.Size(62, 18);
            this.lblPasswordE.TabIndex = 12;
            this.lblPasswordE.Text = "Password";
            // 
            // _txtPasswordE
            // 
            this._txtPasswordE.Font = new System.Drawing.Font("Courier New", 8.5F);
            this._txtPasswordE.Location = new System.Drawing.Point(352, 11);
            this._txtPasswordE.Name = "_txtPasswordE";
            this._txtPasswordE.PasswordChar = '●';
            this._txtPasswordE.Size = new System.Drawing.Size(160, 20);
            this._txtPasswordE.TabIndex = 13;
            // 
            // lblServizioE
            // 
            this.lblServizioE.Location = new System.Drawing.Point(10, 44);
            this.lblServizioE.Name = "lblServizioE";
            this.lblServizioE.Size = new System.Drawing.Size(60, 20);
            this.lblServizioE.TabIndex = 0;
            this.lblServizioE.Text = "Servizio";
            // 
            // _cmbServizioE
            // 
            this._cmbServizioE.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbServizioE.Location = new System.Drawing.Point(10, 66);
            this._cmbServizioE.Name = "_cmbServizioE";
            this._cmbServizioE.Size = new System.Drawing.Size(480, 21);
            this._cmbServizioE.TabIndex = 0;
            // 
            // lblAuth2FE
            // 
            this.lblAuth2FE.Location = new System.Drawing.Point(10, 106);
            this.lblAuth2FE.Name = "lblAuth2FE";
            this.lblAuth2FE.Size = new System.Drawing.Size(340, 20);
            this.lblAuth2FE.TabIndex = 1;
            this.lblAuth2FE.Text = "Authorization2F (token sessione Erogatore)";
            // 
            // _txtAuth2FE
            // 
            this._txtAuth2FE.Font = new System.Drawing.Font("Courier New", 9F);
            this._txtAuth2FE.Location = new System.Drawing.Point(10, 128);
            this._txtAuth2FE.Name = "_txtAuth2FE";
            this._txtAuth2FE.Size = new System.Drawing.Size(1005, 21);
            this._txtAuth2FE.TabIndex = 1;
            // 
            // lblInputE
            // 
            this.lblInputE.Location = new System.Drawing.Point(10, 166);
            this.lblInputE.Name = "lblInputE";
            this.lblInputE.Size = new System.Drawing.Size(300, 20);
            this.lblInputE.TabIndex = 2;
            this.lblInputE.Text = "Input (key=value;key2=value2)";
            // 
            // _txtInputE
            // 
            this._txtInputE.Font = new System.Drawing.Font("Courier New", 9F);
            this._txtInputE.Location = new System.Drawing.Point(10, 188);
            this._txtInputE.Name = "_txtInputE";
            this._txtInputE.Size = new System.Drawing.Size(1005, 21);
            this._txtInputE.TabIndex = 2;
            // 
            // _btnChiamaE
            // 
            this._btnChiamaE.BackColor = System.Drawing.Color.LightGreen;
            this._btnChiamaE.Location = new System.Drawing.Point(10, 222);
            this._btnChiamaE.Name = "_btnChiamaE";
            this._btnChiamaE.Size = new System.Drawing.Size(160, 32);
            this._btnChiamaE.TabIndex = 3;
            this._btnChiamaE.Text = "▶ Chiama Servizio";
            this._btnChiamaE.UseVisualStyleBackColor = false;
            this._btnChiamaE.Click += new System.EventHandler(this.BtnChiamaE_Click);
            // 
            // _btnDebugSoapE
            // 
            this._btnDebugSoapE.BackColor = System.Drawing.Color.LightYellow;
            this._btnDebugSoapE.Location = new System.Drawing.Point(178, 222);
            this._btnDebugSoapE.Name = "_btnDebugSoapE";
            this._btnDebugSoapE.Size = new System.Drawing.Size(150, 32);
            this._btnDebugSoapE.TabIndex = 4;
            this._btnDebugSoapE.Text = "🔍 Debug SOAP";
            this._btnDebugSoapE.UseVisualStyleBackColor = false;
            this._btnDebugSoapE.Click += new System.EventHandler(this.BtnDebugSoapE_Click);
            // 
            // _txtOutputE
            // 
            this._txtOutputE.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this._txtOutputE.Font = new System.Drawing.Font("Courier New", 9F);
            this._txtOutputE.Location = new System.Drawing.Point(10, 262);
            this._txtOutputE.Multiline = true;
            this._txtOutputE.Name = "_txtOutputE";
            this._txtOutputE.ReadOnly = true;
            this._txtOutputE.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this._txtOutputE.Size = new System.Drawing.Size(1035, 510);
            this._txtOutputE.TabIndex = 5;
            this._txtOutputE.WordWrap = false;
            // 
            // _tabCifra
            // 
            this._tabCifra.Controls.Add(this.lblCifraTesto);
            this._tabCifra.Controls.Add(this._txtCifraTesto);
            this._tabCifra.Controls.Add(this.lblCifraCert);
            this._tabCifra.Controls.Add(this._txtCifraCert);
            this._tabCifra.Controls.Add(this._btnCifraCercaCert);
            this._tabCifra.Controls.Add(this._btnCifra);
            this._tabCifra.Controls.Add(this.lblCifraRisultato);
            this._tabCifra.Controls.Add(this._txtCifraRisultato);
            this._tabCifra.Controls.Add(this._btnCifraCopia);
            this._tabCifra.Controls.Add(this.lblCifraInfo);
            this._tabCifra.Location = new System.Drawing.Point(4, 22);
            this._tabCifra.Name = "_tabCifra";
            this._tabCifra.Padding = new System.Windows.Forms.Padding(8);
            this._tabCifra.Size = new System.Drawing.Size(1056, 783);
            this._tabCifra.TabIndex = 3;
            this._tabCifra.Text = "Cifra PinCode";
            // 
            // lblCifraTesto
            // 
            this.lblCifraTesto.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblCifraTesto.Location = new System.Drawing.Point(10, 18);
            this.lblCifraTesto.Name = "lblCifraTesto";
            this.lblCifraTesto.Size = new System.Drawing.Size(65, 18);
            this.lblCifraTesto.TabIndex = 10;
            this.lblCifraTesto.Text = "PinCode";
            // 
            // _txtCifraTesto
            // 
            this._txtCifraTesto.Font = new System.Drawing.Font("Courier New", 9F);
            this._txtCifraTesto.Location = new System.Drawing.Point(82, 14);
            this._txtCifraTesto.Name = "_txtCifraTesto";
            this._txtCifraTesto.Size = new System.Drawing.Size(400, 21);
            this._txtCifraTesto.TabIndex = 11;
            // 
            // lblCifraCert
            // 
            this.lblCifraCert.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblCifraCert.Location = new System.Drawing.Point(10, 50);
            this.lblCifraCert.Name = "lblCifraCert";
            this.lblCifraCert.Size = new System.Drawing.Size(70, 18);
            this.lblCifraCert.TabIndex = 20;
            this.lblCifraCert.Text = "Certificato";
            // 
            // _txtCifraCert
            // 
            this._txtCifraCert.Font = new System.Drawing.Font("Courier New", 9F);
            this._txtCifraCert.Location = new System.Drawing.Point(82, 47);
            this._txtCifraCert.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right));
            this._txtCifraCert.Name = "_txtCifraCert";
            this._txtCifraCert.Size = new System.Drawing.Size(820, 21);
            this._txtCifraCert.TabIndex = 21;
            // 
            // _btnCifraCercaCert
            // 
            this._btnCifraCercaCert.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
            this._btnCifraCercaCert.BackColor = System.Drawing.Color.LightYellow;
            this._btnCifraCercaCert.Location = new System.Drawing.Point(912, 44);
            this._btnCifraCercaCert.Name = "_btnCifraCercaCert";
            this._btnCifraCercaCert.Size = new System.Drawing.Size(120, 28);
            this._btnCifraCercaCert.TabIndex = 22;
            this._btnCifraCercaCert.Text = "🔍 Cerca Cert";
            this._btnCifraCercaCert.UseVisualStyleBackColor = false;
            this._btnCifraCercaCert.Click += new System.EventHandler(this.BtnCifraCercaCert_Click);
            // 
            // _btnCifra
            // 
            this._btnCifra.BackColor = System.Drawing.Color.LightGreen;
            this._btnCifra.Location = new System.Drawing.Point(10, 88);
            this._btnCifra.Name = "_btnCifra";
            this._btnCifra.Size = new System.Drawing.Size(130, 30);
            this._btnCifra.TabIndex = 23;
            this._btnCifra.Text = "🔒 Cifra";
            this._btnCifra.UseVisualStyleBackColor = false;
            this._btnCifra.Click += new System.EventHandler(this.BtnCifra_Click);
            // 
            // lblCifraRisultato
            // 
            this.lblCifraRisultato.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblCifraRisultato.Location = new System.Drawing.Point(10, 134);
            this.lblCifraRisultato.Name = "lblCifraRisultato";
            this.lblCifraRisultato.Size = new System.Drawing.Size(70, 18);
            this.lblCifraRisultato.TabIndex = 24;
            this.lblCifraRisultato.Text = "Risultato";
            // 
            // _txtCifraRisultato
            // 
            this._txtCifraRisultato.Font = new System.Drawing.Font("Courier New", 9F);
            this._txtCifraRisultato.Location = new System.Drawing.Point(82, 131);
            this._txtCifraRisultato.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right));
            this._txtCifraRisultato.Name = "_txtCifraRisultato";
            this._txtCifraRisultato.ReadOnly = true;
            this._txtCifraRisultato.Size = new System.Drawing.Size(820, 21);
            this._txtCifraRisultato.TabIndex = 25;
            // 
            // _btnCifraCopia
            // 
            this._btnCifraCopia.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
            this._btnCifraCopia.BackColor = System.Drawing.Color.LightBlue;
            this._btnCifraCopia.Location = new System.Drawing.Point(912, 128);
            this._btnCifraCopia.Name = "_btnCifraCopia";
            this._btnCifraCopia.Size = new System.Drawing.Size(120, 28);
            this._btnCifraCopia.TabIndex = 26;
            this._btnCifraCopia.Text = "📋 Copia Risultato";
            this._btnCifraCopia.UseVisualStyleBackColor = false;
            this._btnCifraCopia.Click += new System.EventHandler(this.BtnCifraCopia_Click);
            // 
            // lblCifraInfo
            // 
            this.lblCifraInfo.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblCifraInfo.Location = new System.Drawing.Point(10, 168);
            this.lblCifraInfo.Name = "lblCifraInfo";
            this.lblCifraInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right));
            this.lblCifraInfo.Size = new System.Drawing.Size(1030, 18);
            this.lblCifraInfo.TabIndex = 27;
            this.lblCifraInfo.Text = "Info aggiuntive sul certificato e operazione di cifratura";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this._chkProduzione);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1064, 32);
            this.panel1.TabIndex = 1;
            // 
            // _chkProduzione
            // 
            this._chkProduzione.Location = new System.Drawing.Point(14, 8);
            this._chkProduzione.Name = "_chkProduzione";
            this._chkProduzione.Size = new System.Drawing.Size(105, 20);
            this._chkProduzione.TabIndex = 0;
            this._chkProduzione.Text = "Produzione";
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(1064, 841);
            this.Controls.Add(this._tabControl);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MinimumSize = new System.Drawing.Size(1080, 880);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ricetta Dematerializzata - Test Client";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this._tabControl.ResumeLayout(false);
            this._tabToken.ResumeLayout(false);
            this._tabToken.PerformLayout();
            this.panel2.ResumeLayout(false);
            this._grpTokenP.ResumeLayout(false);
            this._grpTokenP.PerformLayout();
            this._tabTokenE.ResumeLayout(false);
            this._tabTokenE.PerformLayout();
            this.panel3.ResumeLayout(false);
            this._grpTokenE.ResumeLayout(false);
            this._grpTokenE.PerformLayout();
            this._tabPrescrittore.ResumeLayout(false);
            this._tabPrescrittore.PerformLayout();
            this._tabErogatore.ResumeLayout(false);
            this._tabErogatore.PerformLayout();
            this._tabCifra.ResumeLayout(false);
            this._tabCifra.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private Panel  panel1;
        private CheckBox _chkProduzione;
        private Panel  panel2;
        private Panel  panel3;
    }
}
