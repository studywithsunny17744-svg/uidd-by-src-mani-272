using System;
using System.Threading.Tasks;
using proxy_uid.Core;

namespace proxy_uid
{
    public sealed class CmdShell
    {
        private readonly BypassEngine _engine;

        public CmdShell(BypassEngine engine)
        {
            _engine = engine;
        }

        public async Task<AppRunResult> RunAsync()
        {
            while (true)
            {
                RPC.SetState("Main Menu", true);
                ConsolePrinter.RenderDashboard(_engine);
                string input = ReadChoiceWithParticles();

                if (string.IsNullOrEmpty(input)) continue;

                switch (input)
                {
                    case "1":
                        await RunActionAsync("Grant emulator access", _engine.GrantAccessAsync);
                        break;
                    case "2":
                        await RunActionAsync("Install certificate", _engine.InstallCertificateAsync);
                        break;
                    case "3":
                        await RunActionAsync("Remove certificate", _engine.RemoveCertificateAsync);
                        break;
                    case "4":
                        await RunActionAsync("Connect bypass", _engine.ConnectAsync);
                        break;
                    case "5":
                        await RunActionAsync("Disconnect bypass", _engine.DisconnectAsync);
                        break;
                    case "6":
                        RunSettings();
                        break;
                    case "8":
                        RunNetworkRefresh();
                        break;
                    case "9":
                        RunGitHubAbout();
                        break;
                    case "7":
                    case "m":
                    case "M":
                        return AppRunResult.SwitchToGui;
                    case "0":
                        return AppRunResult.Exit;
                    default:
                        ConsolePrinter.WriteLog("Invalid option. Choose 0-9.", LogLevel.Warn);
                        ConsolePrinter.WaitReturn();
                        break;
                }
            }
        }

        private void RunSettings()
        {
            while (true)
            {
                ConsolePrinter.RenderSettings(_engine);
                string input = ReadChoiceWithParticles();
                if (string.IsNullOrEmpty(input)) continue;

                switch (input)
                {
                    case "1":
                        ConsolePrinter.WriteLine("    1 = BlueStacks    2 = MSI App Player", ConsoleColor.DarkGray);
                        ConsolePrinter.WriteInputPrompt("emulator");
                        string emu = Console.ReadLine()?.Trim();
                        if (_engine.SetEmulator(emu == "2" ? "msi" : "bluestacks"))
                        {
                            ConsolePrinter.WriteLog("Emulator: " + _engine.GetEmulatorDisplayName(), LogLevel.Success);
                            RPC.SetState("Emulator: " + _engine.GetEmulatorDisplayName());
                        }
                        else
                            ConsolePrinter.WriteLog("Invalid. Use 1 or 2.", LogLevel.Error);
                        ConsolePrinter.WaitReturn();
                        break;

                    case "2":
                        ConsolePrinter.WriteInputPrompt("adb port");
                        string port = Console.ReadLine()?.Trim();
                        if (!string.IsNullOrEmpty(port))
                        {
                            _engine.AdbPort = port;
                            ConsolePrinter.WriteLog("ADB port set to " + port, LogLevel.Success);
                        }
                        ConsolePrinter.WaitReturn();
                        break;

                    case "3":
                        ConsolePrinter.WriteLine("    1 = Free Fire    2 = Free Fire Max", ConsoleColor.DarkGray);
                        ConsolePrinter.WriteInputPrompt("game");
                        string game = Console.ReadLine()?.Trim();
                        if (_engine.SetGame(game == "2" ? "ffmax" : "ff"))
                        {
                            ConsolePrinter.WriteLog("Game: " + _engine.GetGameDisplayName(), LogLevel.Success);
                            RPC.SetState("Playing " + _engine.GetGameDisplayName());
                        }
                        else
                            ConsolePrinter.WriteLog("Invalid. Use 1 or 2.", LogLevel.Error);
                        ConsolePrinter.WaitReturn();
                        break;

                    case "4":
                        RunNetworkRefresh();
                        break;

                    case "0":
                        return;

                    default:
                        ConsolePrinter.WriteLog("Invalid setting.", LogLevel.Warn);
                        ConsolePrinter.WaitReturn();
                        break;
                }
            }
        }

        private static void RunGitHubAbout()
        {
            ConsolePrinter.BeginActionScreen();
            ConsolePrinter.WriteActionHeader("GitHub — full scope");
            try
            {
                ConsolePrinter.WriteGitHubScopeReport();
                if (GitHubScope.IsConfigured)
                {
                    try
                    {
                        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                        {
                            FileName = GitHubScope.RepositoryUrl,
                            UseShellExecute = true
                        });
                        ConsolePrinter.WriteLog("Opened repository in browser.", LogLevel.Info);
                    }
                    catch
                    {
                        ConsolePrinter.WriteLog("Could not open browser — copy URL from above.", LogLevel.Warn);
                    }
                }
            }
            catch (Exception ex)
            {
                ConsolePrinter.WriteLog(ex.Message, LogLevel.Error);
            }

            ConsolePrinter.WaitReturn();
            ConsolePrinter.EndActionScreen();
        }

        private static void RunNetworkRefresh()
        {
            ConsolePrinter.BeginActionScreen();
            ConsolePrinter.WriteActionHeader("Refresh network & proxy");
            try
            {
                var status = NetworkProxy.GetStatus();
                ConsolePrinter.WriteNetworkProxyReport(status);
                RPC.SetState("Proxy: " + status.Endpoint);
            }
            catch (Exception ex)
            {
                ConsolePrinter.WriteLog("Network refresh failed: " + ex.Message, LogLevel.Error);
            }

            ConsolePrinter.WaitReturn();
            ConsolePrinter.EndActionScreen();
        }

        private static string ReadChoiceWithParticles()
        {
            using (var fx = new ParticleEffect())
            {
                fx.Start();
                return Console.ReadLine()?.Trim();
            }
        }

        private async Task RunActionAsync(string title, Func<Task> action)
        {
            ConsolePrinter.BeginActionScreen();
            ConsolePrinter.WriteActionHeader(title);
            try
            {
                await action();
            }
            catch (Exception ex)
            {
                ConsolePrinter.WriteLog(ex.Message, LogLevel.Error);
            }
            ConsolePrinter.WaitReturn();
            ConsolePrinter.EndActionScreen();
        }
    }
}
