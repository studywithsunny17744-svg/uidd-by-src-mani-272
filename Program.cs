using System;
using System.IO;
using System.Windows.Forms;
using proxy_uid.Core;

namespace proxy_uid
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += (s, e) => ShowFatalError(e.Exception);
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
                ShowFatalError(e.ExceptionObject as Exception ?? new Exception("Unknown fatal error."));

            try
            {
                AppRunner.Run();
            }
            catch (Exception ex)
            {
                ShowFatalError(ex);
            }
        }

        private static void ShowFatalError(Exception ex)
        {
            string text = ex?.ToString() ?? "Unknown error";
            try
            {
                File.WriteAllText(Path.Combine(Path.GetTempPath(), "uidtek_startup_error.txt"), text);
            }
            catch { }

            MessageBox.Show(
                "Application failed to start:\n\n" + ex?.Message + "\n\nDetails saved to:\n%TEMP%\\uidtek_startup_error.txt",
                AppBranding.AppTitle,
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}
