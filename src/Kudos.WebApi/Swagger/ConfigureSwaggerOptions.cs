using System.Text;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Kudos.WebApi.Swagger;

public class ConfigureSwaggerOptions(IApiVersionDescriptionProvider versionDescriptionProvider) 
    : IConfigureOptions<SwaggerGenOptions>
{
    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in versionDescriptionProvider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(
                name: description.GroupName, 
                info: CreateInfoForApiVersion(description));
        }

        // options.OrderActionsBy(apiDesc =>
        // {
        //     // var s = $"{apiDesc.ActionDescriptor.EndpointMetadata
        //     //     .OfType<OpenApiOperation>()
        //     //     .FirstOrDefault()?.Tags
        //     //     .FirstOrDefault()?.Name}_{apiDesc.RelativePath}_{apiDesc.HttpMethod switch
        //     // {
        //     //     "GET" => 1,
        //     //     "POST" => 2,
        //     //     "PUT" => 3,
        //     //     "DELETE" => 4,
        //     //     "PATCH" => 5,
        //     //     "OPTIONS" => 6,
        //     //     "HEAD" => 7,
        //     //     _ => 8
        //     // }}";
        //     
        //     // var s = $"{apiDesc.ActionDescriptor.EndpointMetadata
        //     //     .OfType<OpenApiOperation>()
        //     //     .FirstOrDefault()?.Tags
        //     //     .FirstOrDefault()?.Name}_{apiDesc.HttpMethod switch
        //     // {
        //     //     "GET" => 1,
        //     //     "POST" => 2,
        //     //     "PUT" => 3,
        //     //     "DELETE" => 4,
        //     //     "PATCH" => 5,
        //     //     "OPTIONS" => 6,
        //     //     "HEAD" => 7,
        //     //     _ => 8
        //     // }}_{apiDesc.RelativePath}";
        //     
        //     var s = $"{apiDesc.HttpMethod switch
        //     {
        //         "GET" => 1,
        //         "POST" => 2,
        //         "PUT" => 3,
        //         "DELETE" => 4,
        //         "PATCH" => 5,
        //         "OPTIONS" => 6,
        //         "HEAD" => 7,
        //         _ => 8
        //     }}";
        //     
        //     // var s = $"{apiDesc.ActionDescriptor.EndpointMetadata
        //     //     .OfType<OpenApiOperation>()
        //     //     .FirstOrDefault()?.Tags
        //     //     .FirstOrDefault()?.Name}_{apiDesc.RelativePath}";
        //     
        //     return s;
        // });
        
        options.CustomSchemaIds(CustomSchemaIdSelector);
        
        options.UseOneOfForPolymorphism();
        // options.UseAllOfForInheritance();
        options.SupportNonNullableReferenceTypes();
        // options.UseAllOfToExtendReferenceSchemas();
        options.DescribeAllParametersInCamelCase();
    }

    private static string CustomSchemaIdSelector(Type modelType)
    {
        var @namespace = modelType.Namespace;
        var featureName = @namespace?.Split('.')[^1] ?? string.Empty;

        if (!modelType.IsConstructedGenericType)
        {
            var modelName = modelType.Name.Replace("[]", "Array");
            return modelName.StartsWith(featureName) ? modelName : featureName + modelName;
        }

        var prefix = modelType.GetGenericArguments()
            .Select(CustomSchemaIdSelector)
            .Aggregate((previous, current) => $"{previous}{current}");

        var modelNameWithPrefix = $"{prefix}{modelType.Name.Split('`')[0]}";
        return modelNameWithPrefix.StartsWith(featureName) ? modelNameWithPrefix : featureName + modelNameWithPrefix;
    }

    private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
    {
        var descriptionBuilder = new StringBuilder("An example application with OpenAPI, Swashbuckle, and API versioning.");
        
        var info = new OpenApiInfo
        {
            Title = "Kudos API",
            Version = description.ApiVersion.ToString(),
            Contact = new OpenApiContact { Name = "Ladislav Pataki", Email = "ladislav.pataki@eurowag.com" },
            License = new OpenApiLicense { Name = "MIT", Url = new Uri("https://opensource.org/licenses/MIT") }
        };

        if (description.IsDeprecated)
        {
            descriptionBuilder.Append(" This API version has been deprecated.");
        }

        info.Description = descriptionBuilder.ToString();

        return info;
    }
}