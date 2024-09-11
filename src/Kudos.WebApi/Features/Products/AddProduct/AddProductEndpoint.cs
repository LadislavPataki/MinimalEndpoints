using System.Net;
using Kudos.MinimalEndpoints;

namespace Kudos.WebApi.Features.Products.AddProduct;

public class AddProductEndpoint(IHttpContextAccessor httpContextAccessor, LinkGenerator linkGenerator) 
    : IEndpoint<AddProductRequest>
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly LinkGenerator _linkGenerator = linkGenerator;

    public static void ConfigureEndpoint(MinimalEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder
            .MapPost("/products")
            .MapToApiVersion(1)
            .MapToApiVersion(2)
            .Produces<AddProductResponse>(StatusCodes.Status201Created)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesValidationProblem(StatusCodes.Status409Conflict)
            .ProducesValidationProblem(StatusCodes.Status422UnprocessableEntity)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithName("AddProduct")
            .WithTags("Products")
            .DisableAntiforgery();
        // .WithOpenApi();
    }

    public async Task<IResult> HandleAsync(AddProductRequest request, CancellationToken cancellationToken)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        
        var productId = Guid.NewGuid();
        var response = new AddProductResponse(
            Id: productId,
            Name: request.Name,
            Description: request.Description,
            Price: request.Price,
            StockQuantity: request.StockQuantity);
        
        var uri = _linkGenerator.GetUriByName(
            httpContext, 
            endpointName: "GetProduct",
            new { id = productId }
        );

        return TypedResults.Created(uri, response);
    }
}

// public class CreateProductEndpoint2 : IEndpointWitTypedResponse<CreateProductRequest, Ok<CreateProductResponse>>
// {
//     public static void ConfigureEndpoint(MinimalEndpointRouteBuilder endpointRouteBuilder)
//     {
//         endpointRouteBuilder
//             .MapPost("/products")
//             .WithName("CreateProduct")
//             .WithTags("Products")
//             .WithOpenApi();
//     }
//
//     public async Task<Ok<CreateProductResponse>> HandleAsync(CreateProductRequest request,
//         CancellationToken cancellationToken)
//     {
//         var response = new CreateProductResponse(
//             Guid.NewGuid(),
//             "NewProductName",
//             "NewProductDescription",
//             12,
//             123);
//
//         return TypedResults.Ok(response);
//     }
// }

// public class CreateProductEndpoint3 : IEndpointWitTypedResponse<CreateProductRequest,
//     Results<Ok<CreateProductResponse>, Conflict>>
// {
//     public static void ConfigureEndpoint(MinimalEndpointRouteBuilder endpointRouteBuilder)
//     {
//         endpointRouteBuilder
//             .MapPost("/products")
//             .WithName("CreateProduct")
//             .WithTags("Products")
//             .WithOpenApi();
//     }
//
//     public async Task<Results<Ok<CreateProductResponse>, Conflict>> HandleAsync(CreateProductRequest request,
//         CancellationToken cancellationToken)
//     {
//         if (request.CreateProductRequestBody.Name == "invalidName")
//             return TypedResults.Conflict();
//         
//         var response = new CreateProductResponse(
//             Guid.NewGuid(),
//             "NewProductName",
//             "NewProductDescription",
//             12,
//             123);
//
//         return TypedResults.Ok(response);
//     }
// }