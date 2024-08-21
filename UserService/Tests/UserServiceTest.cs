using Blazored.LocalStorage;

using Bunit;

using NUnit.Framework;

namespace UserService.Tests;

public class UserServiceTest : Bunit.TestContext
{
    private UserService _userService = default!;

    [SetUp]
    public void SetUp()
    {
        var localStorage = (ISyncLocalStorageService)this.AddBlazoredLocalStorage();
        _userService = new(localStorage);
    }

    [Test]
    public void ReturnZero_When_CountNewDatabase()
    {
        Assert.AreEqual(0, _userService.CountAllUsers());
    }

    [Test]
    public void ReturnIncrementValue_When_CountAfterAdd()
    {
        _userService.AddUser(new() { Username = "foo" });
        Assert.AreEqual(1, _userService.CountAllUsers());
        _userService.AddUser(new() { Username = "bar" });
        Assert.AreEqual(2, _userService.CountAllUsers());
    }

    [Test]
    public void ReturnUser_When_FindById()
    {
        var user = new User { Username = "foo" };
        Assert.IsNull(_userService.FindUserById(user.Id));
        _userService.AddUser(user);
        var foundUser = _userService.FindUserById(user.Id);
        Assert.IsNotNull(foundUser);
        Assert.AreEqual(user, foundUser);
    }

    [Test]
    public void ReturnUser_When_FindByUsername()
    {
        var user = new User { Username = "foo" };
        Assert.IsNull(_userService.FindUserByUsername("foo"));
        _userService.AddUser(user);
        var foundUser = _userService.FindUserByUsername("foo");
        Assert.IsNotNull(foundUser);
        Assert.AreEqual(user, foundUser);
    }

    [Test]
    public void ReturnTrue_When_AddNewUser()
    {
        var user = new User { Username = "foo" };
        Assert.IsTrue(_userService.AddUser(user));
    }

    [Test]
    public void ReturnFalse_When_AddExistedUser()
    {
        var user = new User { Username = "foo" };
        _userService.AddUser(user);
        Assert.IsFalse(_userService.AddUser(user));
    }
}
