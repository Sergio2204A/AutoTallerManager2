using Application.Abstractions;
using Domain.Entities.Taller;
using MediatR;

namespace Application.UseCase.Clientes;

public sealed class GetClientesPagedHandler
    : IRequestHandler<GetClientesPaged, (IReadOnlyList<Cliente> Items, int Total)>
{
    private readonly IUnitOfWork _uow;
    public GetClientesPagedHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<(IReadOnlyList<Cliente> Items, int Total)> Handle(GetClientesPaged req, CancellationToken ct)
    {
        var items = await _uow.Clientes.GetPagedAsync(req.Page, req.PageSize, req.Search, ct);
        var total = await _uow.Clientes.CountAsync(req.Search, ct);
        return (items, total);
    }
}
