namespace AlertTracking.Abstractions.DataAccess.HttpRequests;

public interface IApiRequestProvider
{
    string AuthorizationToken { get; }

    HttpRequestMessage GetRegionAlertsRequest(string regionName);
    HttpRequestMessage GetRegionsRequest();
    HttpRequestMessage GetRegionsWithAlertRequest();
    HttpRequestMessage GetStatusUpdatesRequest();
}