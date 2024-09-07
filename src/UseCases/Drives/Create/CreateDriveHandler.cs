using Ardalis.Result;
using Ardalis.SharedKernel;
using MiniAssetManagement.Core.DriveAggregate;

namespace MiniAssetManagement.UseCases.Drives.Create;

public class CreateDriveHandler(IRepository<Drive> _repository)
    : ICommandHandler<CreateDriveCommand, Result<int>>
{
    public async Task<Result<int>> Handle(
        CreateDriveCommand request,
        CancellationToken cancellationToken
    )
    {
        var newDrive = new Drive(request.Name, request.OwnerId);
        var createdItem = await _repository.AddAsync(newDrive, cancellationToken);

        return createdItem.Id;
    }
}