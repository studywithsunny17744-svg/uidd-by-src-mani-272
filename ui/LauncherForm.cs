using System;
using System.Windows.Forms;
using proxy_uid.Core;

namespace proxy_uid.UI
{
    public sealed partial class LauncherForm : Form
    {
        public AppMode SelectedMode { get; private set; } = AppMode.Console;

        public LauncherForm()
        {
            InitializeComponent();
            ApplyBranding();
        }

        private void ApplyBranding()
        {
            Text = AppBranding.AppTitle;
            _title.Text = AppBranding.AppTitle;
            _title.ForeColor = AppBranding.TitleColor;
            _close.HoverState.IconColor = AppBranding.TitleColor;
            _linkGitHub.Text = GitHubScope.IsConfigured
                ? "github.com/" + GitHubScope.Owner + "/" + GitHubScope.Repo
                : "GitHub — set Owner in GitHubScope.cs";
        }

        private void linkGitHub_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            e.Link.Visited = true;
            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = GitHubScope.RepositoryUrl,
                    UseShellExecute = true
                });
            }
            catch
            {
                MessageBox.Show(
                    "Open in browser:\n" + GitHubScope.RepositoryUrl,
                    AppBranding.AppTitle,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private void btnCmd_Click(object sender, EventArgs e)
        {
            SelectedMode = AppMode.Console;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnGui_Click(object sender, EventArgs e)
        {
            SelectedMode = AppMode.Gui;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
