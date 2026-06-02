using MediatR;

namespace Application.UseCase.Vehiculos;

public sealed record DeleteVehiculo(int Id) : IRequest;
