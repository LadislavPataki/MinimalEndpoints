using Microsoft.AspNetCore.Http;

namespace Kudos.MinimalEndpoints;

public interface IEndpoint<TRequest> //where TRequest : class
{
    static abstract void ConfigureEndpoint(MinimalEndpointRouteBuilder endpointRouteBuilder);
    
    Task<IResult> HandleAsync(TRequest request, CancellationToken cancellationToken);
}

public interface IEndpointWitTypedResponse<in TRequest, TResponse>
    where TRequest : class
    where TResponse : IResult
{
    static abstract void ConfigureEndpoint(MinimalEndpointRouteBuilder endpointRouteBuilder);
    
    Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken);
}