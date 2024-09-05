using Ardalis.Result;
using Ardalis.SharedKernel;

namespace MiniAssetManagement.UseCases.Users.Create;

public record CreateUserCommand(string Username) : ICommand<Result<int>>;
