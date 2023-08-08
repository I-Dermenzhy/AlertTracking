namespace AlertTracking.Abstractions.Exceptions;

/// <summary>
/// Represents an exception that is thrown when there is an error during the deserialization of an HTTP response.
/// </summary>
public class HttpResponseDeserializationException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HttpResponseDeserializationException"/> class with the specified error message and inner exception.
    /// </summary>
    /// <param name="response">The <see cref="HttpResponseMessage"/> being deserialized when the exception occurred.</param>
    /// <param name="innerException">The inner exception that caused the deserialization error.</param>
    public HttpResponseDeserializationException(HttpResponseMessage response, Exception innerException)
        : base($"Unable to deserialize the followith HttpResponseMessage: {response.ToString}", innerException)
    {
        ArgumentNullException.ThrowIfNull(innerException, nameof(innerException));

        Response = response ?? throw new ArgumentNullException(nameof(response));
    }

    /// <summary>
    /// Gets the <see cref="HttpResponseMessage"/> associated with the deserialization exception.
    /// </summary>
    public HttpResponseMessage Response { get; }
}
