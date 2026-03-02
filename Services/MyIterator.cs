
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
        _currentIndex++;
        return _items[_currentIndex];
    }
    public void Reset()
    {
        _currentIndex = 0;
    }
}