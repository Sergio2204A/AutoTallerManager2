using Application.Abstractions;
using Domain.Entities.Taller;
using MediatR;

namespace Application.UseCase.Clientes;

public sealed class CreateClienteHandler : IRequestHandler<CreateCliente, int>
{
    private readonly IUnitOfWork _uow;
    public CreateClienteHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<int> Handle(CreateCliente req, CancellationToken ct)
    {
        if (await _uow.Clientes.ExistsCedulaAsync(req.Cedula, ct))
            throw new InvalidOperationException("Ya existe un cliente con esa cédula.");

        var cliente = new Cliente
        {
            NombreCompleto = req.NombreCompleto,
            Cedula         = req.Cedula,
            Telefono       = req.Telefono,
            Email          = req.Email,
            PasswordHash   = req.Password
        };

        await _uow.Clientes.AddAsync(cliente, ct);
        await _uow.SaveChangesAsync(ct);
        return cliente.Id;
    }
}
