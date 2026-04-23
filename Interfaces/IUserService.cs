public interface IUserService
{
    IMyCollection<User> GetAllUsers();
    void AddUser(string name);
    void RemoveUser(int id);
    User GetUserById(int id);
}
