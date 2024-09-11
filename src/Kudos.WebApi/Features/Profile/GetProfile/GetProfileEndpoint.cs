using Kudos.MinimalEndpoints;

namespace Kudos.WebApi.Features.Profile.GetProfile;

public class GetProfileEndpoint : IEndpoint<GetProfileRequest>
{
    public static void ConfigureEndpoint(MinimalEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder
            .MapGet("/profiles/me")
            .WithName("GetMyProfile")
            .WithTags("Profiles")
            .WithOpenApi();
    }

    public async Task<IResult> HandleAsync(GetProfileRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}