public interface ITaskRepository
{
    MyCollection<TaskItem> LoadTasks();
    void SaveTasks(MyCollection<TaskItem> tasks);
}