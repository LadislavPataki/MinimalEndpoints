using System.ComponentModel.DataAnnotations;

namespace Kudos.WebApi.Features.Products.GetProducts;

public record GetProductsResponseItem(
    Guid Id,
    string Name,
    string Description,
    string ImageUrl,
    ProductStatus Status,
    int Price);

public enum ProductStatus
{
    OutOfStock = 0,
    Stocked = 1,
}

