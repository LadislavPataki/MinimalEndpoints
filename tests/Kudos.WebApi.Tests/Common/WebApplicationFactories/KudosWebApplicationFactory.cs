using MartinCostello.Logging.XUnit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Kudos.WebApi.Tests.Common.WebApplicationFactories;

public class KudosWebApplicationFactory : WebApplicationFactory<Program>, ITestOutputHelperAccessor
{
    public ITestOutputHelper? OutputHelper { get; set; }

    public KudosWebApplicationFactory()
    {

    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var configuration = new List<KeyValuePair<string, string?>>
        {
            // override some configuration present in appsettings for testing purposes
        };
        
        builder
            .UseEnvironment("IntegrationTest")
            .UseConfiguration(new ConfigurationBuilder().AddInMemoryCollection(configuration).Build())
            .ConfigureAppConfiguration((_, configurationBuilder) =>
            {
                configurationBuilder.AddInMemoryCollection(configuration);
            })
            .ConfigureLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.AddXUnit(this);
            })
            .ConfigureServices(services =>
            {
                // override some services for testing purposes

            })
            .ConfigureTestServices(_ =>
            {
                // override some services for testing purposes
            });
    }
}