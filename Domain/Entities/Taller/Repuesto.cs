namespace Domain.Entities.Taller;

public class Repuesto : BaseEntity<int>
{
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public Domain.Enums.CategoriaRepuesto Categoria { get; set; } = Domain.Enums.CategoriaRepuesto.Otros;
    public int CantidadStock { get; set; }
    public int StockMinimo { get; set; }
    public decimal PrecioCompra { get; set; }
    public decimal PrecioVenta { get; set; }

    // Navigation properties
    public ICollection<DetalleOrden> DetallesOrden { get; set; } = new HashSet<DetalleOrden>();

    // Domain methods
    public void DescontarStock(int cantidad)
    {
        if (cantidad <= 0)
            throw new ArgumentException("La cantidad debe ser positiva.", nameof(cantidad));
        if (CantidadStock < cantidad)
            throw new InvalidOperationException($"Stock insuficiente para '{Nombre}'. Disponible: {CantidadStock}, Solicitado: {cantidad}");
        CantidadStock -= cantidad;
    }

    public void AgregarStock(int cantidad)
    {
        if (cantidad <= 0)
            throw new ArgumentException("La cantidad debe ser positiva.", nameof(cantidad));
        CantidadStock += cantidad;
    }

    public void ActualizarPrecios(decimal precioCompra, decimal precioVenta)
    {
        if (precioCompra < 0) throw new ArgumentException("El precio de compra no puede ser negativo.");
        if (precioVenta < 0) throw new ArgumentException("El precio de venta no puede ser negativo.");
        PrecioCompra = precioCompra;
        PrecioVenta = precioVenta;
    }

    public bool TieneStockBajo() => CantidadStock <= StockMinimo;
}
