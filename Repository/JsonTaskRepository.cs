using System.Text.Json;

class JsonTaskRepository : ITaskRepository
{
    private readonly string _filePath;
    private readonly CollectionType _collectionType;

    public JsonTaskRepository(string filePath, CollectionType collectionType = CollectionType.Array)
    {
        _filePath = filePath;
        _collectionType = collectionType;
    }

    public IMyCollection<TaskItem> LoadTasks()
    {
        var collection = CollectionFactory.CreateCollection<TaskItem>(_collectionType);

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

    public void SaveTasks(IMyCollection<TaskItem> tasks)
    {
        // Convert to array for JSON serialization
        var itemArray = new TaskItem[tasks.Count];
        int index = 0;
        var iterator = tasks.GetIterator();
        while (iterator.HasNext())
        {
            itemArray[index++] = iterator.Next();
        }

        string json = JsonSerializer.Serialize(itemArray, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        File.WriteAllText(_filePath, json);
    }
}