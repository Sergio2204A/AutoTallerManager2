using Application.Abstractions;
using Domain.Entities.Taller;
using MediatR;

namespace Application.UseCase.Vehiculos;

public sealed class CreateVehiculoHandler : IRequestHandler<CreateVehiculo, int>
{
    private readonly IUnitOfWork _uow;
    public CreateVehiculoHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<int> Handle(CreateVehiculo req, CancellationToken ct)
    {
        var vehiculo = new Vehiculo
        {
            Marca = req.Marca, Modelo = req.Modelo, Anio = req.Anio,
            Vin = req.Vin, Kilometraje = req.Kilometraje,
            Placa = req.Placa.ToUpperInvariant(), Color = req.Color,
            ClienteId = req.ClienteId
        };
        await _uow.Vehiculos.AddAsync(vehiculo, ct);
        await _uow.SaveChangesAsync(ct);
        return vehiculo.Id;
    }
}
