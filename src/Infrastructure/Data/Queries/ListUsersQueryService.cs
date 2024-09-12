using MiniAssetManagement.UseCases.Users;
using MiniAssetManagement.UseCases.Users.List;

namespace MiniAssetManagement.Infrastructure.Data.Queries;

public class ListUsersQueryService(AppDbContext db) : IListUsersQueryService
{
    public Task<(IEnumerable<UserDTO>, int)> ListAsync(int? skip = null, int? take = null)
    {
        return ListQuery.ListAsync(db.Users.Select(c => new UserDTO(c.Id, c.Username)), skip, take);
    }
}
