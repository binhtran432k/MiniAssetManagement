using Ardalis.Result;
using Ardalis.SharedKernel;

namespace MiniAssetManagement.UseCases.Drives.Create;

public record CreateDriveCommand(string Name, int OwnerId) : ICommand<Result<int>>;
