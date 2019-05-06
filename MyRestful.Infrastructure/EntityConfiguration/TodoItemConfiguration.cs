using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyRestful.Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyRestful.Infrastructure.EntityConfiguration
{
    class TodoItemConfiguration : IEntityTypeConfiguration<TodoItem>
    {
        public void Configure(EntityTypeBuilder<TodoItem> builder)
        {
            builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
        }
    }
}
