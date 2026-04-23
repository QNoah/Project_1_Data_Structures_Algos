public static class CollectionFactory
{
    public static IMyCollection<T> CreateCollection<T>(CollectionType type) where T : IComparable<T>
    {
        return type switch
        {
            CollectionType.HashMap => new HashMap<T>(),
            CollectionType.Array => new MyCollection<T>(),
            CollectionType.LinkedList => new LinkedListCollection<T>(),
            _ => throw new ArgumentException($"Unknown collection type: {type}")
        };
    }
}
