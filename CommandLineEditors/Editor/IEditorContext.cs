using CommandLineEditors.Console;

namespace CommandLineEditors.Editor
{
    internal interface IEditorContext
    {

        /// <summary>
        /// Gets or sets the console editor line.
        /// </summary>
        IConsoleEditorLine ConsoleEditorLine { get; }

        /// <summary>
        /// Gets the result of reading input from the user.
        /// </summary>
        string? Result { get; }

    }
}
