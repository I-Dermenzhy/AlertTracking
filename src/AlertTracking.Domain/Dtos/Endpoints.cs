using System.Diagnostics.CodeAnalysis;

namespace AlertTracking.Domain.Dtos;

public sealed class Endpoints
{
    public Endpoints()
    {
    }

    [SetsRequiredMembers]
    public Endpoints(string regionsWithAlerts, string regionHistory, string status, string regions, string webhook)
    {
        if (string.IsNullOrWhiteSpace(regionsWithAlerts))
            throw new ArgumentException($"'{nameof(regionsWithAlerts)}' cannot be null or whitespace.", nameof(regionsWithAlerts));

        if (string.IsNullOrWhiteSpace(regionHistory))
            throw new ArgumentException($"'{nameof(regionHistory)}' cannot be null or whitespace.", nameof(regionHistory));

        if (string.IsNullOrWhiteSpace(status))
            throw new ArgumentException($"'{nameof(status)}' cannot be null or whitespace.", nameof(status));

        if (string.IsNullOrWhiteSpace(regions))
            throw new ArgumentException($"'{nameof(regions)}' cannot be null or whitespace.", nameof(regions));

        if (string.IsNullOrWhiteSpace(webhook))
            throw new ArgumentException($"'{nameof(webhook)}' cannot be null or whitespace.", nameof(webhook));

        RegionsWithAlerts = regionsWithAlerts;
        RegionHistory = regionHistory;
        Status = status;
        Regions = regions;
        Webhook = webhook;
    }

    public required string RegionsWithAlerts { get; set; }
    public required string RegionHistory { get; set; }
    public required string Status { get; set; }
    public required string Regions { get; set; }
    public required string Webhook { get; set; }
}
