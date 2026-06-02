using MediatR;

namespace Application.UseCase.Ordenes;

public sealed record CreateOrden(int VehiculoId, int ClienteId, DateTime? FechaEstimadaEntrega, string? Observaciones) : IRequest<int>;
public sealed record AsignarMecanico(int OrdenId, int MecanicoId) : IRequest;
public sealed record RegistrarDiagnostico(int OrdenId, string Diagnostico, string PropuestaReparacion) : IRequest;
public sealed record AprobarOrdenJefe(int OrdenId) : IRequest;
public sealed record AprobarOrdenCliente(int OrdenId) : IRequest;
public sealed record RechazarOrdenCliente(int OrdenId, string? Motivo) : IRequest;
public sealed record IniciarReparacion(int OrdenId) : IRequest;
public sealed record CompletarOrden(int OrdenId) : IRequest;
public sealed record CancelarOrden(int OrdenId, string? Motivo) : IRequest;
public sealed record GetOrdenById(int Id) : IRequest<Domain.Entities.Taller.OrdenServicio?>;
public sealed record GetOrdenesPaged(int Page, int PageSize, string? Search = null)
    : IRequest<(IReadOnlyList<Domain.Entities.Taller.OrdenServicio> Items, int Total)>;
