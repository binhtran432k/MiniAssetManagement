using Ardalis.Result;
using Ardalis.SharedKernel;

namespace MiniAssetManagement.UseCases.Assets.Create;

public record CreateAssetFromDriveCommand(string Name, int AdminId, int DriveId) : ICommand<Result<int>>;
