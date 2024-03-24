using System;
using System.Reflection;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;

public class DatabaseController
{
    private CollaboraCalDBContext dbContext;

    private const string DbPath = @"C:\Users\Augustus\Desktop\CCDB\CollaboraCalDB.db";

    public DatabaseController()
    {
        dbContext = new CollaboraCalDBContext(DbPath);
    }

    public User? GetUserFromUsername(string username)
    {
        return dbContext.Users.Single(a => a.Username == username);
    }

    public void AddUser(User user)
    {
        dbContext.Users.Add(user);
        dbContext.SaveChanges();
    }

    public string createUser(string username, string password)
    {
        (string, bool?, bool?) userCredValidation = UserPassValid(username, password);
        if (userCredValidation.Item2 && userCredValidation.Item3)
            AddUser(new User());    //TODO: Finish User Field
        return userCredValidation.Item1;
    }

    private (string, bool?, bool?) UserPassValid(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username))
            return ("Username field is blank", false, null);
        if (string.IsNullOrWhiteSpace(password))
            return ("Password field is blank", null, false);

        bool freeUserName = true;
        foreach (User user in GetAllUsers())
        {
            if (username.Equals(user.username))
            {
                freeUserName = false;
                break;
            }
        }

        if (!freeUserName)
            return ("Username already exists", false, null);
        if (password.Length < 8 || !password.Any(char.IsUpper) || !password.Any(char.IsLower) || !password.Any(char.IsNumber))
            return ("Pasword must be at least 8 characters long with at least one uppercase letter, lowercase letter, and number", true, false);
        return ("Username and Password are Valid", true, true);
    }

    public IEnumerable<User> GetAllUsers()
    {
        return dbContext.Users;
    }

    public void DeleteEntireDataBaseNoImSeriousReallyActuallyDeleteTheEntireDataBase(bool AreYouSure)
    {
        if (!AreYouSure) return;
        dbContext.Database.EnsureDeleted();
    }


}