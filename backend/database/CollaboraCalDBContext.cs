#nullable disable

using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace CollaboraCal
{
    internal sealed class CollaboraCalDBContext : DbContext
    {

        //const string SQLServerConnectionString = @"Server=localhost;Database=master;Trusted_Connection=True;";
        //const string SQLiteConnectionString = @"Data Source=c:\mydb.db;";

        public string ConnectionString { get; }

        public CollaboraCalDBContext()
        {
            ConnectionString = WebApplication.CreateBuilder().Configuration.GetConnectionString("CollaboraCalDBContext");
            Console.WriteLine($"Connection String: '{ConnectionString}'");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite(ConnectionString);
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Calendar> Calendars => Set<Calendar>();
        public DbSet<Event> Events => Set<Event>();
    }
}