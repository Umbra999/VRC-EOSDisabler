namespace EOSDisabler.Wrappers
{
    internal class Logger
    {
        public static void Log(object obj)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine($"[{DateTime.Now.ToShortTimeString()}] [EOSDisabler] {obj}");
        }

        public static void LogDebug(object obj)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"[{DateTime.Now.ToShortTimeString()}] [EOSDisabler] {obj}");
        }

        public static void LogImportant(object obj)
        {
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine($"[{DateTime.Now.ToShortTimeString()}] [EOSDisabler] {obj}");
        }

        public static void LogSuccess(object obj)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine($"[{DateTime.Now.ToShortTimeString()}] [EOSDisabler] {obj}");
        }

        public static void LogError(object obj)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[{DateTime.Now.ToShortTimeString()}] [EOSDisabler] {obj}");
        }

        public static void LogWarning(object obj)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"[{DateTime.Now.ToShortTimeString()}] [EOSDisabler]  {obj}");
        }
    }
}
