using Ardalis.Result;
using Ardalis.SharedKernel;

namespace MiniAssetManagement.UseCases.Users.List;

public record ListUsersQuery(int? Skip, int? Take) : IQuery<Result<IEnumerable<UserDTO>>>;
