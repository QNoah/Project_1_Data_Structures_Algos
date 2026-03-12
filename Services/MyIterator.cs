
public class MyIterator<T> : IMyIterator<T>
{
    private readonly T[] _items;
    private readonly int _count;
    private int _currentIndex;
    public MyIterator(T[] items, int count)
    {
        _items = items;
        _count = count;
        _currentIndex = 0;
    }

    public bool HasNext()
    {
        return _currentIndex < _count;
    }

    public T Next()
    {
        T item = _items[_currentIndex];
        _currentIndex++;
        return item;
    }

    public void Reset()
    {
        _currentIndex = 0;
    }
}