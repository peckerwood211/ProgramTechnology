using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PassportOffice.Domain.Entities;

namespace PassportOffice.Infrastructure.EntityFramework.Configurations;

public class AttachedDocumentConfiguration : IEntityTypeConfiguration<AttachedDocument>
{
    public void Configure(EntityTypeBuilder<AttachedDocument> builder)
    {
        builder.ToTable("AttachedDocuments");
        builder.HasKey(document => document.Id);

        builder.Property(document => document.Name)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(document => document.Number)
            .HasMaxLength(80);

        builder.HasOne(document => document.Application)
            .WithMany(application => application.Documents)
            .HasForeignKey(document => document.ApplicationId);
    }
}

