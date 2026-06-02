using Application.Abstractions;
using Domain.Entities.Taller;
using Domain.Enums;
using FluentValidation;
using MediatR;

namespace Application.UseCase.Pagos;

public sealed record RegistrarPago(int FacturaId, MetodoPago MetodoPago, decimal Monto, string? ReferenciaTransaccion) : IRequest<int>;
public sealed record GetPagosByFactura(int FacturaId) : IRequest<IReadOnlyList<Pago>>;

public sealed class RegistrarPagoValidator : AbstractValidator<RegistrarPago>
{
    public RegistrarPagoValidator()
    {
        RuleFor(x => x.FacturaId).GreaterThan(0);
        RuleFor(x => x.Monto).GreaterThan(0);
    }
}

public sealed class RegistrarPagoHandler : IRequestHandler<RegistrarPago, int>
{
    private readonly IUnitOfWork _uow;
    public RegistrarPagoHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<int> Handle(RegistrarPago req, CancellationToken ct)
    {
        var factura = await _uow.Facturas.GetByIdAsync(req.FacturaId, ct)
            ?? throw new KeyNotFoundException($"Factura {req.FacturaId} no encontrada.");

        var pago = new Pago { FacturaId = req.FacturaId, MetodoPago = req.MetodoPago, Monto = req.Monto, ReferenciaTransaccion = req.ReferenciaTransaccion, Estado = EstadoPago.Pagado };
        await _uow.Pagos.AddAsync(pago, ct);

        var pagosExistentes = await _uow.Pagos.GetByFacturaIdAsync(req.FacturaId, ct);
        var totalPagado = pagosExistentes.Sum(p => p.Monto) + req.Monto;
        if (totalPagado >= factura.TotalFinal) factura.MarcarPagada();
        await _uow.Facturas.UpdateAsync(factura, ct);
        await _uow.SaveChangesAsync(ct);
        return pago.Id;
    }
}

public sealed class GetPagosByFacturaHandler : IRequestHandler<GetPagosByFactura, IReadOnlyList<Pago>>
{
    private readonly IUnitOfWork _uow;
    public GetPagosByFacturaHandler(IUnitOfWork uow) => _uow = uow;
    public async Task<IReadOnlyList<Pago>> Handle(GetPagosByFactura req, CancellationToken ct)
        => await _uow.Pagos.GetByFacturaIdAsync(req.FacturaId, ct);
}
