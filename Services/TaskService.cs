using System.Reflection.Metadata.Ecma335;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _repository;
    private readonly MyCollection<TaskItem> _tasks;
    public TaskService(ITaskRepository repository)
    {
        _repository = repository;
        // _tasks = _repository.LoadTasks();
        _tasks = new MyCollection<TaskItem>();
    }
    public MyCollection<TaskItem> GetAllTasks() => _tasks;
    public void AddTask(string description)
    {
        int maxId = 0;
        var iterator = _tasks.GetIterator();
        while (iterator.HasNext())
        {
            var task = iterator.Next();
            if(task.Id > maxId) maxId = task.Id;
        }

        var newTask = new TaskItem
        {
            Id = maxId + 1,
            Description =
       description,
            Completed = false
        };
        _tasks.Add(newTask);
        _repository.SaveTasks(_tasks);
    }
    public void RemoveTask(int id)
    {
        var task = _tasks.FindBy(id, (t, key) => t.Id == key);
        if (task is not null)
        {
            _tasks.Remove(task);
            _repository.SaveTasks(_tasks);
        }
    }
    public void ToggleTaskCompletion(int id)
    {
        var task = _tasks.FindBy(id, (t, key) => t.Id == key);
        if (task is not null) task.Completed = !task.Completed;
        _repository.SaveTasks(_tasks);
    }
}