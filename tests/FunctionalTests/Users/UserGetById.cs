using System.Net;
using System.Text.Json;
using Ardalis.HttpClientTestExtensions;
using MiniAssetManagement.Web;
using MiniAssetManagement.Web.Constants;
using MiniAssetManagement.Web.Users;

namespace MiniAssetManagement.FunctionalTests.Users;

[TestFixture]
public class UserGetById : BaseTest
{
    [Test]
    public async Task GetByIdsUserSuccess()
    {
        // When
        var response = await Client.GetAsync(RouteConstant.BuildUserById(SeedData.User1.Id));

        // Then
        Assert.That(
            response.StatusCode,
            Is.EqualTo(HttpStatusCode.OK),
            nameof(response.StatusCode)
        );

        var stringResponse = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<GetByIdUserResponse>(
            stringResponse,
            Constants.DefaultJsonOptions
        );
        Assert.That(result, Is.Not.Null, nameof(result));
        Assert.That(result!.Username, Is.EqualTo(SeedData.User1.Username), nameof(result.Username));
        Assert.That(result.Id, Is.EqualTo(SeedData.User1.Id), nameof(result.Id));
    }

    [Test]
    public async Task GetByIdsUserFailByNotFound()
    {
        // When
        var response = await Client.GetAsync(RouteConstant.BuildUserById(0));

        // Then
        Assert.That(
            response.StatusCode,
            Is.EqualTo(HttpStatusCode.NotFound),
            nameof(response.StatusCode)
        );
    }
}