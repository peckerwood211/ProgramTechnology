using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PassportOffice.Domain.Entities;
using PassportOffice.ValueObjects;

namespace PassportOffice.Infrastructure.EntityFramework.Configurations;

public class OfficeEmployeeConfiguration : IEntityTypeConfiguration<OfficeEmployee>
{
    public void Configure(EntityTypeBuilder<OfficeEmployee> builder)
    {
        builder.ToTable("Employees");
        builder.HasKey(employee => employee.Id);

        builder.Property(employee => employee.FullName)
            .HasConversion(fullName => fullName.Value, value => new FullName(value))
            .HasMaxLength(120)
            .IsRequired();

        builder.Property(employee => employee.PersonnelNumber)
            .HasMaxLength(30)
            .IsRequired();
    }
}

