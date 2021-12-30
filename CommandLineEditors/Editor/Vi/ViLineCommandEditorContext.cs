using CommandLineEditors.Console;

namespace CommandLineEditors.Editor.Vi
{
    internal class ViLineCommandEditorContext : IEditorContext
    {

        public IConsoleEditorLine ConsoleEditorLine { get; set; }

        public string Result { get; set; }

        public bool Aborted { get; set; }

    }
}
