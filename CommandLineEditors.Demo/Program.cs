using CommandLineEditors.Editor.ReadLine;
using CommandLineEditors.Editor.Vi;
using SystemConsole = System.Console;

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
            SystemConsole.WriteLine("Enter a line, then press enter and the entered text will be presented to you. Keep on entering lines, until fed up. If you wish to stop, just press enter on an empty line.");

            // the read line instance we will use
            ReadLineEditor readLineInterface = new ReadLineEditor("", "> ");
            // This is an alternative editor to choose from, but
            // currently it is in beta-state.
            //ViLineEditor readLineInterface = new ViLineEditor("> ");

            // We can also set history items that we might have
            // brought with us from a previous session.
            //List<string> autoCompletionElements = new List<string>() { "test", "singleton" };
            //readLineInterface.SetAutoCompletionStrings(autoCompletionElements);

            string result = "-not-empty-to-start-with-";
            while (!string.IsNullOrEmpty(result))
            {
                // Read text from the user, this is what usually
                // Console.ReadLine() does.
                result = readLineInterface.ReadLine();
                // Show the output.
                SystemConsole.WriteLine();
                SystemConsole.WriteLine($"result: '{result}'");
            }
        }

    }
}
