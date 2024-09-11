using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Kudos.WebApi.Features.Products.GetProduct;

public record GetProductRequest(
    [property: Required, FromRoute] Guid Id);