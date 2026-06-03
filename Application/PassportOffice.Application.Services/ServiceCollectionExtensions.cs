using Microsoft.Extensions.DependencyInjection;
using PassportOffice.Application.Contracts.Services;

namespace PassportOffice.Application.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ICitizensApplicationService, CitizensApplicationService>();
        services.AddScoped<IPassportApplicationsService, PassportApplicationsService>();
        services.AddScoped<IAppointmentsApplicationService, AppointmentsApplicationService>();
        services.AddScoped<IDepartmentsApplicationService, DepartmentsApplicationService>();
        return services;
    }
}

