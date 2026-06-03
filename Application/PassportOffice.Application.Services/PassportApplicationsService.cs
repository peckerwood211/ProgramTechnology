using PassportOffice.Application.Contracts.Applications;
using PassportOffice.Application.Contracts.Services;
using PassportOffice.Application.Services.Mapping;
using PassportOffice.Domain.Entities;
using PassportOffice.Domain.Enums;
using PassportOffice.Domain.Repositories.Abstractions;
using PassportOffice.Domain.Repositories.Abstractions.Base;
using PassportOffice.ValueObjects;

namespace PassportOffice.Application.Services;

public class PassportApplicationsService(
    IPassportApplicationRepository applications,
    ICitizenRepository citizens,
    IOfficeDepartmentRepository departments,
    IRepository<OfficeEmployee, Guid> employees,
    IUnitOfWork unitOfWork) : IPassportApplicationsService
{
    public async Task<IReadOnlyCollection<PassportApplicationModel>> GetAllAsync(CancellationToken cancellationToken = default)
        => (await applications.GetAllAsync(cancellationToken, true))
            .Select(application => application.ToModel())
            .ToArray();

    public async Task<PassportApplicationModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => (await applications.GetByIdAsync(id, cancellationToken))?.ToModel();

    public async Task<IReadOnlyCollection<PassportApplicationModel>> GetByStatusAsync(ApplicationStatus status, CancellationToken cancellationToken = default)
        => (await applications.GetByStatusAsync(status, cancellationToken))
            .Select(application => application.ToModel())
            .ToArray();

    public async Task<PassportApplicationModel?> CreateAsync(CreatePassportApplicationModel model, CancellationToken cancellationToken = default)
    {
        var citizen = await citizens.GetByIdAsync(model.CitizenId, cancellationToken);
        if (citizen is null)
            return null;

        var department = await departments.GetByCodeAsync(model.DepartmentCode, cancellationToken);
        if (department is null)
            return null;

        var serviceCode = new ServiceCode(model.ServiceCode);
        var service = department.Services.FirstOrDefault(item => item.Code == serviceCode && item.IsActive);
        if (service is null)
            return null;

        var application = citizen.SubmitApplication(
            service,
            department,
            model.Type,
            DateTime.UtcNow,
            model.Comment);

        foreach (var document in model.Documents ?? Array.Empty<CreateAttachedDocumentModel>())
        {
            application.AttachDocument(document.Type, document.Name, document.Number, document.ReceivedAt);
        }

        await applications.AddAsync(application, cancellationToken);
        await citizens.UpdateAsync(citizen, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return application.ToModel();
    }

    public Task<bool> AcceptAsync(Guid id, ApplicationDecisionModel model, CancellationToken cancellationToken = default)
        => ChangeStatusAsync(id, model, (application, employee) => application.Accept(employee, DateTime.UtcNow), cancellationToken);

    public Task<bool> ApproveAsync(Guid id, ApplicationDecisionModel model, CancellationToken cancellationToken = default)
        => ChangeStatusAsync(id, model, (application, employee) => application.Approve(employee, model.Comment, DateTime.UtcNow), cancellationToken);

    public Task<bool> RejectAsync(Guid id, ApplicationDecisionModel model, CancellationToken cancellationToken = default)
        => ChangeStatusAsync(id, model, (application, employee) => application.Reject(employee, model.Comment ?? string.Empty, DateTime.UtcNow), cancellationToken);

    public async Task<bool> CancelAsync(Guid id, string reason, CancellationToken cancellationToken = default)
    {
        var application = await applications.GetByIdAsync(id, cancellationToken);
        if (application is null)
            return false;

        application.Cancel(reason);
        await applications.UpdateAsync(application, cancellationToken);
        return await unitOfWork.SaveChangesAsync(cancellationToken) > 0;
    }

    private async Task<bool> ChangeStatusAsync(
        Guid id,
        ApplicationDecisionModel model,
        Action<PassportApplication, OfficeEmployee> action,
        CancellationToken cancellationToken)
    {
        var application = await applications.GetByIdAsync(id, cancellationToken);
        if (application is null)
            return false;

        var employee = await employees.GetByIdAsync(model.EmployeeId, cancellationToken);
        if (employee is null || !employee.IsActive)
            return false;

        action(application, employee);
        await applications.UpdateAsync(application, cancellationToken);
        return await unitOfWork.SaveChangesAsync(cancellationToken) > 0;
    }
}

