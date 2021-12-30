using System;
using System.Linq;

namespace CommandLineEditors.Extensions
{
    internal static class CharExtensions
    {

        public static char ToUpper(this char ch)
        {
            return $"{ch}".ToUpper()[0];
        }

        public static char ToLower(this char ch)
        {
            return $"{ch}".ToLower()[0];
        }

        public static bool IsWhiteSpace(this char ch)
        {
            return ch == ' ' || ch == '\t';
        }

        public static string ToHexString(this char ch)
        {
            return string.Concat(BitConverter.GetBytes(ch).Select(b => b.ToString("x2")));
        }

    }
}
