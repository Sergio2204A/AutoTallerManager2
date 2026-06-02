using Domain.Entities.Taller;

namespace Application.Abstractions.Taller;

public interface IPagoRepository
{
    Task<Pago?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<Pago>> GetByFacturaIdAsync(int facturaId, CancellationToken ct = default);
    Task AddAsync(Pago pago, CancellationToken ct = default);
    Task UpdateAsync(Pago pago, CancellationToken ct = default);
}
