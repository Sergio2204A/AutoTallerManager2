using Domain.Entities.Taller;

namespace Application.Abstractions.Taller;

public interface IVehiculoRepository
{
    Task<Vehiculo?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Vehiculo?> GetByPlacaAsync(string placa, CancellationToken ct = default);
    Task<Vehiculo?> GetByVinAsync(string vin, CancellationToken ct = default);
    Task<IReadOnlyList<Vehiculo>> GetByClienteIdAsync(int clienteId, CancellationToken ct = default);
    Task<IReadOnlyList<Vehiculo>> GetPagedAsync(int page, int pageSize, string? search = null, CancellationToken ct = default);
    Task<int> CountAsync(string? search = null, CancellationToken ct = default);
    Task AddAsync(Vehiculo vehiculo, CancellationToken ct = default);
    Task UpdateAsync(Vehiculo vehiculo, CancellationToken ct = default);
    Task RemoveAsync(Vehiculo vehiculo, CancellationToken ct = default);
}
