using System.ComponentModel.Design;
using System.Configuration.Assemblies;

public static class DisplayTaskView
{
    public static void DisplayTasks(MyCollection<TaskItem> tasks)
    {
        Console.Clear();
        MyCollection<TaskItem> todo = new MyCollection<TaskItem>();
        MyCollection<TaskItem> inProgress = new MyCollection<TaskItem>();
        MyCollection<TaskItem> done = new MyCollection<TaskItem>();

        foreach (var task in tasks)
        {
            if (task.Status == TaskItem.TaskStatus.ToDo)
            {
                todo.Add(task);
            }
            else if (task.Status == TaskItem.TaskStatus.InProgress)
            {
                inProgress.Add(task);
            }
            else if (task.Status == TaskItem.TaskStatus.Done)
            {
                done.Add(task);
            }
        }

        var todoIterator = todo.GetIterator();
        var progressIterator = inProgress.GetIterator();
        var doneIterator = done.GetIterator();

        Console.WriteLine(
            Pad("To Do", 30) +
            Pad("In Progress", 30) +
            Pad("Done", 30)
        );

        Console.WriteLine(new string('-', 90));

        while (todoIterator.HasNext() || progressIterator.HasNext() || doneIterator.HasNext())
        {
            string todoText = "";
            string progressText = "";
            string doneText = "";

            if (todoIterator.HasNext())
            {
                todoText = FormatTask(todoIterator.Next());
            }

            if (progressIterator.HasNext())
            {
                progressText = FormatTask(progressIterator.Next());
            }

            if (doneIterator.HasNext())
            {
                doneText = FormatTask(doneIterator.Next());
            }

            Console.WriteLine(
                Pad(todoText, 30) +
                Pad(progressText, 30) +
                Pad(doneText, 30)
            );
        }
    }

    private static string FormatTask(TaskItem task)
    {
        return $"{task.Id}. {task.Title} ({task.Priority})";
    }

    private static string Pad(string text, int width)
    {
        if (text == null) text = "";

        if (text.Length > width - 1)
        {
            text = text.Substring(0, width - 4) + "...";
        }

        return text.PadRight(width);
    }

    public static void ViewTask(int taskId, ITaskService service)
    {
        Console.Clear();
        TaskItem task = service.GetTaskById(taskId);

        if (task == null)
        {
            Console.WriteLine("Task not found.");
            return;
        }

        Console.WriteLine("1. Back");
        Console.WriteLine("2. Create subtask");
        Console.WriteLine("======= Task Details =======");
        Console.WriteLine($"ID          : {task.Id}");
        Console.WriteLine($"Title       : {task.Title}");
        Console.WriteLine($"Description : {task.Description}");
        Console.WriteLine($"Priority    : {task.Priority}");
        Console.WriteLine($"Status      : {task.Status}");
        Console.WriteLine($"Created At  : {task.CreatedAt}");
        // string opt = Console.ReadLine();
        Console.ReadKey();
    }
}