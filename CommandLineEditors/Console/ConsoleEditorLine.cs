using System.Text;

using SystemConsole = System.Console;

namespace CommandLineEditors.Console
{
    internal sealed class ConsoleEditorLine : IConsoleEditorLine
    {

        /// <summary>
        /// Gets or sets the preview-string which is appended at
        /// the end of the input but cannot be edited.
        /// </summary>
        public string PreviewString
        {
            get => _previewString;
            set => SetPreview(value ?? "");
        }

        /// <summary>
        /// Gets or sets the text of the editor line.
        /// </summary>
        public string Text
        {
            get => _inputBuffer.ToString();
            set => SetText(value);
        }

        /// <summary>
        /// Returns the length of the input text.
        /// </summary>
        public int Length => _inputBuffer.Length;

        /// <summary>
        /// Gets the current position of the cursor
        /// </summary>
        public int CurrentCursorPosition
        {
            get => _positionInInputBuffer;
            set
            {
                if (value >= 0 && value <= Length)
                {
                    _positionInInputBuffer = value;
                    UpdateCursorPosition();
                }
            }
        }

        /// <summary>
        /// Gets or sets whether this editor line operates in insert-mode.
        /// </summary>
        public bool InsertMode
        {
            get => _operationMode == OperationMode.InsertMode;
            set => _operationMode = value ? OperationMode.InsertMode : OperationMode.OverwriteMode;
        }

        public ConsoleEditorLine()
            : this(string.Empty)
        {
        }

        public ConsoleEditorLine(string text, string prefix = "")
        {
            _prefix = prefix;
            _positionInInputBuffer = 0;

            _operationMode = OperationMode.InsertMode;
            _inputBuffer = new StringBuilder(text);

            _previewString = "";

            InitDisplay();
        }

        public void Close()
        {
            ConsoleLayer.ClearAreaAtPosition(_editorLineStartPositionX, _editorLineStartPositionY, _prefix.Length + _inputBuffer.Length + _previewString.Length);
        }

        public void Insert(char ch)
        {
            if (_positionInInputBuffer == _inputBuffer.Length)
            {
                (int posX, int PosY) = CalculatePositionOnConsole();
                ConsoleLayer.WriteCharacterAtPosition(posX, PosY, ch);
                _inputBuffer.Append(ch);
                _positionInInputBuffer++;
            }
            else if (_operationMode == OperationMode.InsertMode)
            {
                (int posX, int PosY) = CalculatePositionOnConsole();
                _inputBuffer.Insert(_positionInInputBuffer, ch);
                ConsoleLayer.WriteStringAtPosition(posX, PosY, _inputBuffer.ToString(_positionInInputBuffer, _inputBuffer.Length - _positionInInputBuffer));
                _positionInInputBuffer++;
            }
            else
            {
                _inputBuffer.Replace(_inputBuffer[_positionInInputBuffer], ch, _positionInInputBuffer, 1);
                _positionInInputBuffer++;
            }
            UpdateCursorPosition();
        }

        public void Insert(string str)
        {
            (int posX, int PosY) = CalculatePositionOnConsole();
            if (_positionInInputBuffer == _inputBuffer.Length)
            {
                ConsoleLayer.WriteStringAtPosition(posX, PosY, str);
                _inputBuffer.Append(str);
                _positionInInputBuffer += str.Length;
            }
            else if (_operationMode == OperationMode.InsertMode)
            {
                _inputBuffer.Insert(_positionInInputBuffer, str);
                ConsoleLayer.WriteStringAtPosition(posX, PosY, _inputBuffer.ToString(_positionInInputBuffer, _inputBuffer.Length - _positionInInputBuffer));
                _positionInInputBuffer += str.Length;
            }
            else
            {
                _inputBuffer.Remove(_positionInInputBuffer, str.Length);
                _inputBuffer.Insert(_positionInInputBuffer, str);
                ConsoleLayer.WriteStringAtPosition(posX, PosY, str);
                _positionInInputBuffer += str.Length;
            }
            UpdateCursorPosition();
        }

        public void RefreshDisplay()
        {
            ConsoleLayer.WriteStringAtPosition(_editorLineStartPositionX, _editorLineStartPositionY, _prefix);
            ConsoleLayer.WriteStringAtPosition(_editableAreaStartPositionX, _editableAreaStartPositionY, _inputBuffer.ToString());
            System.ConsoleColor colorToRestore = ConsoleLayer.TextColor;
            ConsoleLayer.TextColor = System.ConsoleColor.DarkGray;
            ConsoleLayer.WriteString(_previewString);
            ConsoleLayer.TextColor = colorToRestore;
            UpdateCursorPosition();
        }

        public char RemoveBeforeCursor()
        {
            char removedChar = '\0';

            if (_positionInInputBuffer > 0)
            {
                _positionInInputBuffer--;
                removedChar = _inputBuffer[_positionInInputBuffer];
                Remove(_positionInInputBuffer, 1);
                UpdateCursorPosition();
            }

            return removedChar;
        }

        public char RemoveAfterCursor()
        {
            char removedChar = '\0';

            if (_positionInInputBuffer < _inputBuffer.Length)
            {
                removedChar = _inputBuffer[_positionInInputBuffer];
                Remove(_positionInInputBuffer, 1);
                UpdateCursorPosition();
                return removedChar;
            }

            return removedChar;
        }

        public void MoveCursorLeft()
        {
            if (_positionInInputBuffer > 0)
            {
                _positionInInputBuffer--;
                UpdateCursorPosition();
            }
        }

        public void MoveCursorRight()
        {
            if (_positionInInputBuffer < _inputBuffer.Length)
            {
                _positionInInputBuffer++;
                UpdateCursorPosition();
            }
        }

        public void MoveCursorToStartOfLine()
        {
            _positionInInputBuffer = 0;
            UpdateCursorPosition();
        }

        public void MoveCursorToEndOfLine()
        {
            _positionInInputBuffer = _inputBuffer.Length;
            UpdateCursorPosition();
        }

        public void Overwrite(char ch)
        {
            if (_positionInInputBuffer == _inputBuffer.Length)
            {
                (int posX, int PosY) = CalculatePositionOnConsole();
                ConsoleLayer.WriteCharacterAtPosition(posX, PosY, ch);
                _inputBuffer.Append(ch);
                _positionInInputBuffer++;
            }
            else
            {
                _inputBuffer.Replace(_inputBuffer[_positionInInputBuffer], ch, _positionInInputBuffer, 1);
                (int posX, int PosY) = CalculatePositionOnConsole();
                ConsoleLayer.WriteCharacterAtPosition(posX, PosY, ch);
                _positionInInputBuffer++;
            }
            UpdateCursorPosition();
        }

        public void Overwrite(string str)
        {
            // TODO: implment this
        }

        public string Remove(int startPos, int count)
        {
            string removedText = string.Empty;

            if (startPos + count - 1 < _inputBuffer.Length)
            {
                int newCursorPosition = CurrentCursorPosition;
                if (newCursorPosition > startPos && newCursorPosition <= startPos + count)
                {
                    newCursorPosition = startPos;
                }

                (int posX, int PosY) = CalculatePositionOnConsole(startPos);
                removedText = _inputBuffer.ToString().Substring(startPos, count);
                _inputBuffer.Remove(startPos, count);
                ConsoleLayer.WriteStringAtPosition(posX, PosY, _inputBuffer.ToString(startPos, _inputBuffer.Length - startPos));
                ConsoleLayer.ClearArea(count);

                CurrentCursorPosition = newCursorPosition;
            }

            return removedText;
        }

        /// <summary>
        /// Sets the position of the editor line. This will move its complete
        /// contents including its prompt to a new position on screen. The previous
        /// text will not be removed or fixed. This method only prints out what
        /// the current line looks like, but at a different position.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void SetPosition(int x, int y)
        {
            ConsoleLayer.SetCursorPosition(x, y);
            InitDisplay();
            RefreshDisplay();
        }


        private int _editorLineStartPositionX;
        private int _editorLineStartPositionY;
        private readonly string _prefix;

        private int _editableAreaStartPositionX;
        private int _editableAreaStartPositionY;

        private int _positionInInputBuffer;

        private enum OperationMode { InsertMode, OverwriteMode };

        private OperationMode _operationMode;

        private readonly StringBuilder _inputBuffer;

        private string _previewString;


        private void UpdateCursorPosition()
        {
            (int posX, int posY) = CalculatePositionOnConsole();
            ConsoleLayer.SetCursorPosition(posX, posY);
        }

        private (int, int) CalculatePositionOnConsole()
        {
            return CalculatePositionOnConsole(_positionInInputBuffer);
        }

        private (int, int) CalculatePositionOnConsole(int currentPositionInBuffer)
        {
            // This is single-line-only calculation. We need a modulus
            // operation to find the actual position on screen.
            // Use SystemConsole.WindowWidth for this.
            int posX = _editableAreaStartPositionX + currentPositionInBuffer;
            int posY = _editableAreaStartPositionY;
            if (posX > SystemConsole.WindowWidth)
            {
                posY += posX / SystemConsole.WindowWidth;
                posX %= SystemConsole.WindowWidth;
            }
            return (posX, posY);
        }

        public void InitDisplay()
        {
            _editorLineStartPositionX = SystemConsole.CursorLeft;
            _editorLineStartPositionY = SystemConsole.CursorTop;

            ConsoleLayer.WriteStringAtPosition(_editorLineStartPositionX, _editorLineStartPositionY, _prefix);

            _editableAreaStartPositionX = SystemConsole.CursorLeft;
            _editableAreaStartPositionY = SystemConsole.CursorTop;
        }

        private void SetText(string newInputContent)
        {
            if (newInputContent.Length < _inputBuffer.Length)
            {
                // clear the old display, before the new line gets written
                ConsoleLayer.ClearAreaAtPosition(_editableAreaStartPositionX, _editableAreaStartPositionY, _inputBuffer.Length);
            }
            ConsoleLayer.WriteStringAtPosition(_editableAreaStartPositionX, _editableAreaStartPositionY, newInputContent);
            _inputBuffer.Length = 0;
            _inputBuffer.Append(newInputContent);
        }

        private void SetPreview(string newPreview)
        {
            (int previewX, int previewY) = CalculatePositionOnConsole(_inputBuffer.Length);
            if (newPreview.Length < _inputBuffer.Length)
            {
                // clear the old display, before the new line gets written
                ConsoleLayer.ClearAreaAtPosition(previewX, previewY, _previewString.Length);
            }
            ConsoleLayer.WriteStringAtPosition(previewX, previewY, newPreview);
            _previewString = newPreview;
            RefreshDisplay();
        }

    }
}
