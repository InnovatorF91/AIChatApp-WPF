using CreateAndEditImageApp.Services;
using Moq;
using System.Windows.Media.Imaging;

namespace CreateAndEditImageAppTests
{
	/// <summary>
	/// OpenaiImageServiceのテストクラス
	/// </summary>
	public class OpenaiImageServiceTests
	{
		/// <summary>
		/// GenerateImageAsyncメソッドが有効なデータを受け取った場合、BitmapImageを返すことを確認します。
		/// </summary>
		[Fact]
		public async void GenerateImageAsync_ReturnsBitmapImage_WhenDataIsValid()
		{
			// Arrange
			var fakeImageBytes = new byte[] { 137, 80, 78, 71 }; // PNG header
			var binaryData = BinaryData.FromBytes(fakeImageBytes);

			var clientMock = new Mock<IImageClientWrapper>();
			clientMock.Setup(c => c.GenerateImageAsync("test"))
					  .ReturnsAsync(binaryData);

			var image = new BitmapImage();
			clientMock.Setup(c => c.Convert(It.IsAny<byte[]>()))
					  .Returns(image);

			var service = new OpenaiImageService(clientMock.Object);

			// Act
			var result = await service.GenerateImageAsync("test");

			// Assert
			Assert.NotNull(result);
			Assert.IsType<BitmapImage>(result);
		}

		/// <summary>
		/// GenerateImageAsyncメソッドが無効なデータを受け取った場合、nullを返すことを確認します。
		/// </summary>
		[Fact]
		public async void GenerateImageAsync_ReturnsNull_WhenDataIsNull()
		{
			// Arrange
			var clientMock = new Mock<IImageClientWrapper>();
			clientMock.Setup(c => c.GenerateImageAsync("test"))
					  .ReturnsAsync((BinaryData?)null);

			var service = new OpenaiImageService(clientMock.Object);

			// Act
			var result = await service.GenerateImageAsync("test");

			// Assert
			Assert.Null(result);
		}
	}
}
