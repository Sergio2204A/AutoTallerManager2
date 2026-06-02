using Domain.Entities.Taller;
using MediatR;

namespace Application.UseCase.Clientes;

public sealed record GetClientesPaged(int Page, int PageSize, string? Search = null)
    : IRequest<(IReadOnlyList<Cliente> Items, int Total)>;
