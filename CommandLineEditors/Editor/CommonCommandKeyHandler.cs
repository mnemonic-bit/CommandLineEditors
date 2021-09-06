using CommandLineEditors.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommandLineEditors.Editor
{
    internal class CommonCommandKeyHandler<TContext>
        where TContext : IEditorContext
    {

        public CommonCommandKeyHandler()
        {
        }

        public ConsoleKeyHandlerResult MoveCursorToStartOfLine(ConsoleKeyInfo keyInfo, TContext context)
        {
            context.ConsoleEditorLine.MoveCursorToStartOfLine();
            return ConsoleKeyHandlerResult.Consumed;
        }

        public ConsoleKeyHandlerResult MoveCursorToEndOfLine(ConsoleKeyInfo keyInfo, TContext context)
        {
            context.ConsoleEditorLine.MoveCursorToEndOfLine();
            return ConsoleKeyHandlerResult.Consumed;
        }

        public ConsoleKeyHandlerResult MoveCursorLeftToStartOfWord(ConsoleKeyInfo keyInfo, TContext context)
        {
            int newPosition = GetStartOfWord(context);
            context.ConsoleEditorLine.CurrentCursorPosition = newPosition;
            return ConsoleKeyHandlerResult.Consumed;
        }

        public ConsoleKeyHandlerResult MoveCursorRightToEndOfWord(ConsoleKeyInfo keyInfo, TContext context)
        {
            int newPosition = GetEndOfWord(context);
            context.ConsoleEditorLine.CurrentCursorPosition = newPosition;
            return ConsoleKeyHandlerResult.Consumed;
        }

        public ConsoleKeyHandlerResult MoveCursorLeft(ConsoleKeyInfo keyInfo, TContext context)
        {
            context.ConsoleEditorLine.MoveCursorLeft();
            return ConsoleKeyHandlerResult.Consumed;
        }

        public ConsoleKeyHandlerResult RefreshDisplay(ConsoleKeyInfo keyInfo, TContext context)
        {
            context.ConsoleEditorLine.RefreshDisplay();
            return ConsoleKeyHandlerResult.Consumed;
        }

        public ConsoleKeyHandlerResult MoveCursorRight(ConsoleKeyInfo keyInfo, TContext context)
        {
            context.ConsoleEditorLine.MoveCursorRight();
            return ConsoleKeyHandlerResult.Consumed;
        }

        public int GetStartOfWord(TContext context)
        {
            string currentText = context.ConsoleEditorLine.Text;
            int currentPosition = context.ConsoleEditorLine.CurrentCursorPosition;
            int endPosition = currentText.IndexOf(ch => !ch.IsWhiteSpace(), currentPosition - 1, false);
            int startPosition = currentText.IndexOf(ch => ch.IsWhiteSpace(), endPosition, false);
            startPosition++;
            return startPosition;
        }

        private int GetEndOfWord(TContext context)
        {
            string currentText = context.ConsoleEditorLine.Text;
            int currentPosition = context.ConsoleEditorLine.CurrentCursorPosition;
            int startPosition = currentText.IndexOf(ch => !ch.IsWhiteSpace(), currentPosition, true);
            int endPosition = currentText.IndexOf(ch => ch.IsWhiteSpace(), startPosition, true);
            endPosition = endPosition == -1 ? currentText.Length : endPosition;
            return endPosition;
        }

        public string RemoveWordBeforeCursor(ConsoleKeyInfo keyInfo, TContext context)
        {
            int currentPosition = context.ConsoleEditorLine.CurrentCursorPosition;
            int startPosition = GetStartOfWord(context);
            string wordBeforeCursor = context.ConsoleEditorLine.Remove(startPosition, currentPosition - startPosition);
            return wordBeforeCursor;
        }

        public string RemoveTextBeforeCursor(ConsoleKeyInfo keyInfo, TContext context)
        {
            int currentCursorPosition = context.ConsoleEditorLine.CurrentCursorPosition;
            string removedText = context.ConsoleEditorLine.Remove(0, currentCursorPosition);
            return removedText;
        }

        public ConsoleKeyHandlerResult RemoveAfterCursor(ConsoleKeyInfo keyInfo, TContext context)
        {
            context.ConsoleEditorLine.RemoveAfterCursor();
            return ConsoleKeyHandlerResult.Consumed;
        }

        public string RemoveWordAfterCursor(TContext context)
        {
            int currentPosition = context.ConsoleEditorLine.CurrentCursorPosition;
            int endPosition = GetEndOfWord(context);
            string wordAfterCursor = context.ConsoleEditorLine.Remove(currentPosition, endPosition - currentPosition);
            return wordAfterCursor;
        }

        public string RemoveTextAfterCursor(TContext context)
        {
            int currentCursorPosition = context.ConsoleEditorLine.CurrentCursorPosition;
            int currentTextLength = context.ConsoleEditorLine.Length;
            string removedText = context.ConsoleEditorLine.Remove(currentCursorPosition, currentTextLength - currentCursorPosition);
            return removedText;
        }

        public ConsoleKeyHandlerResult CapitalizeCharacterUnderCursor(ConsoleKeyInfo keyInfo, TContext context)
        {
            int currentPosition = context.ConsoleEditorLine.CurrentCursorPosition;
            int endPosition = GetEndOfWord(context);
            string wordAfterCursor = context.ConsoleEditorLine.Text[currentPosition..endPosition];
            context.ConsoleEditorLine.Overwrite(wordAfterCursor[0].ToUpper());
            context.ConsoleEditorLine.CurrentCursorPosition = endPosition;
            return ConsoleKeyHandlerResult.Consumed;
        }

        public ConsoleKeyHandlerResult ChangeCharactersToLowerCaseUpToEndOfWord(ConsoleKeyInfo keyInfo, TContext context)
        {
            string wordBeforeCursor = RemoveWordAfterCursor(context);
            context.ConsoleEditorLine.Insert(wordBeforeCursor.ToLower());
            return ConsoleKeyHandlerResult.Consumed;
        }

        public ConsoleKeyHandlerResult ChangeCharactersToUpperCaseUpToEndOfWord(ConsoleKeyInfo keyInfo, TContext context)
        {
            string wordBeforeCursor = RemoveWordAfterCursor(context);
            context.ConsoleEditorLine.Insert(wordBeforeCursor.ToUpper());
            return ConsoleKeyHandlerResult.Consumed;
        }

        public ConsoleKeyHandlerResult RemoveBeforeCursor(ConsoleKeyInfo keyInfo, TContext context)
        {
            context.ConsoleEditorLine.RemoveBeforeCursor();
            return ConsoleKeyHandlerResult.Consumed;
        }

    }
}
