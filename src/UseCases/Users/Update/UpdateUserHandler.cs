using Ardalis.Result;
using Ardalis.SharedKernel;
using MiniAssetManagement.Core.UserAggregate;

namespace MiniAssetManagement.UseCases.Users.Update;

public class UpdateUserHandler(IRepository<User> _repository)
    : ICommandHandler<UpdateUserCommand, Result<UserDTO>>
{
    public async Task<Result<UserDTO>> Handle(
        UpdateUserCommand request,
        CancellationToken cancellationToken
    )
    {
        var existingUser = await _repository.GetByIdAsync(request.UserId);
        if (existingUser == null)
            return Result.NotFound();

        existingUser.UpdateUsername(request.NewUsername);
        await _repository.UpdateAsync(existingUser, cancellationToken);

        return new UserDTO(existingUser.Id, existingUser.Username);
    }
}
