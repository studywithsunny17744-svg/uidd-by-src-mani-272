namespace proxy_uid.UI
{
    partial class LauncherForm
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
            this._panel = new Guna.UI2.WinForms.Guna2Panel();
            this._title = new System.Windows.Forms.Label();
            this._subtitle = new System.Windows.Forms.Label();
            this._linkGitHub = new System.Windows.Forms.LinkLabel();
            this._btnCmd = new Guna.UI2.WinForms.Guna2GradientButton();
            this._btnGui = new Guna.UI2.WinForms.Guna2GradientButton();
            this._close = new Guna.UI2.WinForms.Guna2ControlBox();
            this._panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // _borderless
            // 
            this._borderless.BorderRadius = 16;
            this._borderless.ContainerControl = this;
            this._borderless.DockIndicatorTransparencyValue = 0.6D;
            this._borderless.ShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(20)))), ((int)(((byte)(60)))));
            this._borderless.TransparentWhileDrag = true;
            // 
            // _panel
            // 
            this._panel.BorderRadius = 16;
            this._panel.Controls.Add(this._title);
            this._panel.Controls.Add(this._subtitle);
            this._panel.Controls.Add(this._linkGitHub);
            this._panel.Controls.Add(this._btnCmd);
            this._panel.Controls.Add(this._btnGui);
            this._panel.Controls.Add(this._close);
            this._panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._panel.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(18)))), ((int)(((byte)(22)))));
            this._panel.Location = new System.Drawing.Point(0, 0);
            this._panel.Name = "_panel";
            this._panel.Size = new System.Drawing.Size(420, 300);
            this._panel.TabIndex = 0;
            // 
            // _title
            // 
            this._title.BackColor = System.Drawing.Color.Transparent;
            this._title.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._title.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(212)))), ((int)(((byte)(255)))));
            this._title.Location = new System.Drawing.Point(20, 36);
            this._title.Name = "_title";
            this._title.Size = new System.Drawing.Size(380, 40);
            this._title.TabIndex = 0;
            this._title.Text = "MANI 272";
            this._title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // _subtitle
            // 
            this._subtitle.BackColor = System.Drawing.Color.Transparent;
            this._subtitle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this._subtitle.ForeColor = System.Drawing.Color.Gray;
            this._subtitle.Location = new System.Drawing.Point(20, 78);
            this._subtitle.Name = "_subtitle";
            this._subtitle.Size = new System.Drawing.Size(380, 24);
            this._subtitle.TabIndex = 1;
            this._subtitle.Text = "Choose how you want to run";
            this._subtitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // _linkGitHub
            // 
            this._linkGitHub.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(220)))), ((int)(((byte)(255)))));
            this._linkGitHub.BackColor = System.Drawing.Color.Transparent;
            this._linkGitHub.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this._linkGitHub.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(220)))));
            this._linkGitHub.Location = new System.Drawing.Point(20, 333);
            this._linkGitHub.Name = "_linkGitHub";
            this._linkGitHub.Size = new System.Drawing.Size(380, 20);
            this._linkGitHub.TabIndex = 5;
            this._linkGitHub.TabStop = true;
            this._linkGitHub.Text = "GitHub — full scope badges";
            this._linkGitHub.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this._linkGitHub.VisitedLinkColor = System.Drawing.Color.Gray;
            this._linkGitHub.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkGitHub_LinkClicked);
            // 
            // _btnCmd
            // 
            this._btnCmd.BorderRadius = 10;
            this._btnCmd.Cursor = System.Windows.Forms.Cursors.Hand;
            this._btnCmd.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(200)))));
            this._btnCmd.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(90)))), ((int)(((byte)(140)))));
            this._btnCmd.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this._btnCmd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this._btnCmd.Location = new System.Drawing.Point(40, 120);
            this._btnCmd.Name = "_btnCmd";
            this._btnCmd.Size = new System.Drawing.Size(340, 52);
            this._btnCmd.TabIndex = 2;
            this._btnCmd.Text = "CMD Console Mode";
            this._btnCmd.Click += new System.EventHandler(this.btnCmd_Click);
            // 
            // _btnGui
            // 
            this._btnGui.BorderRadius = 10;
            this._btnGui.Cursor = System.Windows.Forms.Cursors.Hand;
            this._btnGui.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(20)))), ((int)(((byte)(60)))));
            this._btnGui.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this._btnGui.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this._btnGui.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this._btnGui.Location = new System.Drawing.Point(40, 195);
            this._btnGui.Name = "_btnGui";
            this._btnGui.Size = new System.Drawing.Size(340, 52);
            this._btnGui.TabIndex = 3;
            this._btnGui.Text = "GUI Panel Mode";
            this._btnGui.Click += new System.EventHandler(this.btnGui_Click);
            // 
            // _close
            // 
            this._close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._close.BackColor = System.Drawing.Color.Transparent;
            this._close.FillColor = System.Drawing.Color.Transparent;
            this._close.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(20)))), ((int)(((byte)(60)))));
            this._close.HoverState.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(212)))), ((int)(((byte)(255)))));
            this._close.IconColor = System.Drawing.Color.Gray;
            this._close.Location = new System.Drawing.Point(372, 8);
            this._close.Name = "_close";
            this._close.Size = new System.Drawing.Size(40, 32);
            this._close.TabIndex = 4;
            // 
            // LauncherForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(12)))), ((int)(((byte)(12)))), ((int)(((byte)(14)))));
            this.ClientSize = new System.Drawing.Size(420, 300);
            this.Controls.Add(this._panel);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "LauncherForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MANI 272";
            this._panel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2BorderlessForm _borderless;
        private Guna.UI2.WinForms.Guna2Panel _panel;
        private System.Windows.Forms.Label _title;
        private System.Windows.Forms.Label _subtitle;
        private System.Windows.Forms.LinkLabel _linkGitHub;
        private Guna.UI2.WinForms.Guna2GradientButton _btnCmd;
        private Guna.UI2.WinForms.Guna2GradientButton _btnGui;
        private Guna.UI2.WinForms.Guna2ControlBox _close;
    }
}
