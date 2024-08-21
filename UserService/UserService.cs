using System.ComponentModel;

using Blazored.LocalStorage;

using BrowserDatabase;

namespace UserService;

public class UserService(ISyncLocalStorageService localStorage)
{
    private readonly Database<Guid, User> _database = new(localStorage, "User");

    public int CountAllUsers()
    {
        return _database.Count();
    }

    public User? FindUserById(Guid id)
    {
        return _database.Read(id);
    }

    public User? FindUserByUsername(string username)
    {
        return _database.ReadAll().Find((user) => user.Username == username);
    }

    public bool AddUser(User user)
    {
        return FindUserByUsername(user.Username) == null
            && _database.TryCreate(user.Id, user);
    }
}