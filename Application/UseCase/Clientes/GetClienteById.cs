using Domain.Entities.Taller;
using MediatR;

namespace Application.UseCase.Clientes;

public sealed record GetClienteById(int Id) : IRequest<Cliente?>;
