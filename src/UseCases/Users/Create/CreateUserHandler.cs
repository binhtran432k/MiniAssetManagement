using Ardalis.Result;
using Ardalis.SharedKernel;
using MiniAssetManagement.Core.UserAggregate;

namespace MiniAssetManagement.UseCases.Users.Create;

public class CreateUserHandler(IRepository<User> _repository)
    : ICommandHandler<CreateUserCommand, Result<int>>
{
    public async Task<Result<int>> Handle(
        CreateUserCommand request,
        CancellationToken cancellationToken
    )
    {
        var newUser = new User(request.Username);
        var createdItem = await _repository.AddAsync(newUser, cancellationToken);

        return createdItem.Id;
    }
}
