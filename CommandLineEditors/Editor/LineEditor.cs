﻿using CommandLineEditors.Console;
using System;

using SystemConsole = System.Console;

namespace CommandLineEditors.Editor
{
    /// <summary>
    /// A generic read-key loop which invokes the key-handlers
    /// starting with direct bindings, and if none was found or
    /// the one found did not consume the key-input, the default
    /// handler is invoked.
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    internal sealed class LineEditor<TContext>// : IConsoleEditor
        where TContext : IEditorContext
    {

        public ConsoleKeyHandlerMap<TContext> KeyHandlerMap { get; set; }

        public LineEditor(TContext context, ConsoleKeyHandlerMap<TContext> keyHandlerMap, string text = "", string prefix = "")
        {
            _context = context;
            _prefix = prefix;
            KeyHandlerMap = keyHandlerMap;
        }

        public void Close()
        {
            _context.ConsoleEditorLine.Close();
        }

        public string ReadLine()
        {
            ConsoleKeyHandlerResult result = ConsoleKeyHandlerResult.NotConsumed;
            bool stopReading = false;
            while (!stopReading)
            {
                ConsoleKeyInfo keyInfo = ConsoleLayer.ReadKey();

                result = ConsoleKeyHandlerResult.NotConsumed;
                if (KeyHandlerMap.TryGetKeyHandler(keyInfo, out Func<ConsoleKeyInfo, TContext, ConsoleKeyHandlerResult>? handler))
                {
                    result = handler(keyInfo, _context);
                }

                if (result == ConsoleKeyHandlerResult.Consumed)
                {
                    continue;
                }
                if (result != ConsoleKeyHandlerResult.NotConsumed)
                {
                    stopReading = true;
                    continue;
                }

                if (KeyHandlerMap.DefaultKeyHandler != null)
                {
                    result = KeyHandlerMap.DefaultKeyHandler(keyInfo, _context);
                }

                if (result == ConsoleKeyHandlerResult.Consumed)
                {
                    continue;
                }
                if (result != ConsoleKeyHandlerResult.NotConsumed)
                {
                    stopReading = true;
                    continue;
                }
            }

            if (result == ConsoleKeyHandlerResult.Finished && _context.Result != null)
            {
                return _context.Result;
            }
            else
            {
                return string.Empty;
            }
        }


        private readonly TContext _context;
        private readonly string _prefix;


    }
}
