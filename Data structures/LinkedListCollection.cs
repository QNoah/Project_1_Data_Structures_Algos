using System.Text.Json.Serialization;

public class LinkedListCollection<T> : IMyCollection<T> where T : IComparable<T>
{
    private readonly SinglyLinkedList<T> _linkedList;

    [JsonIgnore]
    public int Count
    {
        get
        {
            int count = 0;
            var currentNode = _linkedList.Head;
            while (currentNode != null)
            {
                count++;
                currentNode = currentNode.Next;
            }
            return count;
        }
    }

    public bool Dirty { get; set; }

    public LinkedListCollection()
    {
        _linkedList = new SinglyLinkedList<T>();
    }

    public void Add(T item)
    {
        _linkedList.AddLast(item);
        Dirty = true;
    }

    public void Remove(T item)
    {
        _linkedList.Remove(item);
        Dirty = true;
    }

    public T FindBy<K>(K key, Func<T, K, bool> comparer)
    {
        var currentNode = _linkedList.Head;
        while (currentNode != null)
        {
            if (comparer(currentNode.Value, key))
                return currentNode.Value;
            currentNode = currentNode.Next;
        }
        return default;
    }

    public MyCollection<T> Filter(Func<T, bool> predicate)
    {
        var result = new MyCollection<T>();
        var currentNode = _linkedList.Head;
        while (currentNode != null)
        {
            if (predicate(currentNode.Value))
                result.Add(currentNode.Value);
            currentNode = currentNode.Next;
        }
        return result;
    }

    public void Sort(Comparison<T> comparison)
    {
        // Convert to array, sort, then rebuild linked list
        var items = ToArray();
        System.Array.Sort(items, comparison);

        _linkedList.Clear();
        foreach (var item in items)
        {
            _linkedList.AddLast(item);
        }
        Dirty = true;
    }

    public R Reduce<R>(Func<R, T, R> accumulator)
    {
        return Reduce(default, accumulator);
    }

    public R Reduce<R>(R initial, Func<R, T, R> accumulator)
    {
        R result = initial;
        var currentNode = _linkedList.Head;
        while (currentNode != null)
        {
            result = accumulator(result, currentNode.Value);
            currentNode = currentNode.Next;
        }
        return result;
    }

    public IMyIterator<T> GetIterator()
    {
        return new LinkedListIterator(this);
    }

    public IEnumerator<T> GetEnumerator()
    {
        var currentNode = _linkedList.Head;
        while (currentNode != null)
        {
            yield return currentNode.Value;
            currentNode = currentNode.Next;
        }
    }

    private T[] ToArray()
    {
        var result = new T[Count];
        int index = 0;
        var currentNode = _linkedList.Head;
        while (currentNode != null)
        {
            result[index++] = currentNode.Value;
            currentNode = currentNode.Next;
        }
        return result;
    }

    private class LinkedListIterator : IMyIterator<T>
    {
        private readonly LinkedListCollection<T> _collection;
        private SingleNode<T> _currentNode;

        public LinkedListIterator(LinkedListCollection<T> collection)
        {
            _collection = collection;
            _currentNode = collection._linkedList.Head;
        }

        public bool HasNext()
        {
            return _currentNode.Next != null;
        }

        public T Next()
        {
            if (!HasNext())
                throw new InvalidOperationException("No more elements");

            var value = _currentNode.Value;
            _currentNode = _currentNode.Next;
            return value;
        }

        public void Reset()
        {
            _currentNode = _collection._linkedList.Head;
        }
    }
}
