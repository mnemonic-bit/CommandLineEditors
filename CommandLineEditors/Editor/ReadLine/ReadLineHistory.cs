using CommandLineEditors.Console;
using CommandLineEditors.Data;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace CommandLineEditors.Editor.ReadLine
{
    /// <summary>
    /// Manages the history of console-editor lines of the ReadLineEditor.
    /// </summary>
    /// <typeparam name="TEditorConsoleLine"></typeparam>
    internal sealed class ReadLineHistory<TEditorConsoleLine>
        where TEditorConsoleLine : IConsoleEditorLine
    {

        public TEditorConsoleLine CurrentEntry
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (_editorHistory[_positionInHistory] == null)
                {
                    TEditorConsoleLine editor = _createEditorFn(_history[_positionInHistory] ?? "");
                    editor.MoveCursorToEndOfLine();
                    _editorHistory[_positionInHistory] = editor;
                }

                return _editorHistory[_positionInHistory];
            }
        }

        public ReadLineHistory(History<string> history, Func<string, TEditorConsoleLine> createEditorFn)
        {
            _history = history;
            _editorHistory = new TEditorConsoleLine[history.Count + 1];
            _positionInHistory = history.Count;
            _createEditorFn = createEditorFn;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryMoveDown([NotNullWhen(true)] out TEditorConsoleLine? historyEntry)
        {
            historyEntry = default;

            if (_positionInHistory >= 0 && _positionInHistory < _editorHistory.Length - 1)
            {
                _positionInHistory++;
                historyEntry = CurrentEntry;
                return true;
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryMoveFirst([NotNullWhen(true)] out TEditorConsoleLine? historyEntry)
        {
            historyEntry = default;

            if (_positionInHistory != 0)
            {
                _positionInHistory = 0;
                historyEntry = CurrentEntry;
                return true;
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryMoveLast([NotNullWhen(true)] out TEditorConsoleLine? historyEntry)
        {
            historyEntry = default;

            if (_editorHistory.Length > 0)
            {
                _positionInHistory = _editorHistory.Length - 1;
                historyEntry = CurrentEntry;
                return true;
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryMoveUp([NotNullWhen(true)] out TEditorConsoleLine? historyEntry)
        {
            historyEntry = default;

            if (_positionInHistory > 0)
            {
                _positionInHistory--;
                historyEntry = CurrentEntry;
                return true;
            }

            return false;
        }


        private readonly History<string> _history;
        private readonly TEditorConsoleLine[] _editorHistory;
        private int _positionInHistory;
        private readonly Func<string, TEditorConsoleLine> _createEditorFn;


    }
}
