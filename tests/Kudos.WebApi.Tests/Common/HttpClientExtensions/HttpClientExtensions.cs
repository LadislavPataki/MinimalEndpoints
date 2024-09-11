using Kudos.WebApi.JsonSerialization.Extensions;
using Kudos.WebApi.Tests.Common.HttpRequestMessage;

namespace Kudos.WebApi.Tests.Common.HttpClientExtensions;

public static class HttpClientExtensions
{
    public static Task<(HttpStatusCode statusCode, TContent? content)> GetAndDeserializeAsync<TContent>(
        this HttpClient? httpClient, string requestUri) where TContent : class
    {
        ArgumentNullException.ThrowIfNull(httpClient);

        var httpRequestMessage = HttpRequestMessageBuilder
            .CreateHttpRequestMessage()
            .WithUsingGet()
            .WithRequestUri(requestUri)
            .Build();

        return httpClient.SendAndDeserializeAsync<TContent>(httpRequestMessage);
    }
    
    public static Task<(HttpStatusCode statusCode, TContent? content)> PostAndDeserializeAsync<TContent, TRequestBody>(
        this HttpClient? httpClient, string requestUri, TRequestBody requestBody) where TContent : class
    {
        ArgumentNullException.ThrowIfNull(httpClient);

        var httpRequestMessage = HttpRequestMessageBuilder
            .CreateHttpRequestMessage()
            .WithUsingPost()
            .WithRequestUri(requestUri)
            .WithBodyAsJson(requestBody)
            .Build();

        return httpClient.SendAndDeserializeAsync<TContent>(httpRequestMessage);
    }
    
    public static Task<(HttpStatusCode statusCode, TContent? content)> PutAndDeserializeAsync<TContent, TRequestBody>(
        this HttpClient? httpClient, string requestUri, TRequestBody requestBody) where TContent : class
    {
        ArgumentNullException.ThrowIfNull(httpClient);

        var httpRequestMessage = HttpRequestMessageBuilder
            .CreateHttpRequestMessage()
            .WithUsingPut()
            .WithRequestUri(requestUri)
            .WithBodyAsJson(requestBody)
            .Build();

        return httpClient.SendAndDeserializeAsync<TContent>(httpRequestMessage);
    }
    
    public static Task<(HttpStatusCode statusCode, TContent? content)> PatchAndDeserializeAsync<TContent, TRequestBody>(
        this HttpClient? httpClient, string requestUri, TRequestBody requestBody) where TContent : class
    {
        ArgumentNullException.ThrowIfNull(httpClient);

        var httpRequestMessage = HttpRequestMessageBuilder
            .CreateHttpRequestMessage()
            .WithUsingPatch()
            .WithRequestUri(requestUri)
            .WithBodyAsJson(requestBody)
            .Build();

        return httpClient.SendAndDeserializeAsync<TContent>(httpRequestMessage);
    }

    public static Task<(HttpStatusCode statusCode, TContent? content)> DeleteAndDeserializeAsync<TContent>(
        this HttpClient? httpClient, string requestUri) where TContent : class
    {
        ArgumentNullException.ThrowIfNull(httpClient);

        var httpRequestMessage = HttpRequestMessageBuilder
            .CreateHttpRequestMessage()
            .WithUsingDelete()
            .WithRequestUri(requestUri)
            .Build();

        return httpClient.SendAndDeserializeAsync<TContent>(httpRequestMessage);
    }

    public static async Task<(HttpStatusCode statusCode, TContent? content)> SendAndDeserializeAsync<TContent>(
        this HttpClient httpClient, System.Net.Http.HttpRequestMessage request) where TContent : class
    {
        var response = await httpClient.SendAsync(request);

        var statusCode = response.StatusCode;

        var stringContent = await response.Content.ReadAsStringAsync();
        var content = string.IsNullOrEmpty(stringContent)
            ? null
            : stringContent.Deserialize<TContent>();
        
        return (statusCode, content);
    }
}