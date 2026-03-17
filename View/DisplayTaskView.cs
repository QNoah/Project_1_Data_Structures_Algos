public static class DisplayTaskView
{
    public static void DisplayTasks(MyCollection<TaskItem> tasks)
    {
        Console.Clear();
        Console.WriteLine("==== ToDo List ====");
        if (tasks.Count.Equals(0)) Console.WriteLine("No tasks.");
        foreach (var task in tasks)
        {
            Console.Write($"{task.Id}. {task.Description} ");
            Console.Write("[");
            if (task.Completed) Console.Write("X");
            Console.Write("]\n");
        }
    }
}