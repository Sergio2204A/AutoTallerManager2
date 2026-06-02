using Domain.Entities.Taller;

namespace Application.Abstractions.Taller;

public interface IRepuestoRepository
{
    Task<Repuesto?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Repuesto?> GetByCodigoAsync(string codigo, CancellationToken ct = default);
    Task<IReadOnlyList<Repuesto>> GetPagedAsync(int page, int pageSize, string? search = null, CancellationToken ct = default);
    Task<int> CountAsync(string? search = null, CancellationToken ct = default);
    Task AddAsync(Repuesto repuesto, CancellationToken ct = default);
    Task UpdateAsync(Repuesto repuesto, CancellationToken ct = default);
    Task RemoveAsync(Repuesto repuesto, CancellationToken ct = default);
    Task<IReadOnlyList<Repuesto>> GetBajosStockAsync(CancellationToken ct = default);
    Task<bool> ExistsCodigoAsync(string codigo, CancellationToken ct = default);
}
