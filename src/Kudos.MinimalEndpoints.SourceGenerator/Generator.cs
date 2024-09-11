﻿using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;

namespace Kudos.MinimalEndpoints.SourceGenerator;

[Generator(LanguageNames.CSharp)]
public class RouteEndpointBuilderGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var pipeline = context.CompilationProvider
            .Combine(context.AnalyzerConfigOptionsProvider)
            .Select((pair, _) => GetEndpoints(pair));

        context.RegisterSourceOutput(pipeline, GenerateCode);
    }

    private readonly record struct EndpointsToGenerate
    {
        public string RootNamespace { get; }
        public List<INamedTypeSymbol> Endpoints { get; }

        public EndpointsToGenerate(string rootNamespace, List<INamedTypeSymbol> endpoints)
        {
            RootNamespace = rootNamespace;
            Endpoints = endpoints;
        }
    }

    private static void GenerateCode(SourceProductionContext context, EndpointsToGenerate endpointsToGenerate)
    {
        GenerateEndpointRouteBuilders(context, endpointsToGenerate);
        GenerateServiceCollectionExtensions(context, endpointsToGenerate);
        GenerateWebApplicationExtensions(context, endpointsToGenerate);
    }

    private static void GenerateEndpointRouteBuilders(SourceProductionContext context, EndpointsToGenerate endpointsToGenerate)
    {
        foreach (var endpoint in endpointsToGenerate.Endpoints)
        {
            var namespaceName = endpoint.ContainingNamespace.ToDisplayString();
            var endpointName = endpoint.Name;
            var routeBuilderName = endpointName + "RouteBuilder";
            var requestTypeName = endpoint.Interfaces.FirstOrDefault()?.TypeArguments[0].Name;
            var handlerName = endpointName + "Handler";

            var endpointRouteBuilderSource =
                // lang=C#
                $$"""
                  // <auto-generated/>
                  #nullable enable

                  using System.Diagnostics.CodeAnalysis;
                  using Microsoft.AspNetCore.Builder;
                  using Microsoft.AspNetCore.Http;
                  using Microsoft.AspNetCore.Mvc;
                  using Microsoft.AspNetCore.Routing;
                  using Kudos.MinimalEndpoints;

                  namespace {{namespaceName}}
                  {
                      public class {{routeBuilderName}} : MinimalEndpointRouteBuilder
                      {
                          public {{routeBuilderName}}(IEndpointRouteBuilder endpointRouteBuilder)
                              : base(endpointRouteBuilder){ }
                  
                          public override RouteHandlerBuilder MapGet([StringSyntax("Route")] string pattern) =>
                              EndpointRouteBuilder
                                  .MapGet(pattern, {{handlerName}})
                                  .AddEndpointFilter<RequestValidationEndpointFilter<{{requestTypeName}}>>();
                          
                          public override RouteHandlerBuilder MapPost([StringSyntax("Route")] string pattern) =>
                              EndpointRouteBuilder
                                  .MapPost(pattern, {{handlerName}})
                                  .AddEndpointFilter<RequestValidationEndpointFilter<{{requestTypeName}}>>();
                          
                          public override RouteHandlerBuilder MapPut([StringSyntax("Route")] string pattern) =>
                              EndpointRouteBuilder
                                  .MapPut(pattern, {{handlerName}})
                                  .AddEndpointFilter<RequestValidationEndpointFilter<{{requestTypeName}}>>();
                          
                          public override RouteHandlerBuilder MapDelete([StringSyntax("Route")] string pattern) =>
                              EndpointRouteBuilder
                                  .MapDelete(pattern, {{handlerName}})
                                  .AddEndpointFilter<RequestValidationEndpointFilter<{{requestTypeName}}>>();
                          
                          private static Task<IResult> {{handlerName}}(
                              [NotNull, FromServices] {{endpointName}} endpoint,
                              [NotNull, AsParameters] {{requestTypeName}} request,
                              CancellationToken cancellationToken)
                              => endpoint.HandleAsync(request, cancellationToken);
                      }
                  }
                  """;

            context.AddSource($"{routeBuilderName}.g.cs", SourceText.From(endpointRouteBuilderSource, Encoding.UTF8));
        }
    }

    private static void GenerateWebApplicationExtensions(SourceProductionContext context, EndpointsToGenerate endpointsToGenerate)
    {
        var rootNameSpace = endpointsToGenerate.RootNamespace;
        var endpointConfigurations = new StringBuilder();

        foreach (var endpoint in endpointsToGenerate.Endpoints)
        {
            var fullyQualifiedEndpointName = endpoint.ToDisplayString();

            endpointConfigurations.Append(
                $"{fullyQualifiedEndpointName}.ConfigureEndpoint(new {fullyQualifiedEndpointName}RouteBuilder(versionedEndpointRouteBuilder));");
            endpointConfigurations.AppendLine();
            endpointConfigurations.Append("                ");
        }

        var mapMinimalEndpointsSource =
            // lang=C#
            $$"""
              // <auto-generated/>
              #nullable enable

              using Kudos.MinimalEndpoints;
              using Asp.Versioning.Builder;

              namespace {{rootNameSpace}}
              {
                  public static class WebApplicationExtensions
                  {
                      public static IEndpointRouteBuilder MapMinimalEndpoints(
                          this IEndpointRouteBuilder endpointRouteBuilder,
                          Action<MinimalEndpointsOptions>? options = null)
                          {
                              var minimalEndpointsOptions = new MinimalEndpointsOptions();
                              options?.Invoke(minimalEndpointsOptions);
                              
                              var apiVersionSet = minimalEndpointsOptions.ApiVersionSet.Invoke(endpointRouteBuilder);
                              
                              var versionedEndpointRouteBuilder = endpointRouteBuilder
                                  .MapGroup("api/v{apiVersion:apiVersion}")
                                  .WithApiVersionSet(apiVersionSet);
                              
                              {{endpointConfigurations}}
                              
                              return endpointRouteBuilder;
                          }
                  }
              }
              """;

        context.AddSource($"WebApplicationExtensions.g.cs", SourceText.From(mapMinimalEndpointsSource, Encoding.UTF8));

    }

    private static void GenerateServiceCollectionExtensions(SourceProductionContext context, EndpointsToGenerate endpointsToGenerate)
    {
        var rootNamespace = endpointsToGenerate.RootNamespace;
        var endpointRegistrations = new StringBuilder();

        foreach (var endpoint in endpointsToGenerate.Endpoints)
        {
            var fullyQualifiedEndpointName = endpoint.ToDisplayString();

            endpointRegistrations.Append($"services.AddScoped<{fullyQualifiedEndpointName}>();");
            endpointRegistrations.AppendLine();
            endpointRegistrations.Append("            ");
        }

        var addMinimalEndpointsSource =
            // lang=C#
            $$"""
              // <auto-generated/>
              #nullable enable

              using Kudos.MinimalEndpoints;
              using FluentValidation;

              namespace {{rootNamespace}}
              {
                  public static class ServiceCollectionExtensions
                  {
                      public static IServiceCollection AddMinimalEndpoints(this IServiceCollection services)
                      {
                          {{endpointRegistrations}}
                          return services;
                      }
                  }
              }
              """;

        context.AddSource($"ServiceCollectionExtensions.g.cs", SourceText.From(addMinimalEndpointsSource, Encoding.UTF8));
    }

    private static EndpointsToGenerate GetEndpoints((Compilation compilation, AnalyzerConfigOptionsProvider options) pair)
    {
        var (compilation, options) = pair;
        
        options.GlobalOptions.TryGetValue("build_property.RootNamespace", out var rootNamespace);

        var endpointInterfaceSymbol = compilation.GetTypeByMetadataName("Kudos.MinimalEndpoints.IEndpoint`1");

        if (endpointInterfaceSymbol == null)
            throw new InvalidOperationException("Could not find the IEndpoint<TRequest> interface.");
        
        var endpoints = new List<INamedTypeSymbol>();

        foreach (var syntaxTree in compilation.SyntaxTrees)
        {
            var semanticModel = compilation.GetSemanticModel(syntaxTree);
            var classNodes = syntaxTree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>();

            foreach (var classNode in classNodes)
            {
                var classSymbol = semanticModel.GetDeclaredSymbol(classNode) as ITypeSymbol;

                if (classSymbol is INamedTypeSymbol namedTypeSymbol &&
                    namedTypeSymbol.Interfaces.Any(i =>
                        SymbolEqualityComparer.Default.Equals(i.ConstructedFrom, endpointInterfaceSymbol)))
                {
                    endpoints.Add(namedTypeSymbol);
                }
            }
        }

        return new EndpointsToGenerate(rootNamespace ?? "GeneratedNamespace", endpoints);
    }
}