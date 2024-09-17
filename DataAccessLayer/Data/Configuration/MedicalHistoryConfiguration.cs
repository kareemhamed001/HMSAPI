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
    public class MedicalHistoryConfiguration : IEntityTypeConfiguration<MedicalHistory>
    {
        public void Configure(EntityTypeBuilder<MedicalHistory> builder)
        {
            builder.HasKey(mh => new { mh.PatientId, mh.DoctorId });
            builder.HasOne(mh => mh.Doctor)
             .WithMany(d => d.MedicalHistories)
             .HasForeignKey(mh => mh.DoctorId)
             .OnDelete(DeleteBehavior.Restrict); 

            builder.HasOne(mh => mh.patient)
                   .WithMany(p => p.MedicalHistories)
                   .HasForeignKey(mh => mh.PatientId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
