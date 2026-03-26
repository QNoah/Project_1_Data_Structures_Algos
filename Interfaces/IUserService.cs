public interface IUserService
{
    MyCollection<User> GetAllUsers();
    void AddUser(string name);
    void RemoveUser(int id);
    User GetUserById(int id);
}
