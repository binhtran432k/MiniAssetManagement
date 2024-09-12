using Ardalis.Result;
using Ardalis.SharedKernel;

namespace MiniAssetManagement.UseCases.Assets.List;

public record ListAssetsFromAssetQuery(
    int UserId,
    int AssetId,
    int? Skip = null,
    int? Take = null
) : IQuery<Result<(IEnumerable<AssetDTO>, int)>>;
