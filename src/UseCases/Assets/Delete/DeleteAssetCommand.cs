using Ardalis.Result;
using Ardalis.SharedKernel;

namespace MiniAssetManagement.UseCases.Assets.Delete;

public record DeleteAssetCommand(int AssetId, int UserId) : ICommand<Result>;
