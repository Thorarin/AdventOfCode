namespace Thorarin.AdventOfCode.Framework
{
    internal static class RunContext
    {
        private static int _statusLength = 0;

        public static string SetStatus(string status)
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
            
            return status;
        }

        public static void Reset()
        {
            _statusLength = 0;
        }
    }
}
