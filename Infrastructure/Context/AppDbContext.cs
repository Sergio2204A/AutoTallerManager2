using Domain.Entities.Auth;
using Domain.Entities.Taller;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    // Auth
    public DbSet<UserMember> UsersMembers { get; set; } = default!;
    public DbSet<Rol> Rols { get; set; } = default!;
    public DbSet<UserMemberRol> UserMemberRols { get; set; } = default!;
    public DbSet<RefreshToken> RefreshTokens { get; set; } = default!;

    // Taller
    public DbSet<Cliente> Clientes { get; set; } = default!;
    public DbSet<Vehiculo> Vehiculos { get; set; } = default!;
    public DbSet<OrdenServicio> OrdeneServicio { get; set; } = default!;
    public DbSet<DetalleOrden> DetallesOrden { get; set; } = default!;
    public DbSet<Repuesto> Repuestos { get; set; } = default!;
    public DbSet<Factura> Facturas { get; set; } = default!;
    public DbSet<Pago> Pagos { get; set; } = default!;
    public DbSet<Auditoria> Auditorias { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
