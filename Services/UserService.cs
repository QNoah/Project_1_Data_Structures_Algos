using System.Reflection;
using System.Reflection.Metadata.Ecma335;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;
    private readonly MyCollection<User> _users;

    public UserService(IUserRepository repository)
    {
        _repository = repository;
        _users = _repository.LoadUsers();
    }

    public MyCollection<User> GetAllUsers() => _users;

    public User GetUserById(int id) => _users.FindBy(id, (t, key) => t.Id == key);

    public void AddUser(string name)
    {
        int maxId = 0;
        var iterator = _users.GetIterator();
        while (iterator.HasNext())
        {
            var task = iterator.Next();
            if(task.Id > maxId) maxId = task.Id;
        }

        var newTask = new User
        {
            Id = maxId + 1,
            Name = name,
            CreatedAt = DateTime.Now
        };

        _users.Add(newTask);
        _repository.SaveUsers(_users);
    }

    public void RemoveUser(int id)
    {
        var task = _users.FindBy(id, (t, key) => t.Id == key);
        if (task is not null)
        {
            _users.Remove(task);
            _repository.SaveUsers(_users);
        }
    }
}