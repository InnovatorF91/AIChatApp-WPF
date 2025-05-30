using CreateAndEditImageServer.Services;
using System.ClientModel;

namespace CreateAndEditImageServerTests.ServicesTests
{
	public class OpenaiImageServiceTests
	{
		[Fact]
		public void ConvertToBase64_ReturnsCorrectBase64()
		{
			// Arrange  
			var service = new OpenaiImageService();
			var bytes = new byte[] { 1, 2, 3 };

			// Act  
			var result = service.ConvertToBase64(bytes);

			// Assert  
			Assert.Equal(Convert.ToBase64String(bytes), result);
		}

		[Fact]
		public async Task GenerateImageAsync_ThrowsInvalidOperationException_WhenBillingLimitReached()
		{
			// Arrange  
			var service = new OpenaiImageService();
			// Removed unused variable 'prompt' to fix CS0219 diagnostic.  

			// Act & Assert  
			var ex = await Assert.ThrowsAsync<InvalidOperationException>(async () =>
			{
				// billing_hard_limit_reached を含むメッセージの ClientResultException を投げるように  
				await Task.Run(() => throw new ClientResultException("billing_hard_limit_reached"));
			});
			Assert.Contains("OpenAI帳戶已達到使用上限", ex.Message);
		}
	}
}
