public class DoublyLinkedList<T> : IDoublyLinkedList<T> where T : IComparable<T>
{
    public DoubleNode<T>? First, Last;

    public DoublyLinkedList() => First = Last = null;

    public DoubleNode<T>? Search(T value)
    {
        if(First == null && Last == null) return null;

        var currentNode = First;
        while(currentNode != null)
        {
            if(currentNode.Value.CompareTo(value)==0) 
                return currentNode;
            
            currentNode = currentNode.Next;
        }
        return currentNode;
    }

    public void AddFirst(T value)
    {
        var newNode = new DoubleNode<T>(value, First, null);
        if (First == null && Last == null)
        {
            First = newNode;
            Last = First;
        }
        else if(First != null && Last != null){
            First.Previous = newNode;
            First = newNode;
        }
    }

    public void AddLast(T value)
    {
        var newNode = new DoubleNode<T>(value, null, Last);
        if(First == null && Last == null)
        {
            First = newNode;
            Last = First;
        }
        else
        {
            Last.Next = newNode;
            Last = newNode;
        }
    }

    public void AddSorted(T value)
    {
        var newNode = new DoubleNode<T>(value);

        if(First == null && Last == null)
        {
            First = newNode;
            Last = First;
            return;
        }

        else if(First != null && Last != null)
        {
            var currentNode = First;
            
            if(currentNode.Value.CompareTo(value) >= 0)
            {
                newNode.Next = First;
                First.Previous = newNode;
                First = newNode;
                return;
            }

            while(currentNode.Next != null && 
                  currentNode.Next.Value.CompareTo(value) < 0)
            {
                currentNode = currentNode.Next;
            }
            
            if(currentNode == Last)
            {
                newNode.Previous = Last;
                Last.Next = newNode;
                Last = newNode;
                return;
            }

            newNode.Previous = currentNode;
            newNode.Next = currentNode.Next;
            currentNode.Next.Previous = newNode;
            currentNode.Next = newNode;
        }
    }

    public bool Remove(T value)
    {
        if(First == null && Last == null) return false;
        DoubleNode<T>? foundNode = Search(value);
        if(foundNode != null  && foundNode.Value.CompareTo(value) == 0)
        {
            if(First != null && Last != null && foundNode != null)
            {
                if(foundNode == First)
                {
                    if(First == Last)
                    {
                        First = foundNode.Next;
                        Last = First;
                        return true;
                    }
                    else
                    {
                        First = foundNode.Next;
                        First!.Previous = foundNode.Previous;
                        return true;    
                    }

                }

                if(foundNode == Last)
                {
                    Last.Previous!.Next = foundNode.Next;
                    Last = foundNode.Previous;
                    return true;
                }

                foundNode.Previous!.Next = foundNode.Next;
                foundNode.Next!.Previous = foundNode.Previous;
            }
            return true;
        }
        return false;
    }

    public void Clear() => First = Last = null;
}