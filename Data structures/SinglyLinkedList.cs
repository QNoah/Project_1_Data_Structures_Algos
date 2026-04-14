public class SinglyLinkedList<T> : ILinkedList<T> where T : IComparable<T>
{
    public SingleNode<T>? Head { get; set; }
    private int count;

    public SinglyLinkedList()
    {
        Head = null;
        count = 0;
    }

    public void AddFirst(T value)
    {
        SingleNode<T> newNode = new SingleNode<T>(value, Head);
        Head = newNode;
        count++;
    }

    public void AddLast(T value)
    {
        if(Head == null)
        {	
            Head = new SingleNode<T>(value, Head);
            count++;
            return;
        }

        SingleNode<T> lastNode = Head;

        while(lastNode.Next != null)
        {
                        lastNode = lastNode.Next;
        }

        lastNode.Next = new SingleNode<T>(value);
        count++;
    }

    public void AddSorted(T value)
    {
        var currentNode = Head;

        if(Head == null || currentNode.Value.CompareTo(value) >= 0){
            Head = new SingleNode<T>(value, Head);
            count++;
            return;
        }

        while(currentNode.Next != null && currentNode.Next.Value.CompareTo(value) < 0)
        {
            currentNode = currentNode.Next;
        }
        
        var newNode = new SingleNode<T>(value, currentNode.Next);
        currentNode.Next = newNode;
        count++;
    }

    public bool Remove(T value)
    {
        if(Head == null) return false;

        SingleNode<T> currentNode = Head;

        if(currentNode.Value.CompareTo(value) == 0)
        {
            Head = Head.Next;
            count--;
            return true;
        }

        while(currentNode.Next != null && currentNode.Next.Value.CompareTo(value) != 0)
        {
            currentNode = currentNode.Next;
        }
        
        if(currentNode.Next == null) return false;
        
        currentNode.Next = currentNode.Next.Next;
        count--;
        return true;
    }

    public SingleNode<T>? Search(T value)
    {
        if(Head == null) return null;

        SingleNode<T>? currentNode = Head;

        while(currentNode != null)
        {
            if(currentNode.Value.CompareTo(value) == 0) return currentNode;
            currentNode = currentNode.Next;
        }

        return null;
    }

    public bool Contains(T value) => Search(value) != null && Search(value)!.Value.CompareTo(value) == 0;

    public void Clear()
    {
      Head = null;
      count = 0;
    }
}
