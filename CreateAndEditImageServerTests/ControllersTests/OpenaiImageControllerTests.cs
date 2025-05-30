using CreateAndEditImageServer.Controllers;
using CreateAndEditImageServer.Logics;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CreateAndEditImageServerTests.ControllersTests
{
	/// <summary>
	/// OpenaiImageControllerTestsのクラス
	/// </summary>
	public class OpenaiImageControllerTests
	{
		/// <summary>
		/// GenerateImageAsyncメソッドの正常系テスト。
		/// </summary>
		[Fact]
		public async void GenerateImageAsync_normal_returnOK()
		{
			// Arrange
			var prompt = "test prompt";
			var base64 = "base64image";
			var logicMock = new Mock<IOpenaiImageLogic>();
			logicMock.Setup(x => x.GenerateImageAsync(prompt)).ReturnsAsync(base64);

			var controller = new OpenaiImageController(logicMock.Object);
			var dto = new PromptDto { Prompt = prompt };

			// Act
			var result = await controller.GenerateImageAsync(dto);

			// Assert
			var okResult = Assert.IsType<OkObjectResult>(result);
			var value = Assert.IsType<ImageResponse>(okResult.Value);
			Assert.NotNull(value); // Ensure value is not null before accessing it
			Assert.Equal(base64, value.Image);
		}

		/// <summary>
		/// GenerateImageAsyncメソッドのプロンプトが空の場合のテスト。
		/// </summary>
		[Fact]
		public async void GenerateImageAsync_promptEmpty_BadRequest()
		{
			// Arrange  
			var logicMock = new Mock<IOpenaiImageLogic>();
			var controller = new OpenaiImageController(logicMock.Object);

			// Act  
			var result = await controller.GenerateImageAsync(new PromptDto { Prompt = string.Empty });

			// Assert  
			var badRequest = Assert.IsType<BadRequestObjectResult>(result);
			Assert.NotNull(badRequest.Value); // Ensure Value is not null before accessing it  
			Assert.Contains("Prompt cannot be null or empty", badRequest.Value.ToString());
		}

		/// <summary>
		/// GenerateImageAsyncメソッドの画像生成に失敗した場合のテスト。
		/// </summary>
		[Fact]
		public async void GenerateImageAsync_cannotCreateImage_NotFound()
		{
			// Arrange
			var prompt = "test";
			var logicMock = new Mock<IOpenaiImageLogic>();
			logicMock.Setup(x => x.GenerateImageAsync(prompt)).ReturnsAsync((string?)null);

			var controller = new OpenaiImageController(logicMock.Object);

			// Act
			var result = await controller.GenerateImageAsync(new PromptDto { Prompt = prompt });

			// Assert
			var notFound = Assert.IsType<NotFoundObjectResult>(result);
			Assert.NotNull(notFound.Value); // Ensure Value is not null before accessing it  
			Assert.Contains("Failed to generate image", notFound.Value.ToString());
		}

		/// <summary>
		/// GenerateImageAsyncメソッドのInvalidOperationExceptionが発生した場合のテスト。
		/// </summary>
		[Fact]
		public async void GenerateImageAsync_InvalidOperationException_BadRequest()
		{
			// Arrange
			var prompt = "test";
			var logicMock = new Mock<IOpenaiImageLogic>();
			logicMock.Setup(x => x.GenerateImageAsync(prompt)).ThrowsAsync(new InvalidOperationException("invalid op"));

			var controller = new OpenaiImageController(logicMock.Object);

			// Act
			var result = await controller.GenerateImageAsync(new PromptDto { Prompt = prompt });

			// Assert
			var badRequest = Assert.IsType<BadRequestObjectResult>(result);
			Assert.NotNull(badRequest.Value); // Ensure Value is not null before accessing it  
			Assert.Contains("invalid op", badRequest.Value.ToString());
		}

		/// <summary>
		/// GenerateImageAsyncメソッドの例外が発生した場合のテスト。
		/// </summary>
		[Fact]
		public async void GenerateImageAsync_ExceptionHappend_500error()
		{
			// Arrange
			var prompt = "test";
			var logicMock = new Mock<IOpenaiImageLogic>();
			logicMock.Setup(x => x.GenerateImageAsync(prompt)).ThrowsAsync(new Exception("unexpected"));

			var controller = new OpenaiImageController(logicMock.Object);

			// Act
			var result = await controller.GenerateImageAsync(new PromptDto { Prompt = prompt });

			// Assert
			var objectResult = Assert.IsType<ObjectResult>(result);
			Assert.Equal(500, objectResult.StatusCode);
			Assert.NotNull(objectResult.Value); // Ensure Value is not null before accessing it  
			Assert.Contains("Server Error", objectResult.Value.ToString());
		}
	}
}
