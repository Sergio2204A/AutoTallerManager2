using MediatR;

namespace Application.UseCase.Vehiculos;

public sealed record UpdateVehiculo(int Id, string Marca, string Modelo, int Anio, int Kilometraje, string Color) : IRequest;
