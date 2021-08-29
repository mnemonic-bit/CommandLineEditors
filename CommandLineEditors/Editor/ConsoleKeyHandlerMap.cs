using System;
using System.Collections.Generic;

namespace CommandLineEditors.Editor
{
    internal class ConsoleKeyHandlerMap<TContext>
        where TContext : IEditorContext
    {

        public Func<ConsoleKeyInfo, TContext, ConsoleKeyHandlerResult> DefaultKeyHandler { get; set; }

        private readonly Dictionary<ConsoleKeyInfo, Func<ConsoleKeyInfo, TContext, ConsoleKeyHandlerResult>> _keyHandlers;

        public ConsoleKeyHandlerMap()
        {
            _keyHandlers = new Dictionary<ConsoleKeyInfo, Func<ConsoleKeyInfo, TContext, ConsoleKeyHandlerResult>>();
        }

        public void AddKeyHandler(ConsoleKeyInfo keyInfo, Func<ConsoleKeyInfo, TContext, ConsoleKeyHandlerResult> handler)
        {
            _keyHandlers.Add(keyInfo, handler);
        }

        public void AddKeyHandler(ConsoleKey key, Func<ConsoleKeyInfo, TContext, ConsoleKeyHandlerResult> handler)
        {
            AddKeyHandler(ConsoleKeyInfo(key), handler);
        }

        public bool ContainsHandlerForKey(ConsoleKeyInfo keyInfo)
        {
            return _keyHandlers.ContainsKey(keyInfo);
        }

        public bool ContainsHandlerForKey(ConsoleKey key)
        {
            return ContainsHandlerForKey(ConsoleKeyInfo(key));
        }

        public bool RemoveKeyHandler(ConsoleKeyInfo keyInfo)
        {
            return _keyHandlers.Remove(keyInfo);
        }

        public bool RemoveKeyHandler(ConsoleKey key)
        {
            return RemoveKeyHandler(ConsoleKeyInfo(key));
        }

        public bool TryGetKeyHandler(ConsoleKeyInfo keyInfo, out Func<ConsoleKeyInfo, TContext, ConsoleKeyHandlerResult> handler)
        {
            return _keyHandlers.TryGetValue(keyInfo, out handler);
        }

        public bool TryGetKeyHandler(ConsoleKey key, out Func<ConsoleKeyInfo, TContext, ConsoleKeyHandlerResult> handler)
        {
            return TryGetKeyHandler(ConsoleKeyInfo(key), out handler);
        }

        private ConsoleKeyInfo ConsoleKeyInfo(ConsoleKey key)
        {
            return new ConsoleKeyInfo('\0', key, false, false, false);
        }

    }
}
