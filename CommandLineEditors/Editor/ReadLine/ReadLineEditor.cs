﻿using CommandLineEditors.Console;
using CommandLineEditors.Data;
using CommandLineEditors.Extensions;
using System;
using System.Collections.Generic;

namespace CommandLineEditors.Editor.ReadLine
{
    /// <summary>
    /// A ConsoleEditor maintains a line of editing on a console screen.
    /// It accepts a series of editing commands and is repsonsible for
    /// the accuracy of the display ofter each command is executed.
    /// </summary>
    public sealed class ReadLineEditor : IConsoleEditor
    {

        /// <summary>
        /// Gets or sets the flag that enables or blocks this class
        /// from showing all alternatives when in auto completion mode
        /// more than one alternative is available.
        /// </summary>
        public bool ShowAutoCompletionAlternatives { get; set; }

        /// <summary>
        /// Gets or sets the text of the editor.
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
        public ReadLineEditor(string text = "", string prompt = "")
        {
            _prompt = prompt;
            _context = new ReadLineEditorContext();
            _lineEditor = new LineEditor<ReadLineEditorContext>(_context, keyHandlerMap: InitKeyHandlers(), text: text, prefix: prompt);
        }

        public void Close()
        {
            _context.ConsoleEditorLine.Close();
        }

        public IEnumerable<string> GetHistory()
        {
            return _history.GetEntries();
        }

        public void RefreshDisplay()
        {
            _context.ConsoleEditorLine.RefreshDisplay();
        }

        public string ReadLine()
        {
            InitContext(_context, "");
            return _lineEditor.ReadLine();
        }

        public void SetHistory(IEnumerable<string> history)
        {
            _history.Clear();
            _history.AppendEntries(history);
        }


        private readonly string _prompt;
        private readonly History<string> _history = new History<string>();
        private readonly CommonCommandKeyHandler<ReadLineEditorContext> _commonHandlers = new CommonCommandKeyHandler<ReadLineEditorContext>();

        /// <summary>
        /// The context stores all information needed to process all
        /// key-input made by the user.
        /// </summary>
        private readonly ReadLineEditorContext _context;

        /// <summary>
        /// The line-editor is the cosole editor instance used to make
        /// our edits visible to the user.
        /// </summary>
        private readonly LineEditor<ReadLineEditorContext> _lineEditor;


        private UndoableConsoleEditorLine CreateEditorLine(string text)
        {
            return new UndoableConsoleEditorLine(new ConsoleEditorLine(text, _prompt));
        }

        private void InitContext(ReadLineEditorContext context, string text)
        {
            ReadLineHistory<UndoableConsoleEditorLine> readLineHistory = new ReadLineHistory<UndoableConsoleEditorLine>(_history, CreateEditorLine);

            context.History = readLineHistory;
            context.ConsoleEditorLine = readLineHistory.CurrentEntry;

            context.AlternateCursorPosition = 0;
            context.CtrlVPressed = false;
            context.CtrlXPressed = false;
            context.InsertMode = true;
        }

        private ConsoleKeyHandlerMap<ReadLineEditorContext> InitKeyHandlers()
        {
            ConsoleKeyHandlerMap<ReadLineEditorContext> keyHandlerMap = new ConsoleKeyHandlerMap<ReadLineEditorContext>(DefaultConsumeKeyInfo);

            keyHandlerMap.AddKeyHandler(new ConsoleKeyInfo('\r', ConsoleKey.Enter, false, false, false), EndLineEntryAndInsertHistory);
            keyHandlerMap.AddKeyHandler(ConsoleKey.LeftArrow, _commonHandlers.MoveCursorLeft); // 
            keyHandlerMap.AddKeyHandler(ConsoleKey.RightArrow, _commonHandlers.MoveCursorRight); // 
            keyHandlerMap.AddKeyHandler(ConsoleKey.UpArrow, MoveOneUpInHistory); // 
            keyHandlerMap.AddKeyHandler(ConsoleKey.DownArrow, MoveOneDownInHistory); // 
            keyHandlerMap.AddKeyHandler(ConsoleKey.Insert, (keyInfo, context) => { context.InsertMode = !context.InsertMode; return ConsoleKeyHandlerResult.Consumed; });
            keyHandlerMap.AddKeyHandler(ConsoleKey.Delete, RemoveAfterCursor); // DELETE
            keyHandlerMap.AddKeyHandler(new ConsoleKeyInfo('\u0008', ConsoleKey.Backspace, false, false, false), _commonHandlers.RemoveBeforeCursor); // BACKSPACE
            keyHandlerMap.AddKeyHandler(ConsoleKey.Home, _commonHandlers.MoveCursorToStartOfLine); // HOME
            keyHandlerMap.AddKeyHandler(ConsoleKey.End, _commonHandlers.MoveCursorToEndOfLine); // END

            // Ctrl-key bindings
            keyHandlerMap.AddKeyHandler(new ConsoleKeyInfo('\u0001', ConsoleKey.A, false, false, true), ProcessCtrlA); // 
            keyHandlerMap.AddKeyHandler(new ConsoleKeyInfo('\u0002', ConsoleKey.B, false, false, true), ProcessCtrlB); // 
            keyHandlerMap.AddKeyHandler(new ConsoleKeyInfo('\u0004', ConsoleKey.D, false, false, true), ProcessCtrlD); // 
            keyHandlerMap.AddKeyHandler(new ConsoleKeyInfo('\u0005', ConsoleKey.E, false, false, true), ProcessCtrlE); // 
            keyHandlerMap.AddKeyHandler(new ConsoleKeyInfo('\u0006', ConsoleKey.F, false, false, true), ProcessCtrlF); // 
            keyHandlerMap.AddKeyHandler(new ConsoleKeyInfo('\u0008', ConsoleKey.H, false, false, true), _commonHandlers.RemoveBeforeCursor); // 
            keyHandlerMap.AddKeyHandler(new ConsoleKeyInfo('\u0009', ConsoleKey.I, false, false, true), ProcessTabExtension);
            keyHandlerMap.AddKeyHandler(new ConsoleKeyInfo('\u000b', ConsoleKey.K, false, false, true), RemoveLineAfterCursor); // 
            keyHandlerMap.AddKeyHandler(new ConsoleKeyInfo('\u000c', ConsoleKey.L, false, false, true), ClearScreen);
            keyHandlerMap.AddKeyHandler(new ConsoleKeyInfo('\u000e', ConsoleKey.N, false, false, true), ProcessCtrlN); // 
            keyHandlerMap.AddKeyHandler(new ConsoleKeyInfo('\u0010', ConsoleKey.P, false, false, true), MoveOneUpInHistory); // 
            keyHandlerMap.AddKeyHandler(new ConsoleKeyInfo('\u0012', ConsoleKey.R, false, false, true), ProcessCtrlR); // 
            keyHandlerMap.AddKeyHandler(new ConsoleKeyInfo('\u0014', ConsoleKey.T, false, false, true), ProcessCtrlT); // 
            keyHandlerMap.AddKeyHandler(new ConsoleKeyInfo('\u0015', ConsoleKey.U, false, false, true), ProcessCtrlU); // 
            keyHandlerMap.AddKeyHandler(new ConsoleKeyInfo('\u0016', ConsoleKey.V, false, false, true), ProcessCtrlV);
            keyHandlerMap.AddKeyHandler(new ConsoleKeyInfo('\u0017', ConsoleKey.W, false, false, true), RemoveWordBeforeCursor); // 
            keyHandlerMap.AddKeyHandler(new ConsoleKeyInfo('\u0018', ConsoleKey.X, false, false, true), ProcessCtrlX);
            keyHandlerMap.AddKeyHandler(new ConsoleKeyInfo('\u0019', ConsoleKey.Y, false, false, true), PasteClipboard); // 
            keyHandlerMap.AddKeyHandler(new ConsoleKeyInfo('\u001d', ConsoleKey.Oem6, false, false, true), MoveForwardToSingleCharacterWord); // Ctrl-]

            // Alt-key bindings
            keyHandlerMap.AddKeyHandler(new ConsoleKeyInfo('b', ConsoleKey.B, false, true, false), _commonHandlers.MoveCursorLeftToStartOfWord); // Alt-b
            keyHandlerMap.AddKeyHandler(new ConsoleKeyInfo('c', ConsoleKey.C, false, true, false), _commonHandlers.CapitalizeCharacterUnderCursor); // Alt-c
            keyHandlerMap.AddKeyHandler(new ConsoleKeyInfo('d', ConsoleKey.D, false, true, false), RemoveWordAfterCursor); // Alt-d
            keyHandlerMap.AddKeyHandler(new ConsoleKeyInfo('f', ConsoleKey.F, false, true, false), _commonHandlers.MoveCursorRightToEndOfWord);//
            keyHandlerMap.AddKeyHandler(new ConsoleKeyInfo('l', ConsoleKey.L, false, true, false), _commonHandlers.ChangeCharactersToLowerCaseUpToEndOfWord);//
            keyHandlerMap.AddKeyHandler(new ConsoleKeyInfo('\u0072', ConsoleKey.R, false, true, false), ProcessAltR); // Alt-r
            keyHandlerMap.AddKeyHandler(new ConsoleKeyInfo('u', ConsoleKey.U, false, true, false), _commonHandlers.ChangeCharactersToUpperCaseUpToEndOfWord);//
            keyHandlerMap.AddKeyHandler(new ConsoleKeyInfo('.', ConsoleKey.OemPeriod, false, true, false), InsertLastWordOfPreviousHistoryEntry);
            keyHandlerMap.AddKeyHandler(new ConsoleKeyInfo('\u003c', ConsoleKey.OemComma, true, true, false), MoveToFirstHistoryEntry); // Alt-<
            keyHandlerMap.AddKeyHandler(new ConsoleKeyInfo('\u003e', ConsoleKey.OemPeriod, true, true, false), MoveToLastHistoryEntry); // Alt->
            keyHandlerMap.AddKeyHandler(new ConsoleKeyInfo('\u005c', ConsoleKey.Oem5, false, true, false), RemoveWhitespaceAroundCursor); // Alt-\
            keyHandlerMap.AddKeyHandler(new ConsoleKeyInfo('\u0000', ConsoleKey.Delete, false, true, false), RemoveWordBeforeCursor); // Alt-Delete
            keyHandlerMap.AddKeyHandler(new ConsoleKeyInfo('\u0023', ConsoleKey.D3, true, true, false), CommentLineAndStartNewOne); // Alt-#

            // Ctrl-Alt-key bindings
            keyHandlerMap.AddKeyHandler(new ConsoleKeyInfo('\u0000', ConsoleKey.Oem6, false, true, true), MoveBackwardToSingleCharacterWord); // Ctrl-Alt-]

            return keyHandlerMap;
        }

        private ConsoleKeyHandlerResult DefaultConsumeKeyInfo(ConsoleKeyInfo keyInfo, ReadLineEditorContext context)
        {
            if ((keyInfo.Modifiers & (ConsoleModifiers.Alt | ConsoleModifiers.Control)) > 0)
            {
                return ConsoleKeyHandlerResult.NotConsumed;
            }

            context.ConsoleEditorLine.Insert(keyInfo.KeyChar);
            return ConsoleKeyHandlerResult.Consumed;
        }

        private ConsoleKeyHandlerResult Abort(ConsoleKeyInfo keyInfo, ReadLineEditorContext context)
        {
            return ConsoleKeyHandlerResult.Aborted;
        }

        private ConsoleKeyHandlerResult EndLineEntryAndInsertHistory(ConsoleKeyInfo keyInfo, ReadLineEditorContext context)
        {
            _history.AppendEntry(context.Result);
            return ConsoleKeyHandlerResult.Finished;
        }

        private ConsoleKeyHandlerResult ClearScreen(ConsoleKeyInfo keyInfo, ReadLineEditorContext context)
        {
            ConsoleLayer.Clear();
            context.ConsoleEditorLine.SetPosition(0, 0);
            return ConsoleKeyHandlerResult.Consumed;
        }

        private ConsoleKeyHandlerResult CommentLineAndStartNewOne(ConsoleKeyInfo keyInfo, ReadLineEditorContext context)
        {
            context.ConsoleEditorLine.MoveCursorToStartOfLine();
            context.ConsoleEditorLine.Insert('#');
            _history.AppendEntry(context.Result);
            return ConsoleKeyHandlerResult.Finished;
        }

        private ConsoleKeyHandlerResult InsertLastWordOfPreviousHistoryEntry(ConsoleKeyInfo keyInfo, ReadLineEditorContext context)
        {
            // inserts the last word of the previous history entry
            // to the current position of the cursor.
            // TODO: implement this

            return ConsoleKeyHandlerResult.Consumed;
        }

        private ConsoleKeyHandlerResult MoveBackwardToSingleCharacterWord(ConsoleKeyInfo keyInfo, ReadLineEditorContext context)
        {
            if (_commonHandlers.TryFindSingleCharacterWordToTheLeft(context, out int startOfSingleCharacterWord))
            {
                context.ConsoleEditorLine.CurrentCursorPosition = startOfSingleCharacterWord;
                //context.ConsoleEditorLine.RefreshDisplay();
            }
            return ConsoleKeyHandlerResult.Consumed;
        }

        private ConsoleKeyHandlerResult MoveForwardToSingleCharacterWord(ConsoleKeyInfo keyInfo, ReadLineEditorContext context)
        {
            if (_commonHandlers.TryFindSingleCharacterWordToTheRight(context, out int startOfSingleCharacterWord))
            {
                context.ConsoleEditorLine.CurrentCursorPosition = startOfSingleCharacterWord;
                //context.ConsoleEditorLine.RefreshDisplay();
            }
            return ConsoleKeyHandlerResult.Consumed;
        }

        private ConsoleKeyHandlerResult MoveOneDownInHistory(ConsoleKeyInfo keyInfo, ReadLineEditorContext context)
        {
            context.ConsoleEditorLine.Close();
            if (context.History.TryMoveDown(out UndoableConsoleEditorLine? editorLine))
            {
                context.ConsoleEditorLine = editorLine;
            }
            context.ConsoleEditorLine.RefreshDisplay();
            return ConsoleKeyHandlerResult.Consumed;
        }

        private ConsoleKeyHandlerResult MoveOneUpInHistory(ConsoleKeyInfo keyInfo, ReadLineEditorContext context)
        {
            context.ConsoleEditorLine.Close();
            if (context.History.TryMoveUp(out UndoableConsoleEditorLine? editorLine))
            {
                context.ConsoleEditorLine = editorLine;
            }
            context.ConsoleEditorLine.RefreshDisplay();
            return ConsoleKeyHandlerResult.Consumed;
        }

        private ConsoleKeyHandlerResult MoveToFirstHistoryEntry(ConsoleKeyInfo keyInfo, ReadLineEditorContext context)
        {
            context.ConsoleEditorLine.Close();
            if (context.History.TryMoveFirst(out UndoableConsoleEditorLine? editorLine))
            {
                context.ConsoleEditorLine = editorLine;
            }
            context.ConsoleEditorLine.RefreshDisplay();
            return ConsoleKeyHandlerResult.Consumed;
        }

        private ConsoleKeyHandlerResult MoveToLastHistoryEntry(ConsoleKeyInfo keyInfo, ReadLineEditorContext context)
        {
            if (context.History.TryMoveLast(out UndoableConsoleEditorLine? editorLine))
            {
                context.ConsoleEditorLine.Close();
                context.ConsoleEditorLine = editorLine;
                context.ConsoleEditorLine.RefreshDisplay();
            }
            return ConsoleKeyHandlerResult.Consumed;
        }

        private ConsoleKeyHandlerResult NoOperation(ConsoleKeyInfo keyInfo, ReadLineEditorContext context)
        {
            return ConsoleKeyHandlerResult.NotConsumed;
        }

        private ConsoleKeyHandlerResult PasteClipboard(ConsoleKeyInfo keyInfo, ReadLineEditorContext context)
        {
            string clipboardText = Clipboard.GetText();
            context.ConsoleEditorLine.Insert(clipboardText);
            return ConsoleKeyHandlerResult.Consumed;
        }

        private ConsoleKeyHandlerResult ProcessAltR(ConsoleKeyInfo keyInfo, ReadLineEditorContext context)
        {
            UndoableConsoleEditorLine? undoableLine = context?.ConsoleEditorLine as UndoableConsoleEditorLine;
            undoableLine?.UndoAll();
            return ConsoleKeyHandlerResult.Consumed;
        }

        private ConsoleKeyHandlerResult ProcessCtrlA(ConsoleKeyInfo keyInfo, ReadLineEditorContext context)
        {
            if (context.CtrlVPressed)
            {
                context.CtrlVPressed = true;
                context.ConsoleEditorLine.Insert('\a');
                return ConsoleKeyHandlerResult.Consumed;
            }

            return _commonHandlers.MoveCursorToStartOfLine(keyInfo, context);
        }

        private ConsoleKeyHandlerResult ProcessCtrlB(ConsoleKeyInfo keyInfo, ReadLineEditorContext context)
        {
            if (context.CtrlVPressed)
            {
                context.CtrlVPressed = true;
                context.ConsoleEditorLine.Insert('\b');
                return ConsoleKeyHandlerResult.Consumed;
            }

            return _commonHandlers.MoveCursorLeft(keyInfo, context);
        }

        private ConsoleKeyHandlerResult ProcessCtrlD(ConsoleKeyInfo keyInfo, ReadLineEditorContext context)
        {
            if (context.ConsoleEditorLine.Length == 0)
            {
                return ConsoleKeyHandlerResult.Aborted;
            }

            return _commonHandlers.RemoveAfterCursor(keyInfo, context);
        }

        private ConsoleKeyHandlerResult ProcessCtrlE(ConsoleKeyInfo keyInfo, ReadLineEditorContext context)
        {
            if (context.CtrlXPressed)
            {
                context.CtrlXPressed = false;
                // Ctrl+x Ctrl+e : Edits the current line in the $EDITOR program, or vi if undefined.
                //TODO
                return ConsoleKeyHandlerResult.Consumed;
            }

            return _commonHandlers.MoveCursorToEndOfLine(keyInfo, context);
        }

        private ConsoleKeyHandlerResult ProcessCtrlF(ConsoleKeyInfo keyInfo, ReadLineEditorContext context)
        {
            if (context.CtrlVPressed)
            {
                context.CtrlVPressed = true;
                context.ConsoleEditorLine.Insert('\f');
                return ConsoleKeyHandlerResult.Consumed;
            }

            return _commonHandlers.MoveCursorRight(keyInfo, context);
        }

        private ConsoleKeyHandlerResult ProcessCtrlN(ConsoleKeyInfo keyInfo, ReadLineEditorContext context)
        {
            if (context.CtrlVPressed)
            {
                context.CtrlVPressed = true;
                context.ConsoleEditorLine.Insert('\n');
                return ConsoleKeyHandlerResult.Consumed;
            }

            return MoveOneDownInHistory(keyInfo, context);
        }

        private ConsoleKeyHandlerResult ProcessCtrlR(ConsoleKeyInfo keyInfo, ReadLineEditorContext context)
        {
            if (context.CtrlXPressed)
            {
                context.CtrlXPressed = false;
                // Ctrl+x Ctrl+r : Read in the contents of the inputrc file, and incorporate any bindings or variable assignments found there.
                // Not supported yet.
                return ConsoleKeyHandlerResult.Consumed;
            }

            if (context.CtrlVPressed)
            {
                context.CtrlVPressed = true;
                context.ConsoleEditorLine.Insert('\r');
                return ConsoleKeyHandlerResult.Consumed;
            }

            return StartReverseSearch(keyInfo, context);
        }

        private ConsoleKeyHandlerResult ProcessCtrlT(ConsoleKeyInfo keyInfo, ReadLineEditorContext context)
        {
            if (context.CtrlVPressed)
            {
                context.CtrlVPressed = true;
                context.ConsoleEditorLine.Insert('\t');
                return ConsoleKeyHandlerResult.Consumed;
            }

            return ToggleCharacterPositions(keyInfo, context);
        }

        private ConsoleKeyHandlerResult ProcessCtrlU(ConsoleKeyInfo keyInfo, ReadLineEditorContext context)
        {
            if (context.CtrlXPressed)
            {
                context.CtrlXPressed = false;
                // Ctrl+x Ctrl+u : Incremental undo, separately remembered for each line.
                UndoableConsoleEditorLine? undoableLine = context?.ConsoleEditorLine as UndoableConsoleEditorLine;
                undoableLine?.Undo();
                return ConsoleKeyHandlerResult.Consumed;
            }

            _ = _commonHandlers.RemoveTextBeforeCursor(keyInfo, context);
            return ConsoleKeyHandlerResult.Consumed;
        }

        private ConsoleKeyHandlerResult ProcessCtrlV(ConsoleKeyInfo keyInfo, ReadLineEditorContext context)
        {
            if (!context.CtrlVPressed)
            {
                context.CtrlVPressed = true;
                return ConsoleKeyHandlerResult.Consumed;
            }

            if (context.CtrlXPressed)
            {
                context.CtrlXPressed = false;
                // Ctrl+x Ctrl+v : Display version information about the current instance of Bash.
                // TODO: display version of this library after multi-line preview for
                // the tab-extension has been implemented.
                return ConsoleKeyHandlerResult.Consumed;
            }

            if (context.CtrlVPressed)
            {
                context.CtrlVPressed = true;
                context.ConsoleEditorLine.Insert('\v');
                return ConsoleKeyHandlerResult.Consumed;
            }

            return ConsoleKeyHandlerResult.NotConsumed;
        }

        private ConsoleKeyHandlerResult ProcessCtrlX(ConsoleKeyInfo keyInfo, ReadLineEditorContext context)
        {
            if (!context.CtrlXPressed)
            {
                context.CtrlXPressed = true;
                return ConsoleKeyHandlerResult.Consumed;
            }

            context.CtrlXPressed = false;
            // Ctrl+x Ctrl+x : Alternates the cursor with its old position. (C-x, because x has a crossing shape).
            int currentCursorPosition = context.ConsoleEditorLine.CurrentCursorPosition;
            context.ConsoleEditorLine.CurrentCursorPosition = context.AlternateCursorPosition;
            context.AlternateCursorPosition = currentCursorPosition;

            return ConsoleKeyHandlerResult.Consumed;
        }

        private ConsoleKeyHandlerResult ProcessCtrlZ(ConsoleKeyInfo keyInfo, ReadLineEditorContext context)
        {
            // suspend the current task. This is useful in a Bash, not implemented here.
            return ConsoleKeyHandlerResult.Consumed;
        }

        private ConsoleKeyHandlerResult ProcessTabExtension(ConsoleKeyInfo keyInfo, ReadLineEditorContext context)
        {
            // TODO: implement tab-extension here
            return ConsoleKeyHandlerResult.Consumed;
        }

        private ConsoleKeyHandlerResult RemoveAfterCursor(ConsoleKeyInfo keyInfo, ReadLineEditorContext context)
        {
            if (context.CtrlXPressed)
            {
                context.CtrlXPressed = true;
                // Ctrl-X, DELETE : remove the text before the cursor up to the start of the line
                _ = _commonHandlers.RemoveTextBeforeCursor(keyInfo, context);
                return ConsoleKeyHandlerResult.Consumed;
            }

            context.ConsoleEditorLine.RemoveAfterCursor();
            return ConsoleKeyHandlerResult.Consumed;
        }

        private ConsoleKeyHandlerResult RemoveWordAfterCursor(ConsoleKeyInfo keyInfo, ReadLineEditorContext context)
        {
            if (context.ConsoleEditorLine.CurrentCursorPosition == context.ConsoleEditorLine.Length)
            {
                return ConsoleKeyHandlerResult.Consumed;
            }

            string removedText = _commonHandlers.RemoveWordAfterCursor(context);
            if (!string.IsNullOrEmpty(removedText))
            {
                Clipboard.SetText(removedText);
            }
            return ConsoleKeyHandlerResult.Consumed;
        }

        private ConsoleKeyHandlerResult RemoveLineAfterCursor(ConsoleKeyInfo keyInfo, ReadLineEditorContext context)
        {
            string removedText = _commonHandlers.RemoveTextAfterCursor(context);
            if (!string.IsNullOrEmpty(removedText))
            {
                Clipboard.SetText(removedText);
            }
            return ConsoleKeyHandlerResult.Consumed;
        }

        private ConsoleKeyHandlerResult RemoveWhitespaceAroundCursor(ConsoleKeyInfo keyInfo, ReadLineEditorContext context)
        {
            var _ = _commonHandlers.RemoveWhitespaceAroundCursor(context);
            return ConsoleKeyHandlerResult.Consumed;
        }

        private ConsoleKeyHandlerResult RemoveWordBeforeCursor(ConsoleKeyInfo keyInfo, ReadLineEditorContext context)
        {
            if (context.ConsoleEditorLine.Length == 0)
            {
                return ConsoleKeyHandlerResult.Consumed;
            }

            string removedText = _commonHandlers.RemoveWordBeforeCursor(keyInfo, context);
            Clipboard.SetText(removedText);
            return ConsoleKeyHandlerResult.Consumed;
        }

        private ConsoleKeyHandlerResult StartReverseSearch(ConsoleKeyInfo keyInfo, ReadLineEditorContext context)
        {
            context.ConsoleEditorLine.Close();

            ReverseSearchEditorLine reverseSearchEditor = new ReverseSearchEditorLine("Reverse search: ", _history);
            string searchResult = reverseSearchEditor.ReadLine();
            reverseSearchEditor.Close();

            context.ConsoleEditorLine.Text = searchResult ?? context.ConsoleEditorLine.Text;

            return ConsoleKeyHandlerResult.Consumed;
        }

        private ConsoleKeyHandlerResult ToggleCharacterPositions(ConsoleKeyInfo keyInfo, ReadLineEditorContext context)
        {
            if (context.ConsoleEditorLine.CurrentCursorPosition == context.ConsoleEditorLine.Length)
            {
                return ConsoleKeyHandlerResult.Consumed;
            }

            char removedChar = context.ConsoleEditorLine.RemoveBeforeCursor();
            context.ConsoleEditorLine.MoveCursorRight();
            context.ConsoleEditorLine.Insert(removedChar);
            context.ConsoleEditorLine.MoveCursorLeft();
            return ConsoleKeyHandlerResult.Consumed;
        }

    }
}