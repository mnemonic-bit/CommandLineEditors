using System;
using System.Runtime.CompilerServices;

namespace CommandLineEditors.Extensions
{
    internal static class StringExtensions
    {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOf(this string str, Func<char, bool> predicate, int startIndex = 0, bool forwardDirection = true)
        {
            if (startIndex < 0 || startIndex >= str.Length)
            {
                return -1;
            }

            Func<int, bool> chck = (forwardDirection == true) ? ((pos) => pos < str.Length) : (Func<int, bool>)((pos) => pos >= 0);
            Func<int, int> inc = (forwardDirection == true) ? ((pos) => pos + 1) : (Func<int, int>)((pos) => pos - 1);

            for (int pos = startIndex; chck(pos); pos = inc(pos))
            {
                if (predicate(str[pos]))
                {
                    return pos;
                }
            }

            return -1;
        }

    }
}
