using System;
using System.IO;
using System.Collections.Generic;

namespace proxy_uid.Core
{
    /// <summary>Reads branding.config at startup (beside EXE or project root).</summary>
    internal static class BrandingConfig
    {
        private static readonly Lazy<Dictionary<string, string>> Values = new Lazy<Dictionary<string, string>>(Load);

        public static string BrandName => Get("BrandName", "TRYHARD");
        public static string Subtitle => Get("Subtitle", "UID Bypass");
        public static string AppTitle => BrandName;
        public static string AppSubtitle => Subtitle;
        public static string AssemblyName => BrandName + " UID BYPASS";

        private static string Get(string key, string fallback)
        {
            string v;
            if (Values.Value.TryGetValue(key, out v) && !string.IsNullOrWhiteSpace(v))
                return v.Trim();
            return fallback;
        }

        private static Dictionary<string, string> Load()
        {
            var cfg = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var path in GetSearchPaths())
            {
                if (!File.Exists(path)) continue;
                try
                {
                    ParseFile(path, cfg);
                    break;
                }
                catch { }
            }
            return cfg;
        }

        private static List<string> GetSearchPaths()
        {
            var list = new List<string>();
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            list.Add(Path.Combine(baseDir, "branding.config"));

            try
            {
                var dir = new DirectoryInfo(baseDir);
                for (int i = 0; i < 6 && dir != null; i++, dir = dir.Parent)
                    list.Add(Path.Combine(dir.FullName, "branding.config"));
            }
            catch { }

            return list;
        }

        private static void ParseFile(string path, Dictionary<string, string> cfg)
        {
            foreach (var line in File.ReadAllLines(path))
            {
                string t = line.Trim();
                if (t.Length == 0 || t.StartsWith("#")) continue;
                int eq = t.IndexOf('=');
                if (eq < 1) continue;
                string key = t.Substring(0, eq).Trim();
                string val = t.Substring(eq + 1).Trim();
                if (key.Length > 0) cfg[key] = val;
            }
        }
    }
}
