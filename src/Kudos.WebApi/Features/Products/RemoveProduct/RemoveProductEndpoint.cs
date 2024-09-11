using Kudos.MinimalEndpoints;

namespace Kudos.WebApi.Features.Products.RemoveProduct;

public class RemoveProductEndpoint : IEndpoint<RemoveProductRequest>
{
    public static void ConfigureEndpoint(MinimalEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder
            .MapDelete("/products/{id:guid}")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithName("DeleteProduct")
            .WithTags("Products")
            .WithOpenApi();
    }

    public async Task<IResult> HandleAsync(RemoveProductRequest request, CancellationToken cancellationToken)
    {
        return TypedResults.NoContent();
    }
}