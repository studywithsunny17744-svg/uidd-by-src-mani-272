namespace proxy_uid.UI
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this._borderless = new Guna.UI2.WinForms.Guna2BorderlessForm(this.components);
            this._titleBar = new Guna.UI2.WinForms.Guna2Panel();
            this._lblTitle = new System.Windows.Forms.Label();
            this._btnClose = new Guna.UI2.WinForms.Guna2ControlBox();
            this._dragTitle = new Guna.UI2.WinForms.Guna2DragControl(this.components);
            this.grpSettings = new Guna.UI2.WinForms.Guna2GroupBox();
            this.lblEmu = new System.Windows.Forms.Label();
            this._cmbEmu = new Guna.UI2.WinForms.Guna2ComboBox();
            this.lblPort = new System.Windows.Forms.Label();
            this._txtPort = new Guna.UI2.WinForms.Guna2TextBox();
            this.lblGame = new System.Windows.Forms.Label();
            this._cmbGame = new Guna.UI2.WinForms.Guna2ComboBox();
            this._btnGrant = new Guna.UI2.WinForms.Guna2GradientButton();
            this._btnInstall = new Guna.UI2.WinForms.Guna2GradientButton();
            this._btnRemove = new Guna.UI2.WinForms.Guna2GradientButton();
            this._btnConnect = new Guna.UI2.WinForms.Guna2GradientButton();
            this._btnDisconnect = new Guna.UI2.WinForms.Guna2GradientButton();
            this._btnSwitchCmd = new Guna.UI2.WinForms.Guna2GradientButton();
            this._lblStatus = new System.Windows.Forms.Label();
            this.grpLog = new Guna.UI2.WinForms.Guna2GroupBox();
            this._log = new System.Windows.Forms.RichTextBox();
            this._titleBar.SuspendLayout();
            this.grpSettings.SuspendLayout();
            this.grpLog.SuspendLayout();
            this.SuspendLayout();
            // 
            // _borderless
            // 
            this._borderless.BorderRadius = 14;
            this._borderless.ContainerControl = this;
            this._borderless.DockIndicatorTransparencyValue = 0.6D;
            this._borderless.ShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(20)))), ((int)(((byte)(60)))));
            this._borderless.TransparentWhileDrag = true;
            // 
            // _titleBar
            // 
            this._titleBar.Controls.Add(this._lblTitle);
            this._titleBar.Controls.Add(this._btnClose);
            this._titleBar.Dock = System.Windows.Forms.DockStyle.Top;
            this._titleBar.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(22)))));
            this._titleBar.Location = new System.Drawing.Point(0, 0);
            this._titleBar.Name = "_titleBar";
            this._titleBar.Size = new System.Drawing.Size(560, 40);
            this._titleBar.TabIndex = 0;
            // 
            // _lblTitle
            // 
            this._lblTitle.AutoSize = true;
            this._lblTitle.BackColor = System.Drawing.Color.Transparent;
            this._lblTitle.Font = new System.Drawing.Font("Sitka Small", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblTitle.ForeColor = System.Drawing.Color.Red;
            this._lblTitle.Location = new System.Drawing.Point(11, 7);
            this._lblTitle.Name = "_lblTitle";
            this._lblTitle.Size = new System.Drawing.Size(209, 23);
            this._lblTitle.TabIndex = 0;
            this._lblTitle.Text = "MANI 272 - ALL SERCER";
            // 
            // _btnClose
            // 
            this._btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._btnClose.FillColor = System.Drawing.Color.Transparent;
            this._btnClose.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(20)))), ((int)(((byte)(60)))));
            this._btnClose.HoverState.IconColor = System.Drawing.Color.White;
            this._btnClose.IconColor = System.Drawing.Color.Gray;
            this._btnClose.Location = new System.Drawing.Point(512, 4);
            this._btnClose.Name = "_btnClose";
            this._btnClose.Size = new System.Drawing.Size(40, 32);
            this._btnClose.TabIndex = 1;
            // 
            // _dragTitle
            // 
            this._dragTitle.DockIndicatorTransparencyValue = 0.6D;
            this._dragTitle.TargetControl = this._titleBar;
            this._dragTitle.UseTransparentDrag = true;
            // 
            // grpSettings
            // 
            this.grpSettings.BorderColor = System.Drawing.Color.Transparent;
            this.grpSettings.BorderRadius = 10;
            this.grpSettings.Controls.Add(this.lblEmu);
            this.grpSettings.Controls.Add(this._cmbEmu);
            this.grpSettings.Controls.Add(this.lblPort);
            this.grpSettings.Controls.Add(this._txtPort);
            this.grpSettings.Controls.Add(this.lblGame);
            this.grpSettings.Controls.Add(this._cmbGame);
            this.grpSettings.CustomBorderColor = System.Drawing.Color.Black;
            this.grpSettings.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(22)))), ((int)(((byte)(24)))));
            this.grpSettings.Font = new System.Drawing.Font("Sitka Small", 9F);
            this.grpSettings.ForeColor = System.Drawing.Color.White;
            this.grpSettings.Location = new System.Drawing.Point(16, 52);
            this.grpSettings.Name = "grpSettings";
            this.grpSettings.Size = new System.Drawing.Size(528, 110);
            this.grpSettings.TabIndex = 1;
            this.grpSettings.Text = "Settings";
            // 
            // lblEmu
            // 
            this.lblEmu.AutoSize = true;
            this.lblEmu.BackColor = System.Drawing.Color.Transparent;
            this.lblEmu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(160)))), ((int)(((byte)(180)))));
            this.lblEmu.Location = new System.Drawing.Point(14, 42);
            this.lblEmu.Name = "lblEmu";
            this.lblEmu.Size = new System.Drawing.Size(64, 18);
            this.lblEmu.TabIndex = 0;
            this.lblEmu.Text = "Emulator";
            // 
            // _cmbEmu
            // 
            this._cmbEmu.BackColor = System.Drawing.Color.Transparent;
            this._cmbEmu.BorderRadius = 6;
            this._cmbEmu.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this._cmbEmu.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbEmu.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(30)))));
            this._cmbEmu.FocusedColor = System.Drawing.Color.Empty;
            this._cmbEmu.Font = new System.Drawing.Font("Segoe UI", 10F);
            this._cmbEmu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(220)))), ((int)(((byte)(240)))));
            this._cmbEmu.ItemHeight = 24;
            this._cmbEmu.Location = new System.Drawing.Point(14, 60);
            this._cmbEmu.Name = "_cmbEmu";
            this._cmbEmu.Size = new System.Drawing.Size(150, 30);
            this._cmbEmu.TabIndex = 1;
            this._cmbEmu.SelectedIndexChanged += new System.EventHandler(this.cmbEmu_SelectedIndexChanged);
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.BackColor = System.Drawing.Color.Transparent;
            this.lblPort.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(160)))), ((int)(((byte)(180)))));
            this.lblPort.Location = new System.Drawing.Point(180, 42);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(62, 18);
            this.lblPort.TabIndex = 2;
            this.lblPort.Text = "ADB Port";
            // 
            // _txtPort
            // 
            this._txtPort.BorderRadius = 6;
            this._txtPort.Cursor = System.Windows.Forms.Cursors.IBeam;
            this._txtPort.DefaultText = "5555";
            this._txtPort.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(30)))));
            this._txtPort.Font = new System.Drawing.Font("Segoe UI", 9F);
            this._txtPort.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(220)))), ((int)(((byte)(240)))));
            this._txtPort.Location = new System.Drawing.Point(180, 60);
            this._txtPort.Name = "_txtPort";
            this._txtPort.PlaceholderText = "";
            this._txtPort.SelectedText = "";
            this._txtPort.Size = new System.Drawing.Size(100, 30);
            this._txtPort.TabIndex = 3;
            this._txtPort.TextChanged += new System.EventHandler(this.txtPort_TextChanged);
            // 
            // lblGame
            // 
            this.lblGame.AutoSize = true;
            this.lblGame.BackColor = System.Drawing.Color.Transparent;
            this.lblGame.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(160)))), ((int)(((byte)(180)))));
            this.lblGame.Location = new System.Drawing.Point(300, 42);
            this.lblGame.Name = "lblGame";
            this.lblGame.Size = new System.Drawing.Size(42, 18);
            this.lblGame.TabIndex = 4;
            this.lblGame.Text = "Game";
            // 
            // _cmbGame
            // 
            this._cmbGame.BackColor = System.Drawing.Color.Transparent;
            this._cmbGame.BorderRadius = 6;
            this._cmbGame.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this._cmbGame.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbGame.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(30)))));
            this._cmbGame.FocusedColor = System.Drawing.Color.Empty;
            this._cmbGame.Font = new System.Drawing.Font("Segoe UI", 10F);
            this._cmbGame.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(220)))), ((int)(((byte)(240)))));
            this._cmbGame.ItemHeight = 24;
            this._cmbGame.Location = new System.Drawing.Point(300, 60);
            this._cmbGame.Name = "_cmbGame";
            this._cmbGame.Size = new System.Drawing.Size(200, 30);
            this._cmbGame.TabIndex = 5;
            this._cmbGame.SelectedIndexChanged += new System.EventHandler(this.cmbGame_SelectedIndexChanged);
            // 
            // _btnGrant
            // 
            this._btnGrant.BorderRadius = 8;
            this._btnGrant.Cursor = System.Windows.Forms.Cursors.Hand;
            this._btnGrant.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(20)))), ((int)(((byte)(60)))));
            this._btnGrant.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(15)))), ((int)(((byte)(45)))));
            this._btnGrant.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this._btnGrant.ForeColor = System.Drawing.Color.White;
            this._btnGrant.Location = new System.Drawing.Point(16, 172);
            this._btnGrant.Name = "_btnGrant";
            this._btnGrant.Size = new System.Drawing.Size(168, 38);
            this._btnGrant.TabIndex = 2;
            this._btnGrant.Text = "GRANT ACCESS";
            this._btnGrant.Click += new System.EventHandler(this.btnGrant_Click);
            // 
            // _btnInstall
            // 
            this._btnInstall.BorderRadius = 8;
            this._btnInstall.Cursor = System.Windows.Forms.Cursors.Hand;
            this._btnInstall.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(120)))), ((int)(((byte)(200)))));
            this._btnInstall.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(90)))), ((int)(((byte)(150)))));
            this._btnInstall.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this._btnInstall.ForeColor = System.Drawing.Color.White;
            this._btnInstall.Location = new System.Drawing.Point(192, 172);
            this._btnInstall.Name = "_btnInstall";
            this._btnInstall.Size = new System.Drawing.Size(168, 38);
            this._btnInstall.TabIndex = 3;
            this._btnInstall.Text = "INSTALL CERT";
            this._btnInstall.Click += new System.EventHandler(this.btnInstall_Click);
            // 
            // _btnRemove
            // 
            this._btnRemove.BorderRadius = 8;
            this._btnRemove.Cursor = System.Windows.Forms.Cursors.Hand;
            this._btnRemove.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(85)))));
            this._btnRemove.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this._btnRemove.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this._btnRemove.ForeColor = System.Drawing.Color.White;
            this._btnRemove.Location = new System.Drawing.Point(372, 172);
            this._btnRemove.Name = "_btnRemove";
            this._btnRemove.Size = new System.Drawing.Size(172, 38);
            this._btnRemove.TabIndex = 4;
            this._btnRemove.Text = "REMOVE CERT";
            this._btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // _btnConnect
            // 
            this._btnConnect.BorderRadius = 8;
            this._btnConnect.Cursor = System.Windows.Forms.Cursors.Hand;
            this._btnConnect.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(180)))), ((int)(((byte)(90)))));
            this._btnConnect.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(135)))), ((int)(((byte)(68)))));
            this._btnConnect.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this._btnConnect.ForeColor = System.Drawing.Color.White;
            this._btnConnect.Location = new System.Drawing.Point(16, 220);
            this._btnConnect.Name = "_btnConnect";
            this._btnConnect.Size = new System.Drawing.Size(256, 42);
            this._btnConnect.TabIndex = 5;
            this._btnConnect.Text = "CONNECT";
            this._btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // _btnDisconnect
            // 
            this._btnDisconnect.BorderRadius = 8;
            this._btnDisconnect.Cursor = System.Windows.Forms.Cursors.Hand;
            this._btnDisconnect.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this._btnDisconnect.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this._btnDisconnect.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this._btnDisconnect.ForeColor = System.Drawing.Color.White;
            this._btnDisconnect.Location = new System.Drawing.Point(284, 220);
            this._btnDisconnect.Name = "_btnDisconnect";
            this._btnDisconnect.Size = new System.Drawing.Size(260, 42);
            this._btnDisconnect.TabIndex = 6;
            this._btnDisconnect.Text = "DISCONNECT";
            this._btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // _btnSwitchCmd
            // 
            this._btnSwitchCmd.BorderRadius = 8;
            this._btnSwitchCmd.Cursor = System.Windows.Forms.Cursors.Hand;
            this._btnSwitchCmd.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(130)))), ((int)(((byte)(180)))));
            this._btnSwitchCmd.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(98)))), ((int)(((byte)(135)))));
            this._btnSwitchCmd.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this._btnSwitchCmd.ForeColor = System.Drawing.Color.White;
            this._btnSwitchCmd.Location = new System.Drawing.Point(16, 268);
            this._btnSwitchCmd.Name = "_btnSwitchCmd";
            this._btnSwitchCmd.Size = new System.Drawing.Size(528, 32);
            this._btnSwitchCmd.TabIndex = 7;
            this._btnSwitchCmd.Text = "SWITCH TO CMD";
            this._btnSwitchCmd.Click += new System.EventHandler(this.btnSwitchCmd_Click);
            // 
            // _lblStatus
            // 
            this._lblStatus.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this._lblStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(160)))), ((int)(((byte)(180)))));
            this._lblStatus.Location = new System.Drawing.Point(16, 304);
            this._lblStatus.Name = "_lblStatus";
            this._lblStatus.Size = new System.Drawing.Size(528, 20);
            this._lblStatus.TabIndex = 8;
            // 
            // grpLog
            // 
            this.grpLog.BorderColor = System.Drawing.Color.Transparent;
            this.grpLog.BorderRadius = 10;
            this.grpLog.Controls.Add(this._log);
            this.grpLog.CustomBorderColor = System.Drawing.Color.Black;
            this.grpLog.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(18)))), ((int)(((byte)(20)))));
            this.grpLog.Font = new System.Drawing.Font("Sitka Small", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpLog.ForeColor = System.Drawing.Color.White;
            this.grpLog.Location = new System.Drawing.Point(16, 326);
            this.grpLog.Name = "grpLog";
            this.grpLog.Size = new System.Drawing.Size(528, 200);
            this.grpLog.TabIndex = 9;
            this.grpLog.Text = "Activity Log";
            // 
            // _log
            // 
            this._log.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(12)))), ((int)(((byte)(12)))), ((int)(((byte)(12)))));
            this._log.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._log.Font = new System.Drawing.Font("Consolas", 9F);
            this._log.ForeColor = System.Drawing.Color.Gainsboro;
            this._log.Location = new System.Drawing.Point(12, 36);
            this._log.Name = "_log";
            this._log.ReadOnly = true;
            this._log.Size = new System.Drawing.Size(504, 160);
            this._log.TabIndex = 0;
            this._log.Text = "";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(15)))), ((int)(((byte)(15)))));
            this.ClientSize = new System.Drawing.Size(560, 545);
            this.Controls.Add(this.grpLog);
            this.Controls.Add(this._lblStatus);
            this.Controls.Add(this._btnSwitchCmd);
            this.Controls.Add(this._btnDisconnect);
            this.Controls.Add(this._btnConnect);
            this.Controls.Add(this._btnRemove);
            this.Controls.Add(this._btnInstall);
            this.Controls.Add(this._btnGrant);
            this.Controls.Add(this.grpSettings);
            this.Controls.Add(this._titleBar);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
this.Text = "MANI 272 - GUI";
            this._titleBar.ResumeLayout(false);
            this._titleBar.PerformLayout();
            this.grpSettings.ResumeLayout(false);
            this.grpSettings.PerformLayout();
            this.grpLog.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2BorderlessForm _borderless;
        private Guna.UI2.WinForms.Guna2Panel _titleBar;
        private System.Windows.Forms.Label _lblTitle;
        private Guna.UI2.WinForms.Guna2ControlBox _btnClose;
        private Guna.UI2.WinForms.Guna2DragControl _dragTitle;
        private Guna.UI2.WinForms.Guna2GroupBox grpSettings;
        private System.Windows.Forms.Label lblEmu;
        private Guna.UI2.WinForms.Guna2ComboBox _cmbEmu;
        private System.Windows.Forms.Label lblPort;
        private Guna.UI2.WinForms.Guna2TextBox _txtPort;
        private System.Windows.Forms.Label lblGame;
        private Guna.UI2.WinForms.Guna2ComboBox _cmbGame;
        private Guna.UI2.WinForms.Guna2GradientButton _btnGrant;
        private Guna.UI2.WinForms.Guna2GradientButton _btnInstall;
        private Guna.UI2.WinForms.Guna2GradientButton _btnRemove;
        private Guna.UI2.WinForms.Guna2GradientButton _btnConnect;
        private Guna.UI2.WinForms.Guna2GradientButton _btnDisconnect;
        private Guna.UI2.WinForms.Guna2GradientButton _btnSwitchCmd;
        private System.Windows.Forms.Label _lblStatus;
        private Guna.UI2.WinForms.Guna2GroupBox grpLog;
        private System.Windows.Forms.RichTextBox _log;
    }
}
