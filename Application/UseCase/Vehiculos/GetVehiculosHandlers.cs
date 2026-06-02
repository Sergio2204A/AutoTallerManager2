using Application.Abstractions;
using Domain.Entities.Taller;
using MediatR;

namespace Application.UseCase.Vehiculos;

public sealed class GetVehiculosPagedHandler
    : IRequestHandler<GetVehiculosPaged, (IReadOnlyList<Vehiculo> Items, int Total)>
{
    private readonly IUnitOfWork _uow;
    public GetVehiculosPagedHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<(IReadOnlyList<Vehiculo> Items, int Total)> Handle(GetVehiculosPaged req, CancellationToken ct)
    {
        var items = await _uow.Vehiculos.GetPagedAsync(req.Page, req.PageSize, req.Search, ct);
        var total = await _uow.Vehiculos.CountAsync(req.Search, ct);
        return (items, total);
    }
}

public sealed class GetVehiculosByClienteHandler
    : IRequestHandler<GetVehiculosByCliente, IReadOnlyList<Vehiculo>>
{
    private readonly IUnitOfWork _uow;
    public GetVehiculosByClienteHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<IReadOnlyList<Vehiculo>> Handle(GetVehiculosByCliente req, CancellationToken ct)
        => await _uow.Vehiculos.GetByClienteIdAsync(req.ClienteId, ct);
}
