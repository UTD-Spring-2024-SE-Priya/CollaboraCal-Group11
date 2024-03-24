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