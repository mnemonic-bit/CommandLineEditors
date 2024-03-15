using System;
using System.Runtime.CompilerServices;
using ConsoleColor = System.ConsoleColor;
using SystemConsole = System.Console;

namespace CommandLineEditors.Console
{
    /// <summary>
    /// This class is a layer over the normal console which is
    /// provided by the CLR of .NET. It provides ease of positioning
    /// and writing of text at positions.
    /// </summary>
    public static class ConsoleLayer
    {

        /// <summary>
        /// Gets or sets the color of the text.
        /// </summary>
        public static ConsoleColor TextColor
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => SystemConsole.ForegroundColor;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => SystemConsole.ForegroundColor = value;
        }

        /// <summary>
        /// Gets or sets the background color of the text.
        /// </summary>
        public static ConsoleColor BackgroundColor
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => SystemConsole.BackgroundColor;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => SystemConsole.BackgroundColor = value;
        }

        /// <summary>
        /// Initializes this static class and sets prepares the System.Console
        /// for our use-case.
        /// </summary>
        static ConsoleLayer()
        {
            SystemConsole.TreatControlCAsInput = true;
        }

        /// <summary>
        /// Reads a single key from the console and returns a <code>ConsoleKeyInfo</code>
        /// representing that key and all modifiers pressed at the same time.
        /// </summary>
        /// <param name="intercept">Set this to false, if you also want console output for that key.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ConsoleKeyInfo ReadKey(bool intercept = true)
        {
            //while (!Console.KeyAvailable)
            //{
            //    Thread.Sleep(5);
            //}

            return SystemConsole.ReadKey(intercept);
        }

        /// <summary>
        /// Writes a text starting from the current cursor position.
        /// </summary>
        /// <param name="text"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteString(string text)
        {
            SystemConsole.WriteLine(text);
        }

        /// <summary>
        /// Prints a character at the given position.
        /// </summary>
        /// <param name="x">The x-position at which the character will be printed.</param>
        /// <param name="y">The y-position at which the character will be printed.</param>
        /// <param name="character">The character which will be printed on the console.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteCharacterAtPosition(int x, int y, char character)
        {
            SetCursorPosition(x, y);
            SystemConsole.Write(character);
        }

        /// <summary>
        /// Displays the rest of the input buffer starting from the given
        /// cursor position.
        /// </summary>
        /// <param name="x">The x-position to start from.</param>
        /// <param name="y">The y-position to start from.</param>
        /// <param name="posInBuffer">The position in the input buffer from where the output starts.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteStringAtPosition(int x, int y, string text)
        {
            SetCursorPosition(x, y);
            SystemConsole.Write(text);
        }

        /// <summary>
        /// Clears the whole console display.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Clear()
        {
            SystemConsole.Clear();
        }

        /// <summary>
        /// Moves the cursor to the next line.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void NewLine()
        {
            SystemConsole.WriteLine();
        }

        /// <summary>
        /// Clears an area of given length starting from the current position.
        /// After the area is overwritten with space characters, the cursor
        /// will be set to that position it was before the method call.
        /// </summary>
        /// <param name="length">The number of characters to write.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ClearArea(int length)
        {
            ClearAreaAtPosition(SystemConsole.CursorLeft, SystemConsole.CursorTop, length);
        }

        /// <summary>
        /// Clears the display from a given position for a given number
        /// of characters, and sets the cursor to the start position of
        /// the cleared area.
        /// </summary>
        /// <param name="x">The x-position on the console display to be cleared.</param>
        /// <param name="y">The y-position on the console display to be cleared.</param>
        /// <param name="length">The number of characters that will be cleared.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ClearAreaAtPosition(int x, int y, int length)
        {
            SetCursorPosition(x, y);
            for (int i = 0; i < length; i++)
            {
                SystemConsole.Write(" ");
            }
            SetCursorPosition(x, y);
        }

        /// <summary>
        /// Sets the cursor position on the display taking the current
        /// window width into account.
        /// </summary>
        /// <param name="x">The new x-position of the cursor.</param>
        /// <param name="y">The new y-position of the cursor.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetCursorPosition(int x, int y)
        {
            int windowWidth = SystemConsole.WindowWidth;
            if (x < 0)
            {
                SystemConsole.SetCursorPosition(windowWidth - (-x) % windowWidth, y - (-x) / windowWidth - 1);
            }
            else if (x < windowWidth)
            {
                SystemConsole.SetCursorPosition(x, y);
            }
            else
            {
                SystemConsole.SetCursorPosition(x % windowWidth, y + x / windowWidth);
            }
        }

    }
}
