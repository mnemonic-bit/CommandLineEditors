using CommandLineEditors.Console;
using System;

namespace CommandLineEditors.Editor.Vi
{
    public sealed class ViLineEditor : IConsoleEditor
    {

        /// <summary>
        /// Gets or sets the current test this editor-line has to edit.
        /// </summary>
        public string Text
        {
            get => _context.ConsoleEditorLine.Text;
            set
            {
                _context.ConsoleEditorLine.Text = value;
                _context.ConsoleEditorLine.MoveCursorToEndOfLine();
            }
        }

        /// <summary>
        /// Gets or sets the preview-string for this editor line.
        /// A preview is a text that is appended to the end of the
        /// current input in a lightly darker color.
        /// </summary>
        public string Preview
        {
            get => _context.ConsoleEditorLine.PreviewString;
            set => _context.ConsoleEditorLine.PreviewString = value;
        }

        /// <summary>
        /// Inits the console editor instance.
        /// </summary>
        public ViLineEditor()
            : this("")
        {
        }

        /// <summary>
        /// Inits the console editor instance.
        /// </summary>
        public ViLineEditor(string text)
        {
            _context = new ViLineEditorContext();

            _commandModeKeyHandlers = InitCommandModeKeyHandlers();
            _editModeKeyHandlers = InitEditModeKeyHandlers();

            _lineEditor = new LineEditor<ViLineEditorContext>(_context, keyHandlerMap: _commandModeKeyHandlers);
        }

        public void Close()
        {
            _lineEditor.Close();
        }

        public string ReadLine()
        {
            InitContext(_context, "");
            return _lineEditor.ReadLine();
        }


        /// <summary>
        /// The context stores all information needed to process all
        /// key-input made by the user.
        /// </summary>
        private readonly ViLineEditorContext _context;
        private readonly CommonCommandKeyHandler<ViLineEditorContext> _commonHandlers = new CommonCommandKeyHandler<ViLineEditorContext>();
        private readonly ConsoleKeyHandlerMap<ViLineEditorContext> _commandModeKeyHandlers;
        private readonly ConsoleKeyHandlerMap<ViLineEditorContext> _editModeKeyHandlers;

        /// <summary>
        /// The line-editor is the cosole editor instance used to make
        /// our edits visible to the user.
        /// </summary>
        private readonly LineEditor<ViLineEditorContext> _lineEditor;


        private ConsoleKeyHandlerResult DefaultCommandModeKeyHandler(ConsoleKeyInfo keyInfo, ViLineEditorContext context)
        {
            //TODO: implement this!!
            if ((keyInfo.Modifiers & (ConsoleModifiers.Alt | ConsoleModifiers.Control)) > 0)
            {
                return ConsoleKeyHandlerResult.NotConsumed;
            }

            context.ConsoleEditorLine.Insert(keyInfo.KeyChar);
            return ConsoleKeyHandlerResult.Consumed;
        }

        private ConsoleKeyHandlerResult DefaultEditModeKeyHandler(ConsoleKeyInfo keyInfo, ViLineEditorContext context)
        {
            if ((keyInfo.Modifiers & (ConsoleModifiers.Alt | ConsoleModifiers.Control)) > 0)
            {
                return ConsoleKeyHandlerResult.NotConsumed;
            }

            context.ConsoleEditorLine.Insert(keyInfo.KeyChar);
            return ConsoleKeyHandlerResult.Consumed;
        }

        private ConsoleKeyHandlerResult ExitEditMode(ConsoleKeyInfo keyInfo, ViLineEditorContext context)
        {
            _lineEditor.KeyHandlerMap = _commandModeKeyHandlers;
            return ConsoleKeyHandlerResult.Consumed;
        }

        private ConsoleKeyHandlerResult FinishInput(ConsoleKeyInfo keyInfo, ViLineEditorContext context)
        {
            return ConsoleKeyHandlerResult.Finished;
        }

        private void InitContext(ViLineEditorContext context, string text)
        {
            context.ConsoleEditorLine = new ConsoleEditorLine(text);
        }

        private ConsoleKeyHandlerMap<ViLineEditorContext> InitCommandModeKeyHandlers()
        {
            ConsoleKeyHandlerMap<ViLineEditorContext> keyHandlerMap = new ConsoleKeyHandlerMap<ViLineEditorContext>(DefaultCommandModeKeyHandler);

            keyHandlerMap.AddKeyHandler(new ConsoleKeyInfo('\r', ConsoleKey.Enter, false, false, false), FinishInput);
            keyHandlerMap.AddKeyHandler(ConsoleKey.LeftArrow, _commonHandlers.MoveCursorLeft);
            keyHandlerMap.AddKeyHandler(ConsoleKey.RightArrow, _commonHandlers.MoveCursorRight);
            keyHandlerMap.AddKeyHandler(ConsoleKey.Home, _commonHandlers.MoveCursorToStartOfLine);
            keyHandlerMap.AddKeyHandler(new ConsoleKeyInfo('^', ConsoleKey.D6, true, false, false), _commonHandlers.MoveCursorToStartOfLine);
            keyHandlerMap.AddKeyHandler(ConsoleKey.End, _commonHandlers.MoveCursorToEndOfLine);
            keyHandlerMap.AddKeyHandler(new ConsoleKeyInfo('\u0024', ConsoleKey.D4, true, false, false), _commonHandlers.MoveCursorToEndOfLine);
            keyHandlerMap.AddKeyHandler(new ConsoleKeyInfo(':', ConsoleKey.Oem1, true, false, false), StartCommandMode);

            // Control key bindings
            keyHandlerMap.AddKeyHandler(new ConsoleKeyInfo('\u000e', ConsoleKey.N, false, false, true), _commonHandlers.MoveCursorLeft);
            keyHandlerMap.AddKeyHandler(new ConsoleKeyInfo('\u000c', ConsoleKey.L, false, false, true), _commonHandlers.RefreshDisplay);

            keyHandlerMap.AddKeyHandler(new ConsoleKeyInfo('a', ConsoleKey.A, false, false, false), StartAppendMode);
            keyHandlerMap.AddKeyHandler(new ConsoleKeyInfo('\u0044', ConsoleKey.D, true, false, false), RemoveLineAfterCursor);
            keyHandlerMap.AddKeyHandler(new ConsoleKeyInfo('i', ConsoleKey.I, false, false, false), StartEditMode);

            keyHandlerMap.AddKeyHandler(new ConsoleKeyInfo('h', ConsoleKey.H, false, false, false), _commonHandlers.MoveCursorLeft);
            keyHandlerMap.AddKeyHandler(new ConsoleKeyInfo('l', ConsoleKey.L, false, false, false), _commonHandlers.MoveCursorRight);

            keyHandlerMap.AddKeyHandler(new ConsoleKeyInfo('\u0078', ConsoleKey.X, false, false, false), _commonHandlers.RemoveAfterCursor);
            keyHandlerMap.AddKeyHandler(new ConsoleKeyInfo('\u0058', ConsoleKey.X, true, false, false), _commonHandlers.RemoveBeforeCursor);

            //keyHandlerMap.AddKeyHandler(new ConsoleKeyInfo('l', ConsoleKey.L, false, false, false), _commonHandlers.MoveCursorRight);

            return keyHandlerMap;
        }

        private ConsoleKeyHandlerMap<ViLineEditorContext> InitEditModeKeyHandlers()
        {
            ConsoleKeyHandlerMap<ViLineEditorContext> keyHandlerMap = new ConsoleKeyHandlerMap<ViLineEditorContext>(DefaultEditModeKeyHandler);

            keyHandlerMap.AddKeyHandler(new ConsoleKeyInfo('\r', ConsoleKey.Enter, false, false, false), FinishInput);
            keyHandlerMap.AddKeyHandler(ConsoleKey.Delete, _commonHandlers.RemoveAfterCursor);
            keyHandlerMap.AddKeyHandler(new ConsoleKeyInfo('\u0008', ConsoleKey.Backspace, false, false, false), _commonHandlers.RemoveBeforeCursor);
            keyHandlerMap.AddKeyHandler(ConsoleKey.Home, _commonHandlers.MoveCursorToStartOfLine);
            keyHandlerMap.AddKeyHandler(ConsoleKey.End, _commonHandlers.MoveCursorToEndOfLine);

            // Control key bindings
            keyHandlerMap.AddKeyHandler(new ConsoleKeyInfo('\u0008', ConsoleKey.H, false, false, true), _commonHandlers.MoveCursorLeft);
            //keyHandlerMap.AddKeyHandler(new ConsoleKeyInfo('\u0015', ConsoleKey.U, false, false, true), ProcessCtrlU); // move to beginning of "insert"
            keyHandlerMap.AddKeyHandler(new ConsoleKeyInfo('\u0017', ConsoleKey.W, false, false, true), _commonHandlers.MoveCursorLeftToStartOfWord);

            keyHandlerMap.AddKeyHandler(ConsoleKey.LeftArrow, _commonHandlers.MoveCursorLeft);
            keyHandlerMap.AddKeyHandler(ConsoleKey.RightArrow, _commonHandlers.MoveCursorRight);

            keyHandlerMap.AddKeyHandler(new ConsoleKeyInfo('\u001b', ConsoleKey.Escape, false, false, false), ExitEditMode);

            return keyHandlerMap;
        }

        private ConsoleKeyHandlerResult StartAppendMode(ConsoleKeyInfo keyInfo, ViLineEditorContext context)
        {
            _commonHandlers.MoveCursorRight(keyInfo, context);
            return StartEditMode(keyInfo, context);
        }

        private ConsoleKeyHandlerResult StartCommandMode(ConsoleKeyInfo keyInfo, ViLineEditorContext context)
        {
            context.ConsoleEditorLine.Close();

            ViLineCommandEditor commandEditor = new ViLineCommandEditor(":");
            string command = commandEditor.ReadLine();
            commandEditor.Close();

            //act upon the command
            //TODO...

            context.ConsoleEditorLine.RefreshDisplay();

            return ConsoleKeyHandlerResult.Consumed;
        }

        private ConsoleKeyHandlerResult StartEditMode(ConsoleKeyInfo keyInfo, ViLineEditorContext context)
        {
            _lineEditor.KeyHandlerMap = _editModeKeyHandlers;
            return ConsoleKeyHandlerResult.Consumed;
        }

        private ConsoleKeyHandlerResult RemoveLineAfterCursor(ConsoleKeyInfo keyInfo, ViLineEditorContext context)
        {
            string removedText = _commonHandlers.RemoveTextAfterCursor(context);
            if (!string.IsNullOrEmpty(removedText))
            {
                Clipboard.SetText(removedText);
            }
            return ConsoleKeyHandlerResult.Consumed;
        }

    }
}
