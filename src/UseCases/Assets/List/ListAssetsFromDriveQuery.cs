using Ardalis.Result;
using Ardalis.SharedKernel;

namespace MiniAssetManagement.UseCases.Assets.List;

public record ListAssetsFromDriveQuery(int UserId, int DriveId, int? Skip = null, int? Take = null)
    : IQuery<Result<(IEnumerable<AssetDTO>, int)>>;
