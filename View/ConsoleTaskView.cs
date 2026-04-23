public class ConsoleTaskView : ITaskView
{
    private readonly ITaskService _taskservice;
    private readonly IUserService _userservice;

    public ConsoleTaskView(ITaskService tservice, IUserService uservice)
    {
        _taskservice = tservice;
        _userservice = uservice;
    }

    private IMyCollection<TaskItem> _tasks;
    private IMyCollection<User> _users;

    string Prompt(string prompt)
    {
        Console.Write(prompt);
        return Console.ReadLine()!;
    }

    int? AskParent()
    {
        while (true)
        {
            Console.Clear();
            DisplayView.DisplayTasksList(_tasks);
            Console.WriteLine("Enter an existing parent id or press ENTER: ");
            Int32.TryParse(Console.ReadLine(), out int id);

            if (_taskservice.GetTaskById(id) is not null) return id;
            return null;
        }
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

    TaskItem.TaskStatus AskStatus()
    {
        while (true)
        {
            Console.WriteLine("\nOptions:");
            Console.WriteLine("1. To Do");
            Console.WriteLine("2. In Progress");
            Console.WriteLine("3. Done");
            string option = Prompt("Select the new status: ");
            switch (option)
            {
                case "1":
                    return TaskItem.TaskStatus.ToDo;
                case "2":
                    return TaskItem.TaskStatus.InProgress;
                case "3":
                    return TaskItem.TaskStatus.Done;
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
            if (_tasks == null) _tasks = _taskservice.GetAllTasks();
            DisplayView.DisplayTasks(_tasks);
            _tasks = _taskservice.GetAllTasks();

            Console.WriteLine("\nOptions:");
            Console.WriteLine("1. Add Task");
            Console.WriteLine("2. Remove Task");
            Console.WriteLine("3. Edit Task");
            Console.WriteLine("4. Move Task");
            Console.WriteLine("5. View Task");
            Console.WriteLine("6. Filter");
            Console.WriteLine("7. User management");
            Console.WriteLine("8. Exit");

            string option = Prompt("Select an option: ");
            switch (option)
            {
                case "1":
                    {
                        string title = Prompt("Enter task title: ");
                        int? parentId = AskParent();
                        string description = Prompt("Enter task description: ");
                        TaskItem.TaskPriority priority = AskPriority();
                        _taskservice.AddTask(title, parentId, description, priority);
                        break;
                    }
                case "2":
                    {
                        string removeIdStr = Prompt("Enter task id to remove: ");
                        if (int.TryParse(removeIdStr, out int removeId))
                        {
                            _taskservice.RemoveTask(removeId);
                        }
                        break;
                    }
                case "3":
                    {
                        string editIdStr = Prompt("Enter task ID to edit: ");
                        if (!int.TryParse(editIdStr, out int editId)) break;

                        Console.WriteLine("Leave empty to keep current value.");
                        string newTitle = Prompt("New title: ");
                        string newDescription = Prompt("New description: ");

                        string prioStr = Prompt("Change priority? (y/n): ");
                        TaskItem.TaskPriority? newPriority = null;
                        if (prioStr.Trim().Equals("y", StringComparison.OrdinalIgnoreCase))
                        {
                            newPriority = AskPriority();
                        }
                        string editUserIdStr = Prompt("Enter User ID to add (leave empty = no change): ");

                        int? editUserId = null;
                        if (!string.IsNullOrWhiteSpace(editUserIdStr))
                        {
                            if (int.TryParse(editUserIdStr, out int parsedId))
                                editUserId = parsedId;
                            else
                            {
                                Console.WriteLine("Invalid user id.");
                                return;
                            }
                        }

                        bool updated = _taskservice.UpdateTask(
                            editId,
                            string.IsNullOrWhiteSpace(newTitle) ? null : newTitle,
                            string.IsNullOrWhiteSpace(newDescription) ? null : newDescription,
                            newPriority,
                            editUserId
                        );

                        Console.WriteLine(updated ? "Task updated!" : "Task not found or update failed.");
                        Thread.Sleep(1500);
                        break;
                    }
                case "4":
                    {
                        string moveIdStr = Prompt("Enter task ID to move: ");
                        TaskItem.TaskStatus newStatus = AskStatus();
                        if (int.TryParse(moveIdStr, out int moveId))
                        {
                            bool success = _taskservice.MoveTask(moveId, newStatus);

                            if (!success && newStatus == TaskItem.TaskStatus.Done)
                            {
                                Console.Clear();
                                Console.WriteLine("Wrong: You can't mark this task as 'Done'!");
                                Console.WriteLine("Reason: There are still incompleted subtaks.\n");
                                Console.WriteLine("Mark all subtasks 'Done'.");
                                Console.WriteLine("\nPress [ENTER] to go back...");
                                Console.ReadKey();
                            }
                            else if (success)
                            {
                                Console.WriteLine("Status edited!");
                                Thread.Sleep(1500);
                            }
                            else
                            {
                                Console.WriteLine("Task not found or error occurred.");
                                Console.ReadKey();
                            }
                        }
                        break;
                    }
                case "5":
                    {
                        string viewIdStr = Prompt("Enter task ID to view: ");
                        if (int.TryParse(viewIdStr, out int viewId))
                        {
                            DisplayView.ViewTask(viewId, _taskservice, _userservice);
                        }
                        break;
                    }
                case "6":
                    {
                        IMyCollection<TaskItem>? filteredTasks = FilterTasksView.AskFilter(_taskservice);
                        if (filteredTasks != null) _tasks = filteredTasks;
                        break;
                    }
                case "7":
                    UserManagementView();
                    break;
                case "8":
                    return;
                default:
                    Console.WriteLine("Invalid option. Press any key to continue...");
                    Console.ReadKey();
                    break;
            }
        }
    }

    public void UserManagementView()
    {
        while (true)
        {
            if (_users == null) _users = _userservice.GetAllUsers();
            DisplayView.DisplayUsers(_users);
            _users = _userservice.GetAllUsers();

            Console.WriteLine("\nOptions:");
            Console.WriteLine("1. Add User");
            Console.WriteLine("2. Remove User");
            Console.WriteLine("3. View User");
            Console.WriteLine("4. Go back");
            string option = Prompt("Select an option: ");
            switch (option)
            {
                case "1":
                    string name = Prompt("Enter name: ");
                    _userservice.AddUser(name);
                    break;
                case "2":
                    string removeIdStr = Prompt("Enter user ID to remove: ");
                    if (int.TryParse(removeIdStr, out int removeId))
                    {
                        _userservice.RemoveUser(removeId);
                    }
                    break;
                case "3":
                    string viewIdStr = Prompt("Enter user ID to view: ");
                    if (int.TryParse(viewIdStr, out int viewId))
                    {
                        DisplayView.ViewUser(viewId, _userservice);
                    }
                    break;
                case "4":
                    return;
            }
        }
    }
}