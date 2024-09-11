using System.ComponentModel.DataAnnotations;

namespace Kudos.WebApi.Features.Products.AddProduct;

public record AddProductResponse(
    [property: Required] Guid Id,
    [property: Required] string Name,
    [property: Required] string Description,
    [property: Required] decimal Price,
    [property: Required] int StockQuantity);