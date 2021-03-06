using System;
using System.Collections.Generic;

namespace CommandLineEditors.Console
{
    internal class UndoableConsoleEditorLine : IConsoleEditorLine
    {

        private readonly IConsoleEditorLine _consoleEditorLine;

        private readonly List<LineState> _lineStates = new List<LineState>();

        public int CurrentCursorPosition
        {
            get => _consoleEditorLine.CurrentCursorPosition;
            set => _consoleEditorLine.CurrentCursorPosition = value;
        }

        public bool InsertMode
        {
            get => _consoleEditorLine.InsertMode;
            set => _consoleEditorLine.InsertMode = value;
        }

        public int Length => _consoleEditorLine.Length;

        public string Text
        {
            get => _consoleEditorLine.Text;
            set => _consoleEditorLine.Text = value;
        }

        public string PreviewString
        {
            get => _consoleEditorLine.PreviewString;
            set
            {
                _consoleEditorLine.PreviewString = value;
                _consoleEditorLine.RefreshDisplay();
            }
        }

        public UndoableConsoleEditorLine(IConsoleEditorLine consoleEditorLine)
        {
            _consoleEditorLine = consoleEditorLine;
        }

        public void Undo()
        {
            int lastIndex = _lineStates.Count - 1;

            if (lastIndex < 0)
            {
                return;
            }

            LineState lineState = _lineStates[lastIndex];
            _lineStates.RemoveAt(lastIndex);

            _consoleEditorLine.Text = lineState.LineContents;
            _consoleEditorLine.CurrentCursorPosition = lineState.CursorPosition;
        }

        public void Redo()
        {
            throw new NotImplementedException("This operation needs to be implemented.");
        }

        public void Close()
        {
            RecordLineState();
            _consoleEditorLine.Close();
        }

        public void Insert(char ch)
        {
            RecordLineState();
            _consoleEditorLine.Insert(ch);
        }

        public void Insert(string str)
        {
            RecordLineState();
            _consoleEditorLine.Insert(str);
        }

        public void MoveCursorLeft()
        {
            RecordLineState();
            _consoleEditorLine.MoveCursorLeft();
        }

        public void MoveCursorRight()
        {
            RecordLineState();
            _consoleEditorLine.MoveCursorRight();
        }

        public void MoveCursorToEndOfLine()
        {
            RecordLineState();
            _consoleEditorLine.MoveCursorToEndOfLine();
        }

        public void MoveCursorToStartOfLine()
        {
            RecordLineState();
            _consoleEditorLine.MoveCursorToStartOfLine();
        }

        public void Overwrite(char ch)
        {
            RecordLineState();
            _consoleEditorLine.Overwrite(ch);
        }

        public void Overwrite(string str)
        {
            RecordLineState();
            _consoleEditorLine.Overwrite(str);
        }

        public void RefreshDisplay()
        {
            RecordLineState();
            _consoleEditorLine.RefreshDisplay();
        }

        public string Remove(int startPos, int count)
        {
            RecordLineState();
            return _consoleEditorLine.Remove(startPos, count);
        }

        public char RemoveAfterCursor()
        {
            RecordLineState();
            return _consoleEditorLine.RemoveAfterCursor();
        }

        public char RemoveBeforeCursor()
        {
            RecordLineState();
            return _consoleEditorLine.RemoveBeforeCursor();
        }

        private void RecordLineState()
        {
            LineState lineState = new LineState()
            {
                CursorPosition = _consoleEditorLine.CurrentCursorPosition,
                LineContents = _consoleEditorLine.Text
            };

            _lineStates.Add(lineState);
        }

        private class LineState
        {

            public int CursorPosition { get; set; }

            public string LineContents { get; set; }

        }

    }
}
