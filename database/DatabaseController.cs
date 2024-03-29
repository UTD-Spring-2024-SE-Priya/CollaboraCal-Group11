using System;
using System.Reflection;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace CollaboraCal
{
    public sealed class DatabaseController
    {

        internal CollaboraCalDBContext Context => dbContext;

        private CollaboraCalDBContext dbContext;

        //private const string DbPath = @"sqlitecloud://admin:collabora2003@cj471jujik.sqlite.cloud:8860";

        // public void InitializeDatabase(string connectionString)
        // {
        //     dbContext = new CollaboraCalDBContext(connectionString);
        //     dbContext.Database.EnsureCreated();
        // }

        public DatabaseController()
        {
            dbContext = new CollaboraCalDBContext();
        }


        public User? GetUserFromEmail(string email)
        {
            return dbContext.Users.SingleOrDefault(a => a.EMail == email);
        }

        public bool AddUser(User user)
        {
            if (user.EMail == null) return false;
            if (GetUserFromEmail(user.EMail) != null)
                return false;
            dbContext.Users.Add(user);
            dbContext.SaveChanges();
            return true;
        }

        public IEnumerable<User> GetAllUsers()
        {
            return dbContext.Users;
        }

        public void DeleteAllUsers(bool AreYouSure)
        {
            if (!AreYouSure) return;
            dbContext.Users.RemoveRange(GetAllUsers());
            dbContext.SaveChanges();
        }

    }
}