using Ardalis.Result;
using Ardalis.SharedKernel;

namespace MiniAssetManagement.UseCases.Folders.Create;

public record CreateFolderFromFolderCommand(string Name, int AdminId, int FolderId) : ICommand<Result<int>>;