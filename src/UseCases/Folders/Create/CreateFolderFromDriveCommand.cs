using Ardalis.Result;
using Ardalis.SharedKernel;

namespace MiniAssetManagement.UseCases.Folders.Create;

public record CreateFolderFromDriveCommand(string Name, int AdminId, int DriveId) : ICommand<Result<int>>;