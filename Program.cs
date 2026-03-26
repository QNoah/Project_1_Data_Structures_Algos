class Program
{
    static void Main(string[] args)
    {
        // Dependency injection: wiring up our components
        string tfilePath = "tasks.json";
        string ufilePath = "users.json";
        ITaskRepository trepository = new JsonTaskRepository(tfilePath);
        IUserRepository urepository = new JsonUserRepository(ufilePath);
        ITaskService tservice = new TaskService(trepository);
        IUserService uservice = new UserService(urepository);
        ITaskView view = new ConsoleTaskView(tservice, uservice);
        // Run the view
        view.Run();
    }
}