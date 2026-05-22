using System;
using System.Text;

namespace proxy_uid.Core
{
    /// <summary>GitHub repo + shields.io badges (README, CMD, GUI) — full project scope.</summary>
    public static class GitHubScope
    {
        /// <summary>Your GitHub username or org — change before publishing.</summary>
        public const string Owner = "studywithsunny17744-svg";

        /// <summary>Repository name on GitHub.</summary>
        public const string Repo = "uidd-by-src-mani-272";

        public const string RemoteGitUrl = "https://github.com/studywithsunny17744-svg/uidd-by-src-mani-272.git";

        public const string DefaultBranch = "main";
        public const string Version = "1.0.0";
        public const string LicenseId = "MIT";

        public static string RepositoryUrl => "https://github.com/" + Owner + "/" + Repo;
        public static string ReleasesUrl => RepositoryUrl + "/releases";
        public static string ActionsUrl => RepositoryUrl + "/actions";
        public static string IssuesUrl => RepositoryUrl + "/issues";

        public static bool IsConfigured =>
            !string.IsNullOrWhiteSpace(Owner) && !string.IsNullOrWhiteSpace(Repo);

        public static string BadgeBuild =>
            "https://img.shields.io/github/actions/workflow/status/" + Owner + "/" + Repo
            + "/build.yml?branch=" + DefaultBranch + "&label=build&logo=github";

        public static string BadgeVersion =>
            "https://img.shields.io/badge/release-v" + Version + "-blue?style=flat-square";

        public static string BadgeDotNet =>
            "https://img.shields.io/badge/.NET%20Framework-4.8-512BD4?style=flat-square&logo=dotnet";

        public static string BadgePlatform =>
            "https://img.shields.io/badge/platform-Windows-0078D6?style=flat-square&logo=windows";

        public static string BadgeScope =>
            "https://img.shields.io/badge/scope-CMD%20%7C%20GUI%20%7C%20ADB%20%7C%20Proxy-0e639c?style=flat-square";

        public static string BadgeEmulator =>
            "https://img.shields.io/badge/emulator-BlueStacks%20%7C%20MSI-2ea043?style=flat-square";

        public static string BadgeGame =>
            "https://img.shields.io/badge/game-Free%20Fire%20%7C%20Max-ff6b35?style=flat-square";

        public static string BadgeProxy =>
            "https://img.shields.io/badge/proxy-LAN%20%3A54233-9b59b6?style=flat-square";

        public static string BadgeDiscord =>
            "https://img.shields.io/badge/discord-Rich%20Presence-5865F2?style=flat-square&logo=discord";

        public static string BadgeLicense =>
            "https://img.shields.io/badge/license-" + LicenseId + "-lightgrey?style=flat-square";

        public static string BadgeLastCommit =>
            "https://img.shields.io/github/last-commit/" + Owner + "/" + Repo + "?style=flat-square&logo=github";

        public static string BadgeStars =>
            "https://img.shields.io/github/stars/" + Owner + "/" + Repo + "?style=flat-square&logo=github";

        public static string GetReadmeBadgeMarkdown()
        {
            var sb = new StringBuilder();
            sb.AppendLine("# " + AppBranding.AppTitle + " — " + AppBranding.AppSubtitle);
            sb.AppendLine();
            sb.AppendLine("[![build](" + BadgeBuild + ")](" + ActionsUrl + ")");
            sb.AppendLine("[![version](" + BadgeVersion + ")](" + ReleasesUrl + ")");
            sb.AppendLine("[![.NET](" + BadgeDotNet + ")](" + RepositoryUrl + ")");
            sb.AppendLine("[![platform](" + BadgePlatform + ")](" + RepositoryUrl + ")");
            sb.AppendLine("[![scope](" + BadgeScope + ")](" + RepositoryUrl + ")");
            sb.AppendLine();
            sb.AppendLine("[![emulator](" + BadgeEmulator + ")](" + RepositoryUrl + ")");
            sb.AppendLine("[![game](" + BadgeGame + ")](" + RepositoryUrl + ")");
            sb.AppendLine("[![proxy](" + BadgeProxy + ")](" + RepositoryUrl + ")");
            sb.AppendLine("[![discord](" + BadgeDiscord + ")](" + RepositoryUrl + ")");
            sb.AppendLine("[![license](" + BadgeLicense + ")](" + RepositoryUrl + "/blob/" + DefaultBranch + "/LICENSE)");
            sb.AppendLine();
            sb.AppendLine("[![last commit](" + BadgeLastCommit + ")](" + RepositoryUrl + ")");
            sb.AppendLine("[![stars](" + BadgeStars + ")](" + RepositoryUrl + ")");
            sb.AppendLine();
            sb.AppendLine("> **Full scope:** CMD console + GUI panel, BlueStacks/MSI ADB bypass, certificate tools, LAN proxy (`NetworkProxy`), Discord RPC, CMD/GUI switch.");
            sb.AppendLine();
            sb.AppendLine("Set `Owner` in `Core/GitHubScope.cs` to your GitHub username, then push — badges update automatically.");
            return sb.ToString();
        }
    }
}
