namespace Kudos.WebApi.Tests.Features.Products.RemoveProduct;

public class RemoveProductEndpointTests(KudosWebApplicationFactory factory, ITestOutputHelper output)
    : IClassFixture<KudosWebApplicationFactory>
{
    private static string DeleteProductRoute(Guid productId) => $"/api/v1/products/{productId}";

    [Theory]
    [InlineAutoData]
    public async Task GetProducts_should_return_204_when_success(Guid productId)
    {
        var client = factory.CreateClient();

        var (statusCode, _) = await client.DeleteAndDeserializeAsync<object>(DeleteProductRoute(productId));

        statusCode.Should().Be(HttpStatusCode.NoContent);
    }
}