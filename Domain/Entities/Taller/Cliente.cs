using Domain.Enums;

namespace Domain.Entities.Taller;

public class Cliente : BaseEntity<int>
{
    public string NombreCompleto { get; set; } = string.Empty;
    public string Cedula { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public EstadoCuenta EstadoCuenta { get; set; } = EstadoCuenta.Activa;

    // Navigation properties
    public ICollection<Vehiculo> Vehiculos { get; set; } = new HashSet<Vehiculo>();
    public ICollection<OrdenServicio> Ordenes { get; set; } = new HashSet<OrdenServicio>();
}
