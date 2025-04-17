using Microsoft.EntityFrameworkCore;
using MobyLabWebProgramming.Core.Entities;
using MobyLabWebProgramming.Core.Requests;
using MobyLabWebProgramming.Core.Responses;
using MobyLabWebProgramming.Infrastructure.Database;
using MobyLabWebProgramming.Infrastructure.Repositories.Interfaces;

namespace MobyLabWebProgramming.Infrastructure.Repositories.Implementation;

public class UserRepository(WebAppDatabaseContext dbContext) : IUserRepository
{
    public async Task<User?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await dbContext.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

    public async Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default) =>
        await dbContext.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

    public async Task<List<User>> GetUsersAsync(CancellationToken cancellationToken = default) =>
        await dbContext.Users.ToListAsync(cancellationToken);

    public async Task<PagedResponse<User>> GetUsersPageAsync(PaginationQueryParams pagination, CancellationToken cancellationToken = default)
    {
        var totalCount = await dbContext.Users.CountAsync(cancellationToken);
        var users = await dbContext.Users
            .OrderBy(u => u.Name)
            .Skip((pagination.Page - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync(cancellationToken);

        return new PagedResponse<User>(pagination.Page, pagination.PageSize, totalCount, users);
    }

    public async Task<User> AddUserAsync(User user, CancellationToken cancellationToken = default)
    {
        await dbContext.Users.AddAsync(user, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return user;
    }

    public async Task<User> UpdateUserAsync(User user, CancellationToken cancellationToken = default)
    {
        user.UpdateTime();
        dbContext.Entry(user).State = EntityState.Modified;
        await dbContext.SaveChangesAsync(cancellationToken);
        return user;
    }

    public async Task<int> DeleteUserAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await GetUserByIdAsync(id, cancellationToken);
        if (user == null) return 0;

        dbContext.Users.Remove(user);
        return await dbContext.SaveChangesAsync(cancellationToken);
    }
}
