using Application.Abstractions;
using Domain.Entities.Taller;
using MediatR;

namespace Application.UseCase.Clientes;

public sealed class GetClienteByIdHandler : IRequestHandler<GetClienteById, Cliente?>
{
    private readonly IUnitOfWork _uow;
    public GetClienteByIdHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<Cliente?> Handle(GetClienteById req, CancellationToken ct)
        => await _uow.Clientes.GetByIdAsync(req.Id, ct);
}
