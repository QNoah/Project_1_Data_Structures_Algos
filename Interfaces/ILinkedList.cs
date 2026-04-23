public interface ILinkedList<T> where T : IComparable<T>
{
    SingleNode<T>? Head { get; set; }
    void AddFirst(T value);
    void AddLast(T value);
    void AddSorted(T value);
    bool Remove(T value);
    SingleNode<T>? Search(T value);
    bool Contains(T value);
    void Clear();
}