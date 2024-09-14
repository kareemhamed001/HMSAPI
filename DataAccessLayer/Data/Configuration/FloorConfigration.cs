using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Data.Configuration
{
    public class FloorConfigration : IEntityTypeConfiguration<Floor>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Floor> builder)
        {
            builder.HasKey(f => f.Id);
            builder.Property(f => f.Id).ValueGeneratedOnAdd();

            builder.Property(f => f.Name)
                .IsRequired()
                .HasColumnType("nvarchar")
                .HasMaxLength(50);

            builder.HasMany(f => f.Rooms)
                .WithOne(f => f.Floor)
                .HasForeignKey(f => f.FloorId);


        }
    }
}
