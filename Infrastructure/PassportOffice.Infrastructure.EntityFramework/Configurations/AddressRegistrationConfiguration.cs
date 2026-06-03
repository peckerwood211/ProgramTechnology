using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PassportOffice.Domain.Entities;
using PassportOffice.ValueObjects;

namespace PassportOffice.Infrastructure.EntityFramework.Configurations;

public class AddressRegistrationConfiguration : IEntityTypeConfiguration<AddressRegistration>
{
    public void Configure(EntityTypeBuilder<AddressRegistration> builder)
    {
        builder.ToTable("AddressRegistrations");
        builder.HasKey(registration => registration.Id);

        builder.Property(registration => registration.Address)
            .HasConversion(address => address.Value, value => new AddressLine(value))
            .HasMaxLength(250)
            .IsRequired();

        builder.Property(registration => registration.Comment)
            .HasMaxLength(500);
    }
}

