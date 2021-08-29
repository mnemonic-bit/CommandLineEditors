using System;
using System.Collections.Generic;

namespace CommandLineEditors.Data
{
    internal class History<TEntry>
    {

        private readonly List<TEntry> _history = new List<TEntry>();

        private int _positionInHistory = -1;

        /// <summary>
        /// Gets the number of elements currently stored in this history.
        /// </summary>
        public int Count => _history.Count;


        public History()
        {

        }

        /// <summary>
        /// Tries to move one entry up in the history.
        /// </summary>
        /// <param name="historyEntry"></param>
        /// <returns>Returns true if an older history element existed, otherwise false.</returns>
        public bool TryMoveUp(out TEntry historyEntry)
        {
            historyEntry = default(TEntry);

            if (_positionInHistory > 0)
            {
                _positionInHistory--;
                historyEntry = _history[_positionInHistory];
                return true;
            }

            return false;
        }

        public bool TryMoveDown(out TEntry historyEntry)
        {
            historyEntry = default(TEntry);

            if (_positionInHistory >= 0 && _positionInHistory < _history.Count - 1)
            {
                _positionInHistory++;
                historyEntry = _history[_positionInHistory];
                return true;
            }

            return false;
        }

        /// <summary>
        /// Appends another history entry and sets the pointer
        /// of the current history element to that last added
        /// element.
        /// </summary>
        /// <param name="historyEntry"></param>
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
        public TEntry FindLast(Predicate<TEntry> predicate)
        {
            return _history.FindLast(predicate);
        }

        /// <summary>
        /// Returns the index of the last element matching the predicate.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public int FindLastIndex(Predicate<TEntry> predicate)
        {
            return _history.FindLastIndex(predicate);
        }

        public TEntry this[int pos]
        {
            get
            {
                if (pos < 0 || pos >= _history.Count)
                {
                    return default(TEntry);
                }
                return _history[pos];
            }
        }

    }
}
