public interface ITaskService
{
    MyCollection<TaskItem> GetAllTasks();
    void AddTask(string title, string description, TaskItem.TaskPriority priority);
    void RemoveTask(int id);
    // void ToggleTaskCompletion(int id);
}
