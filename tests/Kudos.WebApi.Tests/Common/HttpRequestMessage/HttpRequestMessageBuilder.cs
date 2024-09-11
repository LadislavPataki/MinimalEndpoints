using System.Collections;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using Kudos.WebApi.Tests.Common.HttpRequestMessage.Stages;
using Microsoft.AspNetCore.Http;

namespace Kudos.WebApi.Tests.Common.HttpRequestMessage;

public class HttpRequestMessageBuilder :
    IHttpMethodSelectionStage,
    IPathSelectionStage,
    IQueryParamsStage
{
    private HttpMethod _httpMethod = null!;
    private string _path = null!;
    private readonly Dictionary<string, string> _headers;
    private string _body = null!;
    private HttpContent? _content;
    private QueryString _queryString;
    private AuthenticationHeaderValue? _authenticationHeaderValue;

    private HttpRequestMessageBuilder()
    {
        _headers = new Dictionary<string, string>();
    }

    public static IHttpMethodSelectionStage CreateHttpRequestMessage()
    {
        return new HttpRequestMessageBuilder();
    }

    public IPathSelectionStage WithUsingGet()
    {
        _httpMethod = HttpMethod.Get;
        return this;
    }

    public IPathSelectionStage WithUsingPost()
    {
        _httpMethod = HttpMethod.Post;
        return this;
    }

    public IPathSelectionStage WithUsingPut()
    {
        _httpMethod = HttpMethod.Put;
        return this;
    }

    public IPathSelectionStage WithUsingPatch()
    {
        _httpMethod = HttpMethod.Patch;
        return this;
    }

    public IPathSelectionStage WithUsingDelete()
    {
        _httpMethod = HttpMethod.Delete;
        return this;
    }

    public IQueryParamsStage WithRequestUri(string path)
    {
        _path = path;
        return this;
    }

    public IHeaderSelectionStage WithHeader(string name, string? value)
    {
        if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(name))
        {
            return this;
        }

        _headers.Add(name, value);
        return this;
    }

    public IHeaderSelectionStage WithHeader(string name, object? value)
    {
        return WithHeader(name, value?.ToString());
    }

    public IHeaderSelectionStage WithHeader<T>(string name, IEnumerable<T>? values)
    {
        if (values == null)
        {
            return this;
        }
        return WithHeader(name, string.Join(",", values));
    }

    public IHeaderSelectionStage WithBearerToken(string token)
    {
        _authenticationHeaderValue = new AuthenticationHeaderValue("bearer", token);
        return this;
    }

    public IBuildingStage WithBody(string body)
    {
        _body = body;
        return this;
    }

    public IBuildingStage WithBodyAsJson<T>(T body, JsonSerializerOptions? options = null)
    {
        options ??= new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };

        _body = JsonSerializer.Serialize(body, options);

        return this;
    }

    public IBuildingStage WithMultipartFormDataContent(byte[] buffer, string contentName, string fileName)
    {
        var multipartContent = new MultipartFormDataContent();
        multipartContent.Add(new StreamContent(new MemoryStream(buffer)), contentName, fileName);
        _content = multipartContent;
        return this;
    }

    public IHeaderSelectionStage WithQueryParams<T>(T queryParams)
    {
        if (queryParams is null)
        {
            return this;
        }

        var keyValuePairs = new List<KeyValuePair<string, string?>>();

        foreach (var x in queryParams.GetType().GetProperties().Where(x => x.GetValue(queryParams, null) != null))
        {
            if (x.PropertyType == typeof(DateTimeOffset))
            {
                var value = ((DateTimeOffset)x.GetValue(queryParams, null)!).ToString("O");
                keyValuePairs.Add(new KeyValuePair<string, string?>(x.Name, value));
            }
            else if (x.PropertyType == typeof(DateTimeOffset?))
            {
                var value = ((DateTimeOffset)x.GetValue(queryParams, null)!).ToString("O");
                keyValuePairs.Add(new KeyValuePair<string, string?>(x.Name, value));
            }
            else if (x.PropertyType.IsGenericType &&
                     x.PropertyType.GetInterfaces().ToList().Exists(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IList<>)))
            {
                if (x.GetValue(queryParams) is not IList values)
                {
                    continue;
                }

                keyValuePairs.AddRange(
                    from object? value in values select new KeyValuePair<string, string?>(x.Name, value.ToString())
                );
            }
            else if (x.PropertyType.IsArray)
            {
                if (x.GetValue(queryParams) is not Array values)
                {
                    continue;
                }

                keyValuePairs.AddRange(
                    from object? value in values select new KeyValuePair<string, string?>(x.Name, value.ToString())
                );
            }
            else
            {
                var value = x.GetValue(queryParams, null)!.ToString();
                keyValuePairs.Add(new KeyValuePair<string, string?>(x.Name, value));
            }
        }

        _queryString = QueryString.Create(keyValuePairs);

        return this;
    }

    public System.Net.Http.HttpRequestMessage Build()
    {
        var queryStringValue = _queryString.HasValue ? _queryString.Value : string.Empty;
        var requestUri = _path + queryStringValue;
        var httpRequestMessage = new System.Net.Http.HttpRequestMessage(_httpMethod, requestUri);
        foreach (var (name, value) in _headers)
        {
            httpRequestMessage.Headers.Add(name, value);
        }

        if (!string.IsNullOrEmpty(_body))
        {
            httpRequestMessage.Content = new StringContent(_body, Encoding.UTF8, MediaTypeNames.Application.Json);
        }

        if (_content is not null)
        {
            httpRequestMessage.Content = _content;
        }

        if (_authenticationHeaderValue is not null)
        {
            httpRequestMessage.Headers.Authorization = _authenticationHeaderValue;
        }

        return httpRequestMessage;
    }
}