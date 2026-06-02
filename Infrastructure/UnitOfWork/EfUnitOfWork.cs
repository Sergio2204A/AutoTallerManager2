using Application.Abstractions;
using Application.Abstractions.Auth;
using Application.Abstractions.Taller;
using Infrastructure.Context;
using Infrastructure.Repositories.Auth;
using Infrastructure.Repositories.Taller;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.UnitOfWork;

public class EfUnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _db;

    private IClienteRepository? _clientes;
    private IVehiculoRepository? _vehiculos;
    private IOrdenServicioRepository? _ordenes;
    private IRepuestoRepository? _repuestos;
    private IFacturaRepository? _facturas;
    private IPagoRepository? _pagos;
    private IAuditoriaRepository? _auditorias;
    private IUserMemberService? _userMembers;
    private IRolService? _roles;
    private IUserMemberRolService? _userMemberRoles;

    public EfUnitOfWork(AppDbContext db) => _db = db;

    public IClienteRepository Clientes
        => _clientes ??= new ClienteRepository(_db);

    public IVehiculoRepository Vehiculos
        => _vehiculos ??= new VehiculoRepository(_db);

    public IOrdenServicioRepository Ordenes
        => _ordenes ??= new OrdenServicioRepository(_db);

    public IRepuestoRepository Repuestos
        => _repuestos ??= new RepuestoRepository(_db);

    public IFacturaRepository Facturas
        => _facturas ??= new FacturaRepository(_db);

    public IPagoRepository Pagos
        => _pagos ??= new PagoRepository(_db);

    public IAuditoriaRepository Auditorias
        => _auditorias ??= new AuditoriaRepository(_db);

    public IUserMemberService UserMembers
        => _userMembers ??= new UserMemberRepository(_db);

    public IRolService Roles
        => _roles ??= new RolRepository(_db);

    public IUserMemberRolService UserMemberRoles
        => _userMemberRoles ??= new UserRolRepository(_db);

    public Task<int> SaveChangesAsync(CancellationToken ct = default)
        => _db.SaveChangesAsync(ct);

    public async Task ExecuteInTransactionAsync(Func<CancellationToken, Task> operation, CancellationToken ct = default)
    {
        await using var tx = await _db.Database.BeginTransactionAsync(ct);
        try
        {
            await operation(ct);
            await _db.SaveChangesAsync(ct);
            await tx.CommitAsync(ct);
        }
        catch
        {
            await tx.RollbackAsync(ct);
            throw;
        }
    }
}
