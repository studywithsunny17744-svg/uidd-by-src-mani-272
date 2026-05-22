using System.Threading;
using System.Windows.Forms;
using proxy_uid.Core;
using proxy_uid.UI;

namespace proxy_uid
{
    internal static class AppRunner
    {
        public static void Run()
        {
            RPC.InitializeRPC();

            AppMode mode;
            using (var launcher = new LauncherForm())
            {
                if (launcher.ShowDialog() != DialogResult.OK)
                    return;
                mode = launcher.SelectedMode;
            }

            while (true)
            {
                AppRunResult result = mode == AppMode.Console
                    ? RunConsoleMode()
                    : RunGuiMode();

                if (result == AppRunResult.Exit)
                    break;

                if (result == AppRunResult.SwitchToGui)
                    mode = AppMode.Gui;
                else if (result == AppRunResult.SwitchToConsole)
                    mode = AppMode.Console;
            }

            RPC.Shutdown();
        }

        private static AppRunResult RunConsoleMode()
        {
            SynchronizationContext.SetSynchronizationContext(null);

            NativeConsole.Allocate();
            try
            {
                ConsolePrinter.ResetState();
                ConsolePrinter.SetupConsole();

                var engine = new BypassEngine((msg, level) => ConsolePrinter.WriteLog(msg, level));
                try
                {
                    engine.InitializeAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                }
                catch { }

                var shell = new CmdShell(engine);
                return shell.RunAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            }
            finally
            {
                NativeConsole.Free();
            }
        }

        private static AppRunResult RunGuiMode()
        {
            NativeConsole.Free();

            using (var form = new MainForm())
            {
                Application.Run(form);
                return form.RequestSwitchToCmd
                    ? AppRunResult.SwitchToConsole
                    : AppRunResult.Exit;
            }
        }
    }
}
