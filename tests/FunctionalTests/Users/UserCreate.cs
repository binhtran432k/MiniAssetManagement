using System.Net;
using System.Text.Json;
using Ardalis.HttpClientTestExtensions;
using MiniAssetManagement.Web.Users;

namespace MiniAssetManagement.FunctionalTests.Users;

[TestFixture]
public class UserCreate : BaseTest
{
    [Test]
    public async Task CreatesUserSuccess()
    {
        // When
        var testName = Guid.NewGuid().ToString();
        var request = new CreateUserRequest() { Username = testName };
        var content = StringContentHelpers.FromModelAsJson(request);
        var response = await Client.PostAsync(CreateUserRequest.Route, content);

        // Then
        Assert.That(
            response.StatusCode,
            Is.EqualTo(HttpStatusCode.Created),
            nameof(response.StatusCode)
        );

        var stringResponse = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<CreateUserResponse>(
            stringResponse,
            Constants.DefaultJsonOptions
        );
        Assert.That(result, Is.Not.Null, nameof(result));
        Assert.That(result!.Username, Is.EqualTo(testName), nameof(result.Username));
        Assert.That(result.Id, Is.GreaterThan(0), nameof(result.Id));
    }

    [Test]
    public async Task CreatesUserFailByInvalidInput()
    {
        // When
        var request = new CreateUserRequest() { Username = String.Empty };
        var content = StringContentHelpers.FromModelAsJson(request);
        var response = await Client.PostAsync(CreateUserRequest.Route, content);

        // Then
        Assert.That(
            response.StatusCode,
            Is.EqualTo(HttpStatusCode.BadRequest),
            nameof(response.StatusCode)
        );
    }
}
