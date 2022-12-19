using System.Diagnostics;

namespace Thorarin.AdventOfCode.Framework
{
    internal static class RunContext
    {
        private static int _statusLength = 0;
        private static CancellationTokenSource _cts = new();
        private static readonly object SyncRoot = new();

        private static volatile string? _pendingStatus;
        private static bool _consoleOutput;

        public static bool ConsoleOutput
        {
            get => _consoleOutput;
            set
            {
                if (value)
                {
                    Out = Console.Out;
                }
                else
                {
                    Out = new StringWriter();
                }
            }
        }

        public static TextWriter Out { get; private set; }

        public static void SetStatusAsync(string status)
        {
            var cts = new CancellationTokenSource();
            var oldCts = Interlocked.Exchange(ref _cts, cts);
            oldCts.Cancel();

            _pendingStatus = status;

            Task.Run(() =>
            {
                //if (_pendingStatus != null)
                {
                    SetStatus(status);
                    //_pendingStatus = null;
                }
            },
            cts.Token);
        }

        private static void SetStatus(string status)
        {
            _pendingStatus = null;

            lock (SyncRoot)
            {
                if (_statusLength > status.Length)
                {
                    var (left, top) = Console.GetCursorPosition();
                    Console.SetCursorPosition(left - _statusLength + status.Length, top);
                    Console.Write(new string(' ', _statusLength - status.Length));
                    Console.SetCursorPosition(left - _statusLength, top);
                }
                else if (_statusLength > 0)
                {
                    var (left, top) = Console.GetCursorPosition();
                    Console.SetCursorPosition(left - _statusLength, top);
                }

                Console.Write(status);
                _statusLength = status.Length;
            }
        }

        public static void Reset()
        {
            var cts = Interlocked.Exchange(ref _cts, new CancellationTokenSource());
            cts.Cancel();

            SetStatus("");
            //_statusLength = 0;
        }
    }
}
