using System.Collections.Generic;

namespace CommandLineEditors.Data
{
    /// <summary>
    /// The ring-buffer is used to store cut-out text from
    /// the edited line, and being able to access the contents
    /// in a rotary fashion.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class RingBuffer<T>
    {

        public RingBuffer(int capacity = 1)
        {
            _buffer = new List<T>(capacity);
            _current = 0;
        }

        public T Current
        {
            get
            {
                return _buffer[_current];
            }
        }

        public bool IsEmpty
        {
            get
            {
                return _buffer.Count == 0;
            }
        }

        public void Add(T element)
        {
            _buffer.Add(element);
        }

        public void Remove()
        {
            _buffer.RemoveAt(_current);
            _current %= _buffer.Count;
        }

        public void Rotate()
        {
            _current = (_current + 1) % _buffer.Count;
        }


        private List<T> _buffer;
        private int _current;


    }
}
