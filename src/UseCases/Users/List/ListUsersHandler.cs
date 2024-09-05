using Ardalis.Result;
using Ardalis.SharedKernel;

namespace MiniAssetManagement.UseCases.Users.List;

public class ListUsersHandler(IListUsersQueryService _query)
    : IQueryHandler<ListUsersQuery, Result<IEnumerable<UserDTO>>>
{
    public async Task<Result<IEnumerable<UserDTO>>> Handle(
        ListUsersQuery request,
        CancellationToken cancellationToken
    )
    {
        var result = await _query.ListAsync();

        return Result.Success(result);
    }
}
