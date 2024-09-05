using Ardalis.Result;
using Ardalis.SharedKernel;
using MiniAssetManagement.Core.UserAggregate;
using MiniAssetManagement.Core.UserAggregate.Specifications;

namespace MiniAssetManagement.UseCases.Users.Get;

public class GetUserHandler(IReadRepository<User> _repository)
    : IQueryHandler<GetUserQuery, Result<UserDTO>>
{
    public async Task<Result<UserDTO>> Handle(
        GetUserQuery request,
        CancellationToken cancellationToken
    )
    {
        var spec = new UserByIdSpec(request.UserId);
        var entity = await _repository.FirstOrDefaultAsync(spec, cancellationToken);
        if (entity == null)
            return Result.NotFound();

        return new UserDTO(entity.Id, entity.Username);
    }
}
