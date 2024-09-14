using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Data.Configuration
{
    public class BuildingConfigration : IEntityTypeConfiguration<Building>
    {
        public void Configure(EntityTypeBuilder<Building> builder)
        {
            builder.HasKey(b => b.Id);
            builder.Property(b => b.Id).ValueGeneratedOnAdd();

            builder.Property(b => b.Name)
                .IsRequired()
                .HasColumnType("nvarchar")
                .HasMaxLength(50);
    
            builder.Property(b => b.address)
             .IsRequired()
             .HasColumnType("nvarchar")
             .HasMaxLength(250);

            builder.HasMany(b => b.Floors)
                .WithOne(b => b.bulding)
                .HasForeignKey(b => b.BuildingId);

            builder.ToTable("buildings");
        }

        
    }
}
