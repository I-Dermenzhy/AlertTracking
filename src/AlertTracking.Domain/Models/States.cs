using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace AlertTracking.Domain.Models;

public class States
{
    public States()
    {
    }

    [SetsRequiredMembers]
    [JsonConstructor]
    public States(IEnumerable<Region> regions) => Regions = regions ?? throw new ArgumentNullException(nameof(regions));

    [JsonPropertyName("states")]
    public required IEnumerable<Region> Regions { get; init; }
}
