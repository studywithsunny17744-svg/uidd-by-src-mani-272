using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace proxy_uid.Core
{
    public sealed class ParticleEffect : IDisposable
    {
        private readonly object _lock = new object();
        private readonly Random _rng = new Random();
        private readonly List<Particle> _particles = new List<Particle>();
        private CancellationTokenSource _cts;
        private Task _loopTask;

        private static readonly char[] Symbols = { '·', '°', '•', '*', '✦', '˙', '▪' };
        private static readonly ConsoleColor[] Colors =
        {
            ConsoleColor.DarkCyan,
            ConsoleColor.Cyan,
            ConsoleColor.Magenta,
            ConsoleColor.DarkMagenta,
            ConsoleColor.DarkGray,
            ConsoleColor.White
        };

        private class Particle
        {
            public int X;
            public int Y;
            public int PrevX;
            public int PrevY;
            public float Speed;
            public char Symbol;
            public ConsoleColor Color;
            public bool Active;
        }

        public void Start()
        {
            Stop();
            _cts = new CancellationTokenSource();
            _particles.Clear();

            for (int i = 0; i < 28; i++)
                _particles.Add(CreateParticle(true));

            _loopTask = Task.Run(() => LoopAsync(_cts.Token));
        }

        public void Stop()
        {
            if (_cts == null) return;
            _cts.Cancel();
            try { _loopTask?.Wait(300); } catch { }
            _cts.Dispose();
            _cts = null;
            ClearAll();
        }

        public void Dispose() => Stop();

        private Particle CreateParticle(bool randomY)
        {
            int side = _rng.Next(2);
            int x = side == 0 ? _rng.Next(2) : ConsolePrinter.WindowWidth - 1 - _rng.Next(2);

            return new Particle
            {
                X = x,
                Y = randomY ? _rng.Next(4, ConsolePrinter.WindowHeight - 2) : 4,
                PrevX = -1,
                PrevY = -1,
                Speed = 0.15f + (float)_rng.NextDouble() * 0.35f,
                Symbol = Symbols[_rng.Next(Symbols.Length)],
                Color = Colors[_rng.Next(Colors.Length)],
                Active = true
            };
        }

        private async Task LoopAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    Tick();
                    Draw();
                    await Task.Delay(90, token);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch { }
            }
        }

        private void Tick()
        {
            foreach (var p in _particles)
            {
                if (!p.Active) continue;

                p.PrevX = p.X;
                p.PrevY = p.Y;
                p.Y += (int)Math.Max(1, p.Speed);

                if (p.Y >= ConsolePrinter.WindowHeight - 2)
                {
                    var fresh = CreateParticle(false);
                    p.X = fresh.X;
                    p.Y = 4;
                    p.PrevX = -1;
                    p.PrevY = -1;
                    p.Symbol = fresh.Symbol;
                    p.Color = fresh.Color;
                    p.Speed = fresh.Speed;
                }

                if (_rng.Next(100) < 4)
                    p.Symbol = Symbols[_rng.Next(Symbols.Length)];
            }
        }

        private void Draw()
        {
            lock (_lock)
            {
                try
                {
                    foreach (var p in _particles)
                    {
                        if (!p.Active) continue;

                        if (p.PrevX >= 0 && IsMargin(p.PrevX, p.PrevY))
                            WriteAt(p.PrevX, p.PrevY, ' ', ConsoleColor.Black);

                        if (IsMargin(p.X, p.Y))
                            WriteAt(p.X, p.Y, p.Symbol, p.Color);
                    }
                }
                catch { }
            }
        }

        private void ClearAll()
        {
            lock (_lock)
            {
                try
                {
                    for (int y = 0; y < ConsolePrinter.WindowHeight; y++)
                    {
                        WriteAt(0, y, ' ', ConsoleColor.Black);
                        WriteAt(1, y, ' ', ConsoleColor.Black);
                        WriteAt(ConsolePrinter.WindowWidth - 1, y, ' ', ConsoleColor.Black);
                        WriteAt(ConsolePrinter.WindowWidth - 2, y, ' ', ConsoleColor.Black);
                    }
                }
                catch { }
            }
        }

        private static bool IsMargin(int x, int y)
        {
            if (y < 0 || y >= ConsolePrinter.WindowHeight) return false;
            return x <= 1 || x >= ConsolePrinter.WindowWidth - 2;
        }

        private static void WriteAt(int x, int y, char ch, ConsoleColor color)
        {
            Console.SetCursorPosition(x, y);
            Console.ForegroundColor = color;
            Console.Write(ch);
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}
