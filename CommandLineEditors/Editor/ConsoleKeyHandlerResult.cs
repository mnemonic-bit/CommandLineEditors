namespace CommandLineEditors.Editor
{
    /// <summary>
    /// This enum contains all possible values that a console-key handler
    /// can return.
    /// </summary>
    internal enum ConsoleKeyHandlerResult
    {
        NotConsumed = 0,
        Consumed = 1,
        Finished = 2,
        Aborted = 3
    }
}
