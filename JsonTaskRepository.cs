using System.Text.Json;

class JsonTaskRepository : ITaskRepository
{
    private readonly string _filePath;
    public JsonTaskRepository(string filePath) => _filePath = filePath;
    public MyCollection<TaskItem> LoadTasks()
    {
        if (!File.Exists(_filePath))
        {
            return new MyCollection<TaskItem>();
        }
        string json = File.ReadAllText(_filePath);
        var tasks = JsonSerializer.Deserialize<MyCollection<TaskItem>>(json);
        return tasks ?? new MyCollection<TaskItem>();
    }
    public void SaveTasks(MyCollection<TaskItem> tasks)
    {
        string json = JsonSerializer.Serialize(tasks, new
       JsonSerializerOptions
        { WriteIndented = true });
        File.WriteAllText(_filePath, json);
    }
}