namespace AlertTracking.Abstractions.Exceptions;

public class ResponseDeserializationException : Exception
{
    public ResponseDeserializationException(HttpResponseMessage response, Exception innerException)
        : base($"Unable to deserialize the followith HttpResponseMessage: {response.ToString}", innerException)
    {
        ArgumentNullException.ThrowIfNull(innerException, nameof(innerException));

        Response = response ?? throw new ArgumentNullException(nameof(response));
    }

    public HttpResponseMessage Response { get; set; }
}
