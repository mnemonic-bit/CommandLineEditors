using CommandLineEditors.Console;
using System;

namespace CommandLineEditors.Editor.Vi
{
    internal sealed class ViLineCommandEditor : IConsoleEditor
    {

        public ViLineCommandEditor(string prompt)
        {
            _prompt = prompt;
            _context = new ViLineCommandEditorContext();
            _lineEditor = new LineEditor<ViLineCommandEditorContext>(_context, keyHandlerMap: InitKeyHandlers(), prefix: ":");
        }

        public void Close()
        {
            _context.ConsoleEditorLine.Close();
        }

        public string ReadLine()
        {
            InitContext(_context, _prompt);
            return _lineEditor.ReadLine();
        }


        private readonly string _prompt;
        private readonly ViLineCommandEditorContext _context;
        private readonly LineEditor<ViLineCommandEditorContext> _lineEditor;


        private ConsoleKeyHandlerResult AbortCommand(ConsoleKeyInfo keyInfo, ViLineCommandEditorContext context)
        {
            context.Result = string.Empty;
            context.Aborted = true;

            return ConsoleKeyHandlerResult.Aborted;
        }

        private ConsoleKeyHandlerResult HandleSearchInput(ConsoleKeyInfo keyInfo, ViLineCommandEditorContext context)
        {
            if ((keyInfo.Modifiers & (ConsoleModifiers.Alt | ConsoleModifiers.Control)) > 0)
            {
                return ConsoleKeyHandlerResult.NotConsumed;
            }

            context.ConsoleEditorLine.Insert(keyInfo.KeyChar);

            return ConsoleKeyHandlerResult.Consumed;
        }

        private ConsoleKeyHandlerResult HandleBackspace(ConsoleKeyInfo keyInfo, ViLineCommandEditorContext context)
        {
            if (context.ConsoleEditorLine.Length == 0)
            {
                return AbortCommand(keyInfo, context);
            }
            else
            {
                return RemoveBeforeCursor(keyInfo, context);
            }
        }

        private void InitContext(ViLineCommandEditorContext context, string prompt)
        {
            context.ConsoleEditorLine = new ConsoleEditorLine("", prompt);
            context.Result = string.Empty;
        }

        private ConsoleKeyHandlerMap<ViLineCommandEditorContext> InitKeyHandlers()
        {
            ConsoleKeyHandlerMap<ViLineCommandEditorContext> keyHandlerMap = new ConsoleKeyHandlerMap<ViLineCommandEditorContext>(HandleSearchInput);

            keyHandlerMap.AddKeyHandler(new ConsoleKeyInfo('\r', ConsoleKey.Enter, false, false, false), ReturnFromEnteringCommand);//
            keyHandlerMap.AddKeyHandler(ConsoleKey.LeftArrow, MoveCursorLeft);
            keyHandlerMap.AddKeyHandler(ConsoleKey.RightArrow, MoveCursorRight);
            keyHandlerMap.AddKeyHandler(ConsoleKey.Delete, RemoveAfterCursor);
            keyHandlerMap.AddKeyHandler(new ConsoleKeyInfo('\u0008', ConsoleKey.Backspace, false, false, false), HandleBackspace);//
            keyHandlerMap.AddKeyHandler(ConsoleKey.Home, MoveCursorToStartOfLine);
            keyHandlerMap.AddKeyHandler(ConsoleKey.End, MoveCursorToEndOfLine);

            return keyHandlerMap;
        }

        private ConsoleKeyHandlerResult ReturnFromEnteringCommand(ConsoleKeyInfo keyInfo, ViLineCommandEditorContext context)
        {
            context.Result = context.ConsoleEditorLine.Text;
            context.Aborted = false;

            return ConsoleKeyHandlerResult.Finished;
        }

        private ConsoleKeyHandlerResult MoveCursorLeft(ConsoleKeyInfo keyInfo, ViLineCommandEditorContext context)
        {
            context.ConsoleEditorLine.MoveCursorLeft();
            return ConsoleKeyHandlerResult.Consumed;
        }

        private ConsoleKeyHandlerResult MoveCursorRight(ConsoleKeyInfo keyInfo, ViLineCommandEditorContext context)
        {
            context.ConsoleEditorLine.MoveCursorRight();
            return ConsoleKeyHandlerResult.Consumed;
        }

        private ConsoleKeyHandlerResult MoveCursorToEndOfLine(ConsoleKeyInfo keyInfo, ViLineCommandEditorContext context)
        {
            context.ConsoleEditorLine.MoveCursorToEndOfLine();
            return ConsoleKeyHandlerResult.Consumed;
        }

        private ConsoleKeyHandlerResult MoveCursorToStartOfLine(ConsoleKeyInfo keyInfo, ViLineCommandEditorContext context)
        {
            context.ConsoleEditorLine.MoveCursorToStartOfLine();
            return ConsoleKeyHandlerResult.Consumed;
        }

        private ConsoleKeyHandlerResult RemoveAfterCursor(ConsoleKeyInfo keyInfo, ViLineCommandEditorContext context)
        {
            context.ConsoleEditorLine.RemoveAfterCursor();
            context.ConsoleEditorLine.RefreshDisplay();
            return ConsoleKeyHandlerResult.Consumed;
        }

        private ConsoleKeyHandlerResult RemoveBeforeCursor(ConsoleKeyInfo keyInfo, ViLineCommandEditorContext context)
        {
            context.ConsoleEditorLine.RemoveBeforeCursor();
            context.ConsoleEditorLine.RefreshDisplay();
            return ConsoleKeyHandlerResult.Consumed;
        }

    }
}
