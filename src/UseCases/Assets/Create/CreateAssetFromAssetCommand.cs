using Ardalis.Result;
using Ardalis.SharedKernel;

namespace MiniAssetManagement.UseCases.Assets.Create;

public record CreateAssetFromAssetCommand(string Name, int AdminId, int AssetId) : ICommand<Result<int>>;
