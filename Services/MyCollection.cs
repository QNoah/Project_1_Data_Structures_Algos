public class MyCollection<T> : IMyCollection<T>
{
    private T[] _items { get; set; }
    public int Count { get; set; }
    public bool Dirty { get; set; }

    public void Add(T item)
    {
        if (_items == null)
        {
            _items = new T[1];
            Count = 0;
        }
        if (Count >= _items.Length) Array.Copy(_items, _items = new T[_items.Length + 1], Count);
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
        for (int i = 0; i < Count; i++)
        {
            if (comparer(_items[i], key)) return _items[i];
        }
        return default;
    }
    public IMyCollection<T> Filter(Func<T, bool> predicate)
    {
        MyCollection<T> filtered = new MyCollection<T>();
        for (int i = 0; i < Count; i++)
        {
            if (predicate(_items[i])) filtered.Add(_items[i]);
        }
        return filtered;
    }
    public void Sort(Comparison<T> comparison)
    {
        for (int i = 1; i < Count; i++)
        {
            for (int j = 0; j < Count - 1; j++)
            {
                var tempOne = _items[j];
                var tempTwo = _items[j + 1];
                if (comparison(_items[j], _items[j + 1]) > 0)
                {
                    _items[j] = tempTwo;
                    _items[j + 1] = tempOne;
                }
            }
        }
    }
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
        return new MyIterator<T>(_items, Count);
    }
    public IEnumerator<T> GetEnumerator()
    {
        for(int i = 0; i < Count; i++) yield return _items[i];
    } // Extra foreach lookup.
}