using Application.Abstractions.Taller;
using Domain.Entities.Taller;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Taller;

public sealed class ClienteRepository : IClienteRepository
{
    private readonly AppDbContext _db;
    public ClienteRepository(AppDbContext db) => _db = db;

    public async Task<Cliente?> GetByIdAsync(int id, CancellationToken ct = default)
        => await _db.Clientes.FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted, ct);

    public async Task<Cliente?> GetByCedulaAsync(string cedula, CancellationToken ct = default)
        => await _db.Clientes.FirstOrDefaultAsync(c => c.Cedula == cedula && !c.IsDeleted, ct);

    public async Task<IReadOnlyList<Cliente>> GetAllAsync(CancellationToken ct = default)
        => await _db.Clientes.Where(c => !c.IsDeleted).ToListAsync(ct);

    public async Task<IReadOnlyList<Cliente>> GetPagedAsync(int page, int pageSize, string? search = null, CancellationToken ct = default)
    {
        var q = _db.Clientes.Where(c => !c.IsDeleted);
        if (!string.IsNullOrWhiteSpace(search))
            q = q.Where(c => c.NombreCompleto.Contains(search) || c.Cedula.Contains(search) || c.Email.Contains(search));
        return await q.OrderBy(c => c.NombreCompleto).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(ct);
    }

    public async Task<int> CountAsync(string? search = null, CancellationToken ct = default)
    {
        var q = _db.Clientes.Where(c => !c.IsDeleted);
        if (!string.IsNullOrWhiteSpace(search))
            q = q.Where(c => c.NombreCompleto.Contains(search) || c.Cedula.Contains(search) || c.Email.Contains(search));
        return await q.CountAsync(ct);
    }

    public async Task AddAsync(Cliente cliente, CancellationToken ct = default)
        => await _db.Clientes.AddAsync(cliente, ct);

    public Task UpdateAsync(Cliente cliente, CancellationToken ct = default)
    { _db.Clientes.Update(cliente); return Task.CompletedTask; }

    public Task RemoveAsync(Cliente cliente, CancellationToken ct = default)
    { _db.Clientes.Remove(cliente); return Task.CompletedTask; }

    public async Task<bool> ExistsCedulaAsync(string cedula, CancellationToken ct = default)
        => await _db.Clientes.AnyAsync(c => c.Cedula == cedula && !c.IsDeleted, ct);
}
