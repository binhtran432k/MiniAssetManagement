namespace MiniAssetManagement.FunctionalTests;

public class BaseTest
{
    protected HttpClient Client { get; private set; } = default!;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        var factory = new CustomWebApplicationFactory<Program>();
        Client = factory.CreateClient();
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        Client.Dispose();
    }
}
