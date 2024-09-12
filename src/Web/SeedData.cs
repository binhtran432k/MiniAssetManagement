using MiniAssetManagement.Core.UserAggregate;
using MiniAssetManagement.Infrastructure.Data;

namespace MiniAssetManagement.Web;

public static class SeedData
{
    public const int UserCount = 2;
    public static readonly User User1 = new("Binhtran");
    public static readonly User User2 = new("Snowfrog");

    public static void Initialize(AppDbContext dbContext)
    {
        // Look for any TODO items.
        if (dbContext.Users.Any())
            return; // DB has been seeded

        PopulateTestData(dbContext);
    }

    public static void PopulateTestData(AppDbContext dbContext)
    {
        foreach (var item in dbContext.Users)
            dbContext.Remove(item);

        dbContext.SaveChanges();

        User1.Id = dbContext.Users.Add(User1).Entity.Id;
        User2.Id = dbContext.Users.Add(User2).Entity.Id;

        dbContext.SaveChanges();
    }
}