using MiniAssetManagement.Core.UserAggregate;

namespace MiniAssetManagement.IntegrationTests.Data;

public class EfRepositoryAdd : BaseTest
{
    [Test]
    public async Task AddsUserAndSetsId()
    {
        // Given
        var testUserName = "testUser";
        var testUserStatus = UserStatus.Available;
        var repository = GetRepository<User>();
        var user = new User(testUserName);

        // When
        await repository.AddAsync(user);

        // Then
        var newUser = (await repository.ListAsync()).FirstOrDefault();
        Assert.That(newUser, Is.Not.Null, nameof(newUser));
        Assert.Multiple(() =>
        {
            Assert.That(newUser?.Username, Is.EqualTo(testUserName), nameof(newUser.Username));
            Assert.That(newUser?.Status, Is.EqualTo(testUserStatus), nameof(newUser.Status));
            Assert.That(newUser?.Id > 0, Is.True, nameof(newUser.Id));
        });
    }
}