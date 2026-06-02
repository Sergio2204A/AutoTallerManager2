using Domain.Enums;

namespace Domain.Entities.Taller;

public class Factura : BaseEntity<int>
{
    public string NumeroFactura { get; set; } = string.Empty;
    public int OrdenServicioId { get; set; }
    public decimal ManoObra { get; set; }
    public decimal TotalRepuestos { get; set; }
    public decimal Impuestos { get; set; }
    public decimal TotalFinal { get; set; }
    public EstadoFactura EstadoPago { get; set; } = EstadoFactura.Pendiente;

    // Navigation properties
    public OrdenServicio? OrdenServicio { get; set; }
    public ICollection<Pago> Pagos { get; set; } = new HashSet<Pago>();

    public void Calcular(decimal manoObra, decimal totalRepuestos, decimal porcentajeImpuesto = 0.19m)
    {
        ManoObra = manoObra;
        TotalRepuestos = totalRepuestos;
        var subtotal = manoObra + totalRepuestos;
        Impuestos = subtotal * porcentajeImpuesto;
        TotalFinal = subtotal + Impuestos;
    }

    public void Emitir()
    {
        if (EstadoPago != EstadoFactura.Pendiente)
            throw new InvalidOperationException("Solo se pueden emitir facturas pendientes.");
        EstadoPago = EstadoFactura.Emitida;
    }

    public void MarcarPagada()
    {
        EstadoPago = EstadoFactura.Pagada;
    }

    public void Anular()
    {
        if (EstadoPago == EstadoFactura.Pagada)
            throw new InvalidOperationException("No se puede anular una factura ya pagada.");
        EstadoPago = EstadoFactura.Anulada;
    }
}
