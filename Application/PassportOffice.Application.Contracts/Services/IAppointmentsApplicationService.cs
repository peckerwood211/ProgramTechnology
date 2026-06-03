using PassportOffice.Application.Contracts.Appointments;

namespace PassportOffice.Application.Contracts.Services;

public interface IAppointmentsApplicationService
    : IApplicationService<AppointmentModel, CreateAppointmentModel, Guid>
{
    Task<IReadOnlyCollection<AppointmentModel>> GetByCitizenAsync(Guid citizenId, CancellationToken cancellationToken = default);

    Task<bool> ConfirmAsync(Guid id, CancellationToken cancellationToken = default);

    Task<bool> CompleteAsync(Guid id, CancellationToken cancellationToken = default);

    Task<bool> CancelAsync(Guid id, CancellationToken cancellationToken = default);
}

