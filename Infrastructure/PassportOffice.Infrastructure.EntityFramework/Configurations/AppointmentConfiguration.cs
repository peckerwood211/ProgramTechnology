using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PassportOffice.Domain.Entities;

namespace PassportOffice.Infrastructure.EntityFramework.Configurations;

public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.ToTable("Appointments");
        builder.HasKey(appointment => appointment.Id);

        builder.Property(appointment => appointment.Purpose)
            .HasMaxLength(250)
            .IsRequired();

        builder.HasOne(appointment => appointment.Citizen)
            .WithMany(citizen => citizen.Appointments)
            .HasForeignKey(appointment => appointment.CitizenId);

        builder.HasOne(appointment => appointment.Department)
            .WithMany()
            .HasForeignKey(appointment => appointment.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(appointment => appointment.Employee)
            .WithMany()
            .HasForeignKey(appointment => appointment.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(appointment => appointment.Application)
            .WithMany()
            .HasForeignKey(appointment => appointment.ApplicationId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

