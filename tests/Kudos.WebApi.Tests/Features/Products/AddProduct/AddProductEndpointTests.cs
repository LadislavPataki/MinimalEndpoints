using System.Globalization;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using Kudos.WebApi.Features.Products.AddProduct;

namespace Kudos.WebApi.Tests.Features.Products.AddProduct;

public class AddProductEndpointTests(KudosWebApplicationFactory factory, ITestOutputHelper output)
    : IClassFixture<KudosWebApplicationFactory>
{
    private const string AddProductRoute = "/api/v1/products";

    [Theory]
    [InlineAutoData]
    public async Task GetProducts_should_return_201_when_success(
        string expectedName,
        string expectedDescription,
        decimal expectedPrice,
        int expectedQuantity)
    {
        var client = factory.CreateClient();

        using var content = new MultipartFormDataContent();
        var imageBytes = new byte[]
        {
            255, 255, 255, // Pixel 1: white
            255, 255, 255, // Pixel 2: white
            255, 255, 255, // Pixel 3: white
            255, 255, 255 // Pixel 4: white
        };

        var imageContent = new ByteArrayContent(imageBytes);
        imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
        content.Add(imageContent, "Attachments", "image.jpg");

        content.Add(new StringContent(expectedName), "Name");
        content.Add(new StringContent(expectedDescription), "Description");
        content.Add(new StringContent(expectedPrice.ToString(CultureInfo.InvariantCulture)), "Price");
        content.Add(new StringContent(expectedQuantity.ToString(CultureInfo.InvariantCulture)), "StockQuantity");
            
        var response = await client.PostAsync(AddProductRoute, content);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Theory]
    [InlineAutoData]
    public async Task GetProducts_should_return_added_Product(
        string expectedName,
        string expectedDescription,
        decimal expectedPrice,
        int expectedQuantity)
    {
        var client = factory.CreateClient();

        using var requestContent = new MultipartFormDataContent();
        var imageBytes = new byte[]
        {
            255, 255, 255, // Pixel 1: white
            255, 255, 255, // Pixel 2: white
            255, 255, 255, // Pixel 3: white
            255, 255, 255 // Pixel 4: white
        };

        var imageContent = new ByteArrayContent(imageBytes);
        imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
        requestContent.Add(imageContent, "Attachments", "image.jpg");

        requestContent.Add(new StringContent(expectedName), "Name");
        requestContent.Add(new StringContent(expectedDescription), "Description");
        requestContent.Add(new StringContent(expectedPrice.ToString(CultureInfo.InvariantCulture)), "Price");
        requestContent.Add(new StringContent(expectedQuantity.ToString(CultureInfo.InvariantCulture)), "StockQuantity");
        
        var httpRequestMessage = new System.Net.Http.HttpRequestMessage(HttpMethod.Post, AddProductRoute);
        httpRequestMessage.Content = requestContent;

        var (_, content) = await client.SendAndDeserializeAsync<AddProductResponse>(httpRequestMessage);

        content.Should().NotBeNull();
        content!.Id.Should().NotBeEmpty();
        content!.Name.Should().Be(expectedName);
        content!.Description.Should().Be(expectedDescription);
        content!.Price.Should().Be(expectedPrice);
        content!.StockQuantity.Should().Be(expectedQuantity);
    }
}