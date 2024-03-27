
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
which implements 95% of the well-known readline-libray key bindings people usually
encounter on terminals in Linux.

Add the latest Nuget to the project first, by calling

```
$> dotnet add package CommandLineEditors
```

To use the command line editor you will need an instance of an editor, because the editor
holds a history of what the user typed, to make old entries available throught the up and
down-arrow keys on the key-board.

Here is a small program which repeatedly reads a single line from the console until
the user enters an empty line:

```
var readLineInterface = new CommandLineEditors.Editor.ReadLine.ReadLineEditor();
string result = readLineInterface.ReadLine();
```

For a working sample, please refer to the demo project included in the repository,
or have a look at the [Demo Program](https://github.com/mnemonic-bit/CommandLineEditors/blob/main/CommandLineEditors.Demo/Program.cs)

If you bring your own history, or want to persist an existing history, before the application
is shut down, you can use SetHistory() and GetHistory(), which is demonstrated in the next
code snippet:

```
// Init the Readline editor, and add some history entries.
var readLineInterface = new CommandLineEditors.Editor.ReadLine.ReadLineEditor();
readLineInterface.SetHistory(new List<string>() { "first history entry", "second history entry", "third history entry" });

string result = readLineInterface.ReadLine();

// before the app shuts down, you can read the history
var history = readLineInterface.GetHistory().ToList();
```
