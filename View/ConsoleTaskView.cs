public class ConsoleTaskView : ITaskView
{
    private readonly ITaskService _service;
    public ConsoleTaskView(ITaskService service)
    {
        _service = service;
    }

    string Prompt(string prompt)
    {
        Console.Write(prompt);
        return Console.ReadLine();
    }

    TaskItem.TaskPriority AskPriority()
    {
        while (true)
        {
            Console.WriteLine("\nOptions:");
            Console.WriteLine("1. P0");
            Console.WriteLine("2. P1");
            Console.WriteLine("3. P2");
            string option = Prompt("Select the priority: ");
            switch (option)
            {
                case "1":
                    return TaskItem.TaskPriority.P0;
                case "2":
                    return TaskItem.TaskPriority.P1;
                case "3":
                    return TaskItem.TaskPriority.P2;
                default:
                        Console.WriteLine("Invalid option. Press any key to continue...");
                        Console.ReadKey();
                        break;
            }
        }
    }

    public void Run()
    {
        while (true)
        {
            DisplayTaskView.DisplayTasks(_service.GetAllTasks());
            Console.WriteLine("\nOptions:");
            Console.WriteLine("1. Add Task");
            Console.WriteLine("2. Remove Task");
            // Console.WriteLine("3. Toggle Task State");
            Console.WriteLine("4. Exit");
            string option = Prompt("Select an option: ");
            switch (option)
            {
                case "1":
                    string title = Prompt("Enter task title: ");
                    string description = Prompt("Enter task description: ");
                    TaskItem.TaskPriority priority = AskPriority();
                    _service.AddTask(title, description, priority);
                    break;
                case "2":
                    string removeIdStr = Prompt("Enter task id to remove: ");
                    if (int.TryParse(removeIdStr, out int removeId))
                    {
                        _service.RemoveTask(removeId);
                    }
                    break;
                // case "3":
                //     string toggleIdStr = Prompt("Enter task id to toggle: ");
                //     if (int.TryParse(toggleIdStr, out int toggleId))
                //     {
                //         _service.ToggleTaskCompletion(toggleId);
                //     }
                //     break;
                case "4":
                    return;
                default:
                    Console.WriteLine("Invalid option. Press any key to continue...");
                    Console.ReadKey();
                    break;
            }
        }
    }
}