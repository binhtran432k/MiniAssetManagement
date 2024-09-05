using Ardalis.Specification;

namespace MiniAssetManagement.Core.UserAggregate.Specifications;

public class UserByIdSpec : Specification<User>
{
    public UserByIdSpec(int userId) =>
        Query.Where(user => user.Id == userId && user.Status == UserStatus.Available);
}
