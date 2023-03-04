namespace RemoteControlBot
{
    public class LimitedSizeList<T>
    {
        public readonly int Buffer;

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
    }
}
