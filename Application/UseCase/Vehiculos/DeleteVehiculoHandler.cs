using Application.Abstractions;
using MediatR;

namespace Application.UseCase.Vehiculos;

public sealed class DeleteVehiculoHandler : IRequestHandler<DeleteVehiculo>
{
    private readonly IUnitOfWork _uow;
    public DeleteVehiculoHandler(IUnitOfWork uow) => _uow = uow;

    public async Task Handle(DeleteVehiculo req, CancellationToken ct)
    {
        var v = await _uow.Vehiculos.GetByIdAsync(req.Id, ct)
            ?? throw new KeyNotFoundException($"Vehículo con Id {req.Id} no encontrado.");
        v.IsDeleted = true; v.DeletedAt = DateTime.UtcNow;
        await _uow.Vehiculos.UpdateAsync(v, ct);
        await _uow.SaveChangesAsync(ct);
    }
}
