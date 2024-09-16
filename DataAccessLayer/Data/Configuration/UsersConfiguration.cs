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
    internal class UsersConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).ValueGeneratedOnAdd();
            builder.Property(t => t.Name).HasColumnType("nvarchar").HasMaxLength(250); ;
            builder.Property(t => t.Email).HasColumnType("nvarchar").HasMaxLength(250); ;
            builder.Property(t => t.Password).HasColumnType("nvarchar").HasMaxLength(250); ;
            builder.Property(t => t.Address).HasColumnType("nvarchar").HasMaxLength(250); ;
            builder.Property(t => t.NationalId).HasColumnType("nvarchar").HasMaxLength(250); ;
            builder.Property(t => t.Image).HasColumnType("nvarchar").HasMaxLength(250); ;
        }
    }
}
