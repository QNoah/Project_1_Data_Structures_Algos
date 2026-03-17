public static class FilterTasksView
{
    private static string Prompt(string prompt)
    {
        Console.Write(prompt);
        return Console.ReadLine()!;
    }

    public static MyCollection<TaskItem>? AskFilter(ITaskService service)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Options:");
            Console.WriteLine("1. Priority");
            Console.WriteLine("2. Status");
            Console.WriteLine("3. Creation Date");
            Console.WriteLine("4. Show all tasks");

            string option = Prompt("Select the property you want to filter on: ");

            Func<TaskItem, bool> predicate;

            switch (option)
            {
                case "1":
                    predicate = AskPriorityFilter();
                    return service.ApplyFilter(predicate);

                case "2":
                    predicate = AskStatusFilter();
                    return service.ApplyFilter(predicate);

                case "3":
                    predicate = AskDateFilter();
                    return service.ApplyFilter(predicate);

                case "4":
                    return null;

                default:
                    Console.WriteLine("Invalid option. Press any key to continue...");
                    Console.ReadKey();
                    break;
            }
        }
    }

    public static Func<TaskItem, bool> AskPriorityFilter()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Options:");
            Console.WriteLine("1. P0");
            Console.WriteLine("2. P1");
            Console.WriteLine("3. P2");

            string input = Prompt("Select priority: ");

            switch (input)
            {
                case "1":
                    return task => task.Priority == TaskItem.TaskPriority.P0;
                case "2":
                    return task => task.Priority == TaskItem.TaskPriority.P1;
                case "3":
                    return task => task.Priority == TaskItem.TaskPriority.P2;
                default:
                    Console.WriteLine("Invalid option. Press any key to continue...");
                    Console.ReadKey();
                    break;
            }
        }
    }

    public static Func<TaskItem, bool> AskStatusFilter()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Options:");
            Console.WriteLine("1. To Do");
            Console.WriteLine("2. In Progress");
            Console.WriteLine("3. Done");

            string input = Prompt("Select status: ");

            switch (input)
            {
                case "1":
                    return task => task.Status == TaskItem.TaskStatus.ToDo;
                case "2":
                    return task => task.Status == TaskItem.TaskStatus.InProgress;
                case "3":
                    return task => task.Status == TaskItem.TaskStatus.Done;
                default:
                    Console.WriteLine("Invalid option. Press any key to continue...");
                    Console.ReadKey();
                    break;
            }
        }
    }

    public static Func<TaskItem, bool> AskDateFilter()
    {
        while (true)
        {
            Console.Clear();
            string input = Prompt("Enter date (yyyy-mm-dd): ");

            DateTime date;
            if (DateTime.TryParse(input, out date))
            {
                return task => task.CreatedAt.Date == date;
            }

            Console.WriteLine("Invalid option. Press any key to continue...");
            Console.ReadKey();
        }
    }
}