using System.ComponentModel.Design;
using System.Configuration.Assemblies;
using System.Runtime.InteropServices.Marshalling;

public static class DisplayView
{
    public static void DisplayTasksList(MyCollection<TaskItem> tasks)
    {
        Console.Clear();
        foreach (TaskItem t in tasks) Console.WriteLine($"{t.Id}. {t.Title}");
    }
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

        if (task.ParentId != null)
        {
            TaskItem parent = service.GetTaskById(task.ParentId.Value);
            if (parent != null)
            {
                Console.WriteLine("======= Parent Details =======");
                Console.WriteLine($"ID          : {parent.Id}");
                Console.WriteLine($"Title       : {parent.Title}");
            }
        }

        Console.WriteLine("======= Task Details =======");
        Console.WriteLine($"ID          : {task.Id}");
        Console.WriteLine($"Title       : {task.Title}");
        Console.WriteLine($"Description : {task.Description}");
        Console.WriteLine($"Priority    : {task.Priority}");
        Console.WriteLine($"Status      : {task.Status}");
        Console.WriteLine($"Created At  : {task.CreatedAt}");

        ViewTaskChilds(taskId, service);

        Console.ReadKey();
    }
    public static void ViewTaskChilds(int parentId, ITaskService service)
    {
        MyCollection<TaskItem> childs = service.ApplyFilter(t => t.ParentId == parentId);

        if (childs is null || childs.Count <= 0) return;
        Console.WriteLine();
        Console.WriteLine("======= Child Details =======");

        foreach (TaskItem c in childs)
        {
            Console.WriteLine($"ID          : {c.Id}");
            Console.WriteLine($"Title       : {c.Title}");
        }
    }

    public static void ViewUser(int userId, IUserService service)
    {
        Console.Clear();
        User user = service.GetUserById(userId);

        if (user == null)
        {
            Console.WriteLine("User not found.");
            return;
        }

        Console.WriteLine("======= User Details =======");
        Console.WriteLine($"ID          : {user.Id}");
        Console.WriteLine($"Name        : {user.Name}");
        Console.WriteLine($"Created At  : {user.CreatedAt}");
        Console.WriteLine();
        Console.WriteLine("Press any key to go back.");
        Console.ReadKey();
    }

    public static void DisplayUsers(MyCollection<User> users)
    {
        Console.Clear();
        Console.WriteLine("======= All Users =======");
        Console.WriteLine();
        foreach(User user in users)
        {
            Console.WriteLine($"{user.Id}. {user.Name}");
        }
    }
}