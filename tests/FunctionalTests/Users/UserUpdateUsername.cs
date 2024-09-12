using System.Net;
using Ardalis.HttpClientTestExtensions;
using MiniAssetManagement.Web;
using MiniAssetManagement.Web.Constants;
using MiniAssetManagement.Web.Users;

namespace MiniAssetManagement.FunctionalTests.Users;

[TestFixture]
public class UserUpdateUsername : BaseTest
{
    [Test]
    public async Task UpdatesUsernameUserSuccess()
    {
        // When
        var testName = Guid.NewGuid().ToString();
        var request = new UpdateUsernameRequest() { Value = testName };
        var content = StringContentHelpers.FromModelAsJson(request);
        var response = await Client.PutAsync(
            RouteConstant.BuildUserUsernameById(SeedData.User1.Id),
            content
        );

        // Then
        Assert.That(
            response.StatusCode,
            Is.EqualTo(HttpStatusCode.NoContent),
            nameof(response.StatusCode)
        );

        var getResult = await Client.GetAndDeserializeAsync<GetByIdUserResponse>(
            RouteConstant.BuildUserById(SeedData.User1.Id)
        );
        Assert.That(getResult, Is.Not.Null, nameof(getResult));
        Assert.That(getResult!.Username, Is.EqualTo(testName), nameof(getResult.Username));
        Assert.That(getResult.Id, Is.GreaterThan(0), nameof(getResult.Id));
    }

    [Test]
    public async Task UpdatesUsernameUserFailByNotFound()
    {
        // When
        var testName = Guid.NewGuid().ToString();
        var request = new UpdateUsernameRequest() { Value = testName };
        var content = StringContentHelpers.FromModelAsJson(request);
        var response = await Client.PutAsync(RouteConstant.BuildUserUsernameById(0), content);

        // Then
        Assert.That(
            response.StatusCode,
            Is.EqualTo(HttpStatusCode.NotFound),
            nameof(response.StatusCode)
        );
    }

    [Test]
    public async Task CreatesUserFailByInvalidInput()
    {
        // When
        var request = new UpdateUsernameRequest() { Value = String.Empty };
        var content = StringContentHelpers.FromModelAsJson(request);
        var response = await Client.PutAsync(
            RouteConstant.BuildUserUsernameById(SeedData.User1.Id),
            content
        );

        // Then
        Assert.That(
            response.StatusCode,
            Is.EqualTo(HttpStatusCode.BadRequest),
            nameof(response.StatusCode)
        );
    }
}
