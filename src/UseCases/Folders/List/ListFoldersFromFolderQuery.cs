using Ardalis.Result;
using Ardalis.SharedKernel;

namespace MiniAssetManagement.UseCases.Folders.List;

public record ListFoldersFromFolderQuery(
    int UserId,
    int FolderId,
    int? Skip = null,
    int? Take = null
) : IQuery<Result<IEnumerable<FolderDTO>>>;
