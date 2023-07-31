using AlertTracking.Abstractions.Exceptions;
using AlertTracking.Domain.Models;

namespace AlertTracking.Abstractions.Deserialization;

/// <summary>
/// Represents a service responsible for deserializing API responses into strongly-typed objects.
/// </summary>
public interface IApiResponseDeserializer
{
    /// <summary>
    /// Deserialize the last action index from the provided API response.
    /// </summary>
    /// <param name="response">The HTTP response message containing the API response.</param>
    /// <returns>A <see cref="Task{TResult}"/> representing the asynchronous operation, containing the deserialized last action index as a long.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the provided response is null.</exception>
    /// <exception cref="ResponseDeserializationException">Thrown when an error occurs during the JSON deserialization process or when the response contains no instances of the type 'lastActionIndex'.</exception>
    Task<long> DeserializeLastActionIndexFromResponseAsync(HttpResponseMessage response);

    /// <summary>
    /// Deserialize a single <see cref="Region"/> object from the provided API response.
    /// </summary>
    /// <param name="response">The HTTP response message containing the API response.</param>
    /// <returns>A <see cref="Task{TResult}"/> representing the asynchronous operation, containing the deserialized <see cref="Region"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the provided response is null.</exception>
    /// <exception cref="ResponseDeserializationException">Thrown when an error occurs during the JSON deserialization process or when the response contains no instances of the type 'Region'.</exception>
    Task<Region> DeserializeRegionFromResponseAsync(HttpResponseMessage response);

    /// <summary>
    /// Deserialize a collection of <see cref="Region"/> objects from the provided API response.
    /// </summary>
    /// <param name="response">The HTTP response message containing the API response.</param>
    /// <returns>A <see cref="Task{TResult}"/> representing the asynchronous operation, containing the deserialized collection of <see cref="Region"/> objects.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the provided response is null.</exception>
    /// <exception cref="ResponseDeserializationException">Thrown when an error occurs during the JSON deserialization process, when the response contains no instances of the type 'Region', or when the response is not in a valid JSON format.</exception>
    Task<IEnumerable<Region>> DeserializeRegionsFromResponseAsync(HttpResponseMessage response);

    /// <summary>
    /// Deserialize a collection of <see cref="Region"/> objects from the provided API response which is formatted with a container object named 'States'.
    /// </summary>
    /// <param name="response">The HTTP response message containing the API response.</param>
    /// <returns>A <see cref="Task{TResult}"/> representing the asynchronous operation, containing the deserialized collection of <see cref="Region"/> objects from the 'States' container.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the provided response is null.</exception>
    /// <exception cref="ResponseDeserializationException">Thrown when an error occurs during the JSON deserialization process, when the response contains no instances of the type 'States', when the 'states' object contains no regions, or when the response is not in a valid JSON format.</exception>
    Task<IEnumerable<Region>> DeserializeRegionsFromStatesResponseAsync(HttpResponseMessage response);
}