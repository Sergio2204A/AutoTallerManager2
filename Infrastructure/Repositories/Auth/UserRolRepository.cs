using Application.Abstractions.Auth;
using Domain.Entities.Auth;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Auth;

public sealed class UserRolRepository(AppDbContext db) : IUserMemberRolService
{
    public async Task<IEnumerable<UserMemberRol>> GetAllAsync()
        => await db.Set<UserMemberRol>().AsNoTracking().ToListAsync();

    public void Remove(UserMemberRol entity)
        => db.Set<UserMemberRol>().Remove(entity);

    public void Update(UserMemberRol entity)
        => db.Set<UserMemberRol>().Update(entity);

    public Task<UserMemberRol?> GetByIdsAsync(int userMemberId, int roleId)
        => db.Set<UserMemberRol>()
             .AsNoTracking()
             .FirstOrDefaultAsync(ur => ur.UserMemberId == userMemberId && ur.RolId == roleId);
}
