using Ardalis.Result;
using Ardalis.SharedKernel;
using MiniAssetManagement.Core.FolderAggregate;
using MiniAssetManagement.UseCases.Folders.Get;

namespace MiniAssetManagement.UseCases.Folders.Update;

public class UpdateFolderHandler(IRepository<Folder> _repository, IGetFolderQueryService _query)
    : ICommandHandler<UpdateFolderCommand, Result<FolderDTO>>
{
    public async Task<Result<FolderDTO>> Handle(
        UpdateFolderCommand request,
        CancellationToken cancellationToken
    )
    {
        var existingFolder = await _query.GetAsync(request.FolderId, request.UserId);
        if (existingFolder == null)
            return Result.NotFound();

        existingFolder.UpdateName(request.NewName);
        await _repository.UpdateAsync(existingFolder, cancellationToken);

        return new FolderDTO(existingFolder.Id, existingFolder.Name);
    }
}
