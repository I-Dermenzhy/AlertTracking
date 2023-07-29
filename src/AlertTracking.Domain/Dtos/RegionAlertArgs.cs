using AlertTracking.Domain.Models;

using System.Diagnostics.CodeAnalysis;

namespace AlertTracking.Domain.Dtos;

public class RegionAlertArgs : EventArgs
{
    public RegionAlertArgs()
    {
    }

    [SetsRequiredMembers]
    public RegionAlertArgs(Region region, bool isAlert)
    {
        Region = region ?? throw new ArgumentNullException(nameof(region));
        IsAlert = isAlert;
        RegionName = region.Name;
    }

    [SetsRequiredMembers]
    public RegionAlertArgs(string regionName, bool isAlert)
    {
        if (string.IsNullOrWhiteSpace(regionName))
            throw new ArgumentException($"'{nameof(regionName)}' cannot be null or whitespace.", nameof(regionName));

        RegionName = regionName;
        IsAlert = isAlert;
    }

    public Region? Region { get; init; }
    public required string RegionName { get; init; }
    public required bool IsAlert { get; set; }
}
