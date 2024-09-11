namespace Kudos.WebApi.Tests.Common.HttpRequestMessage.Stages;

public interface IHttpMethodSelectionStage
{
    IPathSelectionStage WithUsingGet();
    IPathSelectionStage WithUsingPost();
    IPathSelectionStage WithUsingPut();
    IPathSelectionStage WithUsingPatch();
    IPathSelectionStage WithUsingDelete();
}