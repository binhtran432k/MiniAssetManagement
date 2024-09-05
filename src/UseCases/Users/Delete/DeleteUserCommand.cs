using Ardalis.Result;
using Ardalis.SharedKernel;

namespace MiniAssetManagement.UseCases.Users.Delete;

public record DeleteUserCommand(int UserId) : ICommand<Result>;