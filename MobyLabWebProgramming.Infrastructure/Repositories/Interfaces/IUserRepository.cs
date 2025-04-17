using MobyLabWebProgramming.Core.Entities;
using MobyLabWebProgramming.Core.Requests;
using MobyLabWebProgramming.Core.Responses;

namespace MobyLabWebProgramming.Infrastructure.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<List<User>> GetUsersAsync(CancellationToken cancellationToken = default);
    Task<PagedResponse<User>> GetUsersPageAsync(PaginationQueryParams pagination, CancellationToken cancellationToken = default);
    Task<User> AddUserAsync(User user, CancellationToken cancellationToken = default);
    Task<User> UpdateUserAsync(User user, CancellationToken cancellationToken = default);
    Task<int> DeleteUserAsync(Guid id, CancellationToken cancellationToken = default);
}