using System.Text.Json.Serialization;

public class MyCollection<T> : IMyCollection<T>
{
    [JsonIgnore]
    public T[] Items { get; set; }

    public T[] Data
    {
        get
        {
            T[] result = new T[Count];
            for (int i = 0; i < Count; i++)
            {
                result[i] = Items[i];
            }
            return result;
        }
    }

    public int Count { get; set; }
    public bool Dirty { get; set; }

    public void Add(T item)
    {
        if (Items == null)
        {
            Items = new T[1];
            Count = 0;
        }
        if (Count >= Items.Length) Array.Copy(Items, Items = new T[Items.Length + 1], Count);
        Items[Count] = item;
        Count++;
    }

    public void Remove(T item)
    {
        for (int i = 0; i < Count; i++)
        {
            if (Items[i].Equals(item))
            {
                for (int j = i; j < Count - 1; j++) Items[j] = Items[j + 1];
                Count--;
                Items[Count] = default;
                break;
            }
        }
    }

    public T FindBy<K>(K key, Func<T, K, bool> comparer)
    {
        for (int i = 0; i < Count; i++)
        {
            if (comparer(Items[i], key)) return Items[i];
        }
        return default;
    }
    public IMyCollection<T> Filter(Func<T, bool> predicate)
    {
        MyCollection<T> filtered = new MyCollection<T>();
        for (int i = 0; i < Count; i++)
        {
            if (predicate(Items[i])) filtered.Add(Items[i]);
        }
        return filtered;
    }
    public void Sort(Comparison<T> comparison)
    {
        for (int i = 1; i < Count; i++)
        {
            for (int j = 0; j < Count - 1; j++)
            {
                var tempOne = Items[j];
                var tempTwo = Items[j + 1];
                if (comparison(Items[j], Items[j + 1]) > 0)
                {
                    Items[j] = tempTwo;
                    Items[j + 1] = tempOne;
                }
            }
        }
    }
    public R Reduce<R>(Func<R, T, R> accumulator)
    {
        if (Count == 0) return default;

        R result = (R)(object)Items[0];
        for (int i = 1; i < Count; i++)
        {
            result = accumulator(result, Items[i]);
        }
        return result;
    }
    // OR
    public R Reduce<R>(R initial, Func<R, T, R> accumulator)
    {
        if (Count == 0) return initial;
        R result = initial;
        for (int i = 0; i < Count; i++)
        {
            result = accumulator(result, Items[i]);
        }
        return result;
    }
    public IMyIterator<T> GetIterator()
    {
        return new MyIterator<T>(Items, Count);
    }
    public IEnumerator<T> GetEnumerator()
    {
        for (int i = 0; i < Count; i++) yield return Items[i];
    } // Extra foreach lookup.
}