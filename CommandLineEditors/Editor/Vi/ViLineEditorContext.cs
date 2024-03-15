using CommandLineEditors.Console;
using System;

namespace CommandLineEditors.Editor.Vi
{
    internal sealed class ViLineEditorContext : IEditorContext
    {

        public IConsoleEditorLine ConsoleEditorLine { get; set; }

        public string Result => ConsoleEditorLine.Text;

    }
}
