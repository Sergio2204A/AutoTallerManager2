using MediatR;

namespace Application.UseCase.Clientes;

public sealed record CreateCliente(
    string NombreCompleto,
    string Cedula,
    string Telefono,
    string Email,
    string Password
) : IRequest<int>;
