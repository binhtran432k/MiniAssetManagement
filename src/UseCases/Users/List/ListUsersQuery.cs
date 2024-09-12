using Ardalis.Result;
using Ardalis.SharedKernel;

namespace MiniAssetManagement.UseCases.Users.List;

public record ListUsersQuery(int? Skip = null, int? Take = null)
    : IQuery<Result<(IEnumerable<UserDTO>, int)>>;
