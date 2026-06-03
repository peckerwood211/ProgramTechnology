using PassportOffice.Application.Contracts.Base;

namespace PassportOffice.Application.Contracts.Services;

public interface IApplicationService<TModel, TCreateModel, in TId>
    where TModel : IModel<TId>
    where TCreateModel : ICreateModel
    where TId : struct, IEquatable<TId>
{
    Task<IReadOnlyCollection<TModel>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<TModel?> GetByIdAsync(TId id, CancellationToken cancellationToken = default);

    Task<TModel?> CreateAsync(TCreateModel model, CancellationToken cancellationToken = default);
}

