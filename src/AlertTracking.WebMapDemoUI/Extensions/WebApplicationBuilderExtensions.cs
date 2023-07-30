using AlertTracking.Shared;

namespace AlertTracking.WebMapDemoUI.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder AddSharedConfiguration(this WebApplicationBuilder builder)
    {
        var env = builder.Environment;

        var sharedFolderPath = Path.Combine(env.ContentRootPath, "..", "AlertTracking.Shared");
        var sharedSettingsPath = Path.Combine(sharedFolderPath, "sharedSettings.json");

        builder.Configuration.AddSharedConfiguration(sharedSettingsPath);

        return builder;
    }
}
