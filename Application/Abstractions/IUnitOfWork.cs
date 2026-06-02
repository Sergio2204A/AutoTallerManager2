using Application.Abstractions.Auth;
using Application.Abstractions.Taller;

namespace Application.Abstractions;

public interface IUnitOfWork
{
    IClienteRepository Clientes { get; }
    IVehiculoRepository Vehiculos { get; }
    IOrdenServicioRepository Ordenes { get; }
    IRepuestoRepository Repuestos { get; }
    IFacturaRepository Facturas { get; }
    IPagoRepository Pagos { get; }
    IAuditoriaRepository Auditorias { get; }
    IUserMemberService UserMembers { get; }
    IUserMemberRolService UserMemberRoles { get; }
    IRolService Roles { get; }
    Task<int> SaveChangesAsync(CancellationToken ct = default);
    Task ExecuteInTransactionAsync(Func<CancellationToken, Task> operation, CancellationToken ct = default);
}
