using System;

namespace CommandLineEditors.Editor
{
    internal interface IConsoleKeyProcessor
    {

        /// <summary>
        /// Gets or sets the text of the editor.
        /// </summary>
        string Text { get; set; }

        /// <summary>
        /// Possibly consumes the key-info. Returns false
        /// if the key-info was consumed, otherwise true
        /// is returned.
        /// </summary>
        /// <param name="keyInfo"></param>
        /// <returns></returns>
        bool ConsumeKeyInfo(ConsoleKeyInfo keyInfo);

        /// <summary>
        /// Closes the editor by removing its contents from the display and
        /// leaving the cursor position where it was before the editor was
        /// opened.
        /// </summary>
        void Close();

        /// <summary>
        /// Brings the display up-to-date with the editor's content. This is
        /// useful when switching between editors.
        /// </summary>
        void RefreshDisplay();

    }
}
