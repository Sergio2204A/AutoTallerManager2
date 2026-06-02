using Application.Abstractions;
using MediatR;

namespace Application.UseCase.Clientes;

public sealed class DeleteClienteHandler : IRequestHandler<DeleteCliente>
{
    private readonly IUnitOfWork _uow;
    public DeleteClienteHandler(IUnitOfWork uow) => _uow = uow;

    public async Task Handle(DeleteCliente req, CancellationToken ct)
    {
        var cliente = await _uow.Clientes.GetByIdAsync(req.Id, ct)
            ?? throw new KeyNotFoundException($"Cliente con Id {req.Id} no encontrado.");

        cliente.IsDeleted = true;
        cliente.DeletedAt = DateTime.UtcNow;
        await _uow.Clientes.UpdateAsync(cliente, ct);
        await _uow.SaveChangesAsync(ct);
    }
}
