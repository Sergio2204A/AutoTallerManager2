using Application.Abstractions;
using Domain.Entities.Taller;
using MediatR;

namespace Application.UseCase.Facturas;

public sealed record GenerarFactura(int OrdenServicioId, decimal ManoObra) : IRequest<int>;
public sealed record GetFacturaById(int Id) : IRequest<Factura?>;
public sealed record GetFacturasPaged(int Page, int PageSize, string? Search = null)
    : IRequest<(IReadOnlyList<Factura> Items, int Total)>;

public sealed class GenerarFacturaHandler : IRequestHandler<GenerarFactura, int>
{
    private readonly IUnitOfWork _uow;
    public GenerarFacturaHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<int> Handle(GenerarFactura req, CancellationToken ct)
    {
        var orden = await _uow.Ordenes.GetByIdWithDetailsAsync(req.OrdenServicioId, ct)
            ?? throw new KeyNotFoundException($"Orden {req.OrdenServicioId} no encontrada.");
        var totalRepuestos = orden.Detalles.Sum(d => d.Subtotal);
        var factura = new Factura
        {
            NumeroFactura = $"FAC-{DateTime.UtcNow:yyyyMMddHHmmss}-{Random.Shared.Next(1000,9999)}",
            OrdenServicioId = req.OrdenServicioId
        };
        factura.Calcular(req.ManoObra, totalRepuestos);
        factura.Emitir();
        await _uow.Facturas.AddAsync(factura, ct);
        await _uow.SaveChangesAsync(ct);
        return factura.Id;
    }
}

public sealed class GetFacturaByIdHandler : IRequestHandler<GetFacturaById, Factura?>
{
    private readonly IUnitOfWork _uow;
    public GetFacturaByIdHandler(IUnitOfWork uow) => _uow = uow;
    public async Task<Factura?> Handle(GetFacturaById req, CancellationToken ct)
        => await _uow.Facturas.GetByIdAsync(req.Id, ct);
}

public sealed class GetFacturasPagedHandler
    : IRequestHandler<GetFacturasPaged, (IReadOnlyList<Factura> Items, int Total)>
{
    private readonly IUnitOfWork _uow;
    public GetFacturasPagedHandler(IUnitOfWork uow) => _uow = uow;
    public async Task<(IReadOnlyList<Factura> Items, int Total)> Handle(GetFacturasPaged req, CancellationToken ct)
    {
        var items = await _uow.Facturas.GetPagedAsync(req.Page, req.PageSize, req.Search, ct);
        var total = await _uow.Facturas.CountAsync(req.Search, ct);
        return (items, total);
    }
}
