using Microsoft.EntityFrameworkCore;
using PassportOffice.Domain.Entities;
using PassportOffice.Domain.Repositories.Abstractions;

namespace PassportOffice.Infrastructure.EntityFramework.Repositories;

public class AppointmentRepository(ApplicationDbContext context)
    : EfRepository<Appointment, Guid>(context), IAppointmentRepository
{
    protected override IQueryable<Appointment> Query(bool asNoTracking = false)
    {
        var query = Context.Appointments
            .Include(appointment => appointment.Citizen)
            .Include(appointment => appointment.Department)
            .Include(appointment => appointment.Employee)
            .Include(appointment => appointment.Application)
            .AsQueryable();

        return asNoTracking ? query.AsNoTracking() : query;
    }

    public async Task<IReadOnlyCollection<Appointment>> GetByCitizenAsync(Guid citizenId, CancellationToken cancellationToken = default)
        => await Query(true)
            .Where(appointment => appointment.CitizenId == citizenId)
            .OrderByDescending(appointment => appointment.ScheduledAt)
            .ToArrayAsync(cancellationToken);

    public async Task<IReadOnlyCollection<Appointment>> GetForDateAsync(Guid departmentId, DateOnly date, CancellationToken cancellationToken = default)
        => await Query(true)
            .Where(appointment => appointment.DepartmentId == departmentId
                                  && DateOnly.FromDateTime(appointment.ScheduledAt) == date)
            .ToArrayAsync(cancellationToken);
}

