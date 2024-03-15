using CommandLineEditors.Console;

namespace CommandLineEditors.Editor.Vi
{
    internal sealed class ViLineCommandEditorContext : IEditorContext
    {
        public bool Aborted { get; set; }

        public IConsoleEditorLine ConsoleEditorLine { get; set; }

        public string Result { get; set; }

    }
}
