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
    public class RoomTypeConfigration : IEntityTypeConfiguration<RoomType>
    {
        public void Configure(EntityTypeBuilder<RoomType> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).ValueGeneratedOnAdd();

            builder.Property(c => c.Name)
               .IsRequired()
               .HasColumnType("nvarchar")
               .HasMaxLength(50);


            builder.HasMany(r => r.Rooms)
                .WithOne(r => r.RoomType)
                .HasForeignKey(r => r.RoomsTypesTd);


        }
    }
}
