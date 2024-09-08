using Ardalis.Result;
using Ardalis.SharedKernel;

namespace MiniAssetManagement.UseCases.Folders.RemovePermission;

public record RemovePermissionCommand(
    int FolderId,
    int UserId,
    int RemoveUserId
) : ICommand<Result>;