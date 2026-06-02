using Application.Abstractions;
using MediatR;

namespace Application.UseCase.Vehiculos;

public sealed class UpdateVehiculoHandler : IRequestHandler<UpdateVehiculo>
{
    private readonly IUnitOfWork _uow;
    public UpdateVehiculoHandler(IUnitOfWork uow) => _uow = uow;

    public async Task Handle(UpdateVehiculo req, CancellationToken ct)
    {
        var v = await _uow.Vehiculos.GetByIdAsync(req.Id, ct)
            ?? throw new KeyNotFoundException($"Vehículo con Id {req.Id} no encontrado.");
        v.Marca = req.Marca; v.Modelo = req.Modelo; v.Anio = req.Anio;
        v.Kilometraje = req.Kilometraje; v.Color = req.Color;
        v.UpdatedAt = DateTime.UtcNow;
        await _uow.Vehiculos.UpdateAsync(v, ct);
        await _uow.SaveChangesAsync(ct);
    }
}
