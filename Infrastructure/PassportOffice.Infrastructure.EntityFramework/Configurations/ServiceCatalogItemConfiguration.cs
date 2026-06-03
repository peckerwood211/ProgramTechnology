using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PassportOffice.Domain.Entities;
using PassportOffice.ValueObjects;

namespace PassportOffice.Infrastructure.EntityFramework.Configurations;

public class ServiceCatalogItemConfiguration : IEntityTypeConfiguration<ServiceCatalogItem>
{
    public void Configure(EntityTypeBuilder<ServiceCatalogItem> builder)
    {
        builder.ToTable("Services");
        builder.HasKey(service => service.Id);

        builder.Property(service => service.Code)
            .HasConversion(code => code.Value, value => new ServiceCode(value))
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(service => service.Name)
            .HasMaxLength(160)
            .IsRequired();

        builder.Property(service => service.StateFee)
            .HasPrecision(10, 2);
    }
}

