using Domain.Entities.Taller;
using MediatR;

namespace Application.UseCase.Vehiculos;

public sealed record GetVehiculoById(int Id) : IRequest<Vehiculo?>;
