class Configuration
{
    public static void ConfigureCommonServices(IServiceCollection services)
    {
        services.AddScoped<ServiceGateway.ServiceGateway>();
    }
}