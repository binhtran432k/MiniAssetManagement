using Microsoft.EntityFrameworkCore;
using MiniAssetManagement.Core.UserAggregate;

namespace MiniAssetManagement.IntegrationTests.Data;

public class EfRepositoryUpdate : BaseTest
{
    [Test]
    public async Task UpdatesUserSuccess()
    {
        //  Given
        var initialUsername = "testUser";
        var repository = GetRepository<User>();
        var user = new User(initialUsername);
        await repository.AddAsync(user);
        // detach the item so we get a different instance
        _dbContext.Entry(user).State = EntityState.Detached;
        // fetch the item and update its title
        var newUser = (await repository.ListAsync()).FirstOrDefault(u =>
            u.Username == initialUsername
        );
        Assert.That(newUser, Is.Not.Null.And.Not.SameAs(user), nameof(newUser));

        // When
        var newUsername = Guid.NewGuid().ToString();
        newUser!.UpdateUsername(newUsername);
        await repository.UpdateAsync(newUser);

        // Then
        var updatedUser = (await repository.ListAsync()).FirstOrDefault();
        Assert.That(updatedUser, Is.Not.Null, nameof(updatedUser));
        Assert.Multiple(() =>
        {
            Assert.That(
                updatedUser!.Username,
                Is.Not.EqualTo(user.Username),
                nameof(updatedUser.Username)
            );
            Assert.That(updatedUser.Status, Is.EqualTo(user.Status), nameof(updatedUser.Status));
            Assert.That(updatedUser.Id, Is.EqualTo(user.Id), nameof(updatedUser.Id));
        });
    }
}