using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PassportOffice.Domain.Entities;
using PassportOffice.ValueObjects;

namespace PassportOffice.Infrastructure.EntityFramework.Configurations;

public class OfficeDepartmentConfiguration : IEntityTypeConfiguration<OfficeDepartment>
{
    public void Configure(EntityTypeBuilder<OfficeDepartment> builder)
    {
        builder.ToTable("Departments");
        builder.HasKey(department => department.Id);

        builder.Property(department => department.Code)
            .HasConversion(code => code.Value, value => new DepartmentCode(value))
            .HasMaxLength(7)
            .IsRequired();

        builder.Property(department => department.Name)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(department => department.Address)
            .HasConversion(address => address.Value, value => new AddressLine(value))
            .HasMaxLength(250)
            .IsRequired();

        builder.Property(department => department.WorkingHours)
            .HasMaxLength(120)
            .IsRequired();

        builder.HasMany(department => department.Employees)
            .WithOne(employee => employee.Department)
            .HasForeignKey(employee => employee.DepartmentId);

        builder.HasMany(department => department.Services)
            .WithOne(service => service.Department)
            .HasForeignKey(service => service.DepartmentId);

        builder.Navigation(department => department.Employees).UsePropertyAccessMode(PropertyAccessMode.Field);
        builder.Navigation(department => department.Services).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}

