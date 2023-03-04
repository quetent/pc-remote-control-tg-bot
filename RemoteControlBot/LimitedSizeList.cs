using System.Collections.Immutable;

namespace RemoteControlBot
{
    public class LimitedSizeList<T>
    {
        public readonly int Buffer;
        public int Count => _data.Count;

        private readonly LinkedList<T> _data;

        public LimitedSizeList(int buffer)
        {
            Buffer = buffer;
            _data = new();
        }

        public void Add(T item)
        {
            if (_data.Count >= Buffer)
                _data.RemoveFirst();

            _data.AddLast(item);
        }

        public ImmutableList<T> ToImmutableList()
        {
            return _data.ToImmutableList();
        }
    }
}
