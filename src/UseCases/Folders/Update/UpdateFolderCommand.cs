using Ardalis.Result;
using Ardalis.SharedKernel;

namespace MiniAssetManagement.UseCases.Folders.Update;

public record UpdateFolderCommand(int FolderId, int UserId, string NewName) : ICommand<Result<FolderDTO>>;