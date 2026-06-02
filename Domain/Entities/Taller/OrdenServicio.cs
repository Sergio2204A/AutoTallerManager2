using Domain.Entities.Auth;
using Domain.Enums;

namespace Domain.Entities.Taller;

public class OrdenServicio : BaseEntity<int>
{
    public string NumeroOrden { get; set; } = string.Empty;
    public int VehiculoId { get; set; }
    public int ClienteId { get; set; }
    public EstadoOrden Estado { get; set; } = EstadoOrden.Pendiente;
    public DateTime FechaIngreso { get; set; } = DateTime.UtcNow;
    public DateTime? FechaEstimadaEntrega { get; set; }
    public int? MecanicoId { get; set; }
    public string? Diagnostico { get; set; }
    public string? PropuestaReparacion { get; set; }
    public string? Observaciones { get; set; }

    // Navigation properties
    public Vehiculo? Vehiculo { get; set; }
    public Cliente? Cliente { get; set; }
    public UserMember? Mecanico { get; set; }
    public ICollection<DetalleOrden> Detalles { get; set; } = new HashSet<DetalleOrden>();
    public Factura? Factura { get; set; }

    // Domain methods
    public void AsignarMecanico(int mecanicoId)
    {
        if (Estado != EstadoOrden.Pendiente)
            throw new InvalidOperationException("Solo se puede asignar mecánico a órdenes pendientes.");
        MecanicoId = mecanicoId;
        Estado = EstadoOrden.Diagnostico;
    }

    public void RegistrarDiagnostico(string diagnostico, string propuesta)
    {
        if (Estado != EstadoOrden.Diagnostico)
            throw new InvalidOperationException("La orden debe estar en estado de diagnóstico.");
        Diagnostico = diagnostico;
        PropuestaReparacion = propuesta;
        Estado = EstadoOrden.EnAprobacion;
    }

    public void AprobarPorJefe()
    {
        if (Estado != EstadoOrden.EnAprobacion)
            throw new InvalidOperationException("La orden debe estar en aprobación para ser revisada por el jefe.");
        // Stays in EnAprobacion until client also approves
    }

    public void AprobarPorCliente()
    {
        if (Estado != EstadoOrden.EnAprobacion)
            throw new InvalidOperationException("La orden debe estar en aprobación para ser aprobada por el cliente.");
        Estado = EstadoOrden.Aprobada;
    }

    public void RechazarPorCliente(string? motivo)
    {
        if (Estado != EstadoOrden.EnAprobacion)
            throw new InvalidOperationException("La orden debe estar en aprobación para ser rechazada.");
        Estado = EstadoOrden.Cancelada;
        if (!string.IsNullOrWhiteSpace(motivo))
            Observaciones = motivo;
    }

    public void IniciarReparacion()
    {
        if (Estado != EstadoOrden.Aprobada)
            throw new InvalidOperationException("La orden debe estar aprobada para iniciar la reparación.");
        Estado = EstadoOrden.EnProceso;
    }

    public void Completar()
    {
        if (Estado != EstadoOrden.EnProceso)
            throw new InvalidOperationException("La orden debe estar en proceso para completarse.");
        Estado = EstadoOrden.Completada;
    }

    public void Cancelar(string? motivo)
    {
        if (Estado == EstadoOrden.Completada || Estado == EstadoOrden.Cancelada)
            throw new InvalidOperationException("No se puede cancelar una orden completada o ya cancelada.");
        Estado = EstadoOrden.Cancelada;
        if (!string.IsNullOrWhiteSpace(motivo))
            Observaciones = motivo;
    }
}
