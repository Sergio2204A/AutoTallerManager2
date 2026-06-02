using MediatR;

namespace Application.UseCase.Clientes;

public sealed record UpdateCliente(
    int Id,
    string NombreCompleto,
    string Telefono,
    string Email
) : IRequest;
