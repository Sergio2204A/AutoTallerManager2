using Domain.Enums;

namespace Domain.Entities.Taller;

public class Pago : BaseEntity<int>
{
    public int FacturaId { get; set; }
    public MetodoPago MetodoPago { get; set; }
    public DateTime FechaPago { get; set; } = DateTime.UtcNow;
    public decimal Monto { get; set; }
    public string? ReferenciaTransaccion { get; set; }
    public EstadoPago Estado { get; set; } = EstadoPago.Pendiente;

    // Navigation properties
    public Factura? Factura { get; set; }
}
