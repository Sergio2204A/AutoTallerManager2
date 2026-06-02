using Domain.Entities.Taller;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public sealed class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
{
    public void Configure(EntityTypeBuilder<Cliente> b)
    {
        b.ToTable("Clientes");
        b.HasKey(c => c.Id);
        b.Property(c => c.NombreCompleto).IsRequired().HasMaxLength(150);
        b.Property(c => c.Cedula).IsRequired().HasMaxLength(20);
        b.HasIndex(c => c.Cedula).IsUnique();
        b.Property(c => c.Telefono).IsRequired().HasMaxLength(20);
        b.Property(c => c.Email).IsRequired().HasMaxLength(100);
        b.Property(c => c.PasswordHash).IsRequired();
        b.Property(c => c.EstadoCuenta).HasConversion<string>();
        b.HasQueryFilter(c => !c.IsDeleted);
    }
}

public sealed class VehiculoConfiguration : IEntityTypeConfiguration<Vehiculo>
{
    public void Configure(EntityTypeBuilder<Vehiculo> b)
    {
        b.ToTable("Vehiculos");
        b.HasKey(v => v.Id);
        b.Property(v => v.Marca).IsRequired().HasMaxLength(50);
        b.Property(v => v.Modelo).IsRequired().HasMaxLength(50);
        b.Property(v => v.Vin).IsRequired().HasMaxLength(17);
        b.HasIndex(v => v.Vin).IsUnique();
        b.Property(v => v.Placa).IsRequired().HasMaxLength(10);
        b.HasIndex(v => v.Placa).IsUnique();
        b.Property(v => v.Color).IsRequired().HasMaxLength(30);
        b.HasOne(v => v.Cliente).WithMany(c => c.Vehiculos).HasForeignKey(v => v.ClienteId).OnDelete(DeleteBehavior.Restrict);
        b.HasQueryFilter(v => !v.IsDeleted);
    }
}

public sealed class OrdenServicioConfiguration : IEntityTypeConfiguration<OrdenServicio>
{
    public void Configure(EntityTypeBuilder<OrdenServicio> b)
    {
        b.ToTable("OrdenesServicio");
        b.HasKey(o => o.Id);
        b.Property(o => o.NumeroOrden).IsRequired().HasMaxLength(50);
        b.HasIndex(o => o.NumeroOrden).IsUnique();
        b.Property(o => o.Estado).HasConversion<string>();
        b.Property(o => o.Diagnostico).HasMaxLength(2000);
        b.Property(o => o.PropuestaReparacion).HasMaxLength(2000);
        b.Property(o => o.Observaciones).HasMaxLength(1000);
        b.HasOne(o => o.Vehiculo).WithMany(v => v.Ordenes).HasForeignKey(o => o.VehiculoId).OnDelete(DeleteBehavior.Restrict);
        b.HasOne(o => o.Cliente).WithMany(c => c.Ordenes).HasForeignKey(o => o.ClienteId).OnDelete(DeleteBehavior.Restrict);
        b.HasOne(o => o.Mecanico).WithMany(u => u.OrdenesAsignadas).HasForeignKey(o => o.MecanicoId).IsRequired(false).OnDelete(DeleteBehavior.SetNull);
    }
}

public sealed class RepuestoConfiguration : IEntityTypeConfiguration<Repuesto>
{
    public void Configure(EntityTypeBuilder<Repuesto> b)
    {
        b.ToTable("Repuestos");
        b.HasKey(r => r.Id);
        b.Property(r => r.Codigo).IsRequired().HasMaxLength(50);
        b.HasIndex(r => r.Codigo).IsUnique();
        b.Property(r => r.Nombre).IsRequired().HasMaxLength(150);
        b.Property(r => r.Descripcion).HasMaxLength(500);
        b.Property(r => r.Categoria).HasConversion<string>();
        b.Property(r => r.PrecioCompra).HasColumnType("decimal(18,2)");
        b.Property(r => r.PrecioVenta).HasColumnType("decimal(18,2)");
        b.HasQueryFilter(r => !r.IsDeleted);
    }
}

public sealed class DetalleOrdenConfiguration : IEntityTypeConfiguration<DetalleOrden>
{
    public void Configure(EntityTypeBuilder<DetalleOrden> b)
    {
        b.ToTable("DetallesOrden");
        b.HasKey(d => d.Id);
        b.Property(d => d.PrecioUnitario).HasColumnType("decimal(18,2)");
        b.Property(d => d.Subtotal).HasColumnType("decimal(18,2)");
        b.HasOne(d => d.OrdenServicio).WithMany(o => o.Detalles).HasForeignKey(d => d.OrdenServicioId).OnDelete(DeleteBehavior.Cascade);
        b.HasOne(d => d.Repuesto).WithMany(r => r.DetallesOrden).HasForeignKey(d => d.RepuestoId).OnDelete(DeleteBehavior.Restrict);
    }
}

public sealed class FacturaConfiguration : IEntityTypeConfiguration<Factura>
{
    public void Configure(EntityTypeBuilder<Factura> b)
    {
        b.ToTable("Facturas");
        b.HasKey(f => f.Id);
        b.Property(f => f.NumeroFactura).IsRequired().HasMaxLength(50);
        b.HasIndex(f => f.NumeroFactura).IsUnique();
        b.Property(f => f.ManoObra).HasColumnType("decimal(18,2)");
        b.Property(f => f.TotalRepuestos).HasColumnType("decimal(18,2)");
        b.Property(f => f.Impuestos).HasColumnType("decimal(18,2)");
        b.Property(f => f.TotalFinal).HasColumnType("decimal(18,2)");
        b.Property(f => f.EstadoPago).HasConversion<string>();
        b.HasOne(f => f.OrdenServicio).WithOne(o => o.Factura).HasForeignKey<Factura>(f => f.OrdenServicioId).OnDelete(DeleteBehavior.Restrict);
    }
}

public sealed class PagoConfiguration : IEntityTypeConfiguration<Pago>
{
    public void Configure(EntityTypeBuilder<Pago> b)
    {
        b.ToTable("Pagos");
        b.HasKey(p => p.Id);
        b.Property(p => p.Monto).HasColumnType("decimal(18,2)");
        b.Property(p => p.MetodoPago).HasConversion<string>();
        b.Property(p => p.Estado).HasConversion<string>();
        b.Property(p => p.ReferenciaTransaccion).HasMaxLength(100);
        b.HasOne(p => p.Factura).WithMany(f => f.Pagos).HasForeignKey(p => p.FacturaId).OnDelete(DeleteBehavior.Restrict);
    }
}

public sealed class AuditoriaConfiguration : IEntityTypeConfiguration<Auditoria>
{
    public void Configure(EntityTypeBuilder<Auditoria> b)
    {
        b.ToTable("Auditorias");
        b.HasKey(a => a.Id);
        b.Property(a => a.EntidadAfectada).IsRequired().HasMaxLength(100);
        b.Property(a => a.AccionRealizada).IsRequired().HasMaxLength(200);
        b.Property(a => a.ValoresAnteriores).HasColumnType("text");
        b.Property(a => a.ValoresNuevos).HasColumnType("text");
        b.HasOne(a => a.Usuario).WithMany(u => u.Auditorias).HasForeignKey(a => a.UsuarioId).IsRequired(false).OnDelete(DeleteBehavior.SetNull);
    }
}
