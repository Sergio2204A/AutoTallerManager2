using Application.Abstractions.Taller;
using Domain.Entities.Taller;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Taller;

public sealed class VehiculoRepository : IVehiculoRepository
{
    private readonly AppDbContext _db;
    public VehiculoRepository(AppDbContext db) => _db = db;

    public async Task<Vehiculo?> GetByIdAsync(int id, CancellationToken ct = default)
        => await _db.Vehiculos.Include(v => v.Cliente).FirstOrDefaultAsync(v => v.Id == id && !v.IsDeleted, ct);

    public async Task<Vehiculo?> GetByPlacaAsync(string placa, CancellationToken ct = default)
        => await _db.Vehiculos.FirstOrDefaultAsync(v => v.Placa == placa && !v.IsDeleted, ct);

    public async Task<Vehiculo?> GetByVinAsync(string vin, CancellationToken ct = default)
        => await _db.Vehiculos.FirstOrDefaultAsync(v => v.Vin == vin && !v.IsDeleted, ct);

    public async Task<IReadOnlyList<Vehiculo>> GetByClienteIdAsync(int clienteId, CancellationToken ct = default)
        => await _db.Vehiculos.Where(v => v.ClienteId == clienteId && !v.IsDeleted).ToListAsync(ct);

    public async Task<IReadOnlyList<Vehiculo>> GetPagedAsync(int page, int pageSize, string? search = null, CancellationToken ct = default)
    {
        var q = _db.Vehiculos.Where(v => !v.IsDeleted).Include(v => v.Cliente);
        if (!string.IsNullOrWhiteSpace(search))
            q = q.Where(v => v.Placa.Contains(search) || v.Marca.Contains(search) || v.Modelo.Contains(search) || v.Vin.Contains(search));
        return await q.OrderBy(v => v.Placa).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(ct);
    }

    public async Task<int> CountAsync(string? search = null, CancellationToken ct = default)
    {
        var q = _db.Vehiculos.Where(v => !v.IsDeleted);
        if (!string.IsNullOrWhiteSpace(search))
            q = q.Where(v => v.Placa.Contains(search) || v.Marca.Contains(search) || v.Modelo.Contains(search));
        return await q.CountAsync(ct);
    }

    public async Task AddAsync(Vehiculo vehiculo, CancellationToken ct = default)
        => await _db.Vehiculos.AddAsync(vehiculo, ct);

    public Task UpdateAsync(Vehiculo vehiculo, CancellationToken ct = default)
    { _db.Vehiculos.Update(vehiculo); return Task.CompletedTask; }

    public Task RemoveAsync(Vehiculo vehiculo, CancellationToken ct = default)
    { _db.Vehiculos.Remove(vehiculo); return Task.CompletedTask; }
}
