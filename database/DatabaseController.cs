using System;
using System.Data.Entity;
using System.Reflection;
using System.Security.Cryptography;

public class DatabaseController
{
    private CollaboraCalDBContext dbContext;

    public DatabaseController()
    {
        dbContext = new CollaboraCalDBContext();
    }

    public void AddUser(User user)
    {
        dbContext.Users.Add(user);
        dbContext.SaveChanges();
    }

    public IEnumerable<User> GetAllUsers()
    {
        return dbContext.Users;
    }
}