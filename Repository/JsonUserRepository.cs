using System.Text.Json;

class JsonUserRepository : IUserRepository
{
    private readonly string _filePath;
    public JsonUserRepository(string filePath) => _filePath = filePath;

    public MyCollection<User> LoadUsers()
    {
        var collection = new MyCollection<User>();

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

    public void SaveUsers(MyCollection<User> users)
    {
        string json = JsonSerializer.Serialize(users.Data, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        File.WriteAllText(_filePath, json);
    }
}