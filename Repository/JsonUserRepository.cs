using System.Text.Json;

class JsonUserRepository : IUserRepository
{
    private readonly string _filePath;
    private readonly CollectionType _collectionType;
    public JsonUserRepository(string filePath, CollectionType collectionType) => (_filePath, _collectionType) = (filePath, collectionType);
    private IMyCollection<User> CreateCollection() => _collectionType switch
    {
        CollectionType.Array => new MyArrayCollection<User>(),
        CollectionType.LinkedList => new MyLinkedListCollection<User>(),
        _ => throw new NotSupportedException($"Unsupported collection type: {_collectionType}")
    };

    public IMyCollection<User> LoadUsers()
    {
        var collection = CreateCollection();

        if (!File.Exists(_filePath))
            return collection;

        string json = File.ReadAllText(_filePath);

        if (string.IsNullOrWhiteSpace(json))
            return collection;

        var users = JsonSerializer.Deserialize<User[]>(json);

        if (users != null)
        {
            foreach (var user in users)
            {
                collection.Add(user);
            }
        }

        return collection;
    }

    public void SaveUsers(IMyCollection<User> users)
    {
        string json = JsonSerializer.Serialize(users.Data, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        File.WriteAllText(_filePath, json);
    }
}