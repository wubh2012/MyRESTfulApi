using Microsoft.EntityFrameworkCore;
using MyRestful.Core;
using MyRestful.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyRestful.Infrastructure
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions<MyContext> options) : base(options)
        {

        }

        public DbSet<TodoItem> TodoItems { get; set; }
        public DbSet<Country> Countries { get; set; }
    }
}
