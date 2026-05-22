using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace proxy_uid.Core
{
    public static class ConsolePrinter
    {
        public const int WindowWidth = 78;
        public const int WindowHeight = 34;
        private const int ContentWidth = 68;
        private const int MaxFooterLogs = 3;

        private static readonly string SessionId = NewSessionId();
        private static readonly List<FooterLog> FooterLogs = new List<FooterLog>();
        private static bool _actionScreen;

        private struct FooterLog
        {
            public string Message;
            public LogLevel Level;
        }

        public static void ResetState()
        {
            FooterLogs.Clear();
            _actionScreen = false;
        }

        public static void SetupConsole()
        {
            Console.Title = AppBranding.AppTitle + " | " + AppBranding.AppSubtitle;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Clear();

            try
            {
                Console.SetBufferSize(WindowWidth, WindowHeight);
                Console.SetWindowSize(WindowWidth, WindowHeight);
            }
            catch
            {
                try { Console.SetWindowSize(WindowWidth, WindowHeight); } catch { }
            }
        }

        public static void BeginActionScreen() => _actionScreen = true;
        public static void EndActionScreen() => _actionScreen = false;

        public static void RenderDashboard(BypassEngine engine)
        {
            Console.Clear();
            DrawTitleHeader();
            DrawFrameMid();
            DrawInfoLine("Session " + SessionId + "  ·  Proxy " + NetworkProxy.Endpoint);
            Console.WriteLine();
            DrawStatusCards(engine);
            Console.WriteLine();
            DrawSection("EMULATOR ACCESS");
            DrawChoice('1', "Grant access & restart emulator");
            DrawSection("CERTIFICATE");
            DrawChoice('2', "Install system certificate");
            DrawChoice('3', "Remove system certificate");
            DrawSection("PROXY BYPASS");
            DrawChoice('4', "Connect bypass");
            DrawChoice('5', "Disconnect bypass");
            DrawSection("NETWORK");
            DrawChoice('8', "Refresh proxy IP (fix network issues)");
            DrawSection("GITHUB");
            DrawChoice('9', "About & project badges (full scope)");
            DrawSection("SYSTEM");
            DrawChoice('6', "Open configuration");
            DrawChoice('7', "Switch to GUI Panel");
            DrawChoice('0', "Exit application");
            Console.WriteLine();
            DrawFrameBottom();
            DrawFooterLogs();
            WriteChoicePrompt();
        }

        public static void RenderSettings(BypassEngine engine)
        {
            Console.Clear();
            DrawTitleHeader();
            WriteLine("  " + PadCenter("CONFIGURATION", ContentWidth), ConsoleColor.Magenta);
            DrawFrameMid();
            Console.WriteLine();
            DrawChoice('1', "Emulator     →  " + engine.GetEmulatorDisplayName());
            DrawChoice('2', "ADB port     →  " + engine.AdbPort);
            DrawChoice('3', "Game target  →  " + engine.GetGameDisplayName());
            DrawChoice('4', "Proxy endpoint  →  " + NetworkProxy.Endpoint);
            Console.WriteLine();
            DrawChoice('0', "Back to main menu");
            Console.WriteLine();
            DrawFrameBottom();
            DrawFooterLogs();
            WriteChoicePrompt();
        }

        public static void WriteChoicePrompt()
        {
            Write("  choice ", ConsoleColor.Green);
            Write("» ", ConsoleColor.Cyan);
        }

        public static void WriteInputPrompt(string hint)
        {
            Write("  " + hint + " ", ConsoleColor.DarkGray);
            Write("» ", ConsoleColor.Cyan);
        }

        public static void WriteActionHeader(string title)
        {
            Console.Clear();
            DrawTitleHeader();
            Write("  RUNNING  ", ConsoleColor.DarkGray);
            WriteLine(title, ConsoleColor.Cyan);
            DrawFrameMid();
            Console.WriteLine();
        }

        public static void WriteLog(string message, LogLevel level = LogLevel.Info)
        {
            if (!_actionScreen)
            {
                FooterLogs.Add(new FooterLog { Message = message, Level = level });
                while (FooterLogs.Count > MaxFooterLogs)
                    FooterLogs.RemoveAt(0);
                return;
            }

            WriteLogLine(message, level);
        }

        public static void WaitReturn()
        {
            Console.WriteLine();
            WriteLine("  ── press any key ──", ConsoleColor.DarkGray);
            Console.ReadKey(true);
        }

        public static void WriteLine(string text, ConsoleColor color)
        {
            Write(text + Environment.NewLine, color);
        }

        public static void Write(string text, ConsoleColor color)
        {
            var prev = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ForegroundColor = prev;
        }

        private static void DrawFooterLogs()
        {
            if (FooterLogs.Count == 0) return;
            Console.WriteLine();
            foreach (var log in FooterLogs)
                WriteLogLine(log.Message, log.Level);
        }

        private static void WriteLogLine(string message, LogLevel level)
        {
            ConsoleColor dot;
            switch (level)
            {
                case LogLevel.Success: dot = ConsoleColor.Green; break;
                case LogLevel.Error: dot = ConsoleColor.Red; break;
                case LogLevel.Warn: dot = ConsoleColor.Yellow; break;
                default: dot = ConsoleColor.Cyan; break;
            }

            Write("  • ", dot);
            WriteLine(message, ConsoleColor.Gray);
        }

        private static void DrawTitleHeader()
        {
            Write(" ", ConsoleColor.Black);
            Write("✦", ConsoleColor.Magenta);
            Write("╔", ConsoleColor.DarkCyan);
            Write(new string('═', ContentWidth), ConsoleColor.DarkCyan);
            Write("╗", ConsoleColor.DarkCyan);
            WriteLine("✦", ConsoleColor.Magenta);

            Write(" ", ConsoleColor.Black);
            Write("·", ConsoleColor.Cyan);
            Write("║", ConsoleColor.DarkCyan);
            Write(PadCenter(AppBranding.AppTitle, ContentWidth), ConsoleColor.Cyan);
            Write("║", ConsoleColor.DarkCyan);
            WriteLine("·", ConsoleColor.Cyan);

            Write(" ", ConsoleColor.Black);
            Write("°", ConsoleColor.DarkMagenta);
            Write("║", ConsoleColor.DarkCyan);
            Write(PadCenter(AppBranding.AppSubtitle, ContentWidth), ConsoleColor.Cyan);
            Write("║", ConsoleColor.DarkCyan);
            WriteLine("°", ConsoleColor.DarkMagenta);

            Write(" ", ConsoleColor.Black);
            Write("✦", ConsoleColor.Magenta);
            Write("╚", ConsoleColor.DarkCyan);
            Write(new string('═', ContentWidth), ConsoleColor.DarkCyan);
            Write("╝", ConsoleColor.DarkCyan);
            WriteLine("✦", ConsoleColor.Magenta);
        }

        private static void DrawStatusCards(BypassEngine engine)
        {
            string conn = engine.IsConnected ? "ONLINE " : "OFFLINE";
            string emu = Trunc(engine.GetEmulatorDisplayName(), 14);
            string adb = engine.AdbConnected ? "LINKED " : "NO LINK";
            string game = Trunc(engine.GetGameDisplayName(), 14);
            string proxy = Trunc(NetworkProxy.Ip, 14);

            WriteLine("  ┌─────────────┬─────────────┬─────────────┬─────────────┐", ConsoleColor.DarkGray);
            Write("  │ ", ConsoleColor.DarkGray); Write(Pad("Bypass", 11), ConsoleColor.DarkCyan);
            Write(" │ ", ConsoleColor.DarkGray); Write(Pad("Emulator", 11), ConsoleColor.DarkCyan);
            Write(" │ ", ConsoleColor.DarkGray); Write(Pad("ADB", 11), ConsoleColor.DarkCyan);
            Write(" │ ", ConsoleColor.DarkGray); Write(Pad("Game", 11), ConsoleColor.DarkCyan);
            WriteLine(" │", ConsoleColor.DarkGray);

            WriteLine("  ├─────────────┼─────────────┼─────────────┼─────────────┤", ConsoleColor.DarkGray);
            Write("  │ ", ConsoleColor.DarkGray); Write(Pad(conn, 11), engine.IsConnected ? ConsoleColor.Green : ConsoleColor.Yellow);
            Write(" │ ", ConsoleColor.DarkGray); Write(Pad(emu, 11), ConsoleColor.Cyan);
            Write(" │ ", ConsoleColor.DarkGray); Write(Pad(adb, 11), engine.AdbConnected ? ConsoleColor.Green : ConsoleColor.DarkGray);
            Write(" │ ", ConsoleColor.DarkGray); Write(Pad(game, 11), ConsoleColor.Cyan);
            WriteLine(" │", ConsoleColor.DarkGray);

            WriteLine("  ├─────────────┴─────────────┴─────────────┴─────────────┤", ConsoleColor.DarkGray);
            Write("  │ ", ConsoleColor.DarkGray); Write(Pad("Proxy IP", 11), ConsoleColor.DarkCyan);
            Write(" │ ", ConsoleColor.DarkGray);
            Write(Pad(proxy + ":" + NetworkProxy.Port, 47),
                NetworkProxy.Ip == "127.0.0.1" ? ConsoleColor.Yellow : ConsoleColor.Green);
            WriteLine(" │", ConsoleColor.DarkGray);

            WriteLine("  └─────────────────────────────────────────────────────────┘", ConsoleColor.DarkGray);
        }

        public static void WriteGitHubScopeReport()
        {
            WriteLine("  GitHub — full project scope", ConsoleColor.Magenta);
            DrawFrameMid();
            Write("  • ", ConsoleColor.Cyan);
            WriteLine("Repository : " + GitHubScope.RepositoryUrl, ConsoleColor.Gray);
            Write("  • ", ConsoleColor.Cyan);
            WriteLine("Version    : v" + GitHubScope.Version, ConsoleColor.Gray);
            Write("  • ", ConsoleColor.Cyan);
            WriteLine("CI Build   : " + GitHubScope.ActionsUrl, ConsoleColor.Gray);
            Console.WriteLine();
            WriteLine("  Badges (shields.io)", ConsoleColor.DarkCyan);
            WriteBadgeLine("build", GitHubScope.BadgeBuild);
            WriteBadgeLine("scope", GitHubScope.BadgeScope);
            WriteBadgeLine(".NET", GitHubScope.BadgeDotNet);
            WriteBadgeLine("platform", GitHubScope.BadgePlatform);
            WriteBadgeLine("emulator", GitHubScope.BadgeEmulator);
            WriteBadgeLine("game", GitHubScope.BadgeGame);
            WriteBadgeLine("proxy", GitHubScope.BadgeProxy);
            WriteBadgeLine("discord", GitHubScope.BadgeDiscord);
            WriteBadgeLine("license", GitHubScope.BadgeLicense);
            Console.WriteLine();
            if (!GitHubScope.IsConfigured)
            {
                Write("  • ", ConsoleColor.Yellow);
                WriteLine("Set Core/GitHubScope.cs Owner to your GitHub username, then push for live badges.", ConsoleColor.Yellow);
            }
            else
            {
                Write("  • ", ConsoleColor.Green);
                WriteLine("README.md on GitHub shows all badges automatically.", ConsoleColor.Green);
            }
        }

        private static void WriteBadgeLine(string name, string url)
        {
            Write("    [" + Pad(name, 8) + "] ", ConsoleColor.DarkGray);
            WriteLine(url, ConsoleColor.DarkCyan);
        }

        public static void WriteNetworkProxyReport(NetworkProxyStatus status)
        {
            WriteLine("  Network proxy detection", ConsoleColor.Magenta);
            DrawFrameMid();
            Write("  • ", ConsoleColor.Cyan);
            WriteLine("Proxy IP     : " + status.Ip, ConsoleColor.Gray);
            Write("  • ", ConsoleColor.Cyan);
            WriteLine("Proxy Port   : " + status.Port, ConsoleColor.Gray);
            Write("  • ", ConsoleColor.Cyan);
            WriteLine("Endpoint     : " + status.Endpoint, ConsoleColor.Gray);
            Write("  • ", ConsoleColor.Cyan);
            WriteLine("Emulator uses: adb shell settings put global http_proxy " + status.Endpoint, ConsoleColor.DarkGray);

            if (status.UsingLocalhostFallback)
            {
                Write("  • ", ConsoleColor.Yellow);
                WriteLine("LAN IP not found — check Wi-Fi/Ethernet, disable VPN, then refresh again.", ConsoleColor.Yellow);
            }
            else
            {
                Write("  • ", ConsoleColor.Green);
                WriteLine("Proxy ready for BlueStacks / MSI emulator.", ConsoleColor.Green);
            }
        }

        private static void DrawSection(string title)
        {
            Write("  ▎", ConsoleColor.Magenta);
            WriteLine(title, ConsoleColor.Magenta);
        }

        private static void DrawChoice(char key, string label)
        {
            Write("    ", ConsoleColor.Black);
            Write(key + " ", ConsoleColor.Cyan);
            WriteLine(label, ConsoleColor.Gray);
        }

        private static void DrawFrameTop()
        {
            WriteLine("  ┏" + new string('━', ContentWidth) + "┓", ConsoleColor.DarkGray);
        }

        private static void DrawFrameMid()
        {
            WriteLine("  ┣" + new string('━', ContentWidth) + "┫", ConsoleColor.DarkGray);
        }

        private static void DrawFrameBottom()
        {
            WriteLine("  ┗" + new string('━', ContentWidth) + "┛", ConsoleColor.DarkGray);
        }

        private static void DrawInfoLine(string text)
        {
            WriteLine("  " + text, ConsoleColor.DarkGray);
        }

        private static string PadCenter(string text, int width)
        {
            if (text.Length >= width) return text.Substring(0, width);
            int left = (width - text.Length) / 2;
            return new string(' ', left) + text + new string(' ', width - text.Length - left);
        }

        private static string Pad(string text, int width)
        {
            if (text.Length >= width) return text.Substring(0, width);
            return text + new string(' ', width - text.Length);
        }

        private static string Trunc(string text, int max)
        {
            return text.Length <= max ? text : text.Substring(0, max - 1) + "…";
        }

        private static string NewSessionId()
        {
            try
            {
                using (var rng = RandomNumberGenerator.Create())
                {
                    byte[] b = new byte[4];
                    rng.GetBytes(b);
                    return BitConverter.ToString(b).Replace("-", "").ToLowerInvariant();
                }
            }
            catch
            {
                return Guid.NewGuid().ToString("N").Substring(0, 8);
            }
        }
    }
}
