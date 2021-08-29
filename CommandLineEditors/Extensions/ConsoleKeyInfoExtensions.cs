using System;

namespace CommandLineEditors.Extensions
{
    public static class ConsoleKeyInfoExtensions
    {

        public static bool Equals(this ConsoleKeyInfo keyInfo, ConsoleKey consoleKey, bool shift = false, bool alt = false, bool control = false)
        {
            ConsoleModifiers modifiers = (shift ? ConsoleModifiers.Shift : 0) & (alt ? ConsoleModifiers.Alt : 0) & (control ? ConsoleModifiers.Control : 0);
            return keyInfo.Key == consoleKey && keyInfo.Modifiers == modifiers;
        }

    }
}
