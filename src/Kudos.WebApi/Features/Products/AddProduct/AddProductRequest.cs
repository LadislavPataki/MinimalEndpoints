using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Kudos.WebApi.Features.Products.AddProduct;

public record AddProductRequest(
    [property: Required, FromForm] string Name,
    [property: Required, FromForm] string Description,
    [property: Required, FromForm] decimal Price,
    [property: Required, FromForm] int StockQuantity,
    IFormFileCollection Attachments);