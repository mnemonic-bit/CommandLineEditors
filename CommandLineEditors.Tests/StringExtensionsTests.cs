using CommandLineEditors.Console;
using CommandLineEditors.Editor;
using CommandLineEditors.Extensions;
using FluentAssertions;
using Moq;

namespace CommandLineEditors.Tests
{
    public class StringExtensionsTests
    {

        [Fact]
        public void GetBoundsOfWhitespace_ShouldReturnCurrentPosition_WhenCursorIsPlacedOnWord()
        {
            // Arrange
            string text = "abcd";
            int currentPosition = 2;

            // Act
            (int startPosition, int endPosition) = text.GetBoundsOfWhitespace(currentPosition);

            // Assert
            startPosition.Should().Be(currentPosition);
            endPosition.Should().Be(currentPosition);
        }

        [Fact]
        public void GetBoundsOfWord_ShouldReturnCurrentPosition_WhenCursorIsPlacedOnWhitespace()
        {
            // Arrange
            string text = "123  abcd";
            int currentPosition = 4;

            // Act
            (int startPosition, int endPosition) = text.GetBoundsOfWord(currentPosition);

            // Assert
            startPosition.Should().Be(currentPosition);
            endPosition.Should().Be(currentPosition);
        }

        [Fact]
        public void GetBoundsOfWord_ShouldReturnBounds_WhenCursorIsPlacedAtStartOfWord()
        {
            // Arrange
            string text = "123  abcd";
            int currentPosition = 5;

            // Act
            (int startPosition, int endPosition) = text.GetBoundsOfWord(currentPosition);

            // Assert
            startPosition.Should().Be(5);
            endPosition.Should().Be(9);
        }

        [Fact]
        public void GetBoundsOfWord_ShouldReturnBounds_WhenCursorIsPlacedAtTheEndOfWord_1()
        {
            // Arrange
            string text = "123  abcd";
            int currentPosition = 3;

            // Act
            (int startPosition, int endPosition) = text.GetBoundsOfWord(currentPosition);

            // Assert
            startPosition.Should().Be(0);
            endPosition.Should().Be(3);
        }

        [Fact]
        public void GetBoundsOfWord_ShouldReturnBounds_WhenCursorIsPlacedAtTheEndOfWord_2()
        {
            // Arrange
            string text = "123  abcd";
            int currentPosition = 9;

            // Act
            (int startPosition, int endPosition) = text.GetBoundsOfWord(currentPosition);

            // Assert
            startPosition.Should().Be(5);
            endPosition.Should().Be(9);
        }

        [Fact]
        public void GetNextStartOfWord_ShouldFindTheStartOfTheWord_WhenPositionedAtTheStartOfThePreviousWord_1()
        {
            // Arrange
            string text = "123  abcd";
            int currentPosition = 5;

            int startPosition = text.GetNextStartOfWord(currentPosition);

            // Assert
            startPosition.Should().Be(0);
        }

        [Fact]
        public void GetNextStartOfWord_ShouldFindTheStartOfTheWord_WhenPositionedAtTheStartOfThePreviousWord_2()
        {
            // Arrange
            string text = "123  abcd";
            int currentPosition = 4;

            int startPosition = text.GetNextStartOfWord(currentPosition);

            // Assert
            startPosition.Should().Be(0);
        }

        [Fact]
        public void GetNextStartOfWord_ShouldFindTheStartOfTheWord_WhenPositionedAtTheStartOfThePreviousWord_4()
        {
            // Arrange
            string text = " 123  abcd";
            int currentPosition = 6;

            int startPosition = text.GetNextStartOfWord(currentPosition);

            // Assert
            startPosition.Should().Be(1);
        }

        [Fact]
        public void GetNextStartOfWord_ShouldFindTheStartOfTheWord_WhenPositionedAtTheEndOfTheText_1()
        {
            // Arrange
            string text = "123  abcd";
            int currentPosition = 9;

            int startPosition = text.GetNextStartOfWord(currentPosition);

            // Assert
            startPosition.Should().Be(5);
        }

        [Fact]
        public void GetNextStartOfWord_ShouldFindTheStartOfTheWord_WhenPositionedAtTheEndOfTheText_2()
        {
            // Arrange
            string text = "123  abcd ";
            int currentPosition = 10;

            int startPosition = text.GetNextStartOfWord(currentPosition);

            // Assert
            startPosition.Should().Be(5);
        }

        [Fact]
        public void GetNextStartOfWord_ShouldFindTheStartOfTheWord_WhenPositionedInsideOfThatWord_1()
        {
            // Arrange
            string text = "123  abcd";
            int currentPosition = 6;

            int startPosition = text.GetNextStartOfWord(currentPosition);

            // Assert
            startPosition.Should().Be(5);
        }

        [Fact]
        public void GetNextStartOfWord_ShouldFindTheStartOfTheWord_WhenPositionedInsideOfThatWord_2()
        {
            // Arrange
            string text = "123  abcd";
            int currentPosition = 2;

            int startPosition = text.GetNextStartOfWord(currentPosition);

            // Assert
            startPosition.Should().Be(0);
        }

        [Theory]
        [InlineData(" abc ", 1)]
        [InlineData(" abc ", 2)]
        [InlineData(" abc ", 3)]
        public void IndexOf_ShouldReturnStartPosition_WhenPredicateMatchesAtThatPosition(string text, int position)
        {
            // Arrange

            // Act
            int result = text.IndexOf(ch => !ch.IsWhiteSpace(), position);

            // Assert
            result.Should().Be(position);
        }

        [Theory]
        [InlineData("abc", 0)]
        [InlineData("abc", 1)]
        [InlineData("abc", 2)]
        public void IndexOf_ShouldReturnMinusOne_WhenPredicateDoesNotMatchAtAll(string text, int position)
        {
            // Arrange

            // Act
            int result = text.IndexOf(ch => ch.IsWhiteSpace(), position);

            // Assert
            result.Should().Be(-1);
        }

        [Fact]
        public void TryFindSingleCharacterWordToTheRight_ShouldReturnStartIndexOfSingleWord_WhenSingleWordIsToTheRightOfTheCurrentPosition1()
        {
            // Arrange
            string text = "abcd e fg";
            int currentPosition = 2;

            // Act
            bool result = text.TryFindSingleCharacterWordToTheRight(currentPosition, out int foundPosition);

            // Assert
            result.Should().BeTrue();
            foundPosition.Should().Be(5);
        }

        [Fact]
        public void TryFindSingleCharacterWordToTheRight_ShouldReturnStartIndexOfSingleWord_WhenSingleWordIsToTheRightOfTheCurrentPosition2()
        {
            // Arrange
            string text = "abcd e fg";
            int currentPosition = 4;

            // Act
            bool result = text.TryFindSingleCharacterWordToTheRight(currentPosition, out int foundPosition);

            // Assert
            result.Should().BeTrue();
            foundPosition.Should().Be(5);
        }

        [Fact]
        public void TryFindSingleCharacterWordToTheRight_ShouldReturnStartIndexOfSingleWord_WhenSingleWordIsToTheRightOfTheCurrentPosition3()
        {
            // Arrange
            string text = "abcd e fg";
            int currentPosition = 5;

            // Act
            bool result = text.TryFindSingleCharacterWordToTheRight(currentPosition, out int foundPosition);

            // Assert
            result.Should().BeTrue();
            foundPosition.Should().Be(5);
        }

        [Fact]
        public void TryFindSingleCharacterWordToTheRight_ShouldReturnMinusOne_WhenNoSingleWordCanBeFound1()
        {
            // Arrange
            string text = "abcd";
            int currentPosition = 2;

            // Act
            bool result = text.TryFindSingleCharacterWordToTheRight(currentPosition, out int foundPosition);

            // Assert
            result.Should().BeFalse();
            foundPosition.Should().Be(-1);
        }

        [Fact]
        public void TryFindSingleCharacterWordToTheRight_ShouldReturnMinusOne_WhenNoSingleWordCanBeFound2()
        {
            // Arrange
            string text = "abcd dsf";
            int currentPosition = 2;

            // Act
            bool result = text.TryFindSingleCharacterWordToTheRight(currentPosition, out int foundPosition);

            // Assert
            result.Should().BeFalse();
            foundPosition.Should().Be(-1);
        }

        [Fact]
        public void TryFindSingleCharacterWordToTheLeft_ShouldReturnStartIndexOfSingleWord_WhenSingleWordIsToTheRightOfTheCurrentPosition1()
        {
            // Arrange
            string text = "ab c defg";
            int currentPosition = 7;

            // Act
            bool result = text.TryFindSingleCharacterWordToTheLeft(currentPosition, out int foundPosition);

            // Assert
            result.Should().BeTrue();
            foundPosition.Should().Be(3);
        }

        [Fact]
        public void TryFindSingleCharacterWordToTheLeft_ShouldReturnStartIndexOfSingleWord_WhenSingleWordIsToTheRightOfTheCurrentPosition2()
        {
            // Arrange
            string text = "c defg";
            int currentPosition = 4;

            // Act
            bool result = text.TryFindSingleCharacterWordToTheLeft(currentPosition, out int foundPosition);

            // Assert
            result.Should().BeTrue();
            foundPosition.Should().Be(0);
        }

        [Fact]
        public void TryFindSingleCharacterWordToTheLeft_ShouldReturnStartIndexOfSingleWord_WhenSingleWordIsToTheRightOfTheCurrentPosition3()
        {
            // Arrange
            string text = "ab c defg";
            int currentPosition = 4;

            // Act
            bool result = text.TryFindSingleCharacterWordToTheLeft(currentPosition, out int foundPosition);

            // Assert
            result.Should().BeTrue();
            foundPosition.Should().Be(3);
        }

        [Fact]
        public void TryFindSingleCharacterWordToTheLeft_ShouldReturnStartIndexOfSingleWord_WhenSingleWordIsToTheRightOfTheCurrentPosition4()
        {
            // Arrange
            string text = "ab c d efg";
            int currentPosition = 5;

            // Act
            bool result = text.TryFindSingleCharacterWordToTheLeft(currentPosition, out int foundPosition);

            // Assert
            result.Should().BeTrue();
            foundPosition.Should().Be(3);
        }

        [Fact]
        public void TryFindSingleCharacterWordToTheLeft_ShouldReturnMinusOne_WhenNoSingleWordCanBeFound1()
        {
            // Arrange
            string text = "abcd";
            int currentPosition = 2;

            // Act
            bool result = text.TryFindSingleCharacterWordToTheLeft(currentPosition, out int foundPosition);

            // Assert
            result.Should().BeFalse();
            foundPosition.Should().Be(-1);
        }

        [Fact]
        public void TryFindSingleCharacterWordToTheLeft_ShouldReturnMinusOne_WhenNoSingleWordCanBeFound2()
        {
            // Arrange
            string text = "abcd 1234";
            int currentPosition = 7;

            // Act
            bool result = text.TryFindSingleCharacterWordToTheLeft(currentPosition, out int foundPosition);

            // Assert
            result.Should().BeFalse();
            foundPosition.Should().Be(-1);
        }

        [Fact]
        public void TryFindSingleCharacterWordToTheLeft_ShouldReturnMinusOne_WhenNoSingleWordCanBeFound3()
        {
            // Arrange
            string text = "abcd 1234";
            int currentPosition = 9;

            // Act
            bool result = text.TryFindSingleCharacterWordToTheLeft(currentPosition, out int foundPosition);

            // Assert
            result.Should().BeFalse();
            foundPosition.Should().Be(-1);
        }

        [Fact]
        public void TryFindSingleCharacterWordToTheLeft_ShouldReturnMinusOne_WhenNoSingleWordCanBeFound4()
        {
            // Arrange
            string text = "abcd 1234";
            int currentPosition = 0;

            // Act
            bool result = text.TryFindSingleCharacterWordToTheLeft(currentPosition, out int foundPosition);

            // Assert
            result.Should().BeFalse();
            foundPosition.Should().Be(-1);
        }

        [Fact]
        public void TryFindSingleCharacterWordToTheLeft_ShouldReturnMinusOne_WhenNoSingleWordCanBeFound5()
        {
            // Arrange
            string text = "ab c defg";
            int currentPosition = 3;

            // Act
            bool result = text.TryFindSingleCharacterWordToTheLeft(currentPosition, out int foundPosition);

            // Assert
            result.Should().BeFalse();
            foundPosition.Should().Be(-1);
        }

        //TODO: place this to the tests 
        private IEditorContext CreateContext(string text, int currentPosition)
        {
            Mock<IConsoleEditorLine> consoleEditorLineMock = new Mock<IConsoleEditorLine>();
            consoleEditorLineMock.Setup(c => c.Text).Returns(text);
            consoleEditorLineMock.Setup(c => c.CurrentCursorPosition).Returns(currentPosition);

            Mock<IEditorContext> editorContextMock = new Mock<IEditorContext>();
            editorContextMock.Setup(c => c.ConsoleEditorLine).Returns(consoleEditorLineMock.Object);

            return editorContextMock.Object;
        }

    }
}