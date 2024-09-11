using System.ComponentModel.DataAnnotations;

namespace Kudos.WebApi.Features.Products.GetProduct;

public record GetProductResponse(
    [property: Required] Guid Id,
    [property: Required] string Name,
    [property: Required] string Description,
    [property: Required] string ImageUrl,
    [property: Required] ProductStatus Status,
    [property: Required] int Price,
    SomeGenericThing<string, int> GenericThing);

public enum ProductStatus
{
    OutOfStock = 0,
    Stocked = 1,
}


public record SomeGenericThing<T1, T2>(string Prop, T1 BaseProp, T2 BaseProp2) : SomeBasType<T1, T2>(BaseProp, BaseProp2);

public record SomeBasType<T1, T2>(T1 BaseProp, T2 BaseProp2);