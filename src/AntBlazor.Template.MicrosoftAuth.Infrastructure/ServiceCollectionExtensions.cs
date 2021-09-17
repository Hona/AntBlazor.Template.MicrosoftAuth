using Marten;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMarten(_ =>
        {
            _.Connection(configuration.GetConnectionString("Marten"));

            _.AutoCreateSchemaObjects = AutoCreate.All;
        });
    }
}
