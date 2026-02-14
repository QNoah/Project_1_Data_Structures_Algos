public class MyCollection<T> : IMyCollection<T>
{
    private T[] _items { get; set; }
    public int Count { get; set; }
    public bool Dirty { get; set; }

    public void Add(T item)
    {
        if (_items == null)
        {
            _items = new T[4];
            Count = 0;
        }
        if (Count >= _items.Length) Array.Copy(_items, _items = new T[_items.Length * 2], Count);
        _items[Count] = item;
        Count++;
    }
    public void Remove(T item)
    {
        for (int i = 0; i < Count; i++)
        {
            if (_items[i].Equals(item))
            {
                for (int j = i; j < Count - 1; j++) _items[j] = _items[j + 1];
                Count--;
                _items[Count] = default;
                break;
            }
        }
    }
    public T FindBy<K>(K key, Func<T, K, bool> comparer)
    {
        return default(T);
    }
    public IMyCollection<T> Filter(Func<T, bool> predicate)
    {
        return default;
    }
    public void Sort(Comparison<T> comparison) { }
    public R Reduce<R>(Func<R, T, R> accumulator)
    {
        return default;
    }
    // OR
    public R Reduce<R>(R initial, Func<R, T, R> accumulator)
    {
        return default;
    }
    public IMyIterator<T> GetIterator()
    {
        return default;
    }
    public IEnumerator<T> GetEnumerator()
    {
        return default;
    } // Extra foreach lookup.
}