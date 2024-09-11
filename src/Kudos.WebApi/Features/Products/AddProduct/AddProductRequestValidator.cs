using FluentValidation;

namespace Kudos.WebApi.Features.Products.AddProduct;

public class AddProductRequestValidator : AbstractValidator<AddProductRequest>
{
    public AddProductRequestValidator()
    {

    }
}