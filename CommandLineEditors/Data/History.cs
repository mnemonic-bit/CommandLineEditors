using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace CommandLineEditors.Data
{
    internal class History<TEntry>
    {

        /// <summary>
        /// Gets the number of elements currently stored in this history.
        /// </summary>
        public int Count => _history.Count;

        public TEntry? this[int pos]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (pos < 0 || pos >= _history.Count)
                {
                    return default;
                }
                return _history[pos];
            }
        }

        public History()
        {
        }

        /// <summary>
        /// Appends another history entry and sets the pointer
        /// of the current history element to that last added
        /// element.
        /// </summary>
        /// <param name="historyEntry"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AppendEntry(TEntry historyEntry)
        {
            _history.Add(historyEntry);
            _positionInHistory = _history.Count - 1;
        }

        /// <summary>
        /// Returns the last entry of the history which matches the predicate.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TEntry? FindLast(Predicate<TEntry> predicate)
        {
            return _history.FindLast(predicate);
        }

        /// <summary>
        /// Returns the index of the last element matching the predicate.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int FindLastIndex(Predicate<TEntry> predicate)
        {
            return _history.FindLastIndex(predicate);
        }

        /// <summary>
        /// Tries to move one entry up in the history.
        /// </summary>
        /// <param name="historyEntry"></param>
        /// <returns>Returns true if an older history element existed, otherwise false.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryMoveUp(out TEntry? historyEntry)
        {
            historyEntry = default;

            if (_positionInHistory > 0)
            {
                _positionInHistory--;
                historyEntry = _history[_positionInHistory];
                return true;
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryMoveDown(out TEntry? historyEntry)
        {
            historyEntry = default;

            if (_positionInHistory >= 0 && _positionInHistory < _history.Count - 1)
            {
                _positionInHistory++;
                historyEntry = _history[_positionInHistory];
                return true;
            }

            return false;
        }


        private readonly List<TEntry> _history = new List<TEntry>();
        private int _positionInHistory = -1;


    }
}
