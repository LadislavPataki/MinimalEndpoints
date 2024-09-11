namespace Kudos.WebApi.Tests.Common.HttpRequestMessage.Stages;

public interface IPathSelectionStage
{
    IQueryParamsStage WithRequestUri(string path);
}