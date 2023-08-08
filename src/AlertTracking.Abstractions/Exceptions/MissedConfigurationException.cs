using Microsoft.Extensions.Configuration;

namespace AlertTracking.Abstractions.Exceptions;

/// <summary>
/// The exception that is thrown when a required configuration value is missing or invalid.
/// </summary>
/// <remarks>
/// <para>
/// The <see cref="MissedConfigurationException"/> is thrown when a required configuration value is not found
/// or is empty/whitespace in the application configuration. It provides information about the missing or
/// invalid configuration key and the <see cref="IConfiguration"/> instance containing the configuration data.
/// </para>
/// </remarks>
public class MissedConfigurationException : InvalidOperationException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MissedConfigurationException"/> class with the specified configuration and missed configuration key.
    /// </summary>
    /// <param name="configuration">The <see cref="IConfiguration"/> instance containing the configuration data.</param>
    /// <param name="missedConfiguration">The key of the configuration value that is missing or invalid.</param>
    /// <param name="message">An optional error message that provides additional context for the exception.</param>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="configuration"/> or <paramref name="missedConfiguration"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when the <paramref name="missedConfiguration"/> key is null, empty, or whitespace.</exception>
    public MissedConfigurationException(IConfiguration configuration, string missedConfiguration, string? message = null)
        : this(configuration, new ConfigurationKey(missedConfiguration), message)
    {
        if (string.IsNullOrWhiteSpace(missedConfiguration))
            throw new ArgumentException($"'{nameof(missedConfiguration)}' cannot be null or whitespace.", nameof(missedConfiguration));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MissedConfigurationException"/> class with the specified configuration and missed configuration key.
    /// </summary>
    /// <param name="configuration">The <see cref="IConfiguration"/> instance containing the configuration data.</param>
    /// <param name="missedConfiguration">The key of the configuration value that is missing or invalid.</param>
    /// <param name="message">An optional error message that provides additional context for the exception.</param>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="configuration"/> or <paramref name="missedConfiguration"/> is null.</exception>
    public MissedConfigurationException(IConfiguration configuration, ConfigurationKey missedConfiguration, string? message = null)
        : base(message)
    {
        Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        MissedConfiguration = missedConfiguration ?? throw new ArgumentNullException(nameof(missedConfiguration));
    }

    /// <summary>
    /// Gets the <see cref="IConfiguration"/> instance containing the configuration data.
    /// </summary>
    public IConfiguration Configuration { get; }

    /// <summary>
    /// Gets the key of the configuration value that is missing.
    /// </summary>
    public ConfigurationKey MissedConfiguration { get; }
}

/// <summary>
/// Represents a key for configuration values in the application configuration.
/// </summary>
/// <remarks>
/// <para>
/// The <see cref="ConfigurationKey"/> class is a simple wrapper around a string value, representing a key for configuration
/// values in the application configuration. It provides a more expressive and strongly-typed way to represent configuration keys
/// when throwing <see cref="MissedConfigurationException"/>.
/// </para>
/// </remarks>
public class ConfigurationKey
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurationKey"/> class with the specified value.
    /// </summary>
    /// <param name="value">The value of the configuration key.</param>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="value"/> is null.</exception>
    public ConfigurationKey(string value) => Value = value ?? throw new ArgumentNullException(nameof(value));

    /// <summary>
    /// Gets the value of the configuration key.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Returns the string representation of the configuration key.
    /// </summary>
    /// <returns>The value of the configuration key.</returns>
    public override string ToString() => Value;
}
