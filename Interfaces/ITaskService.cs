public interface ITaskService
{
    MyCollection<TaskItem> GetAllTasks();
    void AddTask(string title, string description, TaskItem.TaskPriority priority);
    void RemoveTask(int id);
    MyCollection<TaskItem> ApplyFilter(Func<TaskItem, bool> predicate);
    // void ToggleTaskCompletion(int id);
}
