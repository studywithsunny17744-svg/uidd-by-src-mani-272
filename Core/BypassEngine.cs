using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace proxy_uid.Core
{
    public sealed class BypassEngine
    {
        private const string EmbeddedResourceName = "proxy_uid.utils.c8750f0d.0";
        private const string CertHash = "c8750f0d";
        private const string CertUrl = "https://github.com/studywithsunny17744-svg/Crt-272/raw/refs/heads/main/Crt";
        private const string DllUrl = "https://github.com/studywithsunny17744-svg/dlll-uid-g/raw/refs/heads/main/UIDBypassDll.dll";
        public const int ProxyPort = NetworkProxy.Port;

        private static readonly HttpClient Http = CreateHttpClient();
        private readonly Action<string, LogLevel> _log;

        private Assembly _bypassAsm;
        private Task _bypassLoadTask;
        private string _adbPath;
        private bool _connected;
        private bool _adbConnected;

        public string EmulatorType { get; private set; } = "Bluestacks";
        public string AdbPort { get; set; } = "5555";
        public string PackageName { get; private set; } = "com.dts.freefireth";
        public bool IsConnected => _connected;
        public bool AdbConnected => _adbConnected;

        public string GetEmulatorDisplayName()
        {
            if (EmulatorType.IndexOf("Msi", StringComparison.OrdinalIgnoreCase) >= 0)
                return "MSI App Player";
            return "BlueStacks";
        }

        public string GetGameDisplayName()
        {
            if (PackageName.Contains("max"))
                return "Free Fire Max";
            return "Free Fire";
        }

        public BypassEngine(Action<string, LogLevel> log)
        {
            _log = log ?? ((m, l) => ConsolePrinter.WriteLog(m, l));
            AutoDetectEmulator();
        }

        public async Task InitializeAsync()
        {
            _bypassLoadTask = LoadBypassDllAsync();
            await Task.WhenAny(_bypassLoadTask, Task.Delay(100));
        }

        public void AutoDetectEmulator()
        {
            if (File.Exists(@"C:\Program Files\BlueStacks_nxt\HD-Adb.exe"))
                EmulatorType = "Bluestacks";
            else if (File.Exists(@"C:\Program Files\Bluestacks_msi5\HD-Adb.exe"))
                EmulatorType = "Msi";
        }

        public bool SetEmulator(string value)
        {
            var v = value?.Trim().ToLowerInvariant();
            if (v == "bluestacks" || v == "nxt" || v == "1")
            {
                EmulatorType = "Bluestacks";
                return true;
            }
            if (v == "msi" || v == "2")
            {
                EmulatorType = "Msi";
                return true;
            }
            return false;
        }

        public bool SetGame(string value)
        {
            var v = value?.Trim().ToLowerInvariant();
            if (v == "ff" || v == "freefire" || v == "1")
            {
                PackageName = "com.dts.freefireth";
                return true;
            }
            if (v == "ffmax" || v == "max" || v == "2")
            {
                PackageName = "com.dts.freefiremax";
                return true;
            }
            return false;
        }

        public string GetStatusText()
        {
            return string.Join(Environment.NewLine, new[]
            {
                $"  Emulator   : {EmulatorType}",
                $"  ADB Port   : {AdbPort}",
                $"  Game       : {PackageName}",
                $"  Proxy      : {GetProxyEndpoint()}",
                $"  Connection : {(_connected ? "CONNECTED" : "DISCONNECTED")}"
            });
        }

        private static HttpClient CreateHttpClient()
        {
            try
            {
                ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12 | (SecurityProtocolType)3072;
            }
            catch { }

            var client = new HttpClient { Timeout = TimeSpan.FromSeconds(60) };
            client.DefaultRequestHeaders.UserAgent.ParseAdd("proxy-uid/1.0");
            return client;
        }

        public static string ProxyIp => NetworkProxy.Ip;

        public static string GetProxyEndpoint() => NetworkProxy.Endpoint;

        public static void RefreshProxyIp() => NetworkProxy.Refresh();

        public static string GetLocalIPv4(bool forceRefresh = false) => NetworkProxy.GetLocalIPv4(forceRefresh);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern IntPtr LoadLibrary(string lpFileName);

        private string ResolveAdbPath()
        {
            string msi = @"C:\Program Files\Bluestacks_msi5\HD-Adb.exe";
            string nxt = @"C:\Program Files\BlueStacks_nxt\HD-Adb.exe";

            if (EmulatorType.IndexOf("Msi", StringComparison.OrdinalIgnoreCase) >= 0 && File.Exists(msi))
            {
                _adbPath = msi;
                return msi;
            }

            if (EmulatorType.IndexOf("Bluestacks", StringComparison.OrdinalIgnoreCase) >= 0 && File.Exists(nxt))
            {
                _adbPath = nxt;
                return nxt;
            }

            throw new FileNotFoundException("HD-Adb.exe was not found for: " + EmulatorType);
        }

        private async Task<string> ObtainCertAsync(string url, string hash)
        {
            string outPath = Path.Combine(Path.GetTempPath(), hash + ".0");

            try
            {
                _log("Downloading certificate from server...", LogLevel.Info);
                byte[] data = await Http.GetByteArrayAsync(url);
                if (data == null || data.Length == 0)
                    throw new Exception("Empty cert response.");
                File.WriteAllBytes(outPath, data);
                _log($"Certificate downloaded ({data.Length} bytes).", LogLevel.Success);
                return outPath;
            }
            catch (Exception ex)
            {
                _log("Online cert download failed: " + ex.Message + " - using embedded fallback.", LogLevel.Warn);
                using (var res = Assembly.GetExecutingAssembly().GetManifestResourceStream(EmbeddedResourceName))
                {
                    if (res == null)
                        throw new FileNotFoundException("Certificate not available online and no embedded fallback found.");

                    using (var fs = File.Create(outPath))
                        res.CopyTo(fs);
                }
                return outPath;
            }
        }

        private async Task LoadBypassDllAsync()
        {
            if (_bypassAsm != null) return;

            try
            {
                _log("Downloading UID bypass module...", LogLevel.Info);
                byte[] data = await Http.GetByteArrayAsync(DllUrl);
                if (data == null || data.Length == 0)
                    throw new Exception("Empty DLL response.");

                _log($"Bypass DLL downloaded ({data.Length} bytes). Loading...", LogLevel.Info);

                try
                {
                    _bypassAsm = Assembly.Load(data);
                    _log($"Managed bypass module loaded: {_bypassAsm.GetName().Name}", LogLevel.Success);
                }
                catch (BadImageFormatException)
                {
                    string dllPath = Path.Combine(Path.GetTempPath(), "UIDBypassDll.dll");
                    File.WriteAllBytes(dllPath, data);
                    IntPtr h = LoadLibrary(dllPath);
                    if (h == IntPtr.Zero)
                        throw new Exception("Native LoadLibrary failed (Win32 error " + Marshal.GetLastWin32Error() + ").");
                    _log("Native bypass DLL loaded.", LogLevel.Success);
                }

                if (_bypassAsm != null && !TryInvokeBypassEntry(_bypassAsm))
                    _log("Bypass module loaded (no Start/Run entry found).", LogLevel.Info);

                RefreshProxyIp();
                _log("Local proxy server target: " + GetProxyEndpoint(), LogLevel.Info);
            }
            catch (Exception ex)
            {
                _log("Failed to load bypass module: " + ex.Message, LogLevel.Error);
            }
        }

        private bool TryInvokeBypassEntry(Assembly asm)
        {
            string[] candidates = { "Start", "StartServer", "StartProxy", "Init", "Initialize", "Run", "Main" };

            foreach (var type in SafeGetTypes(asm))
            {
                foreach (var name in candidates)
                {
                    var m = type.GetMethod(name, BindingFlags.Public | BindingFlags.Static);
                    if (m == null) continue;

                    object[] args;
                    var ps = m.GetParameters();
                    if (ps.Length == 0)
                        args = null;
                    else if (ps.Length == 1 && ps[0].ParameterType == typeof(int))
                        args = new object[] { ProxyPort };
                    else if (ps.Length == 1 && ps[0].ParameterType == typeof(string))
                        args = new object[] { GetLocalIPv4() };
                    else if (ps.Length == 2 && ps[0].ParameterType == typeof(string) && ps[1].ParameterType == typeof(int))
                        args = new object[] { GetLocalIPv4(), ProxyPort };
                    else if (ps.Length == 1 && ps[0].ParameterType == typeof(string[]))
                        args = new object[] { new string[0] };
                    else
                        continue;

                    _ = Task.Run(() =>
                    {
                        try { m.Invoke(null, args); }
                        catch (Exception ex)
                        {
                            _log("Bypass entry threw: " + (ex.InnerException?.Message ?? ex.Message), LogLevel.Error);
                        }
                    });
                    _log($"Bypass started -> {type.FullName}.{m.Name}", LogLevel.Success);
                    return true;
                }
            }

            if (asm.EntryPoint != null)
            {
                var ep = asm.EntryPoint;
                object[] args = ep.GetParameters().Length == 0 ? null : new object[] { new string[0] };
                _ = Task.Run(() =>
                {
                    try { ep.Invoke(null, args); }
                    catch (Exception ex)
                    {
                        _log("Bypass EntryPoint threw: " + (ex.InnerException?.Message ?? ex.Message), LogLevel.Error);
                    }
                });
                _log("Bypass EntryPoint invoked.", LogLevel.Success);
                return true;
            }

            return false;
        }

        private static IEnumerable<Type> SafeGetTypes(Assembly asm)
        {
            try { return asm.GetExportedTypes(); }
            catch (ReflectionTypeLoadException ex) { return ex.Types.Where(t => t != null); }
            catch { return Enumerable.Empty<Type>(); }
        }

        private void RunAdb(string exe, string args, out string stdout, out string stderr)
        {
            var psi = new ProcessStartInfo
            {
                FileName = exe,
                Arguments = $"-s 127.0.0.1:{AdbPort.Trim()} {args}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            try
            {
                using (var proc = Process.Start(psi))
                {
                    if (proc == null)
                        throw new Exception("Failed to start ADB process.");

                    stdout = proc.StandardOutput.ReadToEnd();
                    stderr = proc.StandardError.ReadToEnd();
                    proc.WaitForExit();
                }
            }
            catch (Exception ex) when (ex is Win32Exception || ex is FileNotFoundException)
            {
                throw new Exception($"Failed to execute ADB. Check path: {ex.Message}");
            }
        }

        private bool ConnectAdb(string adbExe)
        {
            string address = $"127.0.0.1:{AdbPort.Trim()}";

            try
            {
                RunAdb(adbExe, $"connect {address}", out var output, out var error);

                if (!string.IsNullOrWhiteSpace(error) && error.Contains("error"))
                {
                    _log("ADB connection failed: " + error.Trim(), LogLevel.Error);
                    return false;
                }

                if (output.Contains("connected"))
                {
                    _adbConnected = true;
                    _log("ADB connected.", LogLevel.Success);
                    return true;
                }

                _adbConnected = false;
                _log("ADB connection failed: " + output.Trim(), LogLevel.Error);
                return false;
            }
            catch (Exception ex)
            {
                _adbConnected = false;
                _log("ADB connection error: " + ex.Message, LogLevel.Error);
                return false;
            }
        }

        private bool FileExistsOnDevice(string adb, string path)
        {
            RunAdb(adb, $"shell \"[ -f {path} ] && echo yes || echo no\"", out var result, out _);
            return result.Trim().Equals("yes", StringComparison.OrdinalIgnoreCase);
        }

        private void EditConfigs(string engineRootPath)
        {
            if (!Directory.Exists(engineRootPath))
                throw new DirectoryNotFoundException("Engine path not found: " + engineRootPath);

            foreach (var dir in Directory.GetDirectories(engineRootPath))
            {
                string baseName = Path.GetFileName(dir);
                string[] files = {
                    Path.Combine(dir, "Android.bstk.in"),
                    Path.Combine(dir, baseName + ".bstk"),
                    Path.Combine(dir, baseName + ".bstk-prev")
                };

                foreach (string file in files.Where(File.Exists))
                {
                    string content = File.ReadAllText(file, Encoding.UTF8);
                    bool changed = false;

                    string newContent = Regex.Replace(content,
                        @"(\<HardDisk\b[^\>]*location\s*=\s*""Root\.vhd""[^\>]*type\s*=\s*"")Readonly(""\s*/?\>)",
                        match => { changed = true; return $"{match.Groups[1].Value}Normal{match.Groups[2].Value}"; },
                        RegexOptions.IgnoreCase);

                    newContent = Regex.Replace(newContent,
                        @"(\<HardDisk\b[^\>]*location\s*=\s*""Data\.vhdx""[^\>]*type\s*=\s*"")Readonly(""\s*/?\>)",
                        match => { changed = true; return $"{match.Groups[1].Value}Normal{match.Groups[2].Value}"; },
                        RegexOptions.IgnoreCase);

                    if (changed)
                        File.WriteAllText(file, newContent, Encoding.UTF8);
                }
            }
        }

        public async Task GrantAccessAsync()
        {
            try
            {
                _log("Stopping BlueStacks processes...", LogLevel.Info);
                string[] processesToKill = { "HD-Player", "HD-Adb", "HD-MultiInstanceManager", "BstkSVC" };
                foreach (var procName in processesToKill)
                {
                    foreach (var proc in Process.GetProcessesByName(procName))
                    {
                        try
                        {
                            proc.Kill();
                            await Task.Run(() => proc.WaitForExit(2000));
                        }
                        catch { }
                    }
                }

                bool isMsi = EmulatorType.IndexOf("Msi", StringComparison.OrdinalIgnoreCase) >= 0;
                string engineRoot = isMsi
                    ? @"C:\ProgramData\Bluestacks_msi5\Engine"
                    : @"C:\ProgramData\BlueStacks_nxt\Engine";

                EditConfigs(engineRoot);
                _log("Configuration files updated.", LogLevel.Success);

                string managerDir = Path.Combine(engineRoot, "Manager");
                if (Directory.Exists(managerDir))
                {
                    var logFiles = Directory.GetFiles(managerDir, "BstkServer.log")
                        .Concat(Directory.GetFiles(managerDir, "BstkServer.log.*"));

                    foreach (var file in logFiles)
                    {
                        try { File.Delete(file); }
                        catch
                        {
                            try { using (var fs = new FileStream(file, FileMode.Create, FileAccess.Write)) { } } catch { }
                        }
                    }
                }

                foreach (var instanceDir in Directory.GetDirectories(engineRoot))
                {
                    string logsDir = Path.Combine(instanceDir, "Logs");
                    if (!Directory.Exists(logsDir)) continue;
                    foreach (var file in Directory.GetFiles(logsDir, "BstkCore.log*"))
                    {
                        try { File.Delete(file); } catch { }
                    }
                }

                _log("Old logs cleared.", LogLevel.Info);
                _log("Access granted.", LogLevel.Success);

                string playerPath = isMsi
                    ? @"C:\Program Files\Bluestacks_msi5\HD-Player.exe"
                    : @"C:\Program Files\BlueStacks_nxt\HD-Player.exe";

                if (File.Exists(playerPath))
                {
                    Process.Start(playerPath);
                    _log("Emulator started.", LogLevel.Success);
                }
                else
                {
                    _log("Emulator executable not found: " + playerPath, LogLevel.Error);
                }
            }
            catch (Exception ex)
            {
                _log("Grant access failed: " + ex.Message, LogLevel.Error);
            }
        }

        public async Task InstallCertificateAsync()
        {
            try
            {
                string adb = ResolveAdbPath();
                if (!ConnectAdb(adb)) return;

                _log("Installing certificate...", LogLevel.Info);
                string tmp = await ObtainCertAsync(CertUrl, CertHash);

                await Task.Run(() => RunAdb(adb, $"push \"{tmp}\" /sdcard/{CertHash}.0", out _, out _));
                _log("Certificate pushed to device.", LogLevel.Info);
                try { File.Delete(tmp); } catch { }

                string suPath = "/boot/android/android/system/xbin/bstk/su";
                string cmd =
                    $"{suPath} -c 'mount -o rw,remount /system && " +
                    $"cp /sdcard/{CertHash}.0 /system/etc/security/cacerts/{CertHash}.0 && " +
                    $"chmod 644 /system/etc/security/cacerts/{CertHash}.0 && " +
                    $"chcon u:object_r:system_file:s0 /system/etc/security/cacerts/{CertHash}.0 && " +
                    $"rm /sdcard/{CertHash}.0 && " +
                    $"mount -o ro,remount /system && " +
                    $"setprop ctl.restart zygote'";

                string shellError = null;
                await Task.Run(() => RunAdb(adb, $"shell \"{cmd}\"", out _, out shellError));

                if (!string.IsNullOrWhiteSpace(shellError))
                    _log("Shell warning: " + shellError.Trim(), LogLevel.Warn);

                await Task.Delay(2000);
                _log("Certificate installed. Emulator may restart.", LogLevel.Success);
            }
            catch (Exception ex)
            {
                _log("Certificate install failed: " + ex.Message, LogLevel.Error);
            }
        }

        public async Task RemoveCertificateAsync()
        {
            try
            {
                string adb = ResolveAdbPath();
                if (!ConnectAdb(adb)) return;

                _log("Removing certificate...", LogLevel.Info);
                string suPath = "/boot/android/android/system/xbin/bstk/su";
                string cmd =
                    $"{suPath} -c 'mount -o rw,remount /dev/sda1 /system && " +
                    $"rm /system/etc/security/cacerts/{CertHash}.0 && " +
                    $"mount -o ro,remount /dev/sda1 /system && " +
                    $"setprop ctl.restart zygote'";

                await Task.Run(() => RunAdb(adb, $"shell \"{cmd}\"", out _, out _));
                await Task.Delay(2000);

                if (!FileExistsOnDevice(adb, $"/system/etc/security/cacerts/{CertHash}.0"))
                    _log("Certificate removed successfully.", LogLevel.Success);
                else
                    _log("Certificate removal failed: file still on device.", LogLevel.Error);
            }
            catch (Exception ex)
            {
                _log("Certificate removal failed: " + ex.Message, LogLevel.Error);
            }
        }

        private string GetEmulatorDevice()
        {
            try
            {
                var psi = new ProcessStartInfo
                {
                    FileName = _adbPath,
                    Arguments = "devices",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (var process = Process.Start(psi))
                {
                    process.WaitForExit();
                    string output = process.StandardOutput.ReadToEnd();
                    foreach (var line in output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        if (line.EndsWith("\tdevice"))
                            return line.Split('\t')[0];
                    }
                }
            }
            catch { }

            return $"127.0.0.1:{AdbPort.Trim()}";
        }

        private Task<string> RunAdbCommandAsync(string arguments)
        {
            return Task.Run(() =>
            {
                try
                {
                    var psi = new ProcessStartInfo
                    {
                        FileName = _adbPath,
                        Arguments = arguments,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };

                    using (var process = Process.Start(psi))
                    {
                        process.WaitForExit();
                        string output = process.StandardOutput.ReadToEnd();
                        string error = process.StandardError.ReadToEnd();
                        if (!string.IsNullOrEmpty(error))
                            return "Error: " + error;
                        return output;
                    }
                }
                catch (Exception ex)
                {
                    return "Exception: " + ex.Message;
                }
            });
        }

        public async Task ConnectAsync()
        {
            try
            {
                RefreshProxyIp();
                if (_bypassLoadTask != null && !_bypassLoadTask.IsCompleted)
                {
                    _log("Waiting for bypass module...", LogLevel.Info);
                    await _bypassLoadTask;
                }

                ResolveAdbPath();
                string device = GetEmulatorDevice();
                string proxyAddr = GetProxyEndpoint();

                _log("Emulator: " + device, LogLevel.Success);
                _log("Routing traffic through " + proxyAddr, LogLevel.Info);

                await RunAdbCommandAsync($"-s {device} shell am force-stop {PackageName}");
                await RunAdbCommandAsync($"-s {device} shell settings put global http_proxy {proxyAddr}");
                await RunAdbCommandAsync($"-s {device} shell monkey -p {PackageName} -c android.intent.category.LAUNCHER 1");

                _connected = true;
                _log("Bypass connected successfully.", LogLevel.Success);
            }
            catch (Exception ex)
            {
                _log("Connection failed: " + ex.Message, LogLevel.Error);
            }
        }

        public async Task DisconnectAsync()
        {
            try
            {
                ResolveAdbPath();
                string device = GetEmulatorDevice();
                await RunAdbCommandAsync($"-s {device} shell settings put global http_proxy :0");
                _connected = false;
                _log("Bypass disconnected.", LogLevel.Info);
            }
            catch (Exception ex)
            {
                _log("Disconnect failed: " + ex.Message, LogLevel.Error);
            }
        }
    }
}
