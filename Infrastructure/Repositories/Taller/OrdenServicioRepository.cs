using Application.Abstractions.Taller;
using Domain.Entities.Taller;
using Domain.Enums;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Taller;

public sealed class OrdenServicioRepository : IOrdenServicioRepository
{
    private readonly AppDbContext _db;
    public OrdenServicioRepository(AppDbContext db) => _db = db;

    public async Task<OrdenServicio?> GetByIdAsync(int id, CancellationToken ct = default)
        => await _db.OrdeneServicio.FirstOrDefaultAsync(o => o.Id == id, ct);

    public async Task<OrdenServicio?> GetByIdWithDetailsAsync(int id, CancellationToken ct = default)
        => await _db.OrdeneServicio
            .Include(o => o.Cliente)
            .Include(o => o.Vehiculo)
            .Include(o => o.Mecanico)
            .Include(o => o.Detalles).ThenInclude(d => d.Repuesto)
            .Include(o => o.Factura)
            .FirstOrDefaultAsync(o => o.Id == id, ct);

    public async Task<OrdenServicio?> GetByNumeroAsync(string numero, CancellationToken ct = default)
        => await _db.OrdeneServicio.FirstOrDefaultAsync(o => o.NumeroOrden == numero, ct);

    public async Task<IReadOnlyList<OrdenServicio>> GetByClienteIdAsync(int clienteId, CancellationToken ct = default)
        => await _db.OrdeneServicio.Where(o => o.ClienteId == clienteId).OrderByDescending(o => o.FechaIngreso).ToListAsync(ct);

    public async Task<IReadOnlyList<OrdenServicio>> GetByMecanicoIdAsync(int mecanicoId, CancellationToken ct = default)
        => await _db.OrdeneServicio.Where(o => o.MecanicoId == mecanicoId).OrderByDescending(o => o.FechaIngreso).ToListAsync(ct);

    public async Task<IReadOnlyList<OrdenServicio>> GetByEstadoAsync(EstadoOrden estado, CancellationToken ct = default)
        => await _db.OrdeneServicio.Where(o => o.Estado == estado).OrderByDescending(o => o.FechaIngreso).ToListAsync(ct);

    public async Task<IReadOnlyList<OrdenServicio>> GetPagedAsync(int page, int pageSize, string? search = null, CancellationToken ct = default)
    {
        var q = _db.OrdeneServicio.Include(o => o.Cliente).Include(o => o.Vehiculo).AsQueryable();
        if (!string.IsNullOrWhiteSpace(search))
            q = q.Where(o => o.NumeroOrden.Contains(search) || o.Cliente!.NombreCompleto.Contains(search) || o.Vehiculo!.Placa.Contains(search));
        return await q.OrderByDescending(o => o.FechaIngreso).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(ct);
    }

    public async Task<int> CountAsync(string? search = null, CancellationToken ct = default)
    {
        var q = _db.OrdeneServicio.AsQueryable();
        if (!string.IsNullOrWhiteSpace(search))
            q = q.Where(o => o.NumeroOrden.Contains(search));
        return await q.CountAsync(ct);
    }

    public async Task AddAsync(OrdenServicio orden, CancellationToken ct = default)
        => await _db.OrdeneServicio.AddAsync(orden, ct);

    public Task UpdateAsync(OrdenServicio orden, CancellationToken ct = default)
    { _db.OrdeneServicio.Update(orden); return Task.CompletedTask; }
}
