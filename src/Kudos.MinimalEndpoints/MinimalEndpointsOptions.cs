using Asp.Versioning;
using Asp.Versioning.Builder;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Kudos.MinimalEndpoints;

public class MinimalEndpointsOptions
{
    public Func<IEndpointRouteBuilder, ApiVersionSet> ApiVersionSet { get; set; } = endpointRouteBuilder =>
        endpointRouteBuilder
            .NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1))
            .ReportApiVersions()
            .Build();
}