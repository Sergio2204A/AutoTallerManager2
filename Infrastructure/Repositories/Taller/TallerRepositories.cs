using Application.Abstractions.Taller;
using Domain.Entities.Taller;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Taller;

public sealed class RepuestoRepository : IRepuestoRepository
{
    private readonly AppDbContext _db;
    public RepuestoRepository(AppDbContext db) => _db = db;

    public async Task<Repuesto?> GetByIdAsync(int id, CancellationToken ct = default)
        => await _db.Repuestos.FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted, ct);

    public async Task<Repuesto?> GetByCodigoAsync(string codigo, CancellationToken ct = default)
        => await _db.Repuestos.FirstOrDefaultAsync(r => r.Codigo == codigo && !r.IsDeleted, ct);

    public async Task<IReadOnlyList<Repuesto>> GetPagedAsync(int page, int pageSize, string? search = null, CancellationToken ct = default)
    {
        var q = _db.Repuestos.Where(r => !r.IsDeleted);
        if (!string.IsNullOrWhiteSpace(search))
            q = q.Where(r => r.Nombre.Contains(search) || r.Codigo.Contains(search));
        return await q.OrderBy(r => r.Nombre).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(ct);
    }

    public async Task<int> CountAsync(string? search = null, CancellationToken ct = default)
    {
        var q = _db.Repuestos.Where(r => !r.IsDeleted);
        if (!string.IsNullOrWhiteSpace(search))
            q = q.Where(r => r.Nombre.Contains(search) || r.Codigo.Contains(search));
        return await q.CountAsync(ct);
    }

    public async Task AddAsync(Repuesto repuesto, CancellationToken ct = default)
        => await _db.Repuestos.AddAsync(repuesto, ct);

    public Task UpdateAsync(Repuesto repuesto, CancellationToken ct = default)
    { _db.Repuestos.Update(repuesto); return Task.CompletedTask; }

    public Task RemoveAsync(Repuesto repuesto, CancellationToken ct = default)
    { _db.Repuestos.Remove(repuesto); return Task.CompletedTask; }

    public async Task<IReadOnlyList<Repuesto>> GetBajosStockAsync(CancellationToken ct = default)
        => await _db.Repuestos.Where(r => !r.IsDeleted && r.CantidadStock <= r.StockMinimo).OrderBy(r => r.Nombre).ToListAsync(ct);

    public async Task<bool> ExistsCodigoAsync(string codigo, CancellationToken ct = default)
        => await _db.Repuestos.AnyAsync(r => r.Codigo == codigo && !r.IsDeleted, ct);
}

public sealed class FacturaRepository : IFacturaRepository
{
    private readonly AppDbContext _db;
    public FacturaRepository(AppDbContext db) => _db = db;

    public async Task<Factura?> GetByIdAsync(int id, CancellationToken ct = default)
        => await _db.Facturas.Include(f => f.Pagos).FirstOrDefaultAsync(f => f.Id == id, ct);

    public async Task<Factura?> GetByNumeroAsync(string numero, CancellationToken ct = default)
        => await _db.Facturas.FirstOrDefaultAsync(f => f.NumeroFactura == numero, ct);

    public async Task<Factura?> GetByOrdenIdAsync(int ordenId, CancellationToken ct = default)
        => await _db.Facturas.Include(f => f.Pagos).FirstOrDefaultAsync(f => f.OrdenServicioId == ordenId, ct);

    public async Task<IReadOnlyList<Factura>> GetPagedAsync(int page, int pageSize, string? search = null, CancellationToken ct = default)
    {
        var q = _db.Facturas.AsQueryable();
        if (!string.IsNullOrWhiteSpace(search))
            q = q.Where(f => f.NumeroFactura.Contains(search));
        return await q.OrderByDescending(f => f.CreatedAt).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(ct);
    }

    public async Task<int> CountAsync(string? search = null, CancellationToken ct = default)
    {
        var q = _db.Facturas.AsQueryable();
        if (!string.IsNullOrWhiteSpace(search))
            q = q.Where(f => f.NumeroFactura.Contains(search));
        return await q.CountAsync(ct);
    }

    public async Task AddAsync(Factura factura, CancellationToken ct = default)
        => await _db.Facturas.AddAsync(factura, ct);

    public Task UpdateAsync(Factura factura, CancellationToken ct = default)
    { _db.Facturas.Update(factura); return Task.CompletedTask; }
}

public sealed class PagoRepository : IPagoRepository
{
    private readonly AppDbContext _db;
    public PagoRepository(AppDbContext db) => _db = db;

    public async Task<Pago?> GetByIdAsync(int id, CancellationToken ct = default)
        => await _db.Pagos.FirstOrDefaultAsync(p => p.Id == id, ct);

    public async Task<IReadOnlyList<Pago>> GetByFacturaIdAsync(int facturaId, CancellationToken ct = default)
        => await _db.Pagos.Where(p => p.FacturaId == facturaId).ToListAsync(ct);

    public async Task AddAsync(Pago pago, CancellationToken ct = default)
        => await _db.Pagos.AddAsync(pago, ct);

    public Task UpdateAsync(Pago pago, CancellationToken ct = default)
    { _db.Pagos.Update(pago); return Task.CompletedTask; }
}

public sealed class AuditoriaRepository : IAuditoriaRepository
{
    private readonly AppDbContext _db;
    public AuditoriaRepository(AppDbContext db) => _db = db;

    public async Task<IReadOnlyList<Auditoria>> GetPagedAsync(int page, int pageSize, string? search = null, CancellationToken ct = default)
    {
        var q = _db.Auditorias.AsQueryable();
        if (!string.IsNullOrWhiteSpace(search))
            q = q.Where(a => a.EntidadAfectada.Contains(search) || a.AccionRealizada.Contains(search));
        return await q.OrderByDescending(a => a.Fecha).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(ct);
    }

    public async Task<int> CountAsync(string? search = null, CancellationToken ct = default)
    {
        var q = _db.Auditorias.AsQueryable();
        if (!string.IsNullOrWhiteSpace(search))
            q = q.Where(a => a.EntidadAfectada.Contains(search));
        return await q.CountAsync(ct);
    }

    public async Task AddAsync(Auditoria auditoria, CancellationToken ct = default)
        => await _db.Auditorias.AddAsync(auditoria, ct);

    public async Task<IReadOnlyList<Auditoria>> GetByEntidadAsync(string entidad, CancellationToken ct = default)
        => await _db.Auditorias.Where(a => a.EntidadAfectada == entidad).OrderByDescending(a => a.Fecha).ToListAsync(ct);
}
