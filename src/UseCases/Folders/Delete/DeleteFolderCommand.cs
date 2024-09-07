using Ardalis.Result;
using Ardalis.SharedKernel;

namespace MiniAssetManagement.UseCases.Folders.Delete;

public record DeleteFolderCommand(int FolderId, int UserId) : ICommand<Result>;