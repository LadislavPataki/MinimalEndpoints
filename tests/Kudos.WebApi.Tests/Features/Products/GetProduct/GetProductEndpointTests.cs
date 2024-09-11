using Kudos.WebApi.Features.Products.GetProduct;

namespace Kudos.WebApi.Tests.Features.Products.GetProduct;

public class GetProductEndpointTests : IClassFixture<KudosWebApplicationFactory>
{
    private readonly KudosWebApplicationFactory _factory;

    public GetProductEndpointTests(KudosWebApplicationFactory factory, ITestOutputHelper outputHelper)
    {
        _factory = factory;
        _factory.OutputHelper = outputHelper;
    }

    private static string GetProductRoute(Guid productId) => $"/api/v1/products/{productId}";

    [Theory]
    [InlineAutoData]
    public async Task GetProduct_should_return_200_when_success(Guid productId)
    {
        var client = _factory.CreateClient();

        var (statusCode, _) = await client.GetAndDeserializeAsync<GetProductResponse>(GetProductRoute(productId));

        statusCode.Should().Be(HttpStatusCode.OK);
    }

    [Theory]
    [InlineAutoData]
    public async Task GetProduct_should_return_Product_when_success(Guid productId)
    {
        var client = _factory.CreateClient();

        var (_, content) = await client.GetAndDeserializeAsync<GetProductResponse>(GetProductRoute(productId));

        content.Should().NotBeNull();
    }
}