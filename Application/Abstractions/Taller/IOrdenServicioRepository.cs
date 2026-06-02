using Domain.Entities.Taller;
using Domain.Enums;

namespace Application.Abstractions.Taller;

public interface IOrdenServicioRepository
{
    Task<OrdenServicio?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<OrdenServicio?> GetByIdWithDetailsAsync(int id, CancellationToken ct = default);
    Task<OrdenServicio?> GetByNumeroAsync(string numero, CancellationToken ct = default);
    Task<IReadOnlyList<OrdenServicio>> GetByClienteIdAsync(int clienteId, CancellationToken ct = default);
    Task<IReadOnlyList<OrdenServicio>> GetByMecanicoIdAsync(int mecanicoId, CancellationToken ct = default);
    Task<IReadOnlyList<OrdenServicio>> GetByEstadoAsync(EstadoOrden estado, CancellationToken ct = default);
    Task<IReadOnlyList<OrdenServicio>> GetPagedAsync(int page, int pageSize, string? search = null, CancellationToken ct = default);
    Task<int> CountAsync(string? search = null, CancellationToken ct = default);
    Task AddAsync(OrdenServicio orden, CancellationToken ct = default);
    Task UpdateAsync(OrdenServicio orden, CancellationToken ct = default);
}
