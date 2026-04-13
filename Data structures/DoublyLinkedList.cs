public class DoublyLinkedList<T> : IDoublyLinkedList<T> where T : IComparable<T>
{
    public DoubleNode<T>? First, Last;
    public DoublyLinkedList() => First = Last = null;
    public void Clear() => First = Last = null;
}