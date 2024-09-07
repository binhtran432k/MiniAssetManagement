using Ardalis.Result;
using Ardalis.SharedKernel;

namespace MiniAssetManagement.UseCases.Folders.Get;

public record GetFolderQuery(int FolderId, int UserId) : IQuery<Result<FolderDTO>>;