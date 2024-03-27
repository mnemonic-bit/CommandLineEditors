using System.Collections;
using System.Collections.Generic;

namespace CommandLineEditors.Editor
{
    internal interface IConsoleEditor
    {

        /// <summary>
        /// Closes the command line editor, and sets the cursor to the
        /// next available line on the screen.
        /// </summary>
        void Close();

        /// <summary>
        /// Gets the current list of history entries from this editor.
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> GetHistory();

        /// <summary>
        /// Reads a single line of input from the user. The input cursor
        /// starts at the cursor's current position.
        /// </summary>
        /// <returns></returns>
        string ReadLine();

        /// <summary>
        /// Sets the list of history entries to the given values. This method
        /// removes any previously stored entries from the history and overwrites
        /// then with whatever is given by the parameter value.
        /// </summary>
        /// <param name="history"></param>
        void SetHistory(IEnumerable<string> history);

    }
}
