using CommandLineEditors.Console;

namespace CommandLineEditors.Editor.ReadLine
{
    internal sealed class ReadLineEditorContext : IEditorContext
    {

        public IConsoleEditorLine ConsoleEditorLine { get; set; }

        public string Result => ConsoleEditorLine.Text;

        public ReadLineEditorContext() { }

        /// <summary>
        /// Gets or sets the flag indicating whether the insert-mode
        /// is active. If false, the editor is in overwrite-mode.
        /// </summary>
        public bool InsertMode { get; set; }

        /// <summary>
        /// Indicated whether the Ctrl-X key has been pressed just before.
        /// </summary>
        public bool CtrlXPressed { get; set; }

        /// <summary>
        /// Indicates whether the Ctrl-V key has been pressed just before.
        /// </summary>
        public bool CtrlVPressed { get; set; }

        /// <summary>
        /// The alternate cursor position to jump back on Ctrl-X,Ctrl-X
        /// </summary>
        public int AlternateCursorPosition { get; set; }

        /// <summary>
        /// Gets or sets the readline history.
        /// </summary>
        public ReadLineHistory<UndoableConsoleEditorLine> History { get; set; }

    }
}
