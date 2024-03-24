#nullable disable

using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

internal class CollaboraCalDBContext : DbContext
{

    //const string SQLServerConnectionString = @"Server=localhost;Database=master;Trusted_Connection=True;";
    //const string SQLiteConnectionString = @"Data Source=c:\mydb.db;";

    private string DbPath;

    public CollaboraCalDBContext(string path)
    {
        DbPath = path;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite($"Data Source={DbPath}");
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Calendar> Calendars { get; set; }
}