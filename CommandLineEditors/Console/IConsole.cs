using System;

namespace CommandLineEditors.Console
{
    public interface IConsole
    {

        ConsoleKeyInfo ReadKey(bool intercept);

        string ReadLine();

        string ReadLine(bool showInput);

    }
}
