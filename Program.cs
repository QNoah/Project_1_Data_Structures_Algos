public enum CollectionType
{
    LinkedList,
    Array
}

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Kies datastructuur:");
        Console.WriteLine("1. Array");
        Console.WriteLine("2. LinkedList");
        Console.Write("Jouw keuze: ");
        string? choice = Console.ReadLine();

        CollectionType collectionType = choice switch
        {
            "2" => CollectionType.LinkedList,
            _ => CollectionType.Array
        };

        string tfilePath = "tasks.json";
        string ufilePath = "users.json";

        ITaskRepository trepository = new JsonTaskRepository(tfilePath);
        IUserRepository urepository = new JsonUserRepository(ufilePath, collectionType);

        IUserService uservice = new UserService(urepository);
        ITaskService tservice = new TaskService(trepository, uservice);
        ITaskView view = new ConsoleTaskView(tservice, uservice, collectionType);

        view.Run();
    }
}