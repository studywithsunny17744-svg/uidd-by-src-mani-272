using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using proxy_uid.Core;

namespace proxy_uid.UI
{
    public sealed partial class MainForm : Form
    {
        public bool RequestSwitchToCmd { get; private set; }

        private readonly BypassEngine _engine;

        public MainForm()
        {
            _engine = new BypassEngine(AppendLog);
            InitializeComponent();
            ApplyBranding();
            Load += MainForm_Load;
        }

        private void ApplyBranding()
        {
            Text = AppBranding.AppTitle + " - GUI";
            _lblTitle.Text = AppBranding.AppTitle + "  ·  " + AppBranding.AppSubtitle;
            _lblTitle.ForeColor = AppBranding.TitleColor;
            grpSettings.ForeColor = AppBranding.SettingsColor;
            grpLog.ForeColor = AppBranding.SettingsColor;
            _lblStatus.ForeColor = AppBranding.MutedColor;
            lblEmu.ForeColor = lblPort.ForeColor = lblGame.ForeColor = AppBranding.MutedColor;
            _cmbEmu.ForeColor = _cmbGame.ForeColor = _txtPort.ForeColor = AppBranding.TextColor;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            _cmbEmu.Items.Clear();
            if (System.IO.File.Exists(@"C:\Program Files\BlueStacks_nxt\HD-Adb.exe"))
                _cmbEmu.Items.Add("Bluestacks");
            if (System.IO.File.Exists(@"C:\Program Files\Bluestacks_msi5\HD-Adb.exe"))
                _cmbEmu.Items.Add("Msi");
            if (_cmbEmu.Items.Count > 0) _cmbEmu.SelectedIndex = 0;

            _cmbGame.Items.AddRange(new object[] { "Free Fire", "Free Fire Max" });
            _cmbGame.SelectedIndex = 0;
            _txtPort.Text = "5555";

            AppendLog("GUI ready.", LogLevel.Success);
            _ = _engine.InitializeAsync();
            UpdateStatus();
        }

        private void cmbEmu_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_cmbEmu.SelectedItem != null)
            {
                string emu = _cmbEmu.SelectedItem.ToString();
                _engine.SetEmulator(emu.Contains("Msi") ? "msi" : "bluestacks");
                RPC.SetState($"Emulator: {emu}");
            }
            UpdateStatus();
        }

        private void txtPort_TextChanged(object sender, EventArgs e)
        {
            _engine.AdbPort = _txtPort.Text.Trim();
        }

        private void cmbGame_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_cmbGame.SelectedItem == null) return;
            string game = _cmbGame.SelectedItem.ToString();
            _engine.SetGame(game.Contains("Max") ? "ffmax" : "ff");
            RPC.SetState($"Playing {game}");
            UpdateStatus();
        }

        private async void btnGrant_Click(object sender, EventArgs e) => await RunAction(_engine.GrantAccessAsync);
        private async void btnInstall_Click(object sender, EventArgs e) => await RunAction(_engine.InstallCertificateAsync);
        private async void btnRemove_Click(object sender, EventArgs e) => await RunAction(_engine.RemoveCertificateAsync);
        private async void btnConnect_Click(object sender, EventArgs e) => await RunAction(_engine.ConnectAsync);
        private async void btnDisconnect_Click(object sender, EventArgs e) => await RunAction(_engine.DisconnectAsync);

        private void btnSwitchCmd_Click(object sender, EventArgs e)
        {
            RequestSwitchToCmd = true;
            Close();
        }

        private async Task RunAction(Func<Task> action)
        {
            SetButtonsEnabled(false);
            try { await action(); }
            catch (Exception ex) { AppendLog(ex.Message, LogLevel.Error); }
            finally { SetButtonsEnabled(true); UpdateStatus(); }
        }

        private void SetButtonsEnabled(bool on)
        {
            _btnGrant.Enabled = _btnInstall.Enabled = _btnRemove.Enabled = on;
            _btnConnect.Enabled = _btnDisconnect.Enabled = on;
        }

        private void UpdateStatus()
        {
            string conn = _engine.IsConnected ? "Connected" : "Disconnected";
            _lblStatus.Text = $"Status: {conn}  |  Emulator: {_engine.GetEmulatorDisplayName()}  |  Proxy: {NetworkProxy.Endpoint}";
        }

        private void AppendLog(string message, LogLevel level)
        {
            if (InvokeRequired) { BeginInvoke(new Action(() => AppendLog(message, level))); return; }

            Color c;
            switch (level)
            {
                case LogLevel.Success: c = Color.FromArgb(100, 220, 140); break;
                case LogLevel.Error: c = Color.FromArgb(255, 120, 120); break;
                case LogLevel.Warn: c = Color.FromArgb(255, 200, 100); break;
                default: c = Color.Gainsboro; break;
            }

            _log.SelectionStart = _log.TextLength;
            _log.SelectionColor = Color.DimGray;
            _log.AppendText($"[{DateTime.Now:HH:mm:ss}] ");
            _log.SelectionColor = c;
            _log.AppendText(message + Environment.NewLine);
            _log.ScrollToCaret();
        }
    }
}
