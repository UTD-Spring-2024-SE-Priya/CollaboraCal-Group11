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
        (string, bool) userCredValidation = UserPassValid(username, password);
        if (userCredValidation.Item2)
            AddUser(new User());    //TODO: Finish User Field
        return userCredValidation.Item1;
    }

    private (string, bool) UserPassValid(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username))
            return ("Username field is blank", false);
        if (string.IsNullOrWhiteSpace(password))
            return ("Password field is blank", false);
        if (GetUserFromUsername(username) != null)
            return ("Username already exists", false);
        if (password.Length < 8 || !password.Any(char.IsUpper) || !password.Any(char.IsLower) || !password.Any(char.IsNumber))
            return ("Password must be at least 8 characters long with at least one uppercase letter, lowercase letter, and number", false);
        return ("Username and Password are Valid", true);
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