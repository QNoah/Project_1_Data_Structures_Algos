public interface IDoublyLinkedList<T> where T : IComparable<T>
{
    void Clear();
    DoubleNode<T>? Search(T value);
    void AddFirst(T value);
    void AddLast(T value); 
    void AddSorted(T value);
    bool Remove(T value);
}