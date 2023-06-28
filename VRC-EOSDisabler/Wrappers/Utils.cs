using System.Diagnostics;

namespace EOSDisabler.Wrappers
{
    internal class Utils
    {
        public static Random Random = new(Environment.TickCount);
        public static string RandomString(int length)
        {
            char[] array = "abcdefghlijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToArray();
            string text = string.Empty;
            for (int i = 0; i < length; i++)
            {
                text += array[Random.Next(array.Length)].ToString();
            }
            return text;
        }

        public static int RandomNumber(int Lowest, int Highest)
        {
            return Random.Next(Lowest, Highest);
        }

        public static byte RandomByte()
        {
            return (byte)Random.Next(0, 255);
        }

        public static Process GetProcessByName(string Name)
        {
            Process[] AllProcesses = Process.GetProcessesByName(Name);
            if (AllProcesses != null && AllProcesses.Length > 0) return AllProcesses[0];

            return null;
        }
    }
}
