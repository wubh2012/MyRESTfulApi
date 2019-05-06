using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyRestful.Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyRestful.Infrastructure.EntityConfiguration
{
    public class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.Property(x => x.EnglishName).IsRequired().HasMaxLength(100);
            builder.Property(x => x.ChineseName).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Abbreviation).IsRequired().HasMaxLength(5);
        }
    }
}
