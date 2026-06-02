using Application.Abstractions;
using Domain.Entities.Taller;
using MediatR;

namespace Application.UseCase.Ordenes;

public sealed class CreateOrdenHandler : IRequestHandler<CreateOrden, int>
{
    private readonly IUnitOfWork _uow;
    public CreateOrdenHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<int> Handle(CreateOrden req, CancellationToken ct)
    {
        var orden = new OrdenServicio
        {
            NumeroOrden = $"ORD-{DateTime.UtcNow:yyyyMMddHHmmss}-{Random.Shared.Next(1000, 9999)}",
            VehiculoId = req.VehiculoId, ClienteId = req.ClienteId,
            FechaEstimadaEntrega = req.FechaEstimadaEntrega,
            Observaciones = req.Observaciones
        };
        await _uow.Ordenes.AddAsync(orden, ct);
        await _uow.SaveChangesAsync(ct);
        return orden.Id;
    }
}

public sealed class AsignarMecanicoHandler : IRequestHandler<AsignarMecanico>
{
    private readonly IUnitOfWork _uow;
    public AsignarMecanicoHandler(IUnitOfWork uow) => _uow = uow;

    public async Task Handle(AsignarMecanico req, CancellationToken ct)
    {
        var orden = await _uow.Ordenes.GetByIdAsync(req.OrdenId, ct)
            ?? throw new KeyNotFoundException($"Orden {req.OrdenId} no encontrada.");
        orden.AsignarMecanico(req.MecanicoId);
        await _uow.Ordenes.UpdateAsync(orden, ct);
        await _uow.SaveChangesAsync(ct);
    }
}

public sealed class RegistrarDiagnosticoHandler : IRequestHandler<RegistrarDiagnostico>
{
    private readonly IUnitOfWork _uow;
    public RegistrarDiagnosticoHandler(IUnitOfWork uow) => _uow = uow;

    public async Task Handle(RegistrarDiagnostico req, CancellationToken ct)
    {
        var orden = await _uow.Ordenes.GetByIdAsync(req.OrdenId, ct)
            ?? throw new KeyNotFoundException($"Orden {req.OrdenId} no encontrada.");
        orden.RegistrarDiagnostico(req.Diagnostico, req.PropuestaReparacion);
        await _uow.Ordenes.UpdateAsync(orden, ct);
        await _uow.SaveChangesAsync(ct);
    }
}

public sealed class AprobarOrdenJefeHandler : IRequestHandler<AprobarOrdenJefe>
{
    private readonly IUnitOfWork _uow;
    public AprobarOrdenJefeHandler(IUnitOfWork uow) => _uow = uow;

    public async Task Handle(AprobarOrdenJefe req, CancellationToken ct)
    {
        var orden = await _uow.Ordenes.GetByIdAsync(req.OrdenId, ct)
            ?? throw new KeyNotFoundException($"Orden {req.OrdenId} no encontrada.");
        orden.AprobarPorJefe();
        await _uow.Ordenes.UpdateAsync(orden, ct);
        await _uow.SaveChangesAsync(ct);
    }
}

public sealed class AprobarOrdenClienteHandler : IRequestHandler<AprobarOrdenCliente>
{
    private readonly IUnitOfWork _uow;
    public AprobarOrdenClienteHandler(IUnitOfWork uow) => _uow = uow;

    public async Task Handle(AprobarOrdenCliente req, CancellationToken ct)
    {
        var orden = await _uow.Ordenes.GetByIdAsync(req.OrdenId, ct)
            ?? throw new KeyNotFoundException($"Orden {req.OrdenId} no encontrada.");
        orden.AprobarPorCliente();
        await _uow.Ordenes.UpdateAsync(orden, ct);
        await _uow.SaveChangesAsync(ct);
    }
}

public sealed class RechazarOrdenClienteHandler : IRequestHandler<RechazarOrdenCliente>
{
    private readonly IUnitOfWork _uow;
    public RechazarOrdenClienteHandler(IUnitOfWork uow) => _uow = uow;

    public async Task Handle(RechazarOrdenCliente req, CancellationToken ct)
    {
        var orden = await _uow.Ordenes.GetByIdAsync(req.OrdenId, ct)
            ?? throw new KeyNotFoundException($"Orden {req.OrdenId} no encontrada.");
        orden.RechazarPorCliente(req.Motivo);
        await _uow.Ordenes.UpdateAsync(orden, ct);
        await _uow.SaveChangesAsync(ct);
    }
}

public sealed class IniciarReparacionHandler : IRequestHandler<IniciarReparacion>
{
    private readonly IUnitOfWork _uow;
    public IniciarReparacionHandler(IUnitOfWork uow) => _uow = uow;

    public async Task Handle(IniciarReparacion req, CancellationToken ct)
    {
        var orden = await _uow.Ordenes.GetByIdAsync(req.OrdenId, ct)
            ?? throw new KeyNotFoundException($"Orden {req.OrdenId} no encontrada.");
        orden.IniciarReparacion();
        await _uow.Ordenes.UpdateAsync(orden, ct);
        await _uow.SaveChangesAsync(ct);
    }
}

public sealed class CompletarOrdenHandler : IRequestHandler<CompletarOrden>
{
    private readonly IUnitOfWork _uow;
    public CompletarOrdenHandler(IUnitOfWork uow) => _uow = uow;

    public async Task Handle(CompletarOrden req, CancellationToken ct)
    {
        var orden = await _uow.Ordenes.GetByIdAsync(req.OrdenId, ct)
            ?? throw new KeyNotFoundException($"Orden {req.OrdenId} no encontrada.");
        orden.Completar();
        await _uow.Ordenes.UpdateAsync(orden, ct);
        await _uow.SaveChangesAsync(ct);
    }
}

public sealed class CancelarOrdenHandler : IRequestHandler<CancelarOrden>
{
    private readonly IUnitOfWork _uow;
    public CancelarOrdenHandler(IUnitOfWork uow) => _uow = uow;

    public async Task Handle(CancelarOrden req, CancellationToken ct)
    {
        var orden = await _uow.Ordenes.GetByIdAsync(req.OrdenId, ct)
            ?? throw new KeyNotFoundException($"Orden {req.OrdenId} no encontrada.");
        orden.Cancelar(req.Motivo);
        await _uow.Ordenes.UpdateAsync(orden, ct);
        await _uow.SaveChangesAsync(ct);
    }
}

public sealed class GetOrdenByIdHandler : IRequestHandler<GetOrdenById, OrdenServicio?>
{
    private readonly IUnitOfWork _uow;
    public GetOrdenByIdHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<OrdenServicio?> Handle(GetOrdenById req, CancellationToken ct)
        => await _uow.Ordenes.GetByIdWithDetailsAsync(req.Id, ct);
}

public sealed class GetOrdenesPagedHandler
    : IRequestHandler<GetOrdenesPaged, (IReadOnlyList<OrdenServicio> Items, int Total)>
{
    private readonly IUnitOfWork _uow;
    public GetOrdenesPagedHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<(IReadOnlyList<OrdenServicio> Items, int Total)> Handle(GetOrdenesPaged req, CancellationToken ct)
    {
        var items = await _uow.Ordenes.GetPagedAsync(req.Page, req.PageSize, req.Search, ct);
        var total = await _uow.Ordenes.CountAsync(req.Search, ct);
        return (items, total);
    }
}
