using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Kudos.WebApi.Features.Products.RemoveProduct;

public record RemoveProductRequest(
    [property: Required, FromRoute] Guid Id);