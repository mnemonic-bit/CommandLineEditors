using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace CommandLineEditors.Extensions
{
    internal static class CharExtensions
    {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static char ToUpper(this char ch)
        {
            return $"{ch}".ToUpper()[0];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static char ToLower(this char ch)
        {
            return $"{ch}".ToLower()[0];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsWhiteSpace(this char ch)
        {
            return ch == ' ' || ch == '\t';
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ToHexString(this char ch)
        {
            return string.Concat(BitConverter.GetBytes(ch).Select(b => b.ToString("x2")));
        }

    }
}
