using CommandLineEditors.Console;
using CommandLineEditors.Data;
using System;

namespace CommandLineEditors.Editor.ReadLine
{
    /// <summary>
    /// Manages the history of console-editor lines of the ReadLineEditor.
    /// </summary>
    /// <typeparam name="TEditorConsoleLine"></typeparam>
    internal class ReadLineHistory<TEditorConsoleLine>
        where TEditorConsoleLine : IConsoleEditorLine
    {

        private readonly History<string> _history;

        private readonly TEditorConsoleLine[] _editorHistory;

        private int _positionInHistory;

        private readonly Func<string, TEditorConsoleLine> _createEditorFn;

        public ReadLineHistory(History<string> history, Func<string, TEditorConsoleLine> createEditorFn)
        {
            _history = history;
            _editorHistory = new TEditorConsoleLine[history.Count + 1];
            _positionInHistory = history.Count;
            _createEditorFn = createEditorFn;
        }

        public bool TryMoveUp(out TEditorConsoleLine historyEntry)
        {
            historyEntry = default(TEditorConsoleLine);

            if (_positionInHistory > 0)
            {
                _positionInHistory--;
                historyEntry = CurrentEntry;
                return true;
            }

            return false;
        }

        public bool TryMoveDown(out TEditorConsoleLine historyEntry)
        {
            historyEntry = default(TEditorConsoleLine);

            if (_positionInHistory >= 0 && _positionInHistory < _editorHistory.Length - 1)
            {
                _positionInHistory++;
                historyEntry = CurrentEntry;
                return true;
            }

            return false;
        }

        public TEditorConsoleLine CurrentEntry
        {
            get
            {
                if (_editorHistory[_positionInHistory] == null)
                {
                    _editorHistory[_positionInHistory] = _createEditorFn(_history[_positionInHistory] ?? "");
                }

                return _editorHistory[_positionInHistory];
            }
        }

    }
}
