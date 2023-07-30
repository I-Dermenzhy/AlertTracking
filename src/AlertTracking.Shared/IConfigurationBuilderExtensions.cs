using Microsoft.Extensions.Configuration;

namespace AlertTracking.Shared;

public static class IConfigurationBuilderExtensions
{
    public static IConfigurationBuilder AddSharedConfiguration(this IConfigurationBuilder configuration, string? relativePath = null)
    {
        return relativePath is null
            ? configuration.AddJsonFile("sharedSettings.json", optional: true, reloadOnChange: true)
            : configuration.AddJsonFile(relativePath, optional: true, reloadOnChange: true);
    }
}
