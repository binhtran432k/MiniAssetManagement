using MiniAssetManagement.Core.UserAggregate;
using MiniAssetManagement.UseCases.Users;
using MiniAssetManagement.UseCases.Users.List;

namespace MiniAssetManagement.Infrastructure.Data.Queries;

public class ListUsersQueryService(AppDbContext db) : IListUsersQueryService
{
    public Task<(IEnumerable<UserDTO>, int)> ListAsync(int? skip = null, int? take = null)
    {
        return ListQuery.ListAsync(
            db.Users.Where(u => u.Status == UserStatus.Available)
                .Select(u => new UserDTO(u.Id, u.Username)),
            skip,
            take
        );
    }
}
