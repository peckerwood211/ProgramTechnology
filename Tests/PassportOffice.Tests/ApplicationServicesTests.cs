using Microsoft.Extensions.DependencyInjection;
using PassportOffice.Application.Contracts.Citizens;
using PassportOffice.Application.Contracts.Passports;
using PassportOffice.Application.Contracts.Services;
using PassportOffice.Application.Services;
using PassportOffice.Domain.Enums;
using PassportOffice.Infrastructure.EntityFramework;

namespace PassportOffice.Tests;

public class ApplicationServicesTests
{
    [Fact]
    public async Task CitizenService_CreatesCitizenAndIssuesPassport()
    {
        await using var provider = await BuildProviderAsync();
        using var scope = provider.CreateScope();

        var citizens = scope.ServiceProvider.GetRequiredService<ICitizensApplicationService>();

        var model = await citizens.CreateAsync(
            new CreateCitizenModel(
                "Volkova Maria Ivanovna",
                new DateOnly(2000, 2, 2),
                "Irkutsk",
                Gender.Female,
                "555-666-777 88",
                "+7 999 222-33-44",
                "maria.volkova@example.com"));

        Assert.NotNull(model);

        var passport = await citizens.IssuePassportAsync(
            new IssuePassportModel(
                model!.Id,
                "2526",
                "777888",
                "380-001",
                "Central passport office",
                DateOnly.FromDateTime(DateTime.UtcNow)));

        Assert.NotNull(passport);
        Assert.Equal("2526 777888", passport!.FullNumber);
    }

    [Fact]
    public async Task DepartmentsService_ReturnsSeededDepartmentWithServices()
    {
        await using var provider = await BuildProviderAsync();
        using var scope = provider.CreateScope();

        var departments = scope.ServiceProvider.GetRequiredService<IDepartmentsApplicationService>();

        var department = await departments.GetByCodeAsync("380-001");

        Assert.NotNull(department);
        Assert.NotEmpty(department!.Employees);
        Assert.Contains(department.Services, service => service.Code == "FIRST-PASSPORT");
    }

    private static async Task<ServiceProvider> BuildProviderAsync()
    {
        var services = new ServiceCollection();
        services.AddApplicationServices();
        services.AddInfrastructure($"PassportOfficeTests-{Guid.NewGuid():N}");

        var provider = services.BuildServiceProvider();
        using var scope = provider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await DatabaseSeeder.SeedAsync(db);
        return provider;
    }
}

