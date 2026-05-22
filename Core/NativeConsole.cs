using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace proxy_uid.Core
{
    internal static class NativeConsole
    {
        private const int AttachParentProcess = -1;
        private static bool _active;

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool AllocConsole();

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool FreeConsole();

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool AttachConsole(int dwProcessId);

        public static void Allocate()
        {
            if (_active)
            {
                RebindStreams();
                return;
            }

            if (!AllocConsole())
                AttachConsole(AttachParentProcess);

            RebindStreams();
            _active = true;
        }

        public static void Free()
        {
            if (!_active) return;
            _active = false;
            try { FreeConsole(); } catch { }
        }

        private static void RebindStreams()
        {
            try
            {
                Console.SetOut(new StreamWriter(Console.OpenStandardOutput(), Encoding.UTF8) { AutoFlush = true });
                Console.SetIn(new StreamReader(Console.OpenStandardInput(), Encoding.UTF8));
                Console.SetError(new StreamWriter(Console.OpenStandardError(), Encoding.UTF8) { AutoFlush = true });
                Console.OutputEncoding = Encoding.UTF8;
            }
            catch { }
        }
    }
}
