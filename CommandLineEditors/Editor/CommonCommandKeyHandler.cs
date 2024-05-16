using CommandLineEditors.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommandLineEditors.Editor
{
    internal sealed class CommonCommandKeyHandler<TContext>
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

        public bool TryFindSingleCharacterWordToTheLeft(TContext context, out int position)
        {
            string currentText = context.ConsoleEditorLine.Text;
            int currentPosition = context.ConsoleEditorLine.CurrentCursorPosition;
            return currentText.TryFindSingleCharacterWordToTheLeft(currentPosition, out position);
        }

        /// <summary>
        /// This method tries to find a word to the right which has a length
        /// of one character. If no such word is found, which method returns
        /// false.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public bool TryFindSingleCharacterWordToTheRight(TContext context, out int position)
        {
            string currentText = context.ConsoleEditorLine.Text;
            int currentPosition = context.ConsoleEditorLine.CurrentCursorPosition;
            return currentText.TryFindSingleCharacterWordToTheRight(currentPosition, out position);
        }

        public (int, int) GetBoundsOfWhitespace(TContext context)
        {
            string currentText = context.ConsoleEditorLine.Text;
            int currentPosition = context.ConsoleEditorLine.CurrentCursorPosition;
            return currentText.GetBoundsOfWhitespace(currentPosition);
        }

        public (int, int) GetBoundsOfWord(TContext context)
        {
            string currentText = context.ConsoleEditorLine.Text;
            int currentPosition = context.ConsoleEditorLine.CurrentCursorPosition;
            return currentText.GetBoundsOfWord(currentPosition);
        }

        public int GetStartOfWhitespace(TContext context)
        {
            string currentText = context.ConsoleEditorLine.Text;
            int currentPosition = context.ConsoleEditorLine.CurrentCursorPosition;
            return currentText.GetBoundsOfWhitespace(currentPosition).Item1;
        }

        public int GetStartOfWord(TContext context)
        {
            string currentText = context.ConsoleEditorLine.Text;
            int currentPosition = context.ConsoleEditorLine.CurrentCursorPosition;
            return currentText.GetNextStartOfWord(currentPosition);
        }

        private int GetEndOfWhitespace(TContext context)
        {
            string currentText = context.ConsoleEditorLine.Text;
            int currentPosition = context.ConsoleEditorLine.CurrentCursorPosition;
            return currentText.GetBoundsOfWhitespace(currentPosition).Item2;
        }

        private int GetEndOfWord(TContext context)
        {
            string currentText = context.ConsoleEditorLine.Text;
            int currentPosition = context.ConsoleEditorLine.CurrentCursorPosition;
            return currentText.GetBoundsOfWord(currentPosition).Item2;
        }

        public string RemoveWordBeforeCursor(ConsoleKeyInfo keyInfo, TContext context)
        {
            int startPosition = GetStartOfWord(context);
            int currentPosition = context.ConsoleEditorLine.CurrentCursorPosition;
            return context.ConsoleEditorLine.Remove(startPosition, currentPosition - startPosition);
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

        public string RemoveTextAfterCursor(TContext context)
        {
            int currentCursorPosition = context.ConsoleEditorLine.CurrentCursorPosition;
            int currentTextLength = context.ConsoleEditorLine.Length;
            return context.ConsoleEditorLine.Remove(currentCursorPosition, currentTextLength - currentCursorPosition);
        }

        public string RemoveWordAfterCursor(TContext context)
        {
            int currentPosition = context.ConsoleEditorLine.CurrentCursorPosition;
            int endPosition = GetEndOfWord(context);
            return context.ConsoleEditorLine.Remove(currentPosition, endPosition - currentPosition);
        }

        public string RemoveWhitespaceAroundCursor(TContext context)
        {
            (int startPosition, int endPosition) = GetBoundsOfWhitespace(context);
            return context.ConsoleEditorLine.Remove(startPosition, endPosition - startPosition);
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
