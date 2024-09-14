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
    public class WarehouseConfigration : IEntityTypeConfiguration<Warehouse>
    {
        public void Configure(EntityTypeBuilder<Warehouse> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).ValueGeneratedOnAdd();

            builder.Property(c => c.Name)
             .IsRequired()
             .HasColumnType("nvarchar")
             .HasMaxLength(50);

            builder.HasOne(W => W.Section)
                .WithMany(w => w.Warehouses)
                .HasForeignKey(w => w.SectionId);

        }
    }
}
