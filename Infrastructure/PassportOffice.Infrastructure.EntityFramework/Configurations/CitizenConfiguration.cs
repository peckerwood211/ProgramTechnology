using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PassportOffice.Domain.Entities;
using PassportOffice.ValueObjects;

namespace PassportOffice.Infrastructure.EntityFramework.Configurations;

public class CitizenConfiguration : IEntityTypeConfiguration<Citizen>
{
    public void Configure(EntityTypeBuilder<Citizen> builder)
    {
        builder.ToTable("Citizens");
        builder.HasKey(citizen => citizen.Id);

        builder.Property(citizen => citizen.FullName)
            .HasConversion(fullName => fullName.Value, value => new FullName(value))
            .HasMaxLength(120)
            .IsRequired();

        builder.Property(citizen => citizen.BirthPlace)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(citizen => citizen.Snils)
            .HasMaxLength(20);

        builder.Property(citizen => citizen.Phone)
            .HasConversion(phone => phone == null ? null : phone.Value, value => string.IsNullOrWhiteSpace(value) ? null : new PhoneNumber(value))
            .HasMaxLength(25);

        builder.Property(citizen => citizen.Email)
            .HasConversion(email => email == null ? null : email.Value, value => string.IsNullOrWhiteSpace(value) ? null : new EmailAddress(value))
            .HasMaxLength(150);

        builder.HasMany(citizen => citizen.Passports)
            .WithOne(passport => passport.Citizen)
            .HasForeignKey(passport => passport.CitizenId);

        builder.HasMany(citizen => citizen.Registrations)
            .WithOne(registration => registration.Citizen)
            .HasForeignKey(registration => registration.CitizenId);

        builder.HasMany(citizen => citizen.Applications)
            .WithOne(application => application.Citizen)
            .HasForeignKey(application => application.CitizenId);

        builder.Navigation(citizen => citizen.Passports).UsePropertyAccessMode(PropertyAccessMode.Field);
        builder.Navigation(citizen => citizen.Registrations).UsePropertyAccessMode(PropertyAccessMode.Field);
        builder.Navigation(citizen => citizen.Applications).UsePropertyAccessMode(PropertyAccessMode.Field);
        builder.Navigation(citizen => citizen.Appointments).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}

