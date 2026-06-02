using Domain.Entities.Taller;

namespace Application.Abstractions.Taller;

public interface IAuditoriaRepository
{
    Task<IReadOnlyList<Auditoria>> GetPagedAsync(int page, int pageSize, string? search = null, CancellationToken ct = default);
    Task<int> CountAsync(string? search = null, CancellationToken ct = default);
    Task AddAsync(Auditoria auditoria, CancellationToken ct = default);
    Task<IReadOnlyList<Auditoria>> GetByEntidadAsync(string entidad, CancellationToken ct = default);
}
