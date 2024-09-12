using Ardalis.Result;
using Ardalis.SharedKernel;
using MiniAssetManagement.Core.AssetAggregate;

namespace MiniAssetManagement.UseCases.Assets.AddOrUpdatePermission;

public record AddOrUpdateAssetPermissionCommand(
    int AssetId,
    int UserId,
    int AddOrUpdateUserId,
    PermissionType AddOrUpdateAssetPermission
) : ICommand<Result>;
