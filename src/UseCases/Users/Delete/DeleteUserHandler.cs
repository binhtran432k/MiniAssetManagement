using Ardalis.Result;
using Ardalis.SharedKernel;
using MediatR;
using MiniAssetManagement.Core.UserAggregate;
using MiniAssetManagement.Core.UserAggregate.Events;
using MiniAssetManagement.Core.UserAggregate.Specifications;

namespace MiniAssetManagement.UseCases.Users.Delete;

public class DeleteUserHandler(IRepository<User> _repository, IMediator _mediator)
    : ICommandHandler<DeleteUserCommand, Result>
{
    public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var aggregateToDelete = await _repository.FirstOrDefaultAsync(
            new UserByIdSpec(request.UserId),
            cancellationToken
        );
        if (aggregateToDelete is null)
            return Result.NotFound();

        aggregateToDelete.UpdateStatus(UserStatus.Deleted);

        await _repository.UpdateAsync(aggregateToDelete);
        await _mediator.Publish(new UserDeletedEvent(request.UserId));

        return Result.Success();
    }
}
