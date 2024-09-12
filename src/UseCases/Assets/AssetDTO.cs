using MiniAssetManagement.Core.AssetAggregate;

namespace MiniAssetManagement.UseCases.Assets;

public record AssetDTO(int Id, string Name, FileType? FileType);
