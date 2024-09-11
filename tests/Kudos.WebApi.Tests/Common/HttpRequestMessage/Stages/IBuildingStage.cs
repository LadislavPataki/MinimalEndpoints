namespace Kudos.WebApi.Tests.Common.HttpRequestMessage.Stages;

public interface IBuildingStage
{
    System.Net.Http.HttpRequestMessage Build();
}