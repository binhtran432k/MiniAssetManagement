using System.Reflection;
using Ardalis.SharedKernel;
using Microsoft.EntityFrameworkCore;
using MiniAssetManagement.Core.DriveAggregate;
using MiniAssetManagement.Core.AssetAggregate;
using MiniAssetManagement.Core.UserAggregate;

namespace MiniAssetManagement.Infrastructure.Data;

public class AppDbContext(
    DbContextOptions<AppDbContext> options,
    IDomainEventDispatcher? dispatcher
) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Drive> Drives => Set<Drive>();
    public DbSet<Asset> Assets => Set<Asset>();
    public DbSet<Permission> Permissions => Set<Permission>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        int result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        // ignore events if no dispatcher provided
        if (dispatcher is null)
            return result;

        // dispatch events only if save was successful
        var entitiesWithEvents = ChangeTracker
            .Entries<EntityBase>()
            .Select(e => e.Entity)
            .Where(e => e.DomainEvents.Any())
            .ToArray();

        await dispatcher.DispatchAndClearEvents(entitiesWithEvents);

        return result;
    }

    public override int SaveChanges() => SaveChangesAsync().GetAwaiter().GetResult();
}
