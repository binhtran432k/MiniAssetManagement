using MiniAssetManagement.Core.UserAggregate;
using MiniAssetManagement.UseCases.Users;

namespace MiniAssetManagement.UnitTests.Fixtures;

public static class UserFixture
{
    public const string UsernameDefault = "testname";
    public const string UsernameNew = "newtestname";
    public const int IdDefault = 1;
    public const int IdInvalid = 100;
    public const int IdDeleted = 2;
    public const int IdToDelete = 3;

    public static User MockUser(int id, string username) => new(username) { Id = id };

    public static User GetUserDefault() => MockUser(IdDefault, UsernameDefault);

    public static User MockDeletedUser(int id, string username)
    {
        var user = MockUser(id, username);
        user.UpdateStatus(UserStatus.Deleted);
        return user;
    }

    public static List<UserDTO> GetListUserDTODefault() => new() { new(1, "foo"), new(2, "bar") };
}