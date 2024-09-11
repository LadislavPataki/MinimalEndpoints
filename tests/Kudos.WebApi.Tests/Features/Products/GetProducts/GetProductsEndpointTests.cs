using Kudos.WebApi.Features.Products.GetProducts;

namespace Kudos.WebApi.Tests.Features.Products.GetProducts;

public class GetProductsEndpointTests(KudosWebApplicationFactory factory, ITestOutputHelper output)
    : IClassFixture<KudosWebApplicationFactory>
{
    private const string GetProductsRoute = "/api/v1/products";

    [Fact]
    public async Task GetProducts_should_return_200_when_success()
    {
        var client = factory.CreateClient();

        var (statusCode, _) = await client.GetAndDeserializeAsync<List<GetProductsResponseItem>>(GetProductsRoute);

        statusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetProducts_should_return_Products_when_success()
    {
        var client = factory.CreateClient();

        var (_, content) = await client.GetAndDeserializeAsync<List<GetProductsResponseItem>>(GetProductsRoute);

        content.Should().NotBeNullOrEmpty();
    }
}