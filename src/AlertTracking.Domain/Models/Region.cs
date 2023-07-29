using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace AlertTracking.Domain.Models;

public class Region
{
    public Region()
    {
    }

    [SetsRequiredMembers]
    [JsonConstructor]
    public Region(string id, string name, string type, IEnumerable<Alert>? activeAlerts = null)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentException($"'{nameof(id)}' cannot be null or whitespace.", nameof(id));

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));

        if (string.IsNullOrWhiteSpace(type))
            throw new ArgumentException($"'{nameof(type)}' cannot be null or whitespace.", nameof(type));

        ActiveAlerts = activeAlerts ?? Enumerable.Empty<Alert>();

        Id = id;
        Name = name;
        Type = type;
    }

    [JsonPropertyName("regionId")]
    public required string Id { get; init; }

    [JsonPropertyName("regionName")]
    public required string Name { get; init; }

    [JsonPropertyName("regionType")]
    public required string Type { get; init; }

    [JsonPropertyName("activeAlerts")]
    public required IEnumerable<Alert> ActiveAlerts { get; init; }
}
