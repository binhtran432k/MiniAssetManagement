using System.Net;
using System.Text.Json;
using Ardalis.HttpClientTestExtensions;
using Microsoft.AspNetCore.Http;
using MiniAssetManagement.Web;
using MiniAssetManagement.Web.Constants;
using MiniAssetManagement.Web.Users;

namespace MiniAssetManagement.FunctionalTests.Users;

[TestFixture]
public class UserList : BaseTest
{
    [TestCase(null, null, SeedData.UserCount, 1)]
    [TestCase(1, null, SeedData.UserCount, 1)]
    [TestCase(null, 1, 1, 2)]
    [TestCase(1, 1, 1, 2)]
    [TestCase(2, 1, 0, 2)]
    public async Task ListsUsersSuccess(
        int? pageIndex,
        int? pageSize,
        int expectedSize,
        int expectedPageCount
    )
    {
        // When
        var qb = new QueryString();
        if (pageIndex is not null)
            qb = qb.Add(nameof(pageIndex), pageIndex.ToString()!);
        if (pageSize is not null)
            qb = qb.Add(nameof(pageSize), pageSize.ToString()!);
        var response = await Client.GetAsync(RouteConstant.User + qb.ToString());

        // Then
        Assert.That(
            response.StatusCode,
            Is.EqualTo(HttpStatusCode.OK),
            nameof(response.StatusCode)
        );

        var stringResponse = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<ListUsersResponse>(
            stringResponse,
            Constants.DefaultJsonOptions
        );
        Assert.That(result, Is.Not.Null, nameof(result));
        var (users, pageCount) = result!;
        Assert.That(users.Count(), Is.EqualTo(expectedSize), nameof(users));
        Assert.That(pageCount, Is.EqualTo(expectedPageCount), nameof(pageCount));
    }

    [TestCase(-1, null)]
    [TestCase(null, -1)]
    [TestCase(-1, -1)]
    public async Task ListsUsersFailByInvalidInput(int? pageIndex, int? pageSize)
    {
        // When
        var qb = new QueryString();
        if (pageIndex is not null)
            qb = qb.Add(nameof(pageIndex), pageIndex.ToString()!);
        if (pageSize is not null)
            qb = qb.Add(nameof(pageSize), pageSize.ToString()!);
        var response = await Client.GetAsync(RouteConstant.User + qb.ToString());

        // Then
        Assert.That(
            response.StatusCode,
            Is.EqualTo(HttpStatusCode.BadRequest),
            nameof(response.StatusCode)
        );
    }
}
