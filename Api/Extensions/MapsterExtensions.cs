using Mapster;

namespace Api.Extensions;

public static class MapsterExtensions
{
    public static IServiceCollection AddMapsterConfiguration(this IServiceCollection services)
    {
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(typeof(MapsterExtensions).Assembly);
        services.AddSingleton(config);
        // Note: MapsterMapper.ServiceMapper requires MapsterMapper package; using simple Mapster is sufficient
        return services;
    }
}
