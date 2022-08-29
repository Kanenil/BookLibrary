using DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Context
{
    public class LibraryContext : DbContext
    {
        public LibraryContext(string connectionString) : base(connectionString) { }
        public DbSet<User> Users { get; set; }
        public DbSet<People> Peoples { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Autors { get; set; }
        public DbSet<Models.Action> Actions { get; set; }
        public DbSet<Publisher> Publishers { get; set; }

    }
}
