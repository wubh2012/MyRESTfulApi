using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyRestful.Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyRestful.Infrastructure.EntityConfiguration
{
    public class CityConfiguration : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
            builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Description).HasMaxLength(100);
            //设置级联删除
            builder.HasOne(x => x.Country).WithMany(x => x.Cities).HasForeignKey(x => x.CountryId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
