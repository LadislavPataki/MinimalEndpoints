using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Kudos.MinimalEndpoints;

public class RequestValidationEndpointFilter<TRequest>(IServiceProvider serviceProvider) : IEndpointFilter
{
    private readonly IValidator<TRequest>? _validator = serviceProvider.GetService<IValidator<TRequest>>();

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        const int requestIndex = 1;
        var request = context.GetArgument<TRequest>(requestIndex);

        if (_validator is null)
            return await next(context);

        var validationResult =
            await _validator.ValidateAsync(request, context.HttpContext.RequestAborted);

        if (validationResult.IsValid)
            return await next(context);

        return TypedResults.ValidationProblem(validationResult.ToDictionary());
    }
}