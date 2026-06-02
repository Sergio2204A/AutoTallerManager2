using Domain.Entities.Taller;
using Domain.Enums;

namespace Domain.Entities.Auth;

public class UserMember : BaseEntity<int>
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public EstadoCuenta Estado { get; set; } = EstadoCuenta.Activa;
    public ICollection<Rol> Rols { get; set; } = new HashSet<Rol>();
    public ICollection<UserMemberRol> UserMemberRols { get; set; } = new HashSet<UserMemberRol>();
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new HashSet<RefreshToken>();
    public ICollection<Auditoria> Auditorias { get; set; } = new HashSet<Auditoria>();
    public ICollection<OrdenServicio> OrdenesAsignadas { get; set; } = new HashSet<OrdenServicio>();
}