using Ardalis.Result;
using Ardalis.SharedKernel;
using MediatR;
using MiniAssetManagement.Core.UserAggregate;
using MiniAssetManagement.Core.UserAggregate.Events;

namespace MiniAssetManagement.UseCases.Users.Delete;

public class DeleteUserHandler(IRepository<User> _repository, IMediator _mediator)
    : ICommandHandler<DeleteUserCommand, Result>
{
    public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var aggregateToDelete = await _repository.GetByIdAsync(request.UserId);
        if (aggregateToDelete == null)
            return Result.NotFound();

        aggregateToDelete.UpdateStatus(UserStatus.Deleted);

        await _repository.UpdateAsync(aggregateToDelete);
        var domainEvent = new UserDeletedEvent(request.UserId);
        await _mediator.Publish(domainEvent);

        return Result.Success();
    }
}