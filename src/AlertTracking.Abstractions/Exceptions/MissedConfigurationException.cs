using Microsoft.Extensions.Configuration;

namespace AlertTracking.Abstractions.Exceptions;

public class MissedConfigurationException : InvalidOperationException
{
    public MissedConfigurationException(IConfiguration configuration, string missedConfiguration, string? message = null)
        : this(configuration, new ConfigurationKey(missedConfiguration), message)
    {
        if (string.IsNullOrWhiteSpace(missedConfiguration))
            throw new ArgumentException($"'{nameof(missedConfiguration)}' cannot be null or whitespace.", nameof(missedConfiguration));
    }

    public MissedConfigurationException(IConfiguration configuration, ConfigurationKey missedConfiguration, string? message = null)
        : base(message)
    {
        Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        MissedConfiguration = missedConfiguration ?? throw new ArgumentNullException(nameof(missedConfiguration));
    }

    public IConfiguration Configuration { get; init; }
    public ConfigurationKey MissedConfiguration { get; init; }
}

public class ConfigurationKey
{
    public ConfigurationKey(string value) => Value = value ?? throw new ArgumentNullException(nameof(value));

    public string Value { get; }

    public override string ToString() => Value;
}
