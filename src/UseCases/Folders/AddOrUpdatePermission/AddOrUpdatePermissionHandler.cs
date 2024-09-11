using Ardalis.Result;
using Ardalis.SharedKernel;
using MiniAssetManagement.Core.FolderAggregate;
using MiniAssetManagement.Core.FolderAggregate.Specifications;
using MiniAssetManagement.UseCases.Folders.GetPermission;

namespace MiniAssetManagement.UseCases.Folders.AddOrUpdatePermission;

public class AddOrUpdatePermissionHandler(
    IRepository<Folder> _repository,
    IGetFolderPermissionQueryService _permissionQuery
) : ICommandHandler<AddOrUpdatePermissionCommand, Result>
{
    public async Task<Result> Handle(
        AddOrUpdatePermissionCommand request,
        CancellationToken cancellationToken
    )
    {
        if (
            request.UserId == request.AddOrUpdateUserId
            || request.AddOrUpdatePermission == PermissionType.Admin
        )
            return Result.Conflict();

        var permission = await _permissionQuery.GetAsync(request.FolderId, request.UserId);
        if (permission != PermissionType.Admin)
            return Result.Unauthorized();

        var existingFolder = await _repository.FirstOrDefaultAsync(
            new FolderWithPermissionsByIdSpec(request.FolderId),
            cancellationToken
        );
        if (existingFolder is null)
            return Result.NotFound();

        existingFolder.AddOrUpdatePermission(
            request.AddOrUpdateUserId,
            request.AddOrUpdatePermission
        );

        await _repository.UpdateAsync(existingFolder);

        return Result.Success();
    }
}
