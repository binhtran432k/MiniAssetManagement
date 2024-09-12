using System.Net;
using MiniAssetManagement.Web;
using MiniAssetManagement.Web.Constants;

namespace MiniAssetManagement.FunctionalTests.Users;

[TestFixture]
public class UserDelete : BaseTest
{
    [Test]
    public async Task DeletesUserSuccess()
    {
        // When
        var response = await Client.DeleteAsync(RouteConstant.BuildUserById(SeedData.User1.Id));

        // Then
        Assert.That(
            response.StatusCode,
            Is.EqualTo(HttpStatusCode.NoContent),
            nameof(response.StatusCode)
        );

        var getResponse = await Client.GetAsync(
            RouteConstant.BuildUserById(SeedData.User1.Id)
        );
        Assert.That(
            getResponse.StatusCode,
            Is.EqualTo(HttpStatusCode.NotFound),
            nameof(getResponse.StatusCode)
        );
    }

    [Test]
    public async Task DeletesUserFailByInvalidInput()
    {
        // When
        var response = await Client.DeleteAsync(RouteConstant.BuildUserById(0));

        // Then
        Assert.That(
            response.StatusCode,
            Is.EqualTo(HttpStatusCode.NotFound),
            nameof(response.StatusCode)
        );
    }
}
