using System.Drawing;
using System.Windows.Forms;

namespace ricetta_dematerializzata_test_ui
{
    public partial class MainForm
    {
        private void InitializeComponent()
        {
            this.lblUsername = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            this.lblServizio = new System.Windows.Forms.Label();
            this.lblAuthorization2F = new System.Windows.Forms.Label();
            this.lblInput = new System.Windows.Forms.Label();
            this.lblOutput = new System.Windows.Forms.Label();
            this._txtUsername = new System.Windows.Forms.TextBox();
            this._txtPassword = new System.Windows.Forms.TextBox();
            this._cmbServizio = new System.Windows.Forms.ComboBox();
            this._txtAuthorization2F = new System.Windows.Forms.TextBox();
            this._txtInput = new System.Windows.Forms.TextBox();
            this._btnChiama = new System.Windows.Forms.Button();
            this._txtOutput = new System.Windows.Forms.TextBox();
            this._chkIgnoraSsl = new System.Windows.Forms.CheckBox();
            this._chkProduzione = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // lblUsername
            // 
            this.lblUsername.Location = new System.Drawing.Point(20, 20);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(120, 23);
            this.lblUsername.TabIndex = 0;
            this.lblUsername.Text = "Username";
            // 
            // lblPassword
            // 
            this.lblPassword.Location = new System.Drawing.Point(300, 20);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(120, 23);
            this.lblPassword.TabIndex = 2;
            this.lblPassword.Text = "Password";
            // 
            // lblServizio
            // 
            this.lblServizio.Location = new System.Drawing.Point(20, 78);
            this.lblServizio.Name = "lblServizio";
            this.lblServizio.Size = new System.Drawing.Size(120, 23);
            this.lblServizio.TabIndex = 6;
            this.lblServizio.Text = "Servizio";
            // 
            // lblAuthorization2F
            // 
            this.lblAuthorization2F.Location = new System.Drawing.Point(20, 138);
            this.lblAuthorization2F.Name = "lblAuthorization2F";
            this.lblAuthorization2F.Size = new System.Drawing.Size(260, 23);
            this.lblAuthorization2F.TabIndex = 8;
            this.lblAuthorization2F.Text = "Authorization2F (Bearer ID-SESSIONE)";
            // 
            // lblInput
            // 
            this.lblInput.Location = new System.Drawing.Point(20, 194);
            this.lblInput.Name = "lblInput";
            this.lblInput.Size = new System.Drawing.Size(300, 23);
            this.lblInput.TabIndex = 10;
            this.lblInput.Text = "Input (key=value;key2=value2)";
            // 
            // lblOutput
            // 
            this.lblOutput.Location = new System.Drawing.Point(20, 455);
            this.lblOutput.Name = "lblOutput";
            this.lblOutput.Size = new System.Drawing.Size(120, 23);
            this.lblOutput.TabIndex = 13;
            this.lblOutput.Text = "Output";
            // 
            // _txtUsername
            // 
            this._txtUsername.Location = new System.Drawing.Point(20, 42);
            this._txtUsername.Name = "_txtUsername";
            this._txtUsername.Size = new System.Drawing.Size(260, 20);
            this._txtUsername.TabIndex = 1;
            this._txtUsername.Text = "PROVAX00X00X000Y";
            // 
            // _txtPassword
            // 
            this._txtPassword.Location = new System.Drawing.Point(300, 42);
            this._txtPassword.Name = "_txtPassword";
            this._txtPassword.Size = new System.Drawing.Size(260, 20);
            this._txtPassword.TabIndex = 3;
            this._txtPassword.Text = "Salve123";
            this._txtPassword.UseSystemPasswordChar = true;
            // 
            // _cmbServizio
            // 
            this._cmbServizio.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbServizio.Location = new System.Drawing.Point(20, 100);
            this._cmbServizio.Name = "_cmbServizio";
            this._cmbServizio.Size = new System.Drawing.Size(540, 21);
            this._cmbServizio.TabIndex = 7;
            this._cmbServizio.SelectedIndexChanged += new System.EventHandler(this.CmbServizio_SelectedIndexChanged);
            // 
            // _txtAuthorization2F
            // 
            this._txtAuthorization2F.Location = new System.Drawing.Point(20, 160);
            this._txtAuthorization2F.Name = "_txtAuthorization2F";
            this._txtAuthorization2F.Size = new System.Drawing.Size(840, 20);
            this._txtAuthorization2F.TabIndex = 9;
            // 
            // _txtInput
            // 
            this._txtInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._txtInput.Location = new System.Drawing.Point(20, 216);
            this._txtInput.Multiline = true;
            this._txtInput.Name = "_txtInput";
            this._txtInput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._txtInput.Size = new System.Drawing.Size(860, 180);
            this._txtInput.TabIndex = 11;
            // 
            // _btnChiama
            // 
            this._btnChiama.Location = new System.Drawing.Point(20, 410);
            this._btnChiama.Name = "_btnChiama";
            this._btnChiama.Size = new System.Drawing.Size(140, 32);
            this._btnChiama.TabIndex = 12;
            this._btnChiama.Text = "Chiama servizio";
            this._btnChiama.Click += new System.EventHandler(this.BtnChiama_Click);
            // 
            // _txtOutput
            // 
            this._txtOutput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._txtOutput.Location = new System.Drawing.Point(20, 477);
            this._txtOutput.Multiline = true;
            this._txtOutput.Name = "_txtOutput";
            this._txtOutput.ReadOnly = true;
            this._txtOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._txtOutput.Size = new System.Drawing.Size(860, 170);
            this._txtOutput.TabIndex = 14;
            // 
            // _chkIgnoraSsl
            // 
            this._chkIgnoraSsl.Checked = true;
            this._chkIgnoraSsl.CheckState = System.Windows.Forms.CheckState.Checked;
            this._chkIgnoraSsl.Location = new System.Drawing.Point(580, 44);
            this._chkIgnoraSsl.Name = "_chkIgnoraSsl";
            this._chkIgnoraSsl.Size = new System.Drawing.Size(180, 24);
            this._chkIgnoraSsl.TabIndex = 4;
            this._chkIgnoraSsl.Text = "Ignora errori SSL";
            // 
            // _chkProduzione
            // 
            this._chkProduzione.Location = new System.Drawing.Point(760, 44);
            this._chkProduzione.Name = "_chkProduzione";
            this._chkProduzione.Size = new System.Drawing.Size(120, 24);
            this._chkProduzione.TabIndex = 5;
            this._chkProduzione.Text = "Produzione";
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(884, 661);
            this.Controls.Add(this.lblUsername);
            this.Controls.Add(this._txtUsername);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this._txtPassword);
            this.Controls.Add(this._chkIgnoraSsl);
            this.Controls.Add(this._chkProduzione);
            this.Controls.Add(this.lblServizio);
            this.Controls.Add(this._cmbServizio);
            this.Controls.Add(this.lblAuthorization2F);
            this.Controls.Add(this._txtAuthorization2F);
            this.Controls.Add(this.lblInput);
            this.Controls.Add(this._txtInput);
            this.Controls.Add(this._btnChiama);
            this.Controls.Add(this.lblOutput);
            this.Controls.Add(this._txtOutput);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ricetta Dematerializzata - Test UI";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private Label lblUsername;
        private Label lblPassword;
        private Label lblServizio;
        private Label lblAuthorization2F;
        private Label lblInput;
        private Label lblOutput;
    }
}
