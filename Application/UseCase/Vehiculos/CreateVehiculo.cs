using MediatR;

namespace Application.UseCase.Vehiculos;

public sealed record CreateVehiculo(
    string Marca, string Modelo, int Anio,
    string Vin, int Kilometraje, string Placa,
    string Color, int ClienteId
) : IRequest<int>;
