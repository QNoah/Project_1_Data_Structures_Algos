public interface IUserRepository
{
    MyCollection<User> LoadUsers();
    void SaveUsers(MyCollection<User> users);
}