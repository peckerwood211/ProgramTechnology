using PassportOffice.Domain.Entities;
using PassportOffice.Domain.Repositories.Abstractions.Base;

namespace PassportOffice.Domain.Repositories.Abstractions;

public interface IAppointmentRepository : IRepository<Appointment, Guid>
{
    Task<IReadOnlyCollection<Appointment>> GetByCitizenAsync(Guid citizenId, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<Appointment>> GetForDateAsync(Guid departmentId, DateOnly date, CancellationToken cancellationToken = default);
}

