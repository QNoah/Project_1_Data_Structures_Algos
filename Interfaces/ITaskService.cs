public interface ITaskService
{
    MyCollection<TaskItem> GetAllTasks();
    void AddTask(string title, int? parentId, string description, TaskItem.TaskPriority priority);
    void RemoveTask(int id);
    MyCollection<TaskItem> ApplyFilter(Func<TaskItem, bool> predicate);
    bool MoveTask(int id, TaskItem.TaskStatus newStatus);
    TaskItem GetTaskById(int id);
}
