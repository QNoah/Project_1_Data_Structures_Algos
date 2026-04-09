public class SinglyLinkedList<T> : ILinkedList<T> where T : IComparable<T>
{
    public SingleNode<T>? Head;
    private int count;

    public SinglyLinkedList(T value, SingleNode<T>? next = null)
    {
        Head = null;
        count = 0;
    }
}
