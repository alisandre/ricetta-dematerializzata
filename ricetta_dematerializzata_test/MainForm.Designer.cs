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
        private System.Windows.Forms.Label      lblUsernameE;
        private System.Windows.Forms.TextBox    _txtUsernameE;
        private System.Windows.Forms.Label      lblPasswordE;
        private System.Windows.Forms.TextBox    _txtPasswordE;
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
            this.lblUsername = new System.Windows.Forms.Label();
            this._txtUsername = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this._txtPassword = new System.Windows.Forms.TextBox();
            this.lblUsernameE = new System.Windows.Forms.Label();
            this._txtUsernameE = new System.Windows.Forms.TextBox();
            this.lblPasswordE = new System.Windows.Forms.Label();
            this._txtPasswordE = new System.Windows.Forms.TextBox();
            this._chkProduzione = new System.Windows.Forms.CheckBox();
            this._tabControl = new System.Windows.Forms.TabControl();
            this._tabToken = new System.Windows.Forms.TabPage();
            this._grpTokenP = new System.Windows.Forms.GroupBox();
            this.lblTokenP = new System.Windows.Forms.Label();
            this._txtTokenP = new System.Windows.Forms.TextBox();
            this._btnCreateTokenP = new System.Windows.Forms.Button();
            this._btnCheckTokenP = new System.Windows.Forms.Button();
            this._btnRevokeTokenP = new System.Windows.Forms.Button();
            this._btnInsertTokenP = new System.Windows.Forms.Button();
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
            this.lblStatoE = new System.Windows.Forms.Label();
            this._txtStatoE = new System.Windows.Forms.TextBox();
            this.lblInizioE = new System.Windows.Forms.Label();
            this._txtInizioE = new System.Windows.Forms.TextBox();
            this.lblFineE = new System.Windows.Forms.Label();
            this._txtFineE = new System.Windows.Forms.TextBox();
            this._btnDebugSoapHeadersA2F = new System.Windows.Forms.Button();
            this.lblA2FOutput = new System.Windows.Forms.Label();
            this._txtA2FOutput = new System.Windows.Forms.TextBox();
            this._tabPrescrittore = new System.Windows.Forms.TabPage();
            this.lblServizioP = new System.Windows.Forms.Label();
            this._cmbServizioP = new System.Windows.Forms.ComboBox();
            this.lblAuth2FP = new System.Windows.Forms.Label();
            this._txtAuth2FP = new System.Windows.Forms.TextBox();
            this.lblInputP = new System.Windows.Forms.Label();
            this._txtInputP = new System.Windows.Forms.TextBox();
            this._btnChiamaP = new System.Windows.Forms.Button();
            this._btnDebugSoapP = new System.Windows.Forms.Button();
            this.lblOutputP = new System.Windows.Forms.Label();
            this._txtOutputP = new System.Windows.Forms.TextBox();
            this._tabErogatore = new System.Windows.Forms.TabPage();
            this.lblServizioE = new System.Windows.Forms.Label();
            this._cmbServizioE = new System.Windows.Forms.ComboBox();
            this.lblAuth2FE = new System.Windows.Forms.Label();
            this._txtAuth2FE = new System.Windows.Forms.TextBox();
            this.lblInputE = new System.Windows.Forms.Label();
            this._txtInputE = new System.Windows.Forms.TextBox();
            this._btnChiamaE = new System.Windows.Forms.Button();
            this._btnDebugSoapE = new System.Windows.Forms.Button();
            this.lblOutputE = new System.Windows.Forms.Label();
            this._txtOutputE = new System.Windows.Forms.TextBox();
            this._tabControl.SuspendLayout();
            this._tabToken.SuspendLayout();
            this._grpTokenP.SuspendLayout();
            this._grpTokenE.SuspendLayout();
            this._tabPrescrittore.SuspendLayout();
            this._tabErogatore.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblUsername
            // 
            this.lblUsername.Location = new System.Drawing.Point(12, 14);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(70, 20);
            this.lblUsername.TabIndex = 0;
            this.lblUsername.Text = "User Presc.";
            // 
            // _txtUsername
            // 
            this._txtUsername.Location = new System.Drawing.Point(85, 11);
            this._txtUsername.Name = "_txtUsername";
            this._txtUsername.Size = new System.Drawing.Size(180, 20);
            this._txtUsername.TabIndex = 0;
            // 
            // lblPassword
            // 
            this.lblPassword.Location = new System.Drawing.Point(278, 14);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(65, 20);
            this.lblPassword.TabIndex = 1;
            this.lblPassword.Text = "Pass Presc.";
            // 
            // _txtPassword
            // 
            this._txtPassword.Location = new System.Drawing.Point(346, 11);
            this._txtPassword.Name = "_txtPassword";
            this._txtPassword.PasswordChar = '●';
            this._txtPassword.Size = new System.Drawing.Size(160, 20);
            this._txtPassword.TabIndex = 1;
            // 
            // lblUsernameE
            // 
            this.lblUsernameE.Location = new System.Drawing.Point(12, 39);
            this.lblUsernameE.Name = "lblUsernameE";
            this.lblUsernameE.Size = new System.Drawing.Size(70, 20);
            this.lblUsernameE.TabIndex = 2;
            this.lblUsernameE.Text = "User Erog.";
            // 
            // _txtUsernameE
            // 
            this._txtUsernameE.Location = new System.Drawing.Point(85, 36);
            this._txtUsernameE.Name = "_txtUsernameE";
            this._txtUsernameE.Size = new System.Drawing.Size(180, 20);
            this._txtUsernameE.TabIndex = 3;
            // 
            // lblPasswordE
            // 
            this.lblPasswordE.Location = new System.Drawing.Point(278, 39);
            this.lblPasswordE.Name = "lblPasswordE";
            this.lblPasswordE.Size = new System.Drawing.Size(65, 20);
            this.lblPasswordE.TabIndex = 4;
            this.lblPasswordE.Text = "Pass Erog.";
            // 
            // _txtPasswordE
            // 
            this._txtPasswordE.Location = new System.Drawing.Point(346, 36);
            this._txtPasswordE.Name = "_txtPasswordE";
            this._txtPasswordE.PasswordChar = '●';
            this._txtPasswordE.Size = new System.Drawing.Size(160, 20);
            this._txtPasswordE.TabIndex = 5;
            // 
            // _chkProduzione
            // 
            this._chkProduzione.Location = new System.Drawing.Point(525, 36);
            this._chkProduzione.Name = "_chkProduzione";
            this._chkProduzione.Size = new System.Drawing.Size(105, 20);
            this._chkProduzione.TabIndex = 6;
            this._chkProduzione.Text = "Produzione";
            // 
            // _tabControl
            // 
            this._tabControl.Controls.Add(this._tabToken);
            this._tabControl.Controls.Add(this._tabPrescrittore);
            this._tabControl.Controls.Add(this._tabErogatore);
            this._tabControl.Location = new System.Drawing.Point(12, 69);
            this._tabControl.Name = "_tabControl";
            this._tabControl.SelectedIndex = 0;
            this._tabControl.Size = new System.Drawing.Size(1040, 680);
            this._tabControl.TabIndex = 4;
            // 
            // _tabToken
            // 
            this._tabToken.Controls.Add(this._grpTokenP);
            this._tabToken.Controls.Add(this._grpTokenE);
            this._tabToken.Controls.Add(this._btnDebugSoapHeadersA2F);
            this._tabToken.Controls.Add(this.lblA2FOutput);
            this._tabToken.Controls.Add(this._txtA2FOutput);
            this._tabToken.Location = new System.Drawing.Point(4, 22);
            this._tabToken.Name = "_tabToken";
            this._tabToken.Padding = new System.Windows.Forms.Padding(8);
            this._tabToken.Size = new System.Drawing.Size(1032, 654);
            this._tabToken.TabIndex = 0;
            this._tabToken.Text = "Token A2F";
            // 
            // _grpTokenP
            // 
            this._grpTokenP.Controls.Add(this.lblTokenP);
            this._grpTokenP.Controls.Add(this._txtTokenP);
            this._grpTokenP.Controls.Add(this._btnCreateTokenP);
            this._grpTokenP.Controls.Add(this._btnCheckTokenP);
            this._grpTokenP.Controls.Add(this._btnRevokeTokenP);
            this._grpTokenP.Controls.Add(this._btnInsertTokenP);
            this._grpTokenP.Controls.Add(this.lblStatoP);
            this._grpTokenP.Controls.Add(this._txtStatoP);
            this._grpTokenP.Controls.Add(this.lblInizioP);
            this._grpTokenP.Controls.Add(this._txtInizioP);
            this._grpTokenP.Controls.Add(this.lblFineP);
            this._grpTokenP.Controls.Add(this._txtFineP);
            this._grpTokenP.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this._grpTokenP.Location = new System.Drawing.Point(10, 8);
            this._grpTokenP.Name = "_grpTokenP";
            this._grpTokenP.Size = new System.Drawing.Size(1005, 145);
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
            this._txtTokenP.Size = new System.Drawing.Size(470, 21);
            this._txtTokenP.TabIndex = 0;
            // 
            // _btnCreateTokenP
            // 
            this._btnCreateTokenP.BackColor = System.Drawing.Color.LightGreen;
            this._btnCreateTokenP.Location = new System.Drawing.Point(10, 52);
            this._btnCreateTokenP.Name = "_btnCreateTokenP";
            this._btnCreateTokenP.Size = new System.Drawing.Size(130, 30);
            this._btnCreateTokenP.TabIndex = 1;
            this._btnCreateTokenP.Text = "🔑 Create";
            this._btnCreateTokenP.UseVisualStyleBackColor = false;
            this._btnCreateTokenP.Click += new System.EventHandler(this.BtnCreateTokenP_Click);
            // 
            // _btnCheckTokenP
            // 
            this._btnCheckTokenP.BackColor = System.Drawing.Color.LightYellow;
            this._btnCheckTokenP.Location = new System.Drawing.Point(148, 52);
            this._btnCheckTokenP.Name = "_btnCheckTokenP";
            this._btnCheckTokenP.Size = new System.Drawing.Size(130, 30);
            this._btnCheckTokenP.TabIndex = 2;
            this._btnCheckTokenP.Text = "✅ Check";
            this._btnCheckTokenP.UseVisualStyleBackColor = false;
            this._btnCheckTokenP.Click += new System.EventHandler(this.BtnCheckTokenP_Click);
            // 
            // _btnRevokeTokenP
            // 
            this._btnRevokeTokenP.BackColor = System.Drawing.Color.LightCoral;
            this._btnRevokeTokenP.Location = new System.Drawing.Point(286, 52);
            this._btnRevokeTokenP.Name = "_btnRevokeTokenP";
            this._btnRevokeTokenP.Size = new System.Drawing.Size(130, 30);
            this._btnRevokeTokenP.TabIndex = 3;
            this._btnRevokeTokenP.Text = "🗑 Revoke";
            this._btnRevokeTokenP.UseVisualStyleBackColor = false;
            this._btnRevokeTokenP.Click += new System.EventHandler(this.BtnRevokeTokenP_Click);
            // 
            // _btnInsertTokenP
            // 
            this._btnInsertTokenP.BackColor = System.Drawing.Color.LightBlue;
            this._btnInsertTokenP.Location = new System.Drawing.Point(424, 52);
            this._btnInsertTokenP.Name = "_btnInsertTokenP";
            this._btnInsertTokenP.Size = new System.Drawing.Size(190, 30);
            this._btnInsertTokenP.TabIndex = 4;
            this._btnInsertTokenP.Text = "📋 Usa in Servizi Prescrittore";
            this._btnInsertTokenP.UseVisualStyleBackColor = false;
            this._btnInsertTokenP.Click += new System.EventHandler(this.BtnInsertTokenP_Click);
            // 
            // lblStatoP
            // 
            this.lblStatoP.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblStatoP.Location = new System.Drawing.Point(10, 95);
            this.lblStatoP.Name = "lblStatoP";
            this.lblStatoP.Size = new System.Drawing.Size(40, 18);
            this.lblStatoP.TabIndex = 5;
            this.lblStatoP.Text = "Stato";
            // 
            // _txtStatoP
            // 
            this._txtStatoP.BackColor = System.Drawing.Color.WhiteSmoke;
            this._txtStatoP.Location = new System.Drawing.Point(53, 92);
            this._txtStatoP.Name = "_txtStatoP";
            this._txtStatoP.ReadOnly = true;
            this._txtStatoP.Size = new System.Drawing.Size(130, 23);
            this._txtStatoP.TabIndex = 6;
            this._txtStatoP.TabStop = false;
            // 
            // lblInizioP
            // 
            this.lblInizioP.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblInizioP.Location = new System.Drawing.Point(196, 95);
            this.lblInizioP.Name = "lblInizioP";
            this.lblInizioP.Size = new System.Drawing.Size(65, 18);
            this.lblInizioP.TabIndex = 7;
            this.lblInizioP.Text = "Valido dal";
            // 
            // _txtInizioP
            // 
            this._txtInizioP.BackColor = System.Drawing.Color.WhiteSmoke;
            this._txtInizioP.Location = new System.Drawing.Point(264, 92);
            this._txtInizioP.Name = "_txtInizioP";
            this._txtInizioP.ReadOnly = true;
            this._txtInizioP.Size = new System.Drawing.Size(148, 23);
            this._txtInizioP.TabIndex = 8;
            this._txtInizioP.TabStop = false;
            // 
            // lblFineP
            // 
            this.lblFineP.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblFineP.Location = new System.Drawing.Point(424, 95);
            this.lblFineP.Name = "lblFineP";
            this.lblFineP.Size = new System.Drawing.Size(58, 18);
            this.lblFineP.TabIndex = 9;
            this.lblFineP.Text = "Valido al";
            // 
            // _txtFineP
            // 
            this._txtFineP.BackColor = System.Drawing.Color.WhiteSmoke;
            this._txtFineP.Location = new System.Drawing.Point(485, 92);
            this._txtFineP.Name = "_txtFineP";
            this._txtFineP.ReadOnly = true;
            this._txtFineP.Size = new System.Drawing.Size(148, 23);
            this._txtFineP.TabIndex = 10;
            this._txtFineP.TabStop = false;
            // 
            // _grpTokenE
            // 
            this._grpTokenE.Controls.Add(this.lblTokenE);
            this._grpTokenE.Controls.Add(this._txtTokenE);
            this._grpTokenE.Controls.Add(this._btnCreateTokenE);
            this._grpTokenE.Controls.Add(this._btnCheckTokenE);
            this._grpTokenE.Controls.Add(this._btnRevokeTokenE);
            this._grpTokenE.Controls.Add(this._btnInsertTokenE);
            this._grpTokenE.Controls.Add(this.lblStatoE);
            this._grpTokenE.Controls.Add(this._txtStatoE);
            this._grpTokenE.Controls.Add(this.lblInizioE);
            this._grpTokenE.Controls.Add(this._txtInizioE);
            this._grpTokenE.Controls.Add(this.lblFineE);
            this._grpTokenE.Controls.Add(this._txtFineE);
            this._grpTokenE.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this._grpTokenE.Location = new System.Drawing.Point(10, 162);
            this._grpTokenE.Name = "_grpTokenE";
            this._grpTokenE.Size = new System.Drawing.Size(1005, 145);
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
            this._txtTokenE.Size = new System.Drawing.Size(470, 21);
            this._txtTokenE.TabIndex = 0;
            // 
            // _btnCreateTokenE
            // 
            this._btnCreateTokenE.BackColor = System.Drawing.Color.LightGreen;
            this._btnCreateTokenE.Location = new System.Drawing.Point(10, 52);
            this._btnCreateTokenE.Name = "_btnCreateTokenE";
            this._btnCreateTokenE.Size = new System.Drawing.Size(130, 30);
            this._btnCreateTokenE.TabIndex = 1;
            this._btnCreateTokenE.Text = "🔑 Create";
            this._btnCreateTokenE.UseVisualStyleBackColor = false;
            this._btnCreateTokenE.Click += new System.EventHandler(this.BtnCreateTokenE_Click);
            // 
            // _btnCheckTokenE
            // 
            this._btnCheckTokenE.BackColor = System.Drawing.Color.LightYellow;
            this._btnCheckTokenE.Location = new System.Drawing.Point(148, 52);
            this._btnCheckTokenE.Name = "_btnCheckTokenE";
            this._btnCheckTokenE.Size = new System.Drawing.Size(130, 30);
            this._btnCheckTokenE.TabIndex = 2;
            this._btnCheckTokenE.Text = "✅ Check";
            this._btnCheckTokenE.UseVisualStyleBackColor = false;
            this._btnCheckTokenE.Click += new System.EventHandler(this.BtnCheckTokenE_Click);
            // 
            // _btnRevokeTokenE
            // 
            this._btnRevokeTokenE.BackColor = System.Drawing.Color.LightCoral;
            this._btnRevokeTokenE.Location = new System.Drawing.Point(286, 52);
            this._btnRevokeTokenE.Name = "_btnRevokeTokenE";
            this._btnRevokeTokenE.Size = new System.Drawing.Size(130, 30);
            this._btnRevokeTokenE.TabIndex = 3;
            this._btnRevokeTokenE.Text = "🗑 Revoke";
            this._btnRevokeTokenE.UseVisualStyleBackColor = false;
            this._btnRevokeTokenE.Click += new System.EventHandler(this.BtnRevokeTokenE_Click);
            // 
            // _btnInsertTokenE
            // 
            this._btnInsertTokenE.BackColor = System.Drawing.Color.LightBlue;
            this._btnInsertTokenE.Location = new System.Drawing.Point(424, 52);
            this._btnInsertTokenE.Name = "_btnInsertTokenE";
            this._btnInsertTokenE.Size = new System.Drawing.Size(190, 30);
            this._btnInsertTokenE.TabIndex = 4;
            this._btnInsertTokenE.Text = "📋 Usa in Servizi Erogatore";
            this._btnInsertTokenE.UseVisualStyleBackColor = false;
            this._btnInsertTokenE.Click += new System.EventHandler(this.BtnInsertTokenE_Click);
            // 
            // lblStatoE
            // 
            this.lblStatoE.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblStatoE.Location = new System.Drawing.Point(10, 95);
            this.lblStatoE.Name = "lblStatoE";
            this.lblStatoE.Size = new System.Drawing.Size(40, 18);
            this.lblStatoE.TabIndex = 5;
            this.lblStatoE.Text = "Stato";
            // 
            // _txtStatoE
            // 
            this._txtStatoE.BackColor = System.Drawing.Color.WhiteSmoke;
            this._txtStatoE.Location = new System.Drawing.Point(53, 92);
            this._txtStatoE.Name = "_txtStatoE";
            this._txtStatoE.ReadOnly = true;
            this._txtStatoE.Size = new System.Drawing.Size(130, 23);
            this._txtStatoE.TabIndex = 6;
            this._txtStatoE.TabStop = false;
            // 
            // lblInizioE
            // 
            this.lblInizioE.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblInizioE.Location = new System.Drawing.Point(196, 95);
            this.lblInizioE.Name = "lblInizioE";
            this.lblInizioE.Size = new System.Drawing.Size(65, 18);
            this.lblInizioE.TabIndex = 7;
            this.lblInizioE.Text = "Valido dal";
            // 
            // _txtInizioE
            // 
            this._txtInizioE.BackColor = System.Drawing.Color.WhiteSmoke;
            this._txtInizioE.Location = new System.Drawing.Point(264, 92);
            this._txtInizioE.Name = "_txtInizioE";
            this._txtInizioE.ReadOnly = true;
            this._txtInizioE.Size = new System.Drawing.Size(148, 23);
            this._txtInizioE.TabIndex = 8;
            this._txtInizioE.TabStop = false;
            // 
            // lblFineE
            // 
            this.lblFineE.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblFineE.Location = new System.Drawing.Point(424, 95);
            this.lblFineE.Name = "lblFineE";
            this.lblFineE.Size = new System.Drawing.Size(58, 18);
            this.lblFineE.TabIndex = 9;
            this.lblFineE.Text = "Valido al";
            // 
            // _txtFineE
            // 
            this._txtFineE.BackColor = System.Drawing.Color.WhiteSmoke;
            this._txtFineE.Location = new System.Drawing.Point(485, 92);
            this._txtFineE.Name = "_txtFineE";
            this._txtFineE.ReadOnly = true;
            this._txtFineE.Size = new System.Drawing.Size(148, 23);
            this._txtFineE.TabIndex = 10;
            this._txtFineE.TabStop = false;
            // 
            // _btnDebugSoapHeadersA2F
            // 
            this._btnDebugSoapHeadersA2F.BackColor = System.Drawing.Color.LightYellow;
            this._btnDebugSoapHeadersA2F.Location = new System.Drawing.Point(10, 316);
            this._btnDebugSoapHeadersA2F.Name = "_btnDebugSoapHeadersA2F";
            this._btnDebugSoapHeadersA2F.Size = new System.Drawing.Size(150, 30);
            this._btnDebugSoapHeadersA2F.TabIndex = 5;
            this._btnDebugSoapHeadersA2F.Text = "🔍 Debug SOAP";
            this._btnDebugSoapHeadersA2F.UseVisualStyleBackColor = false;
            this._btnDebugSoapHeadersA2F.Click += new System.EventHandler(this.BtnDebugSoapHeadersA2F_Click);
            // 
            // lblA2FOutput
            // 
            this.lblA2FOutput.Location = new System.Drawing.Point(10, 355);
            this.lblA2FOutput.Name = "lblA2FOutput";
            this.lblA2FOutput.Size = new System.Drawing.Size(100, 20);
            this.lblA2FOutput.TabIndex = 6;
            this.lblA2FOutput.Text = "Risposta A2F";
            // 
            // _txtA2FOutput
            // 
            this._txtA2FOutput.Font = new System.Drawing.Font("Courier New", 9F);
            this._txtA2FOutput.Location = new System.Drawing.Point(10, 377);
            this._txtA2FOutput.Multiline = true;
            this._txtA2FOutput.Name = "_txtA2FOutput";
            this._txtA2FOutput.ReadOnly = true;
            this._txtA2FOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this._txtA2FOutput.Size = new System.Drawing.Size(1005, 248);
            this._txtA2FOutput.TabIndex = 6;
            this._txtA2FOutput.WordWrap = false;
            // 
            // _tabPrescrittore
            // 
            this._tabPrescrittore.Controls.Add(this.lblServizioP);
            this._tabPrescrittore.Controls.Add(this._cmbServizioP);
            this._tabPrescrittore.Controls.Add(this.lblAuth2FP);
            this._tabPrescrittore.Controls.Add(this._txtAuth2FP);
            this._tabPrescrittore.Controls.Add(this.lblInputP);
            this._tabPrescrittore.Controls.Add(this._txtInputP);
            this._tabPrescrittore.Controls.Add(this._btnChiamaP);
            this._tabPrescrittore.Controls.Add(this._btnDebugSoapP);
            this._tabPrescrittore.Controls.Add(this.lblOutputP);
            this._tabPrescrittore.Controls.Add(this._txtOutputP);
            this._tabPrescrittore.Location = new System.Drawing.Point(4, 22);
            this._tabPrescrittore.Name = "_tabPrescrittore";
            this._tabPrescrittore.Padding = new System.Windows.Forms.Padding(8);
            this._tabPrescrittore.Size = new System.Drawing.Size(1032, 654);
            this._tabPrescrittore.TabIndex = 1;
            this._tabPrescrittore.Text = "Servizi Prescrittore";
            // 
            // lblServizioP
            // 
            this.lblServizioP.Location = new System.Drawing.Point(10, 14);
            this.lblServizioP.Name = "lblServizioP";
            this.lblServizioP.Size = new System.Drawing.Size(60, 20);
            this.lblServizioP.TabIndex = 0;
            this.lblServizioP.Text = "Servizio";
            // 
            // _cmbServizioP
            // 
            this._cmbServizioP.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbServizioP.Location = new System.Drawing.Point(10, 36);
            this._cmbServizioP.Name = "_cmbServizioP";
            this._cmbServizioP.Size = new System.Drawing.Size(480, 21);
            this._cmbServizioP.TabIndex = 0;
            // 
            // lblAuth2FP
            // 
            this.lblAuth2FP.Location = new System.Drawing.Point(10, 76);
            this.lblAuth2FP.Name = "lblAuth2FP";
            this.lblAuth2FP.Size = new System.Drawing.Size(340, 20);
            this.lblAuth2FP.TabIndex = 1;
            this.lblAuth2FP.Text = "Authorization2F (token sesssione Prescrittore)";
            // 
            // _txtAuth2FP
            // 
            this._txtAuth2FP.Font = new System.Drawing.Font("Courier New", 9F);
            this._txtAuth2FP.Location = new System.Drawing.Point(10, 98);
            this._txtAuth2FP.Name = "_txtAuth2FP";
            this._txtAuth2FP.Size = new System.Drawing.Size(1005, 21);
            this._txtAuth2FP.TabIndex = 1;
            // 
            // lblInputP
            // 
            this.lblInputP.Location = new System.Drawing.Point(10, 136);
            this.lblInputP.Name = "lblInputP";
            this.lblInputP.Size = new System.Drawing.Size(300, 20);
            this.lblInputP.TabIndex = 2;
            this.lblInputP.Text = "Input (key=value;key2=value2)";
            // 
            // _txtInputP
            // 
            this._txtInputP.Font = new System.Drawing.Font("Courier New", 9F);
            this._txtInputP.Location = new System.Drawing.Point(10, 158);
            this._txtInputP.Name = "_txtInputP";
            this._txtInputP.Size = new System.Drawing.Size(1005, 21);
            this._txtInputP.TabIndex = 2;
            // 
            // _btnChiamaP
            // 
            this._btnChiamaP.BackColor = System.Drawing.Color.LightGreen;
            this._btnChiamaP.Location = new System.Drawing.Point(10, 192);
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
            this._btnDebugSoapP.Location = new System.Drawing.Point(178, 192);
            this._btnDebugSoapP.Name = "_btnDebugSoapP";
            this._btnDebugSoapP.Size = new System.Drawing.Size(150, 32);
            this._btnDebugSoapP.TabIndex = 4;
            this._btnDebugSoapP.Text = "🔍 Debug SOAP";
            this._btnDebugSoapP.UseVisualStyleBackColor = false;
            this._btnDebugSoapP.Click += new System.EventHandler(this.BtnDebugSoapP_Click);
            // 
            // lblOutputP
            // 
            this.lblOutputP.Location = new System.Drawing.Point(10, 240);
            this.lblOutputP.Name = "lblOutputP";
            this.lblOutputP.Size = new System.Drawing.Size(60, 20);
            this.lblOutputP.TabIndex = 5;
            this.lblOutputP.Text = "Output";
            // 
            // _txtOutputP
            // 
            this._txtOutputP.Font = new System.Drawing.Font("Courier New", 9F);
            this._txtOutputP.Location = new System.Drawing.Point(10, 262);
            this._txtOutputP.Multiline = true;
            this._txtOutputP.Name = "_txtOutputP";
            this._txtOutputP.ReadOnly = true;
            this._txtOutputP.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this._txtOutputP.Size = new System.Drawing.Size(1005, 358);
            this._txtOutputP.TabIndex = 5;
            this._txtOutputP.WordWrap = false;
            // 
            // _tabErogatore
            // 
            this._tabErogatore.Controls.Add(this.lblServizioE);
            this._tabErogatore.Controls.Add(this._cmbServizioE);
            this._tabErogatore.Controls.Add(this.lblAuth2FE);
            this._tabErogatore.Controls.Add(this._txtAuth2FE);
            this._tabErogatore.Controls.Add(this.lblInputE);
            this._tabErogatore.Controls.Add(this._txtInputE);
            this._tabErogatore.Controls.Add(this._btnChiamaE);
            this._tabErogatore.Controls.Add(this._btnDebugSoapE);
            this._tabErogatore.Controls.Add(this.lblOutputE);
            this._tabErogatore.Controls.Add(this._txtOutputE);
            this._tabErogatore.Location = new System.Drawing.Point(4, 22);
            this._tabErogatore.Name = "_tabErogatore";
            this._tabErogatore.Padding = new System.Windows.Forms.Padding(8);
            this._tabErogatore.Size = new System.Drawing.Size(1032, 654);
            this._tabErogatore.TabIndex = 2;
            this._tabErogatore.Text = "Servizi Erogatore";
            // 
            // lblServizioE
            // 
            this.lblServizioE.Location = new System.Drawing.Point(10, 14);
            this.lblServizioE.Name = "lblServizioE";
            this.lblServizioE.Size = new System.Drawing.Size(60, 20);
            this.lblServizioE.TabIndex = 0;
            this.lblServizioE.Text = "Servizio";
            // 
            // _cmbServizioE
            // 
            this._cmbServizioE.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbServizioE.Location = new System.Drawing.Point(10, 36);
            this._cmbServizioE.Name = "_cmbServizioE";
            this._cmbServizioE.Size = new System.Drawing.Size(480, 21);
            this._cmbServizioE.TabIndex = 0;
            // 
            // lblAuth2FE
            // 
            this.lblAuth2FE.Location = new System.Drawing.Point(10, 76);
            this.lblAuth2FE.Name = "lblAuth2FE";
            this.lblAuth2FE.Size = new System.Drawing.Size(340, 20);
            this.lblAuth2FE.TabIndex = 1;
            this.lblAuth2FE.Text = "Authorization2F (token sessione Erogatore)";
            // 
            // _txtAuth2FE
            // 
            this._txtAuth2FE.Font = new System.Drawing.Font("Courier New", 9F);
            this._txtAuth2FE.Location = new System.Drawing.Point(10, 98);
            this._txtAuth2FE.Name = "_txtAuth2FE";
            this._txtAuth2FE.Size = new System.Drawing.Size(1005, 21);
            this._txtAuth2FE.TabIndex = 1;
            // 
            // lblInputE
            // 
            this.lblInputE.Location = new System.Drawing.Point(10, 136);
            this.lblInputE.Name = "lblInputE";
            this.lblInputE.Size = new System.Drawing.Size(300, 20);
            this.lblInputE.TabIndex = 2;
            this.lblInputE.Text = "Input (key=value;key2=value2)";
            // 
            // _txtInputE
            // 
            this._txtInputE.Font = new System.Drawing.Font("Courier New", 9F);
            this._txtInputE.Location = new System.Drawing.Point(10, 158);
            this._txtInputE.Name = "_txtInputE";
            this._txtInputE.Size = new System.Drawing.Size(1005, 21);
            this._txtInputE.TabIndex = 2;
            // 
            // _btnChiamaE
            // 
            this._btnChiamaE.BackColor = System.Drawing.Color.LightGreen;
            this._btnChiamaE.Location = new System.Drawing.Point(10, 192);
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
            this._btnDebugSoapE.Location = new System.Drawing.Point(178, 192);
            this._btnDebugSoapE.Name = "_btnDebugSoapE";
            this._btnDebugSoapE.Size = new System.Drawing.Size(150, 32);
            this._btnDebugSoapE.TabIndex = 4;
            this._btnDebugSoapE.Text = "🔍 Debug SOAP";
            this._btnDebugSoapE.UseVisualStyleBackColor = false;
            this._btnDebugSoapE.Click += new System.EventHandler(this.BtnDebugSoapE_Click);
            // 
            // lblOutputE
            // 
            this.lblOutputE.Location = new System.Drawing.Point(10, 240);
            this.lblOutputE.Name = "lblOutputE";
            this.lblOutputE.Size = new System.Drawing.Size(60, 20);
            this.lblOutputE.TabIndex = 5;
            this.lblOutputE.Text = "Output";
            // 
            // _txtOutputE
            // 
            this._txtOutputE.Font = new System.Drawing.Font("Courier New", 9F);
            this._txtOutputE.Location = new System.Drawing.Point(10, 262);
            this._txtOutputE.Multiline = true;
            this._txtOutputE.Name = "_txtOutputE";
            this._txtOutputE.ReadOnly = true;
            this._txtOutputE.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this._txtOutputE.Size = new System.Drawing.Size(1005, 358);
            this._txtOutputE.TabIndex = 5;
            this._txtOutputE.WordWrap = false;
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(1064, 741);
            this.Controls.Add(this.lblUsername);
            this.Controls.Add(this._txtUsername);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this._txtPassword);
            this.Controls.Add(this.lblUsernameE);
            this.Controls.Add(this._txtUsernameE);
            this.Controls.Add(this.lblPasswordE);
            this.Controls.Add(this._txtPasswordE);
            this.Controls.Add(this._chkProduzione);
            this.Controls.Add(this._tabControl);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MinimumSize = new System.Drawing.Size(1080, 780);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ricetta Dematerializzata - Test Client";
            this._tabControl.ResumeLayout(false);
            this._tabToken.ResumeLayout(false);
            this._tabToken.PerformLayout();
            this._grpTokenP.ResumeLayout(false);
            this._grpTokenP.PerformLayout();
            this._grpTokenE.ResumeLayout(false);
            this._grpTokenE.PerformLayout();
            this._tabPrescrittore.ResumeLayout(false);
            this._tabPrescrittore.PerformLayout();
            this._tabErogatore.ResumeLayout(false);
            this._tabErogatore.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
