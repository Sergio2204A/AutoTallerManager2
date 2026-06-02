using MediatR;

namespace Application.UseCase.Clientes;

public sealed record DeleteCliente(int Id) : IRequest;
