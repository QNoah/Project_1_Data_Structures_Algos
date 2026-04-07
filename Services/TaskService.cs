using System.Reflection;
using System.Reflection.Metadata.Ecma335;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _repository;
    private readonly MyCollection<TaskItem> _tasks;

    public TaskService(ITaskRepository repository)
    {
        _repository = repository;
        _tasks = _repository.LoadTasks();
    }

    public MyCollection<TaskItem> GetAllTasks() => _tasks;

    public TaskItem GetTaskById(int id) => _tasks.FindBy(id, (t, key) => t.Id == key);

    public void AddTask(string title, int? parentId, string description, TaskItem.TaskPriority priority)
    {
        int maxId = 0;
        var iterator = _tasks.GetIterator();
        while (iterator.HasNext())
        {
            var task = iterator.Next();
            if (task.Id > maxId) maxId = task.Id;
        }

        var newTask = new TaskItem
        {
            Id = maxId + 1,
            ParentId = parentId,
            Title = title,
            Description = description,
            Priority = priority,
            Status = TaskItem.TaskStatus.ToDo,
            CreatedAt = DateTime.Now
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

    public bool UpdateTask(int id, string? title, string? description, TaskItem.TaskPriority? priority)
    {
        var task = GetTaskById(id);
        if (task is null) return false;

        if (title is not null) task.Title = title;
        if (description is not null) task.Description = description;
        if (priority is not null) task.Priority = priority.Value;

        return true;
    }

    public bool MoveTask(int id, TaskItem.TaskStatus newStatus)
    {
        var task = _tasks.FindBy(id, (t, key) => t.Id == key);
        if (task is null) return false;

        if (newStatus == TaskItem.TaskStatus.Done)
        {
            if (!AreAllChildrenCompleted(id))
            {
                return false;
            }
        }

        task.Status = newStatus;
        _repository.SaveTasks(_tasks);
        return true;
    }

    private bool AreAllChildrenCompleted(int taskId)
    {
        var children = _tasks.Filter(t => t.ParentId == taskId);

        if (children is null || children.Count == 0)
            return true;

        var iterator = children.GetIterator();
        while (iterator.HasNext())
        {
            var child = iterator.Next();
            if (child.Status != TaskItem.TaskStatus.Done)
                return false;
        }

        return true;
    }

    public MyCollection<TaskItem> ApplyFilter(Func<TaskItem, bool> predicate)
    {
        return _tasks.Filter(predicate);
    }
}