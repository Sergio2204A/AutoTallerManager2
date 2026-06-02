using Application.Abstractions;
using Domain.Entities.Taller;
using MediatR;

namespace Application.UseCase.Auditorias;

public sealed record GetAuditoriasPaged(int Page, int PageSize, string? Search = null)
    : IRequest<(IReadOnlyList<Auditoria> Items, int Total)>;

public sealed class GetAuditoriasPagedHandler
    : IRequestHandler<GetAuditoriasPaged, (IReadOnlyList<Auditoria> Items, int Total)>
{
    private readonly IUnitOfWork _uow;
    public GetAuditoriasPagedHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<(IReadOnlyList<Auditoria> Items, int Total)> Handle(GetAuditoriasPaged req, CancellationToken ct)
    {
        var items = await _uow.Auditorias.GetPagedAsync(req.Page, req.PageSize, req.Search, ct);
        var total = await _uow.Auditorias.CountAsync(req.Search, ct);
        return (items, total);
    }
}
