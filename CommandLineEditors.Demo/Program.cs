using CommandLineEditors;
using CommandLineEditors.Editor;
using System.Collections.Generic;

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
            System.Console.WriteLine("Enter a line, then press enter and the entered text will be presented to you. Keep on entering lines, until fed up. If you wish to stop, just press enter on an empty line.");

            // the read line instance we will use
            ReadLineEditor readLineInterface = new ReadLineEditor();

            // set the auto completion items
            //List<string> autoCompletionElements = new List<string>() { "test", "singleton" };
            //readLineInterface.SetAutoCompletionStrings(autoCompletionElements);

            string result = "-not-empty-to-start-with-";
            while (!string.IsNullOrEmpty(result))
            {
                System.Console.Write("> ");
                // testing ConsoleInput
                result = readLineInterface.ReadLine();
                // output the result
                System.Console.WriteLine();
                System.Console.WriteLine($"result: '{result}'");
            }
        }

    }
}
