using System.Text.Json;

class JsonTaskRepository : ITaskRepository
{
    private readonly string _filePath;
    public JsonTaskRepository(string filePath) => _filePath = filePath;

    public MyCollection<TaskItem> LoadTasks()
    {
        var collection = new MyCollection<TaskItem>();

        if (!File.Exists(_filePath))
            return collection;

        string json = File.ReadAllText(_filePath);

        if (string.IsNullOrWhiteSpace(json))
            return collection;

        var items = JsonSerializer.Deserialize<TaskItem[]>(json);

        if (items != null)
        {
            foreach (var item in items)
            {
                collection.Add(item);
            }
        }

        return collection;
    }

    public void SaveTasks(MyCollection<TaskItem> tasks)
    {
        string json = JsonSerializer.Serialize(tasks.Data, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        File.WriteAllText(_filePath, json);
    }
}