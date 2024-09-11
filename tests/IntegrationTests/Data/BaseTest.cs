using Ardalis.SharedKernel;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using MiniAssetManagement.Infrastructure.Data;
using NSubstitute;

namespace MiniAssetManagement.IntegrationTests.Data;

public abstract class BaseTest
{
    public AppDbContext DbContext => _dbContext;
    protected AppDbContext _dbContext = default!;

    [SetUp]
    public void SetUp()
    {
        var options = CreateNewContextOptions();
        var _fakeEventDispatcher = Substitute.For<IDomainEventDispatcher>();
        _dbContext = new AppDbContext(options, _fakeEventDispatcher);
        _dbContext.Database.EnsureCreated();
    }

    [TearDown]
    public void TearDown()
    {
        _dbContext.Dispose();
    }

    protected static DbContextOptions<AppDbContext> CreateNewContextOptions()
    {
        DbContextOptionsBuilder<AppDbContext> builder = new();

        SqliteConnection connection = new("Filename=:memory:");
        connection.Open();
        builder.UseSqlite(connection);

        return builder.Options;
    }

    protected EfRepository<T> GetRepository<T>()
        where T : class, Ardalis.SharedKernel.IAggregateRoot => new(_dbContext);
}
