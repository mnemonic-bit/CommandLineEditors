using CommandLineEditors.Console;
using CommandLineEditors.Data;
using System;

namespace CommandLineEditors.Editor.ReadLine
{
    internal sealed class ReverseSearchEditorLine : IConsoleEditor
    {

        public ReverseSearchEditorLine(string prompt, History<string> history)
        {
            _prompt = prompt;
            _history = history;
            _context = new ReverseSearchEditorContext();
            InitContext(_context, _prompt, _history);
            _lineEditor = new LineEditor<ReverseSearchEditorContext>(_context, keyHandlerMap: InitKeyHandlers());
        }

        public void Close()
        {
            _context.ConsoleEditorLine.Close();
        }

        public string ReadLine()
        {
            return _lineEditor.ReadLine();
        }


        private readonly History<string> _history;
        private readonly string _prompt;
        private readonly ReverseSearchEditorContext _context;
        private readonly LineEditor<ReverseSearchEditorContext> _lineEditor;


        private ConsoleKeyHandlerResult AbortSearch(ConsoleKeyInfo keyInfo, ReverseSearchEditorContext context)
        {
            // abort reverse-search, and restore the original line
            context.Hit = null;
            context.Aborted = true;

            return ConsoleKeyHandlerResult.Aborted;
        }

        private ConsoleKeyHandlerResult HandleSearchInput(ConsoleKeyInfo keyInfo, ReverseSearchEditorContext context)
        {
            if ((keyInfo.Modifiers & (ConsoleModifiers.Alt | ConsoleModifiers.Control)) > 0)
            {
                return ConsoleKeyHandlerResult.NotConsumed;
            }

            context.ConsoleEditorLine.Insert(keyInfo.KeyChar);

            string text = context.ConsoleEditorLine.Text;
            int lastIndex = context.History.FindLastIndex(str => str.Contains(text));

            if (lastIndex < 0)
            {
                return ConsoleKeyHandlerResult.Consumed;
            }

            string? searchResult = context.History[lastIndex];
            if (!string.IsNullOrEmpty(searchResult))
            {
                context.Hit = searchResult;
                context.ConsoleEditorLine.PreviewString = $": {searchResult}";
                context.ConsoleEditorLine.RefreshDisplay();
            }

            return ConsoleKeyHandlerResult.Consumed;
        }

        private void InitContext(ReverseSearchEditorContext context, string prompt, History<string> history)
        {
            context.ConsoleEditorLine = new ConsoleEditorLine("", prompt);
            context.Aborted = false;
            context.History = history;
            context.Hit = null;
        }

        private ConsoleKeyHandlerMap<ReverseSearchEditorContext> InitKeyHandlers()
        {
            ConsoleKeyHandlerMap<ReverseSearchEditorContext> keyHandlerMap = new ConsoleKeyHandlerMap<ReverseSearchEditorContext>(HandleSearchInput);

            keyHandlerMap.AddKeyHandler(new ConsoleKeyInfo('\r', ConsoleKey.Enter, false, false, false), ReturnFromSearch);
            keyHandlerMap.AddKeyHandler(new ConsoleKeyInfo('\a', ConsoleKey.G, false, false, true), AbortSearch);
            keyHandlerMap.AddKeyHandler(ConsoleKey.LeftArrow, MoveCursorLeft);
            keyHandlerMap.AddKeyHandler(ConsoleKey.RightArrow, MoveCursorRight);
            keyHandlerMap.AddKeyHandler(ConsoleKey.Delete, RemoveAfterCursor);
            keyHandlerMap.AddKeyHandler(new ConsoleKeyInfo('\u0008', ConsoleKey.Backspace, false, false, false), RemoveBeforeCursor);
            keyHandlerMap.AddKeyHandler(ConsoleKey.Home, MoveCursorToStartOfLine);
            keyHandlerMap.AddKeyHandler(ConsoleKey.End, MoveCursorToEndOfLine);

            return keyHandlerMap;
        }

        private ConsoleKeyHandlerResult ReturnFromSearch(ConsoleKeyInfo keyInfo, ReverseSearchEditorContext context)
        {
            // Set the found search result, and end this editor.
            context.Aborted = true;

            return ConsoleKeyHandlerResult.Aborted;
        }

        private ConsoleKeyHandlerResult MoveCursorLeft(ConsoleKeyInfo keyInfo, ReverseSearchEditorContext context)
        {
            context.ConsoleEditorLine.MoveCursorLeft();
            return ConsoleKeyHandlerResult.Consumed;
        }

        private ConsoleKeyHandlerResult MoveCursorRight(ConsoleKeyInfo keyInfo, ReverseSearchEditorContext context)
        {
            context.ConsoleEditorLine.MoveCursorRight();
            return ConsoleKeyHandlerResult.Consumed;
        }

        private ConsoleKeyHandlerResult MoveCursorToEndOfLine(ConsoleKeyInfo keyInfo, ReverseSearchEditorContext context)
        {
            context.ConsoleEditorLine.MoveCursorToEndOfLine();
            return ConsoleKeyHandlerResult.Consumed;
        }

        private ConsoleKeyHandlerResult MoveCursorToStartOfLine(ConsoleKeyInfo keyInfo, ReverseSearchEditorContext context)
        {
            context.ConsoleEditorLine.MoveCursorToStartOfLine();
            return ConsoleKeyHandlerResult.Consumed;
        }

        private ConsoleKeyHandlerResult RemoveAfterCursor(ConsoleKeyInfo keyInfo, ReverseSearchEditorContext context)
        {
            context.ConsoleEditorLine.RemoveAfterCursor();
            context.ConsoleEditorLine.RefreshDisplay();
            return ConsoleKeyHandlerResult.Consumed;
        }

        private ConsoleKeyHandlerResult RemoveBeforeCursor(ConsoleKeyInfo keyInfo, ReverseSearchEditorContext context)
        {
            context.ConsoleEditorLine.RemoveBeforeCursor();
            context.ConsoleEditorLine.RefreshDisplay();
            return ConsoleKeyHandlerResult.Consumed;
        }

    }
}
