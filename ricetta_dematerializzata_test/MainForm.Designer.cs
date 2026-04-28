using System.Drawing;
using System.Windows.Forms;

namespace ricetta_dematerializzata_test_ui
{
    public partial class MainForm
    {
        // ── Header condiviso ──────────────────────────────────────────────────────
        private System.Windows.Forms.Label      lblUsername;
        private System.Windows.Forms.TextBox    _txtUsername;
        private System.Windows.Forms.Label      lblPassword;
        private System.Windows.Forms.TextBox    _txtPassword;
        private System.Windows.Forms.CheckBox   _chkIgnoraSsl;
        private System.Windows.Forms.CheckBox   _chkProduzione;

        // ── TabControl principale ─────────────────────────────────────────────────
        private System.Windows.Forms.TabControl _tabControl;
        private System.Windows.Forms.TabPage    _tabToken;
        private System.Windows.Forms.TabPage    _tabPrescrittore;
        private System.Windows.Forms.TabPage    _tabErogatore;

        // ── Tab Token A2F – GroupBox Prescrittore ─────────────────────────────────
        private System.Windows.Forms.GroupBox   _grpTokenP;
        private System.Windows.Forms.Label      lblTokenP;
        private System.Windows.Forms.TextBox    _txtTokenP;
        private System.Windows.Forms.Button     _btnCreateTokenP;
        private System.Windows.Forms.Button     _btnCheckTokenP;
        private System.Windows.Forms.Button     _btnRevokeTokenP;
        private System.Windows.Forms.Button     _btnInsertTokenP;
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
        private System.Windows.Forms.Label      lblStatoE;
        private System.Windows.Forms.TextBox    _txtStatoE;
        private System.Windows.Forms.Label      lblInizioE;
        private System.Windows.Forms.TextBox    _txtInizioE;
        private System.Windows.Forms.Label      lblFineE;
        private System.Windows.Forms.TextBox    _txtFineE;

        // ── Tab Token A2F – Output condiviso ──────────────────────────────────────
        private System.Windows.Forms.Button     _btnDebugSoapHeadersA2F;
        private System.Windows.Forms.Label      lblA2FOutput;
        private System.Windows.Forms.TextBox    _txtA2FOutput;

        // ── Tab Servizi Prescrittore ───────────────────────────────────────────────
        private System.Windows.Forms.Label      lblServizioP;
        private System.Windows.Forms.ComboBox   _cmbServizioP;
        private System.Windows.Forms.Label      lblAuth2FP;
        private System.Windows.Forms.TextBox    _txtAuth2FP;
        private System.Windows.Forms.Label      lblInputP;
        private System.Windows.Forms.TextBox    _txtInputP;
        private System.Windows.Forms.Button     _btnChiamaP;
        private System.Windows.Forms.Button     _btnDebugSoapP;
        private System.Windows.Forms.Label      lblOutputP;
        private System.Windows.Forms.TextBox    _txtOutputP;

        // ── Tab Servizi Erogatore ─────────────────────────────────────────────────
        private System.Windows.Forms.Label      lblServizioE;
        private System.Windows.Forms.ComboBox   _cmbServizioE;
        private System.Windows.Forms.Label      lblAuth2FE;
        private System.Windows.Forms.TextBox    _txtAuth2FE;
        private System.Windows.Forms.Label      lblInputE;
        private System.Windows.Forms.TextBox    _txtInputE;
        private System.Windows.Forms.Button     _btnChiamaE;
        private System.Windows.Forms.Button     _btnDebugSoapE;
        private System.Windows.Forms.Label      lblOutputE;
        private System.Windows.Forms.TextBox    _txtOutputE;

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // ── Istanze ───────────────────────────────────────────────────────────
            this.lblUsername         = new System.Windows.Forms.Label();
            this._txtUsername        = new System.Windows.Forms.TextBox();
            this.lblPassword         = new System.Windows.Forms.Label();
            this._txtPassword        = new System.Windows.Forms.TextBox();
            this._chkIgnoraSsl       = new System.Windows.Forms.CheckBox();
            this._chkProduzione      = new System.Windows.Forms.CheckBox();
            this._tabControl         = new System.Windows.Forms.TabControl();
            this._tabToken           = new System.Windows.Forms.TabPage();
            this._tabPrescrittore    = new System.Windows.Forms.TabPage();
            this._tabErogatore       = new System.Windows.Forms.TabPage();
            this._grpTokenP          = new System.Windows.Forms.GroupBox();
            this.lblTokenP           = new System.Windows.Forms.Label();
            this._txtTokenP          = new System.Windows.Forms.TextBox();
            this._btnCreateTokenP    = new System.Windows.Forms.Button();
            this._btnCheckTokenP     = new System.Windows.Forms.Button();
            this._btnRevokeTokenP    = new System.Windows.Forms.Button();
            this._btnInsertTokenP    = new System.Windows.Forms.Button();
            this.lblStatoP           = new System.Windows.Forms.Label();
            this._txtStatoP          = new System.Windows.Forms.TextBox();
            this.lblInizioP          = new System.Windows.Forms.Label();
            this._txtInizioP         = new System.Windows.Forms.TextBox();
            this.lblFineP            = new System.Windows.Forms.Label();
            this._txtFineP           = new System.Windows.Forms.TextBox();
            this._grpTokenE          = new System.Windows.Forms.GroupBox();
            this.lblTokenE           = new System.Windows.Forms.Label();
            this._txtTokenE          = new System.Windows.Forms.TextBox();
            this._btnCreateTokenE    = new System.Windows.Forms.Button();
            this._btnCheckTokenE     = new System.Windows.Forms.Button();
            this._btnRevokeTokenE    = new System.Windows.Forms.Button();
            this._btnInsertTokenE    = new System.Windows.Forms.Button();
            this.lblStatoE           = new System.Windows.Forms.Label();
            this._txtStatoE          = new System.Windows.Forms.TextBox();
            this.lblInizioE          = new System.Windows.Forms.Label();
            this._txtInizioE         = new System.Windows.Forms.TextBox();
            this.lblFineE            = new System.Windows.Forms.Label();
            this._txtFineE           = new System.Windows.Forms.TextBox();
            this._btnDebugSoapHeadersA2F = new System.Windows.Forms.Button();
            this.lblA2FOutput        = new System.Windows.Forms.Label();
            this._txtA2FOutput       = new System.Windows.Forms.TextBox();
            this.lblServizioP        = new System.Windows.Forms.Label();
            this._cmbServizioP       = new System.Windows.Forms.ComboBox();
            this.lblAuth2FP          = new System.Windows.Forms.Label();
            this._txtAuth2FP         = new System.Windows.Forms.TextBox();
            this.lblInputP           = new System.Windows.Forms.Label();
            this._txtInputP          = new System.Windows.Forms.TextBox();
            this._btnChiamaP         = new System.Windows.Forms.Button();
            this._btnDebugSoapP      = new System.Windows.Forms.Button();
            this.lblOutputP          = new System.Windows.Forms.Label();
            this._txtOutputP         = new System.Windows.Forms.TextBox();
            this.lblServizioE        = new System.Windows.Forms.Label();
            this._cmbServizioE       = new System.Windows.Forms.ComboBox();
            this.lblAuth2FE          = new System.Windows.Forms.Label();
            this._txtAuth2FE         = new System.Windows.Forms.TextBox();
            this.lblInputE           = new System.Windows.Forms.Label();
            this._txtInputE          = new System.Windows.Forms.TextBox();
            this._btnChiamaE         = new System.Windows.Forms.Button();
            this._btnDebugSoapE      = new System.Windows.Forms.Button();
            this.lblOutputE          = new System.Windows.Forms.Label();
            this._txtOutputE         = new System.Windows.Forms.TextBox();

            // ── Header ────────────────────────────────────────────────────────────
            this.lblUsername.Location  = new System.Drawing.Point(12, 14);
            this.lblUsername.Size      = new System.Drawing.Size(70, 20);
            this.lblUsername.Text      = "Username";

            this._txtUsername.Location = new System.Drawing.Point(85, 11);
            this._txtUsername.Size     = new System.Drawing.Size(180, 22);
            this._txtUsername.TabIndex = 0;

            this.lblPassword.Location      = new System.Drawing.Point(278, 14);
            this.lblPassword.Size          = new System.Drawing.Size(65, 20);
            this.lblPassword.Text          = "Password";

            this._txtPassword.Location     = new System.Drawing.Point(346, 11);
            this._txtPassword.Size         = new System.Drawing.Size(160, 22);
            this._txtPassword.PasswordChar = (char)9679;
            this._txtPassword.TabIndex     = 1;

            this._chkIgnoraSsl.Location  = new System.Drawing.Point(525, 12);
            this._chkIgnoraSsl.Size      = new System.Drawing.Size(110, 20);
            this._chkIgnoraSsl.Text      = "Ignora SSL";
            this._chkIgnoraSsl.TabIndex  = 2;

            this._chkProduzione.Location = new System.Drawing.Point(640, 12);
            this._chkProduzione.Size     = new System.Drawing.Size(105, 20);
            this._chkProduzione.Text     = "Produzione";
            this._chkProduzione.TabIndex = 3;

            // ── TabControl ────────────────────────────────────────────────────────
            this._tabControl.Location = new System.Drawing.Point(12, 44);
            this._tabControl.Size     = new System.Drawing.Size(1040, 680);
            this._tabControl.TabIndex = 4;
            this._tabControl.Controls.Add(this._tabToken);
            this._tabControl.Controls.Add(this._tabPrescrittore);
            this._tabControl.Controls.Add(this._tabErogatore);

            // ════════════════════════════════════════════════════════════════════
            // TAB 1 – TOKEN A2F
            // ════════════════════════════════════════════════════════════════════
            this._tabToken.Text    = "Token A2F";
            this._tabToken.Padding = new System.Windows.Forms.Padding(8);

            // ── GroupBox Prescrittore ─────────────────────────────────────────────
            this._grpTokenP.Text     = "Prescrittore (PRESCRITTORE)";
            this._grpTokenP.Location = new System.Drawing.Point(10, 8);
            this._grpTokenP.Size     = new System.Drawing.Size(1005, 145);
            this._grpTokenP.Font     = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold);

            this.lblTokenP.Text     = "Token (GUID)";
            this.lblTokenP.Location = new System.Drawing.Point(10, 22);
            this.lblTokenP.Size     = new System.Drawing.Size(85, 18);
            this.lblTokenP.Font     = new System.Drawing.Font("Segoe UI", 8.5f, System.Drawing.FontStyle.Regular);

            this._txtTokenP.Location = new System.Drawing.Point(98, 19);
            this._txtTokenP.Size     = new System.Drawing.Size(470, 22);
            this._txtTokenP.Font     = new System.Drawing.Font("Courier New", 9f);
            this._txtTokenP.TabIndex = 0;

            this._btnCreateTokenP.Location  = new System.Drawing.Point(10, 52);
            this._btnCreateTokenP.Size      = new System.Drawing.Size(130, 30);
            this._btnCreateTokenP.Text      = "🔑 Create";
            this._btnCreateTokenP.BackColor = System.Drawing.Color.LightGreen;
            this._btnCreateTokenP.TabIndex  = 1;
            this._btnCreateTokenP.Click    += new System.EventHandler(this.BtnCreateTokenP_Click);

            this._btnCheckTokenP.Location   = new System.Drawing.Point(148, 52);
            this._btnCheckTokenP.Size       = new System.Drawing.Size(130, 30);
            this._btnCheckTokenP.Text       = "✅ Check";
            this._btnCheckTokenP.BackColor  = System.Drawing.Color.LightYellow;
            this._btnCheckTokenP.TabIndex   = 2;
            this._btnCheckTokenP.Click     += new System.EventHandler(this.BtnCheckTokenP_Click);

            this._btnRevokeTokenP.Location  = new System.Drawing.Point(286, 52);
            this._btnRevokeTokenP.Size      = new System.Drawing.Size(130, 30);
            this._btnRevokeTokenP.Text      = "🗑 Revoke";
            this._btnRevokeTokenP.BackColor = System.Drawing.Color.LightCoral;
            this._btnRevokeTokenP.TabIndex  = 3;
            this._btnRevokeTokenP.Click    += new System.EventHandler(this.BtnRevokeTokenP_Click);

            this._btnInsertTokenP.Location  = new System.Drawing.Point(424, 52);
            this._btnInsertTokenP.Size      = new System.Drawing.Size(190, 30);
            this._btnInsertTokenP.Text      = "📋 Usa in Servizi Prescrittore";
            this._btnInsertTokenP.BackColor = System.Drawing.Color.LightBlue;
            this._btnInsertTokenP.TabIndex  = 4;
            this._btnInsertTokenP.Click    += new System.EventHandler(this.BtnInsertTokenP_Click);

            this.lblStatoP.Text     = "Stato";
            this.lblStatoP.Location = new System.Drawing.Point(10, 95);
            this.lblStatoP.Size     = new System.Drawing.Size(40, 18);
            this.lblStatoP.Font     = new System.Drawing.Font("Segoe UI", 8.5f, System.Drawing.FontStyle.Regular);

            this._txtStatoP.Location  = new System.Drawing.Point(53, 92);
            this._txtStatoP.Size      = new System.Drawing.Size(130, 22);
            this._txtStatoP.ReadOnly  = true;
            this._txtStatoP.BackColor = System.Drawing.Color.WhiteSmoke;
            this._txtStatoP.TabStop   = false;

            this.lblInizioP.Text     = "Valido dal";
            this.lblInizioP.Location = new System.Drawing.Point(196, 95);
            this.lblInizioP.Size     = new System.Drawing.Size(65, 18);
            this.lblInizioP.Font     = new System.Drawing.Font("Segoe UI", 8.5f, System.Drawing.FontStyle.Regular);

            this._txtInizioP.Location  = new System.Drawing.Point(264, 92);
            this._txtInizioP.Size      = new System.Drawing.Size(148, 22);
            this._txtInizioP.ReadOnly  = true;
            this._txtInizioP.BackColor = System.Drawing.Color.WhiteSmoke;
            this._txtInizioP.TabStop   = false;

            this.lblFineP.Text     = "Valido al";
            this.lblFineP.Location = new System.Drawing.Point(424, 95);
            this.lblFineP.Size     = new System.Drawing.Size(58, 18);
            this.lblFineP.Font     = new System.Drawing.Font("Segoe UI", 8.5f, System.Drawing.FontStyle.Regular);

            this._txtFineP.Location  = new System.Drawing.Point(485, 92);
            this._txtFineP.Size      = new System.Drawing.Size(148, 22);
            this._txtFineP.ReadOnly  = true;
            this._txtFineP.BackColor = System.Drawing.Color.WhiteSmoke;
            this._txtFineP.TabStop   = false;

            this._grpTokenP.Controls.AddRange(new System.Windows.Forms.Control[]
            {
                this.lblTokenP, this._txtTokenP,
                this._btnCreateTokenP, this._btnCheckTokenP, this._btnRevokeTokenP, this._btnInsertTokenP,
                this.lblStatoP, this._txtStatoP, this.lblInizioP, this._txtInizioP, this.lblFineP, this._txtFineP
            });

            // ── GroupBox Erogatore ────────────────────────────────────────────────
            this._grpTokenE.Text     = "Erogatore (EROGATORE)";
            this._grpTokenE.Location = new System.Drawing.Point(10, 162);
            this._grpTokenE.Size     = new System.Drawing.Size(1005, 145);
            this._grpTokenE.Font     = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold);

            this.lblTokenE.Text     = "Token (GUID)";
            this.lblTokenE.Location = new System.Drawing.Point(10, 22);
            this.lblTokenE.Size     = new System.Drawing.Size(85, 18);
            this.lblTokenE.Font     = new System.Drawing.Font("Segoe UI", 8.5f, System.Drawing.FontStyle.Regular);

            this._txtTokenE.Location = new System.Drawing.Point(98, 19);
            this._txtTokenE.Size     = new System.Drawing.Size(470, 22);
            this._txtTokenE.Font     = new System.Drawing.Font("Courier New", 9f);
            this._txtTokenE.TabIndex = 0;

            this._btnCreateTokenE.Location  = new System.Drawing.Point(10, 52);
            this._btnCreateTokenE.Size      = new System.Drawing.Size(130, 30);
            this._btnCreateTokenE.Text      = "🔑 Create";
            this._btnCreateTokenE.BackColor = System.Drawing.Color.LightGreen;
            this._btnCreateTokenE.TabIndex  = 1;
            this._btnCreateTokenE.Click    += new System.EventHandler(this.BtnCreateTokenE_Click);

            this._btnCheckTokenE.Location   = new System.Drawing.Point(148, 52);
            this._btnCheckTokenE.Size       = new System.Drawing.Size(130, 30);
            this._btnCheckTokenE.Text       = "✅ Check";
            this._btnCheckTokenE.BackColor  = System.Drawing.Color.LightYellow;
            this._btnCheckTokenE.TabIndex   = 2;
            this._btnCheckTokenE.Click     += new System.EventHandler(this.BtnCheckTokenE_Click);

            this._btnRevokeTokenE.Location  = new System.Drawing.Point(286, 52);
            this._btnRevokeTokenE.Size      = new System.Drawing.Size(130, 30);
            this._btnRevokeTokenE.Text      = "🗑 Revoke";
            this._btnRevokeTokenE.BackColor = System.Drawing.Color.LightCoral;
            this._btnRevokeTokenE.TabIndex  = 3;
            this._btnRevokeTokenE.Click    += new System.EventHandler(this.BtnRevokeTokenE_Click);

            this._btnInsertTokenE.Location  = new System.Drawing.Point(424, 52);
            this._btnInsertTokenE.Size      = new System.Drawing.Size(190, 30);
            this._btnInsertTokenE.Text      = "📋 Usa in Servizi Erogatore";
            this._btnInsertTokenE.BackColor = System.Drawing.Color.LightBlue;
            this._btnInsertTokenE.TabIndex  = 4;
            this._btnInsertTokenE.Click    += new System.EventHandler(this.BtnInsertTokenE_Click);

            this.lblStatoE.Text     = "Stato";
            this.lblStatoE.Location = new System.Drawing.Point(10, 95);
            this.lblStatoE.Size     = new System.Drawing.Size(40, 18);
            this.lblStatoE.Font     = new System.Drawing.Font("Segoe UI", 8.5f, System.Drawing.FontStyle.Regular);

            this._txtStatoE.Location  = new System.Drawing.Point(53, 92);
            this._txtStatoE.Size      = new System.Drawing.Size(130, 22);
            this._txtStatoE.ReadOnly  = true;
            this._txtStatoE.BackColor = System.Drawing.Color.WhiteSmoke;
            this._txtStatoE.TabStop   = false;

            this.lblInizioE.Text     = "Valido dal";
            this.lblInizioE.Location = new System.Drawing.Point(196, 95);
            this.lblInizioE.Size     = new System.Drawing.Size(65, 18);
            this.lblInizioE.Font     = new System.Drawing.Font("Segoe UI", 8.5f, System.Drawing.FontStyle.Regular);

            this._txtInizioE.Location  = new System.Drawing.Point(264, 92);
            this._txtInizioE.Size      = new System.Drawing.Size(148, 22);
            this._txtInizioE.ReadOnly  = true;
            this._txtInizioE.BackColor = System.Drawing.Color.WhiteSmoke;
            this._txtInizioE.TabStop   = false;

            this.lblFineE.Text     = "Valido al";
            this.lblFineE.Location = new System.Drawing.Point(424, 95);
            this.lblFineE.Size     = new System.Drawing.Size(58, 18);
            this.lblFineE.Font     = new System.Drawing.Font("Segoe UI", 8.5f, System.Drawing.FontStyle.Regular);

            this._txtFineE.Location  = new System.Drawing.Point(485, 92);
            this._txtFineE.Size      = new System.Drawing.Size(148, 22);
            this._txtFineE.ReadOnly  = true;
            this._txtFineE.BackColor = System.Drawing.Color.WhiteSmoke;
            this._txtFineE.TabStop   = false;

            this._grpTokenE.Controls.AddRange(new System.Windows.Forms.Control[]
            {
                this.lblTokenE, this._txtTokenE,
                this._btnCreateTokenE, this._btnCheckTokenE, this._btnRevokeTokenE, this._btnInsertTokenE,
                this.lblStatoE, this._txtStatoE, this.lblInizioE, this._txtInizioE, this.lblFineE, this._txtFineE
            });

            // ── Output A2F + Debug ─────────────────────────────────────────────────
            this._btnDebugSoapHeadersA2F.Location  = new System.Drawing.Point(10, 316);
            this._btnDebugSoapHeadersA2F.Size      = new System.Drawing.Size(150, 30);
            this._btnDebugSoapHeadersA2F.Text      = "🔍 Debug SOAP";
            this._btnDebugSoapHeadersA2F.BackColor = System.Drawing.Color.LightYellow;
            this._btnDebugSoapHeadersA2F.TabIndex  = 5;
            this._btnDebugSoapHeadersA2F.Click    += new System.EventHandler(this.BtnDebugSoapHeadersA2F_Click);

            this.lblA2FOutput.Text     = "Risposta A2F";
            this.lblA2FOutput.Location = new System.Drawing.Point(10, 355);
            this.lblA2FOutput.Size     = new System.Drawing.Size(100, 20);

            this._txtA2FOutput.Location   = new System.Drawing.Point(10, 377);
            this._txtA2FOutput.Size       = new System.Drawing.Size(1005, 248);
            this._txtA2FOutput.Multiline  = true;
            this._txtA2FOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this._txtA2FOutput.ReadOnly   = true;
            this._txtA2FOutput.Font       = new System.Drawing.Font("Courier New", 9f);
            this._txtA2FOutput.WordWrap   = false;
            this._txtA2FOutput.TabIndex   = 6;

            this._tabToken.Controls.AddRange(new System.Windows.Forms.Control[]
            {
                this._grpTokenP, this._grpTokenE,
                this._btnDebugSoapHeadersA2F, this.lblA2FOutput, this._txtA2FOutput
            });

            // ════════════════════════════════════════════════════════════════════
            // TAB 2 – SERVIZI PRESCRITTORE
            // ════════════════════════════════════════════════════════════════════
            this._tabPrescrittore.Text    = "Servizi Prescrittore";
            this._tabPrescrittore.Padding = new System.Windows.Forms.Padding(8);

            this.lblServizioP.Text     = "Servizio";
            this.lblServizioP.Location = new System.Drawing.Point(10, 14);
            this.lblServizioP.Size     = new System.Drawing.Size(60, 20);

            this._cmbServizioP.Location      = new System.Drawing.Point(10, 36);
            this._cmbServizioP.Size          = new System.Drawing.Size(480, 24);
            this._cmbServizioP.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbServizioP.TabIndex      = 0;

            this.lblAuth2FP.Text     = "Authorization2F (token sesssione Prescrittore)";
            this.lblAuth2FP.Location = new System.Drawing.Point(10, 76);
            this.lblAuth2FP.Size     = new System.Drawing.Size(340, 20);

            this._txtAuth2FP.Location = new System.Drawing.Point(10, 98);
            this._txtAuth2FP.Size     = new System.Drawing.Size(1005, 22);
            this._txtAuth2FP.Font     = new System.Drawing.Font("Courier New", 9f);
            this._txtAuth2FP.TabIndex = 1;

            this.lblInputP.Text     = "Input (key=value;key2=value2)";
            this.lblInputP.Location = new System.Drawing.Point(10, 136);
            this.lblInputP.Size     = new System.Drawing.Size(300, 20);

            this._txtInputP.Location = new System.Drawing.Point(10, 158);
            this._txtInputP.Size     = new System.Drawing.Size(1005, 22);
            this._txtInputP.Font     = new System.Drawing.Font("Courier New", 9f);
            this._txtInputP.TabIndex = 2;

            this._btnChiamaP.Location  = new System.Drawing.Point(10, 192);
            this._btnChiamaP.Size      = new System.Drawing.Size(160, 32);
            this._btnChiamaP.Text      = "▶ Chiama Servizio";
            this._btnChiamaP.BackColor = System.Drawing.Color.LightGreen;
            this._btnChiamaP.TabIndex  = 3;
            this._btnChiamaP.Click    += new System.EventHandler(this.BtnChiamaP_Click);

            this._btnDebugSoapP.Location  = new System.Drawing.Point(178, 192);
            this._btnDebugSoapP.Size      = new System.Drawing.Size(150, 32);
            this._btnDebugSoapP.Text      = "🔍 Debug SOAP";
            this._btnDebugSoapP.BackColor = System.Drawing.Color.LightYellow;
            this._btnDebugSoapP.TabIndex  = 4;
            this._btnDebugSoapP.Click    += new System.EventHandler(this.BtnDebugSoapP_Click);

            this.lblOutputP.Text     = "Output";
            this.lblOutputP.Location = new System.Drawing.Point(10, 240);
            this.lblOutputP.Size     = new System.Drawing.Size(60, 20);

            this._txtOutputP.Location   = new System.Drawing.Point(10, 262);
            this._txtOutputP.Size       = new System.Drawing.Size(1005, 358);
            this._txtOutputP.Multiline  = true;
            this._txtOutputP.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this._txtOutputP.ReadOnly   = true;
            this._txtOutputP.Font       = new System.Drawing.Font("Courier New", 9f);
            this._txtOutputP.WordWrap   = false;
            this._txtOutputP.TabIndex   = 5;

            this._tabPrescrittore.Controls.AddRange(new System.Windows.Forms.Control[]
            {
                this.lblServizioP, this._cmbServizioP,
                this.lblAuth2FP, this._txtAuth2FP,
                this.lblInputP, this._txtInputP,
                this._btnChiamaP, this._btnDebugSoapP,
                this.lblOutputP, this._txtOutputP
            });

            // ════════════════════════════════════════════════════════════════════
            // TAB 3 – SERVIZI EROGATORE
            // ════════════════════════════════════════════════════════════════════
            this._tabErogatore.Text    = "Servizi Erogatore";
            this._tabErogatore.Padding = new System.Windows.Forms.Padding(8);

            this.lblServizioE.Text     = "Servizio";
            this.lblServizioE.Location = new System.Drawing.Point(10, 14);
            this.lblServizioE.Size     = new System.Drawing.Size(60, 20);

            this._cmbServizioE.Location      = new System.Drawing.Point(10, 36);
            this._cmbServizioE.Size          = new System.Drawing.Size(480, 24);
            this._cmbServizioE.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbServizioE.TabIndex      = 0;

            this.lblAuth2FE.Text     = "Authorization2F (token sessione Erogatore)";
            this.lblAuth2FE.Location = new System.Drawing.Point(10, 76);
            this.lblAuth2FE.Size     = new System.Drawing.Size(340, 20);

            this._txtAuth2FE.Location = new System.Drawing.Point(10, 98);
            this._txtAuth2FE.Size     = new System.Drawing.Size(1005, 22);
            this._txtAuth2FE.Font     = new System.Drawing.Font("Courier New", 9f);
            this._txtAuth2FE.TabIndex = 1;

            this.lblInputE.Text     = "Input (key=value;key2=value2)";
            this.lblInputE.Location = new System.Drawing.Point(10, 136);
            this.lblInputE.Size     = new System.Drawing.Size(300, 20);

            this._txtInputE.Location = new System.Drawing.Point(10, 158);
            this._txtInputE.Size     = new System.Drawing.Size(1005, 22);
            this._txtInputE.Font     = new System.Drawing.Font("Courier New", 9f);
            this._txtInputE.TabIndex = 2;

            this._btnChiamaE.Location  = new System.Drawing.Point(10, 192);
            this._btnChiamaE.Size      = new System.Drawing.Size(160, 32);
            this._btnChiamaE.Text      = "▶ Chiama Servizio";
            this._btnChiamaE.BackColor = System.Drawing.Color.LightGreen;
            this._btnChiamaE.TabIndex  = 3;
            this._btnChiamaE.Click    += new System.EventHandler(this.BtnChiamaE_Click);

            this._btnDebugSoapE.Location  = new System.Drawing.Point(178, 192);
            this._btnDebugSoapE.Size      = new System.Drawing.Size(150, 32);
            this._btnDebugSoapE.Text      = "🔍 Debug SOAP";
            this._btnDebugSoapE.BackColor = System.Drawing.Color.LightYellow;
            this._btnDebugSoapE.TabIndex  = 4;
            this._btnDebugSoapE.Click    += new System.EventHandler(this.BtnDebugSoapE_Click);

            this.lblOutputE.Text     = "Output";
            this.lblOutputE.Location = new System.Drawing.Point(10, 240);
            this.lblOutputE.Size     = new System.Drawing.Size(60, 20);

            this._txtOutputE.Location   = new System.Drawing.Point(10, 262);
            this._txtOutputE.Size       = new System.Drawing.Size(1005, 358);
            this._txtOutputE.Multiline  = true;
            this._txtOutputE.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this._txtOutputE.ReadOnly   = true;
            this._txtOutputE.Font       = new System.Drawing.Font("Courier New", 9f);
            this._txtOutputE.WordWrap   = false;
            this._txtOutputE.TabIndex   = 5;

            this._tabErogatore.Controls.AddRange(new System.Windows.Forms.Control[]
            {
                this.lblServizioE, this._cmbServizioE,
                this.lblAuth2FE, this._txtAuth2FE,
                this.lblInputE, this._txtInputE,
                this._btnChiamaE, this._btnDebugSoapE,
                this.lblOutputE, this._txtOutputE
            });

            // ── Form ──────────────────────────────────────────────────────────────
            this.ClientSize    = new System.Drawing.Size(1064, 740);
            this.MinimumSize   = new System.Drawing.Size(1080, 780);
            this.Text          = "Ricetta Dematerializzata - Test Client";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

            this.Controls.AddRange(new System.Windows.Forms.Control[]
            {
                this.lblUsername, this._txtUsername,
                this.lblPassword, this._txtPassword,
                this._chkIgnoraSsl, this._chkProduzione,
                this._tabControl
            });

            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
