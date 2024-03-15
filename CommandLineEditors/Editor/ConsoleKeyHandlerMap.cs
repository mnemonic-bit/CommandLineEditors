using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace CommandLineEditors.Editor
{
    internal sealed class ConsoleKeyHandlerMap<TContext>
        where TContext : IEditorContext
    {

        public Func<ConsoleKeyInfo, TContext, ConsoleKeyHandlerResult> DefaultKeyHandler { get; set; }

        public ConsoleKeyHandlerMap()
        {
            _keyHandlers = new Dictionary<ConsoleKeyInfo, Func<ConsoleKeyInfo, TContext, ConsoleKeyHandlerResult>>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddKeyHandler(ConsoleKeyInfo keyInfo, Func<ConsoleKeyInfo, TContext, ConsoleKeyHandlerResult> handler)
        {
            _keyHandlers.Add(keyInfo, handler);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddKeyHandler(ConsoleKey key, Func<ConsoleKeyInfo, TContext, ConsoleKeyHandlerResult> handler)
        {
            AddKeyHandler(ConsoleKeyInfo(key), handler);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ContainsHandlerForKey(ConsoleKeyInfo keyInfo)
        {
            return _keyHandlers.ContainsKey(keyInfo);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ContainsHandlerForKey(ConsoleKey key)
        {
            return ContainsHandlerForKey(ConsoleKeyInfo(key));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool RemoveKeyHandler(ConsoleKeyInfo keyInfo)
        {
            return _keyHandlers.Remove(keyInfo);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool RemoveKeyHandler(ConsoleKey key)
        {
            return RemoveKeyHandler(ConsoleKeyInfo(key));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetKeyHandler(ConsoleKeyInfo keyInfo, out Func<ConsoleKeyInfo, TContext, ConsoleKeyHandlerResult> handler)
        {
            return _keyHandlers.TryGetValue(keyInfo, out handler);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetKeyHandler(ConsoleKey key, out Func<ConsoleKeyInfo, TContext, ConsoleKeyHandlerResult> handler)
        {
            return TryGetKeyHandler(ConsoleKeyInfo(key), out handler);
        }


        private readonly Dictionary<ConsoleKeyInfo, Func<ConsoleKeyInfo, TContext, ConsoleKeyHandlerResult>> _keyHandlers;


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ConsoleKeyInfo ConsoleKeyInfo(ConsoleKey key)
        {
            return new ConsoleKeyInfo('\0', key, false, false, false);
        }

    }
}
