namespace CommandLineEditors.Console
{
    internal interface IConsoleEditorLine
    {
        int CurrentCursorPosition { get; set; }
        bool InsertMode { get; set; }
        int Length { get; }
        string Text { get; set; }
        string PreviewString { get; set; }

        void Close();
        void Insert(char ch);
        void Insert(string str);
        void MoveCursorLeft();
        void MoveCursorRight();
        void MoveCursorToEndOfLine();
        void MoveCursorToStartOfLine();
        void Overwrite(char ch);
        void Overwrite(string str);
        void RefreshDisplay();
        string Remove(int startPos, int count);
        char RemoveAfterCursor();
        char RemoveBeforeCursor();
        void SetPosition(int x, int y);
    }
}