namespace Kudos.WebApi.Tests.Common.HttpRequestMessage.Stages;

public interface IQueryParamsStage : IHeaderSelectionStage
{
    IHeaderSelectionStage WithQueryParams<T>(T queryParams);
}