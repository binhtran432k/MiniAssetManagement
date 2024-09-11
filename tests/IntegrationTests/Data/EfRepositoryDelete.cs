using MiniAssetManagement.Core.UserAggregate;

namespace MiniAssetManagement.IntegrationTests.Data;

public class EfRepositoryDelete : BaseTest
{
    [Test]
    public async Task DeletesUserSuccess()
    {
        //  Given
        var initialUsername = "testUser";
        var repository = GetRepository<User>();
        var user = new User(initialUsername);
        await repository.AddAsync(user);

        // When
        await repository.DeleteAsync(user);

        // Then
        var users = await repository.ListAsync();
        Assert.That(users, Is.Empty, nameof(users));
    }
}