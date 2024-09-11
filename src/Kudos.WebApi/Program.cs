using Asp.Versioning;
using FluentValidation;
using Kudos.WebApi;
using Kudos.WebApi.JsonSerialization.Extensions;
using Kudos.WebApi.Swagger;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services
    .Configure<RouteHandlerOptions>(options => { options.ThrowOnBadRequest = false; });

// Configure json serialization
services
    .ConfigureHttpJsonOptions(options => 
        options.SerializerOptions.ConfigureDefaultOptions())
    .Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
        options.SerializerOptions.ConfigureDefaultOptions())
    .Configure<JsonOptions>(options =>
        options.JsonSerializerOptions.ConfigureDefaultOptions());


// Add services to the container.
services
    .AddEndpointsApiExplorer();

services
    .AddApiVersioning(options => { options.ReportApiVersions = true; })
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'V";
        options.SubstituteApiVersionInUrl = true;
    });

services
    .ConfigureOptions<ConfigureSwaggerOptions>()
    .AddSwaggerGen();

services
    .AddProblemDetails(options =>
    {
        options.CustomizeProblemDetails = problemDetailsContext =>
        {
            problemDetailsContext.ProblemDetails.Extensions.Add(
                "trace-id",
                problemDetailsContext.HttpContext.TraceIdentifier);

            problemDetailsContext.ProblemDetails.Extensions.Add(
                "instance",
                $"{problemDetailsContext.HttpContext.Request.Method} {problemDetailsContext.HttpContext.Request.Path}");
            // context.ProblemDetails.Extensions.Add(
            //     "my-detail", $"{context.HttpContext. .Request.Method} {context.HttpContext.Request.Path}");
        };
    });

services
    .AddMinimalEndpoints();

services
    .AddValidatorsFromAssemblyContaining<Program>();

services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

var app = builder.Build();

app.UseStatusCodePages();
app.UseExceptionHandler();

app.MapMinimalEndpoints(options =>
{
    options.ApiVersionSet = endpointRouteBuilder =>
        endpointRouteBuilder
            .NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1))
            .HasApiVersion(new ApiVersion(2))
            .ReportApiVersions()
            .Build();
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        var descriptions = app.DescribeApiVersions();

        // build a swagger endpoint for each discovered API version
        foreach (var groupName in descriptions.Select(description => description.GroupName).Reverse())
        {
            var url = $"/swagger/{groupName}/swagger.json";
            var name = groupName.ToUpperInvariant();
            options.SwaggerEndpoint(url, name);
        }

        // options.DocExpansion(DocExpansion.List);
        // options.EnableFilter();
        // options.EnableTryItOutByDefault();
        // options.EnablePersistAuthorization();
        options.EnableDeepLinking();
        // options.ConfigObject.AdditionalItems.Add("tagsSorter", "alpha");
        // options.ConfigObject.AdditionalItems.Add("operationsSorter", "alpha");
        // options.ConfigObject.AdditionalItems.Add("operationsSorter", "method");
        
        // // Custom operationsSorter
        // options.ConfigObject.AdditionalItems.Add("operationsSorter",
        //     "(a, b) => { const methodOrder = ['get', 'post', 'put', 'delete']; return methodOrder.indexOf(a.get('method')) - methodOrder.indexOf(b.get('method')); }");
    });
}

app.UseHttpsRedirection();

app.Run();

namespace Kudos.WebApi
{
    public partial class Program;
}
