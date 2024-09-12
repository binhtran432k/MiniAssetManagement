using System.Reflection;
using Ardalis.ListStartupServices;
using Ardalis.SharedKernel;
using FastEndpoints;
using FastEndpoints.Swagger;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MiniAssetManagement.Core;
using MiniAssetManagement.Core.UserAggregate;
using MiniAssetManagement.Infrastructure;
using MiniAssetManagement.Infrastructure.Data;
using MiniAssetManagement.UseCases.Users.Create;
using MiniAssetManagement.Web;
using Serilog;
using Serilog.Extensions.Logging;

var logger = Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

logger.Information("Starting web host");

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((_, config) => config.ReadFrom.Configuration(builder.Configuration));
var microsoftLogger = new SerilogLoggerFactory(logger).CreateLogger<Program>();

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

string? connectionString = builder.Configuration.GetConnectionString("SqliteConnection");
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(connectionString));

builder
    .Services.AddFastEndpoints()
    .SwaggerDocument(o =>
    {
        o.DocumentSettings = s =>
        {
            s.Title = "Mini Asset Management API";
            s.Version = "v1";
        };
    });

builder.Services.AddCoreServices(microsoftLogger).AddInfrastructureServices(microsoftLogger);

ConfigureMediatR();

// add list services for diagnostic purposes - see https://github.com/ardalis/AspNetCoreStartupServices
builder.Services.Configure<ServiceConfig>(config =>
{
    config.Services = new List<ServiceDescriptor>(builder.Services);

    // optional - default path to view services is /listallservices - recommended to choose your own path
    config.Path = "/listservices";
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseShowAllServicesMiddleware(); // see https://github.com/ardalis/AspNetCoreStartupServices
}
else
{
    app.UseDefaultExceptionHandler(); // from FastEndpoints
    app.UseHsts();
}
app.UseFastEndpoints().UseSwaggerGen(); // FastEndpoints middleware

app.UseHttpsRedirection();

// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
//app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"));

// Seed Database
SeedDatabase(app);

app.Run();

void ConfigureMediatR()
{
    var mediatRAssemblies = new[]
    {
        Assembly.GetAssembly(typeof(User)), // Core
        Assembly.GetAssembly(typeof(CreateUserCommand)), // UseCases
        Assembly.GetAssembly(
            typeof(InfrastructureServiceExtensions)
        ) // Infrastructure
        ,
    };

    builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(mediatRAssemblies!));
    builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
    builder.Services.AddScoped<IDomainEventDispatcher, MediatRDomainEventDispatcher>();
}

void SeedDatabase(WebApplication app)
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;

        try
        {
            var context = services.GetRequiredService<AppDbContext>();
            // context.Database.Migrate();
            context.Database.EnsureCreated();
            SeedData.Initialize(context);
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred seeding the DB. {exceptionMessage}", ex.Message);
        }
    }
}

// Make the implicit Program.cs class public, so integration tests can reference the correct assembly for host building
public partial class Program { }
