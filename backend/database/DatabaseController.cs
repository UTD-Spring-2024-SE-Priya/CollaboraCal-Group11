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

        //      USER

        public User? GetLightUserFromEmail(string email)
        {
            return dbContext.Users.SingleOrDefault(a => a.EMail == email);
        }

        public User? GetHeavyUserFromEmail(string email)
        {
            return dbContext.Users.Include(a => a.Calendars).SingleOrDefault(a => a.EMail == email);
        }

        public bool AddUser(User user)
        {
            if (user.EMail == null) return false;
            if (GetLightUserFromEmail(user.EMail) != null)
                return false;
            dbContext.Users.Add(user);
            dbContext.SaveChanges();
            return true;
        }

        public void RemoveUserByEmail(string email)
        {
            User? user = GetLightUserFromEmail(email);
            if (user == null) return;
            dbContext.Users.Remove(user);
            dbContext.SaveChanges();
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

        //      CALENDAR

        public void AddCalendar(Calendar calendar)
        {
            dbContext.Calendars.Add(calendar);
            dbContext.SaveChanges();
        }

        public List<Calendar>? GetLightAllCalendars(string email)
        { 
            User? user = GetHeavyUserFromEmail(email);
            return user?.Calendars?.ToList();
        }

        public Calendar GetHeavyCalendar(int calendarID)
        {
            return dbContext.Calendars
                .Where(a => a.ID == calendarID)
                .Include(a => a.Events)
                .Include(a => a.Users)
                .Single();
        }

        //      EVENTS

        public void AddEvent(Event ev)
        {
            dbContext.Events.Add(ev);
            dbContext.SaveChanges();
        }

    }
}