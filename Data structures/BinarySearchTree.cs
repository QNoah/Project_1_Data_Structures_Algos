using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json.Serialization;

public class BinarySearchTree<T> : IMyCollection<T> where T : IComparable<T>
{
    private Node _root;

    [JsonIgnore]
    public int Count { get; private set; }

    public bool Dirty { get; set; }

    private class Node
    {
        public T Value;
        public Node Left;
        public Node Right;

        public Node(T value)
        {
            Value = value;
        }
    }

    public BinarySearchTree()
    {
        _root = null;
        Count = 0;
        Dirty = false;
    }

    public void Add(T item)
    {
        if (_root == null)
        {
            _root = new Node(item);
            Count = 1;
            Dirty = true;
            return;
        }

        Node curr = _root;
        while (true)
        {
            int cmp = item.CompareTo(curr.Value);

            if (cmp == 0)
                return;

            if (cmp < 0)
            {
                if (curr.Left == null)
                {
                    curr.Left = new Node(item);
                    Count++;
                    Dirty = true;
                    return;
                }
                curr = curr.Left;
            }
            else
            {
                if (curr.Right == null)
                {
                    curr.Right = new Node(item);
                    Count++;
                    Dirty = true;
                    return;
                }
                curr = curr.Right;
            }
        }
    }

    public void Remove(T item)
    {
        bool removed;
        _root = RemoveNode(_root, item, out removed);
        if (removed)
        {
            Count--;
            Dirty = true;
        }
    }

    private Node RemoveNode(Node node, T item, out bool removed)
    {
        removed = false;
        if (node == null) return null;

        int cmp = item.CompareTo(node.Value);

        if (cmp < 0)
        {
            node.Left = RemoveNode(node.Left, item, out removed);
            return node;
        }

        if (cmp > 0)
        {
            node.Right = RemoveNode(node.Right, item, out removed);
            return node;
        }

        removed = true;

        if (node.Left == null && node.Right == null) return null;

        if (node.Left == null) return node.Right;
        if (node.Right == null) return node.Left;

        Node successorParent = node;
        Node successor = node.Right;
        while (successor.Left != null)
        {
            successorParent = successor;
            successor = successor.Left;
        }

        node.Value = successor.Value;

        if (successorParent == node)
            successorParent.Right = successor.Right;
        else
            successorParent.Left = successor.Right;

        return node;
    }

    public T FindBy<K>(K key, Func<T, K, bool> comparer)
    {
        foreach (var item in this)
            if (comparer(item, key)) return item;

        return default;
    }

    public MyCollection<T> Filter(Func<T, bool> predicate)
    {
        var result = new MyCollection<T>();
        foreach (var item in this)
            if (predicate(item)) result.Add(item);
        return result;
    }

    public void Sort(Comparison<T> comparison)
    {
        var arr = ToArray();
        Array.Sort(arr, comparison);

        _root = null;
        Count = 0;
        foreach (var item in arr) Add(item);

        Dirty = true;
    }

    public R Reduce<R>(Func<R, T, R> accumulator)
        => Reduce(default, accumulator);

    public R Reduce<R>(R initial, Func<R, T, R> accumulator)
    {
        R result = initial;
        foreach (var item in this)
            result = accumulator(result, item);
        return result;
    }

    public IMyIterator<T> GetIterator()
    {
        return new MyIterator<T>(ToArray(), Count);
    }

    public IEnumerator<T> GetEnumerator()
    {
        foreach (var v in InOrder(_root))
            yield return v;
    }
    private IEnumerable<T> InOrder(Node node)
    {
        if (node == null) yield break;

        foreach (var v in InOrder(node.Left))
            yield return v;

        yield return node.Value;

        foreach (var v in InOrder(node.Right))
            yield return v;
    }

    private T[] ToArray()
    {
        var result = new T[Count];
        int idx = 0;

        foreach (var item in this)
            result[idx++] = item;

        return result;
    }
}