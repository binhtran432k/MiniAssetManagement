using Ardalis.Result;
using Ardalis.SharedKernel;

namespace MiniAssetManagement.UseCases.Assets.Update;

public record UpdateAssetCommand(int AssetId, int UserId, string NewName) : ICommand<Result<AssetDTO>>;
