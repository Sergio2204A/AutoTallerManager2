using Domain.Entities.Taller;

namespace Application.Abstractions.Taller;

public interface IFacturaRepository
{
    Task<Factura?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Factura?> GetByNumeroAsync(string numero, CancellationToken ct = default);
    Task<Factura?> GetByOrdenIdAsync(int ordenId, CancellationToken ct = default);
    Task<IReadOnlyList<Factura>> GetPagedAsync(int page, int pageSize, string? search = null, CancellationToken ct = default);
    Task<int> CountAsync(string? search = null, CancellationToken ct = default);
    Task AddAsync(Factura factura, CancellationToken ct = default);
    Task UpdateAsync(Factura factura, CancellationToken ct = default);
}
