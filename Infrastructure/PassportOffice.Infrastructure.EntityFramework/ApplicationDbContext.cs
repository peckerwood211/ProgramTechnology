using Microsoft.EntityFrameworkCore;
using PassportOffice.Domain.Entities;

namespace PassportOffice.Infrastructure.EntityFramework;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Citizen> Citizens => Set<Citizen>();

    public DbSet<Passport> Passports => Set<Passport>();

    public DbSet<AddressRegistration> AddressRegistrations => Set<AddressRegistration>();

    public DbSet<PassportApplication> PassportApplications => Set<PassportApplication>();

    public DbSet<AttachedDocument> AttachedDocuments => Set<AttachedDocument>();

    public DbSet<OfficeDepartment> Departments => Set<OfficeDepartment>();

    public DbSet<OfficeEmployee> Employees => Set<OfficeEmployee>();

    public DbSet<ServiceCatalogItem> Services => Set<ServiceCatalogItem>();

    public DbSet<Appointment> Appointments => Set<Appointment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}

