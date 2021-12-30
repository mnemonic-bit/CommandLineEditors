using System;
using System.Linq;

namespace ConsoleKeyReader
{
    class Program
    {

        private static readonly ConsoleKeyInfo CtrlD = new ConsoleKeyInfo();

        private static readonly ConsoleKeyInfo YKey = new ConsoleKeyInfo();

        static void Main(string[] args)
        {
            Console.WriteLine("Press any key to see how the ConsoleKeyInfo looks like");

            bool stopReading = false;
            while (!stopReading)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                Console.WriteLine($"new ConsoleKeyInfo('\\u{ ToHexString(keyInfo.KeyChar) }', ConsoleKey.{ keyInfo.Key }, { ToString(keyInfo.Modifiers) })");

                if (keyInfo == CtrlD)
                {
                    Console.WriteLine($"You have hit Ctrl-D. Do you want to quit this application? [Y|n]");
                    ConsoleKeyInfo answerKeyInfo = Console.ReadKey(true);
                    if (answerKeyInfo == YKey)
                    {
                        stopReading = true;
                    }
                }
            }
        }

        private static string ToString(ConsoleModifiers modifiers)
        {
            return $"{ToString((modifiers & ConsoleModifiers.Shift) > 0)}, {ToString((modifiers & ConsoleModifiers.Alt) > 0)}, {ToString((modifiers & ConsoleModifiers.Control) > 0)}";
        }

        private static string ToString(bool b)
        {
            return b ? "true" : "false";
        }

        public static string ToHexString(char ch)
        {
            return string.Concat(BitConverter.GetBytes(ch).Reverse().Select(b => b.ToString("x2")));
        }

    }
}
