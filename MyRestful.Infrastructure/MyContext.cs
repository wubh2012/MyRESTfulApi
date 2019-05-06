using Microsoft.EntityFrameworkCore;
using MyRestful.Core;
using MyRestful.Core.Entity;
using MyRestful.Infrastructure.EntityConfiguration;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // 配置实体类约束
            modelBuilder.ApplyConfiguration(new CountryConfiguration());
            modelBuilder.ApplyConfiguration(new CityConfiguration());
            modelBuilder.ApplyConfiguration(new TodoItemConfiguration());
        }

        public DbSet<TodoItem> TodoItems { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }
    }
}
