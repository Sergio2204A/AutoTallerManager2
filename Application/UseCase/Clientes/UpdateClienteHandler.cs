using Application.Abstractions;
using MediatR;

namespace Application.UseCase.Clientes;

public sealed class UpdateClienteHandler : IRequestHandler<UpdateCliente>
{
    private readonly IUnitOfWork _uow;
    public UpdateClienteHandler(IUnitOfWork uow) => _uow = uow;

    public async Task Handle(UpdateCliente req, CancellationToken ct)
    {
        var cliente = await _uow.Clientes.GetByIdAsync(req.Id, ct)
            ?? throw new KeyNotFoundException($"Cliente con Id {req.Id} no encontrado.");

        cliente.NombreCompleto = req.NombreCompleto;
        cliente.Telefono       = req.Telefono;
        cliente.Email          = req.Email;
        cliente.UpdatedAt      = DateTime.UtcNow;

        await _uow.Clientes.UpdateAsync(cliente, ct);
        await _uow.SaveChangesAsync(ct);
    }
}
