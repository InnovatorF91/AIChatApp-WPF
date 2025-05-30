using CreateAndEditImageServer.Logics;
using CreateAndEditImageServer.Services;
using Moq;

namespace CreateAndEditImageServerTests.LogicsTests
{
	/// <summary>
	/// OpenaiImageLogicTestsのクラス
	/// </summary>
	public class OpenaiImageLogicTests
	{
		/// <summary>
		/// GenerateImageAsyncメソッドの正常系テスト。
		/// </summary>
		/// <returns></returns>
		[Fact]
		public async void GenerateImageAsync_ReturnsDataUri_WhenImageGenerated()
		{
			// Arrange
			var prompt = "test prompt";
			var imageBytes = new byte[] { 1, 2, 3 };
			var base64 = "QUJD"; // 適当なBase64文字列
			var expected = $"data:image/png;base64,{base64}";

			var serviceMock = new Mock<IOpenaiImageService>();
			serviceMock.Setup(s => s.GenerateImageAsync(prompt))
				.ReturnsAsync(BinaryData.FromBytes(imageBytes));
			serviceMock.Setup(s => s.ConvertToBase64(imageBytes))
				.Returns(base64);

			var logic = new OpenaiImageLogic(serviceMock.Object);

			// Act
			var result = await logic.GenerateImageAsync(prompt);

			// Assert
			Assert.Equal(expected, result);
		}

		/// <summary>
		/// GenerateImageAsyncメソッドのサービスがnullを返す場合のテスト。
		/// </summary>
		[Fact]
		public async void GenerateImageAsync_ReturnsNull_WhenServiceReturnsNull()
		{
			// Arrange
			var prompt = "test prompt";
			var serviceMock = new Mock<IOpenaiImageService>();
			serviceMock.Setup(s => s.GenerateImageAsync(prompt))
				.ReturnsAsync((BinaryData?)null);

			var logic = new OpenaiImageLogic(serviceMock.Object);

			// Act
			var result = await logic.GenerateImageAsync(prompt);

			// Assert
			Assert.Null(result);
		}

		/// <summary>
		/// GenerateImageAsyncメソッドのサービスが空のバイト配列を返す場合のテスト。
		/// </summary>
		[Fact]
		public async void GenerateImageAsync_ReturnsNull_WhenServiceReturnsEmptyBytes()
		{
			// Arrange
			var prompt = "test prompt";
			var serviceMock = new Mock<IOpenaiImageService>();
			serviceMock.Setup(s => s.GenerateImageAsync(prompt))
				.ReturnsAsync(BinaryData.FromBytes(Array.Empty<byte>()));

			var logic = new OpenaiImageLogic(serviceMock.Object);

			// Act
			var result = await logic.GenerateImageAsync(prompt);

			// Assert
			Assert.Null(result);
		}
	}
}
