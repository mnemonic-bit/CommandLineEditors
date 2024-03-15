namespace CommandLineEditors.Console
{
    /// <summary>
    /// The <code>Clipboard</code> gives access to the clipboard
    /// of this command line editors and allows the user of this
    /// editors to read and modify the clipboard contents.
    /// </summary>
    public static class Clipboard
    {

        private static string _text = null;

        public static string GetText()
        {
            return _text;
        }

        public static void SetText(string text)
        {
            _text = text;
        }

    }
}
