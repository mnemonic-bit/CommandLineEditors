
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

Add the latest Nuget to the project first, by calling

```
$> dotnet add package CommandLineEditors
```

You will need an instance of the editor, because the editor holds a history of what
the user typed, to make it available throught the up and down-arrow keys.

Here is a small program which repeatedly reads a single line from the console until
the user enters an empty line:

```
var readLineInterface = new CommandLineEditors.Editor.ReadLine.ReadLineEditor();
string result = readLineInterface.ReadLine();
```

For a working sample, please refer to the demo project included in the repository,
or have a look at the [Demo Program](https://github.com/mnemonic-bit/CommandLineEditors/blob/main/CommandLineEditors.Demo/Program.cs)