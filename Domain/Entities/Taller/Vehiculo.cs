namespace Domain.Entities.Taller;

public class Vehiculo : BaseEntity<int>
{
    public string Marca { get; set; } = string.Empty;
    public string Modelo { get; set; } = string.Empty;
    public int Anio { get; set; }
    public string Vin { get; set; } = string.Empty;
    public int Kilometraje { get; set; }
    public string Placa { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;

    // Foreign keys
    public int ClienteId { get; set; }

    // Navigation properties
    public Cliente? Cliente { get; set; }
    public ICollection<OrdenServicio> Ordenes { get; set; } = new HashSet<OrdenServicio>();
}
