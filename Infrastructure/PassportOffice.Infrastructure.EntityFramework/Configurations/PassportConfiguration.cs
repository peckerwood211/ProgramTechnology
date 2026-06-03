using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PassportOffice.Domain.Entities;
using PassportOffice.ValueObjects;

namespace PassportOffice.Infrastructure.EntityFramework.Configurations;

public class PassportConfiguration : IEntityTypeConfiguration<Passport>
{
    public void Configure(EntityTypeBuilder<Passport> builder)
    {
        builder.ToTable("Passports");
        builder.HasKey(passport => passport.Id);

        builder.Property(passport => passport.Series)
            .HasConversion(series => series.Value, value => new PassportSeries(value))
            .HasMaxLength(4)
            .IsRequired();

        builder.Property(passport => passport.Number)
            .HasConversion(number => number.Value, value => new PassportNumber(value))
            .HasMaxLength(6)
            .IsRequired();

        builder.Property(passport => passport.DepartmentCode)
            .HasConversion(code => code.Value, value => new DepartmentCode(value))
            .HasMaxLength(7)
            .IsRequired();

        builder.Property(passport => passport.IssuedBy)
            .HasMaxLength(250)
            .IsRequired();
    }
}

