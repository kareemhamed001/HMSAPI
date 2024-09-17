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
    public class PrescriptionsConfiguration : IEntityTypeConfiguration<Prescription>
    {
        public void Configure(EntityTypeBuilder<Prescription> builder)
        {
            builder.HasOne(mh => mh.Doctor)
             .WithMany(d => d.Prescriptions)
             .HasForeignKey(mh => mh.DoctorId)
             .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(mh => mh.Patient)
                   .WithMany(p => p.Prescriptions)
                   .HasForeignKey(mh => mh.PatientId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
