using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MiniAssetManagement.Infrastructure.Data;
using MiniAssetManagement.Web;

namespace MiniAssetManagement.FunctionalTests;

public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram>
    where TProgram : class
{
    /// <summary>
    /// Overriding CreateHost to avoid creating a separate ServiceProvider per this thread:
    /// https://github.com/dotnet-architecture/eShopOnWeb/issues/465
    /// </summary>
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.UseEnvironment("Development"); // will not send real emails
        var host = builder.Build();
        host.Start();

        // Get service provider.
        var serviceProvider = host.Services;

        // Create a scope to obtain a reference to the database
        // context (AppDbContext).
        using (var scope = serviceProvider.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<AppDbContext>();

            var logger = scopedServices.GetRequiredService<
                ILogger<CustomWebApplicationFactory<TProgram>>
            >();

            // Ensure the database is created.
            db.Database.EnsureCreated();

            try
            {
                // Seed the database with test data.
                SeedData.PopulateTestData(db);
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "An error occurred seeding the "
                        + "database with test messages. Error: {exceptionMessage}",
                    ex.Message
                );
            }
        }

        return host;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove the app's ApplicationDbContext registration.
            var descriptor = services.SingleOrDefault(d =>
                d.ServiceType == typeof(DbContextOptions<AppDbContext>)
            );

            if (descriptor is not null)
            {
                services.Remove(descriptor);
            }

            // This should be set for each individual test run
            SqliteConnection connection = new("Filename=:memory:");
            connection.Open();

            // Add ApplicationDbContext using an SQLite in-memory database for testing.
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlite(connection);
            });

            // Add MediatR
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(
                    typeof(Program).Assembly,
                    typeof(AppDbContext).Assembly
                );
            });
        });
    }
}