using PassportOffice.Application.Contracts.Applications;
using PassportOffice.Domain.Enums;

namespace PassportOffice.Application.Contracts.Services;

public interface IPassportApplicationsService
    : IApplicationService<PassportApplicationModel, CreatePassportApplicationModel, Guid>
{
    Task<IReadOnlyCollection<PassportApplicationModel>> GetByStatusAsync(ApplicationStatus status, CancellationToken cancellationToken = default);

    Task<bool> AcceptAsync(Guid id, ApplicationDecisionModel model, CancellationToken cancellationToken = default);

    Task<bool> ApproveAsync(Guid id, ApplicationDecisionModel model, CancellationToken cancellationToken = default);

    Task<bool> RejectAsync(Guid id, ApplicationDecisionModel model, CancellationToken cancellationToken = default);

    Task<bool> CancelAsync(Guid id, string reason, CancellationToken cancellationToken = default);
}

