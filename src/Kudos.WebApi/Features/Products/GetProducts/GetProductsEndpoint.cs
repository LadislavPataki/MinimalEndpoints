using Kudos.MinimalEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Kudos.WebApi.Features.Products.GetProducts;

public class GetProductsEndpoint : IEndpoint<GetProductsRequest>
{
    public static void ConfigureEndpoint(MinimalEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder
            .MapGet("/products")
            .Produces<List<GetProductsResponseItem>>()
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithName("GetProducts")
            .WithTags("Products")
            .WithOpenApi();
    }

    public async Task<IResult> HandleAsync(GetProductsRequest request, CancellationToken cancellationToken)
    {
        var response = new List<GetProductsResponseItem>
        {
            new(
                Guid.NewGuid(), // Id
                "Product Name 1", // Name
                "Product Description 1", // Description
                "https://example.com/image1.jpg", // ImageUrl
                ProductStatus.Stocked, // Status
                100 // Price
            ),
            new(
                Guid.NewGuid(), // Id
                "Product Name 2", // Name
                "Product Description 2", // Description
                "https://example.com/image2.jpg", // ImageUrl
                ProductStatus.Stocked, // Status
                200 // Price
            ),
            new(
                Guid.NewGuid(), // Id
                "Product Name 3", // Name
                "Product Description 3", // Description
                "https://example.com/image3.jpg", // ImageUrl
                ProductStatus.Stocked, // Status
                300 // Price
            )
        };
        
        return TypedResults.Ok(response);
    }
}