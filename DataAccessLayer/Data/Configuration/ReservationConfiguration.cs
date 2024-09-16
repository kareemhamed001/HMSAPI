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
    public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
    {
        public void Configure(EntityTypeBuilder<Reservation> builder)
        {
            builder.HasKey(mh => new { mh.PatientId, mh.DoctorId });
            builder.HasOne(mh => mh.Doctor)
             .WithMany(d => d.Reservations)
             .HasForeignKey(mh => mh.DoctorId)
             .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(mh => mh.Patient)
            .WithMany(d => d.Reservations)
            .HasForeignKey(mh => mh.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(mh => mh.Clinic)
                   .WithMany(p => p.Reservations)
                   .HasForeignKey(mh => mh.ClinicId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
