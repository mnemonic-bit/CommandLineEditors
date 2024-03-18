using System;
using System.Runtime.CompilerServices;

namespace CommandLineEditors.Extensions
{
    internal static class StringExtensions
    {

        public static (int, int) GetBoundsOfWhitespace(this string text, int currentPosition)
        {
            int startPosition = text.IndexOf(ch => !ch.IsWhiteSpace(), currentPosition - 1, false);
            startPosition++;
            int endPosition = text.IndexOf(ch => !ch.IsWhiteSpace(), currentPosition);
            return (startPosition, endPosition);
        }

        public static (int, int) GetBoundsOfWord(this string text, int position)
        {
            if (text[position].IsWhiteSpace())
            {
                return (position, position);
            }

            int startPosition = text.IndexOf(ch => ch.IsWhiteSpace(), position, false);
            startPosition = startPosition == -1 ? 0 : startPosition + 1;
            int endPosition = text.IndexOf(ch => ch.IsWhiteSpace(), position, true);
            endPosition = endPosition == -1 ? text.Length : endPosition;
            return (startPosition, endPosition);
        }

        /// <summary>
        /// Finds the position in a text which does match the given predicate.
        /// If the start-index already points at a positin which is matching that
        /// predicate, then the start-index is the result. Otherwise the string
        /// is searched in either forward or backward direction, until the predicate
        /// matches, or the string's end is reached on either side.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="predicate"></param>
        /// <param name="startIndex"></param>
        /// <param name="forwardDirection"></param>
        /// <returns></returns>
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

        public static bool TryFindSingleCharacterWordToTheRight(this string text, int currentPosition, out int position)
        {
            position = -1;

            while (currentPosition < text.Length)
            {
                // Find a position that is not whitespace, before we check
                // the size of the word, otherwise we will include leading or
                // trailing whitespaces.
                currentPosition = text.IndexOf(ch => !ch.IsWhiteSpace(), currentPosition);

                (int startPosition, int endPosition) = text.GetBoundsOfWord(currentPosition);
                if (endPosition - startPosition == 1)
                {
                    position = startPosition;
                    return true;
                }

                currentPosition = endPosition;
            }

            return false;
        }

        public static bool TryFindSingleCharacterWordToTheLeft(this string text, int currentPosition, out int position)
        {
            position = -1;

            while (currentPosition >= 0)
            {
                // Find a position that is not whitespace, before we check
                // the size of the word, otherwise we will include leading or
                // trailing whitespaces.
                currentPosition = text.IndexOf(ch => !ch.IsWhiteSpace(), currentPosition, false);

                (int startPosition, int endPosition) = text.GetBoundsOfWord(currentPosition);
                if (endPosition - startPosition == 1)
                {
                    position = startPosition;
                    return true;
                }

                currentPosition = startPosition - 1;
            }

            return false;
        }

    }
}
