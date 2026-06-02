using Application.Abstractions;
using Domain.Entities.Taller;
using MediatR;

namespace Application.UseCase.Repuestos;

public sealed class CreateRepuestoHandler : IRequestHandler<CreateRepuesto, int>
{
    private readonly IUnitOfWork _uow;
    public CreateRepuestoHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<int> Handle(CreateRepuesto req, CancellationToken ct)
    {
        if (await _uow.Repuestos.ExistsCodigoAsync(req.Codigo, ct))
            throw new InvalidOperationException("Ya existe un repuesto con ese código.");
        var r = new Repuesto { Codigo = req.Codigo, Nombre = req.Nombre, Descripcion = req.Descripcion, Categoria = req.Categoria, CantidadStock = req.CantidadStock, StockMinimo = req.StockMinimo, PrecioCompra = req.PrecioCompra, PrecioVenta = req.PrecioVenta };
        await _uow.Repuestos.AddAsync(r, ct);
        await _uow.SaveChangesAsync(ct);
        return r.Id;
    }
}

public sealed class UpdateRepuestoHandler : IRequestHandler<UpdateRepuesto>
{
    private readonly IUnitOfWork _uow;
    public UpdateRepuestoHandler(IUnitOfWork uow) => _uow = uow;

    public async Task Handle(UpdateRepuesto req, CancellationToken ct)
    {
        var r = await _uow.Repuestos.GetByIdAsync(req.Id, ct)
            ?? throw new KeyNotFoundException($"Repuesto {req.Id} no encontrado.");
        r.Nombre = req.Nombre; r.Descripcion = req.Descripcion; r.Categoria = req.Categoria;
        r.StockMinimo = req.StockMinimo; r.ActualizarPrecios(req.PrecioCompra, req.PrecioVenta);
        r.UpdatedAt = DateTime.UtcNow;
        await _uow.Repuestos.UpdateAsync(r, ct);
        await _uow.SaveChangesAsync(ct);
    }
}

public sealed class DeleteRepuestoHandler : IRequestHandler<DeleteRepuesto>
{
    private readonly IUnitOfWork _uow;
    public DeleteRepuestoHandler(IUnitOfWork uow) => _uow = uow;

    public async Task Handle(DeleteRepuesto req, CancellationToken ct)
    {
        var r = await _uow.Repuestos.GetByIdAsync(req.Id, ct)
            ?? throw new KeyNotFoundException($"Repuesto {req.Id} no encontrado.");
        r.IsDeleted = true; r.DeletedAt = DateTime.UtcNow;
        await _uow.Repuestos.UpdateAsync(r, ct);
        await _uow.SaveChangesAsync(ct);
    }
}

public sealed class AjustarStockHandler : IRequestHandler<AjustarStock>
{
    private readonly IUnitOfWork _uow;
    public AjustarStockHandler(IUnitOfWork uow) => _uow = uow;

    public async Task Handle(AjustarStock req, CancellationToken ct)
    {
        var r = await _uow.Repuestos.GetByIdAsync(req.RepuestoId, ct)
            ?? throw new KeyNotFoundException($"Repuesto {req.RepuestoId} no encontrado.");
        if (req.EsIngreso) r.AgregarStock(req.Cantidad);
        else r.DescontarStock(req.Cantidad);
        await _uow.Repuestos.UpdateAsync(r, ct);
        await _uow.SaveChangesAsync(ct);
    }
}

public sealed class GetRepuestoByIdHandler : IRequestHandler<GetRepuestoById, Repuesto?>
{
    private readonly IUnitOfWork _uow;
    public GetRepuestoByIdHandler(IUnitOfWork uow) => _uow = uow;
    public async Task<Repuesto?> Handle(GetRepuestoById req, CancellationToken ct)
        => await _uow.Repuestos.GetByIdAsync(req.Id, ct);
}

public sealed class GetRepuestosPagedHandler
    : IRequestHandler<GetRepuestosPaged, (IReadOnlyList<Repuesto> Items, int Total)>
{
    private readonly IUnitOfWork _uow;
    public GetRepuestosPagedHandler(IUnitOfWork uow) => _uow = uow;
    public async Task<(IReadOnlyList<Repuesto> Items, int Total)> Handle(GetRepuestosPaged req, CancellationToken ct)
    {
        var items = await _uow.Repuestos.GetPagedAsync(req.Page, req.PageSize, req.Search, ct);
        var total = await _uow.Repuestos.CountAsync(req.Search, ct);
        return (items, total);
    }
}

public sealed class GetBajosStockHandler : IRequestHandler<GetBajosStock, IReadOnlyList<Repuesto>>
{
    private readonly IUnitOfWork _uow;
    public GetBajosStockHandler(IUnitOfWork uow) => _uow = uow;
    public async Task<IReadOnlyList<Repuesto>> Handle(GetBajosStock req, CancellationToken ct)
        => await _uow.Repuestos.GetBajosStockAsync(ct);
}
