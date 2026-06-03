using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PassportOffice.Domain.Entities;

namespace PassportOffice.Infrastructure.EntityFramework.Configurations;

public class PassportApplicationConfiguration : IEntityTypeConfiguration<PassportApplication>
{
    public void Configure(EntityTypeBuilder<PassportApplication> builder)
    {
        builder.ToTable("PassportApplications");
        builder.HasKey(application => application.Id);

        builder.Property(application => application.Number)
            .HasMaxLength(24)
            .IsRequired();

        builder.Property(application => application.Comment)
            .HasMaxLength(1000);

        builder.HasOne(application => application.Service)
            .WithMany()
            .HasForeignKey(application => application.ServiceId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(application => application.Department)
            .WithMany()
            .HasForeignKey(application => application.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(application => application.Employee)
            .WithMany()
            .HasForeignKey(application => application.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(application => application.Documents)
            .WithOne(document => document.Application)
            .HasForeignKey(document => document.ApplicationId);

        builder.Navigation(application => application.Documents).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}

