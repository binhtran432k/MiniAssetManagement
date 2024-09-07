using MiniAssetManagement.Core.UserAggregate;
using MiniAssetManagement.Core.UserAggregate.Specifications;
using MiniAssetManagement.UnitTests.Fixtures;

namespace MiniAssetManagement.UnitTests.Core.Specifications;

public class UserByIdSpecificationsConstructor
{
    [Test]
    public void FilterCollectionToOnlyReturnItemsWithValidId()
    {
        // Given
        var user1 = UserFixture.CreateUser(1, "a");
        var user2 = UserFixture.CreateUser(2, "b");
        var user3 = UserFixture.CreateUser(3, "c");
        List<User> users = new() { user1, user2, user3 };

        // When
        var spec = new UserByIdSpec(1);
        var filteredUsers = spec.Evaluate(users);

        // Then
        Assert.That(
            filteredUsers,
            Is.EquivalentTo(new List<User>() { user1 }),
            nameof(filteredUsers)
        );
    }

    [Test]
    public void FilterCollectionToReturnEmptyWithInvalidId()
    {
        // Given
        var user1 = UserFixture.CreateUser(1, "a");
        var user2 = UserFixture.CreateUser(2, "b");
        var user3 = UserFixture.CreateUser(3, "c");
        List<User> users = new() { user1, user2, user3 };

        // When
        var spec = new UserByIdSpec(100);
        var filteredUsers = spec.Evaluate(users);

        // Then
        Assert.That(filteredUsers, Is.Empty, nameof(filteredUsers));
    }

    [Test]
    public void FilterCollectionToReturnEmptyWithDeletedId()
    {
        // Given
        var user1 = UserFixture.CreateUser(1, "a");
        var user2 = UserFixture.CreateUser(2, "b", UserStatus.Deleted);
        var user3 = UserFixture.CreateUser(3, "c");
        List<User> users = new() { user1, user2, user3 };

        // When
        var spec = new UserByIdSpec(2);
        var filteredUsers = spec.Evaluate(users);

        // Then
        Assert.That(filteredUsers, Is.Empty, nameof(filteredUsers));
    }
}
