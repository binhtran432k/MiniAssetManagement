using Ardalis.Result;
using Ardalis.SharedKernel;

namespace MiniAssetManagement.UseCases.Folders.List;

public record ListFoldersFromDriveQuery(int UserId, int DriveId, int? Skip = null, int? Take = null)
    : IQuery<Result<(IEnumerable<FolderDTO>, int)>>;
