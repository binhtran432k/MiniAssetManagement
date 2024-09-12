using Ardalis.Result;
using Ardalis.SharedKernel;

namespace MiniAssetManagement.UseCases.Assets.RemovePermission;

public record RemoveAssetPermissionCommand(int AssetId, int UserId, int RemoveUserId)
    : ICommand<Result>;
