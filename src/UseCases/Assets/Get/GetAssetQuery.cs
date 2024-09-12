using Ardalis.Result;
using Ardalis.SharedKernel;

namespace MiniAssetManagement.UseCases.Assets.Get;

public record GetAssetQuery(int AssetId, int UserId) : IQuery<Result<AssetDTO>>;
