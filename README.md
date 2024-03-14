
# CommandLineEditors Library

This library provides editors for the command line that can be used as a replacement
for the standard System.Console.ReadLine() method call.

Currently there is only one line-editor fully implemented, which emulates the 
ReadLine library, similar to a Emacs key-binding.

An early version of VI key-bindings is also in this library at the current state, but
we do not recommend using this.


# Quick Start

If you just want to get a replacement of the C# standard function Console.ReadLine()
the ReadLineEditor is probably the best way to start with. It provides a key-binding
which implements 95% of the well-known readline-libray key kindings people usually
encounter on terminals in Linux.

To start with this editor, you have to add one using-statement and also create an
instance of this editor, because the editor holds a history of what the user typed
to make it available throught the up and down-arrow keys.

Here is a small program which repeatedly reads a single line from the console until
the user enters an empty line:

```
using CommandLineEditors.Editor.ReadLine;

namespace CommandLineEditors.Demo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            TestConsoleInput();
        }

        public static void TestConsoleInput()
        {
            ReadLineEditor readLineInterface = new ReadLineEditor();

            string result = "-not-empty-to-start-with-";
            while (!string.IsNullOrEmpty(result))
            {
                System.Console.Write("> ");
                result = readLineInterface.ReadLine();
                System.Console.WriteLine();
                System.Console.WriteLine($"result: '{result}'");
            }
        }

    }
}
```
