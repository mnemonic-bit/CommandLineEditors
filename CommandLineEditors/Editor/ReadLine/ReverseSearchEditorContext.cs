using CommandLineEditors.Console;
using CommandLineEditors.Data;

namespace CommandLineEditors.Editor.ReadLine
{
    internal class ReverseSearchEditorContext : IEditorContext
    {

        public IConsoleEditorLine ConsoleEditorLine { get; set; }

        public bool Aborted { get; set; }

        public string Result => Hit;

        public History<string> History { get; set; }

        public string Hit { get; set; }

    }
}
