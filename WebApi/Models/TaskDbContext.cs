using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    public class TaskDbContext : DbContext
    {
        public TaskDbContext() : base("DefaultConnection") { }

        public DbSet<Symbol> Symbols { get; set; }
        public DbSet<Quote> Quotes { get; set; }
    }
}