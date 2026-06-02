namespace Domain.Entities.Taller;

public class DetalleOrden : BaseEntity<int>
{
    public int OrdenServicioId { get; set; }
    public int RepuestoId { get; set; }
    public int CantidadUtilizada { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal Subtotal { get; set; }

    // Navigation properties
    public OrdenServicio? OrdenServicio { get; set; }
    public Repuesto? Repuesto { get; set; }

    public void Calcular()
    {
        Subtotal = CantidadUtilizada * PrecioUnitario;
    }
}
