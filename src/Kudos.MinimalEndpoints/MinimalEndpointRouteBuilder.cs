using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Kudos.MinimalEndpoints;

public abstract class MinimalEndpointRouteBuilder(IEndpointRouteBuilder endpointRouteBuilder)
{
    protected readonly IEndpointRouteBuilder EndpointRouteBuilder = endpointRouteBuilder;

    public abstract RouteHandlerBuilder MapGet([StringSyntax("Route")] string pattern);
    public abstract RouteHandlerBuilder MapPost([StringSyntax("Route")] string pattern);
    public abstract RouteHandlerBuilder MapPut([StringSyntax("Route")] string pattern);
    public abstract RouteHandlerBuilder MapDelete([StringSyntax("Route")] string pattern);
}