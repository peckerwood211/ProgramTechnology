using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PassportOffice.Domain.Entities;
using PassportOffice.Domain.Repositories.Abstractions;
using PassportOffice.Domain.Repositories.Abstractions.Base;
using PassportOffice.Infrastructure.EntityFramework.Repositories;

namespace PassportOffice.Infrastructure.EntityFramework;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string databaseName = "PassportOfficeDb")
    {
        services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase(databaseName));

        services.AddScoped(typeof(IRepository<,>), typeof(EfRepository<,>));
        services.AddScoped<ICitizenRepository, CitizenRepository>();
        services.AddScoped<IPassportRepository, PassportRepository>();
        services.AddScoped<IPassportApplicationRepository, PassportApplicationRepository>();
        services.AddScoped<IAppointmentRepository, AppointmentRepository>();
        services.AddScoped<IOfficeDepartmentRepository, OfficeDepartmentRepository>();
        services.AddScoped<IUnitOfWork, EfUnitOfWork>();

        return services;
    }
}

