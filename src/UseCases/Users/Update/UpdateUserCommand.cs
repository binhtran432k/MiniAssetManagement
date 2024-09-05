using Ardalis.Result;
using Ardalis.SharedKernel;

namespace MiniAssetManagement.UseCases.Users.Update;

public record UpdateUserCommand(int UserId, string NewUsername) : ICommand<Result<UserDTO>>;
