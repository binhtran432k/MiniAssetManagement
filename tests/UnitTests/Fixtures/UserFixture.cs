using MiniAssetManagement.Core.UserAggregate;

namespace MiniAssetManagement.UnitTests.Fixtures;

public static class UserFixture
{
    public const string UsernameDefault = "testname";
    public const string UsernameNew = "newtestname";
    public const int IdDefault = 1;
    public const int IdInvalid = 100;
    public const int IdDeleted = 2;
    public const int IdToDelete = 3;
    public const int IdAlternative = 4;

    public static User CreateUser(int id, string username, UserStatus? status = null)
    {
        User user = new(username) { Id = id };
        if (status is not null)
            user.UpdateStatus(status);
        return user;
    }

    public static User CreateUserDefault() => CreateUser(IdDefault, UsernameDefault);
}
