using System.Text.Json.Serialization;

public class HashMap<T> : IMyCollection<T> where T : IComparable<T>
{
    private LinkedList<T>[] _buckets;
    private const double LoadFactorThreshold = 0.75;

    [JsonIgnore]
    public int Count { get; private set; }

    public bool Dirty { get; set; }

    public HashMap(int capacity = 16)
    {
        if (capacity < 1) capacity = 16;
        _buckets = new LinkedList<T>[capacity];
        Count = 0;
        Dirty = false;
    }

    public void Add(T item)
    {
        EnsureCapacity();

        int index = GetBucketIndex(item, _buckets.Length);
        _buckets[index] ??= new LinkedList<T>();

        if (_buckets[index].Contains(item)) return;

        _buckets[index].AddLast(item);
        Count++;
        Dirty = true;
    }

    public void Remove(T item)
    {
        int index = GetBucketIndex(item, _buckets.Length);
        var bucket = _buckets[index];
        if (bucket == null) return;

        if (bucket.Remove(item))
        {
            Count--;
            Dirty = true;
        }
    }

    public T FindBy<K>(K key, Func<T, K, bool> comparer)
    {
        foreach (var item in this)
        {
            if (comparer(item, key)) return item;
        }
        return default;
    }

    public MyCollection<T> Filter(Func<T, bool> predicate)
    {
        var result = new MyCollection<T>();
        foreach (var item in this)
        {
            if (predicate(item)) result.Add(item);
        }
        return result;
    }

    public void Sort(Comparison<T> comparison)
    {
        var arr = ToArray();
        Array.Sort(arr, comparison);

        _buckets = new LinkedList<T>[Math.Max(16, _buckets.Length)];
        Count = 0;
        foreach (var item in arr) Add(item);
        Dirty = true;
    }

    public R Reduce<R>(Func<R, T, R> accumulator)
    {
        return Reduce(default, accumulator);
    }

    public R Reduce<R>(R initial, Func<R, T, R> accumulator)
    {
        R result = initial;
        foreach (var item in this)
        {
            result = accumulator(result, item);
        }
        return result;
    }

    public IMyIterator<T> GetIterator()
    {
        return new MyIterator<T>(ToArray(), Count);
    }

    public IEnumerator<T> GetEnumerator()
    {
        for (int i = 0; i < _buckets.Length; i++)
        {
            var bucket = _buckets[i];
            if (bucket == null) continue;

            foreach (var item in bucket)
                yield return item;
        }
    }

    private T[] ToArray()
    {
        var result = new T[Count];
        int idx = 0;

        foreach (var item in this)
            result[idx++] = item;

        return result;
    }

    private int GetBucketIndex(T item, int bucketCount)
    {
        int hash = item?.GetHashCode() ?? 0;
        return (hash & 0x7fffffff) % bucketCount;
    }

    private void EnsureCapacity()
    {
        if ((double)(Count + 1) / _buckets.Length <= LoadFactorThreshold) return;

        var old = _buckets;
        _buckets = new LinkedList<T>[old.Length * 2];
        int oldCount = Count;
        Count = 0;

        foreach (var bucket in old)
        {
            if (bucket == null) continue;
            foreach (var item in bucket)
            {
                int idx = GetBucketIndex(item, _buckets.Length);
                _buckets[idx] ??= new LinkedList<T>();
                _buckets[idx].AddLast(item);
                Count++;
            }
        }

        if (Count != oldCount) throw new InvalidOperationException("Rehash failed.");
    }
}