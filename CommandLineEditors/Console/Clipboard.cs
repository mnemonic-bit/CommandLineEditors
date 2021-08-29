namespace CommandLineEditors.Console
{
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
