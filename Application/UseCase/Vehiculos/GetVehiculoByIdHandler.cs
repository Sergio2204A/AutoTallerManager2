using Application.Abstractions;
using Domain.Entities.Taller;
using MediatR;

namespace Application.UseCase.Vehiculos;

public sealed class GetVehiculoByIdHandler : IRequestHandler<GetVehiculoById, Vehiculo?>
{
    private readonly IUnitOfWork _uow;
    public GetVehiculoByIdHandler(IUnitOfWork uow) => _uow = uow;
    public async Task<Vehiculo?> Handle(GetVehiculoById req, CancellationToken ct)
        => await _uow.Vehiculos.GetByIdAsync(req.Id, ct);
}
