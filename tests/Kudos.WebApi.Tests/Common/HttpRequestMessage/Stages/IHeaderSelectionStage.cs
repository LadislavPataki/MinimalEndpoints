namespace Kudos.WebApi.Tests.Common.HttpRequestMessage.Stages;

public interface IHeaderSelectionStage : IBodyStage
{
    IHeaderSelectionStage WithHeader(string name, string? value);
    IHeaderSelectionStage WithHeader(string name, object? value);
    IHeaderSelectionStage WithHeader<T>(string name, IEnumerable<T>? values);
    IHeaderSelectionStage WithBearerToken(string token);
}