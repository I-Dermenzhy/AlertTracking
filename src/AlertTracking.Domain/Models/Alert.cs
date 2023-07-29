using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace AlertTracking.Domain.Models;

public class Alert
{
    public Alert()
    {
    }

    [SetsRequiredMembers]
    [JsonConstructor]
    public Alert(string regionId, string type, string lastUpdate)
    {
        if (string.IsNullOrWhiteSpace(regionId))
            throw new ArgumentException($"'{nameof(regionId)}' cannot be null or whitespace.", nameof(regionId));

        if (string.IsNullOrWhiteSpace(type))
            throw new ArgumentException($"'{nameof(type)}' cannot be null or whitespace.", nameof(type));

        if (string.IsNullOrWhiteSpace(lastUpdate))
            throw new ArgumentException($"'{nameof(lastUpdate)}' cannot be null or whitespace.", nameof(lastUpdate));

        RegionId = regionId;
        Type = type;
        LastUpdate = lastUpdate;
    }

    [JsonPropertyName("regionId")]
    public required string RegionId { get; init; }

    [JsonPropertyName("type")]
    public required string Type { get; init; }

    [JsonPropertyName("lastUpdate")]
    public required string LastUpdate { get; init; }
}