using System.Text.Json;

class JsonUserRepository : IUserRepository
{
    private readonly string _filePath;
    private readonly CollectionType _collectionType;

    public JsonUserRepository(string filePath, CollectionType collectionType = CollectionType.Array)
    {
        _filePath = filePath;
        _collectionType = collectionType;
    }

    public IMyCollection<User> LoadUsers()
    {
        var collection = CollectionFactory.CreateCollection<User>(_collectionType);

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
        // Convert to array for JSON serialization
        var userArray = new User[users.Count];
        int index = 0;
        var iterator = users.GetIterator();
        while (iterator.HasNext())
        {
            userArray[index++] = iterator.Next();
        }

        string json = JsonSerializer.Serialize(userArray, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        File.WriteAllText(_filePath, json);
    }
}