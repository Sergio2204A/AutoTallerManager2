using Domain.Entities.Auth;

namespace Domain.Entities.Taller;

public class Auditoria : BaseEntity<int>
{
    public string EntidadAfectada { get; set; } = string.Empty;
    public string AccionRealizada { get; set; } = string.Empty;
    public int? UsuarioId { get; set; }
    public DateTime Fecha { get; set; } = DateTime.UtcNow;
    public string? ValoresAnteriores { get; set; }
    public string? ValoresNuevos { get; set; }

    // Navigation properties
    public UserMember? Usuario { get; set; }
}
