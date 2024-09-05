using Ardalis.Result;
using Ardalis.SharedKernel;

namespace MiniAssetManagement.UseCases.Users.Get;

public record GetUserQuery(int UserId) : IQuery<Result<UserDTO>>;
