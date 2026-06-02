using Domain.Entities.Taller;
using MediatR;

namespace Application.UseCase.Vehiculos;

public sealed record GetVehiculosPaged(int Page, int PageSize, string? Search = null)
    : IRequest<(IReadOnlyList<Vehiculo> Items, int Total)>;

public sealed record GetVehiculosByCliente(int ClienteId) : IRequest<IReadOnlyList<Vehiculo>>;
