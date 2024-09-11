using Kudos.MinimalEndpoints;

namespace Kudos.WebApi.Features.Products.GetProduct;

public class GetProductEndpoint : IEndpoint<GetProductRequest>
{
    public static void ConfigureEndpoint(MinimalEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder
            .MapGet("/products/{id:guid}")
            .Produces<GetProductResponse>()
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithName("GetProduct")
            .WithTags("Products")
            .WithOpenApi();
    }

    public async Task<IResult> HandleAsync(GetProductRequest request, CancellationToken cancellationToken)
    {
        var response = new GetProductResponse(
            request.Id, 
            "Product Name", 
            "Product Description", 
            "https://example.com/image.jpg", 
            ProductStatus.Stocked, 
            100,
            null
        );

        return TypedResults.Ok(response);
    }
}