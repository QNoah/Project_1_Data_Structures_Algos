public static class DisplayTaskView
{
    public static void DisplayTasks(MyCollection<TaskItem> tasks)
    {
        Console.Clear();
        Console.WriteLine("========= ToDo List =========");
        if (tasks.Count.Equals(0)) Console.WriteLine("No tasks.");
        foreach (var task in tasks)
        {
            Console.Write($"[{task.Status}] {task.Id}. {task.Title} - {task.Priority}\n");
        }
    }
}