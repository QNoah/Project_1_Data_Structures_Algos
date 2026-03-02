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
    // public void AddTask(string description)
    // {
    //     int newId = _tasks.Count > 0 ? _tasks[_tasks.Count - 1].Id + 1 : 1;
    //     var newTask = new TaskItem
    //     {
    //         Id = newId,
    //         Description =
    //    description,
    //         Completed = false
    //     };
    //     _tasks.Add(newTask);
    //     _repository.SaveTasks(_tasks);
    // }
    // public void RemoveTask(int id)
    // {
    //     var task = _tasks.Find(t => t.Id == id);
    //     if (task != null)
    //     {
    //         _tasks.Remove(task);
    //         _repository.SaveTasks(_tasks);
    //     }
    // }
    public void ToggleTaskCompletion(int id)
    {
        var task = _tasks.Filter(t => t.Id == id);
        if (task != null)
        {
            _tasks.FindBy(id, (t, key) => t.Id == key);
            task.Completed = !task.Completed;
            _repository.SaveTasks(_tasks);
        }
    }
}