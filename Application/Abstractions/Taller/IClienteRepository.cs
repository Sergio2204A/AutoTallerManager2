using Domain.Entities.Taller;

namespace Application.Abstractions.Taller;

public interface IClienteRepository
{
    Task<Cliente?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Cliente?> GetByCedulaAsync(string cedula, CancellationToken ct = default);
    Task<IReadOnlyList<Cliente>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<Cliente>> GetPagedAsync(int page, int pageSize, string? search = null, CancellationToken ct = default);
    Task<int> CountAsync(string? search = null, CancellationToken ct = default);
    Task AddAsync(Cliente cliente, CancellationToken ct = default);
    Task UpdateAsync(Cliente cliente, CancellationToken ct = default);
    Task RemoveAsync(Cliente cliente, CancellationToken ct = default);
    Task<bool> ExistsCedulaAsync(string cedula, CancellationToken ct = default);
}
