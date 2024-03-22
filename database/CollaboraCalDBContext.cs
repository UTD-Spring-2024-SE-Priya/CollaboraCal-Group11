using System;
using System.Data.Entity;

class CollaboraCalDBContext : DbContext
{
    public CollaboraCalDBContext() : base(@"Server=localhost;Database=master;Trusted_Connection=True;") { }
    public DbSet<User> Users { get; set; }
    public DbSet<Calendar> Calendars { get; set; }
}