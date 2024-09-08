using Ardalis.Result;
using Ardalis.SharedKernel;
using MiniAssetManagement.Core.FolderAggregate;

namespace MiniAssetManagement.UseCases.Folders.AddOrUpdatePermission;

public record AddOrUpdatePermissionCommand(
    int FolderId,
    int UserId,
    int AddOrUpdateUserId,
    PermissionType AddOrUpdatePermission
) : ICommand<Result>;