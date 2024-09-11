using System.Text.Json;

namespace Kudos.WebApi.Tests.Common.HttpRequestMessage.Stages;

public interface IBodyStage : IBuildingStage
{
    IBuildingStage WithBody(string body);

    IBuildingStage WithBodyAsJson<T>(T body, JsonSerializerOptions? options = null);

    IBuildingStage WithMultipartFormDataContent(byte[] buffer, string contentName, string fileName);
}