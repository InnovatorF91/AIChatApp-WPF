using CreateAndEditImageApp.Repositories;
using Moq.Protected;
using Moq;
using System.Net.Http.Json;
using System.Net;

namespace CreateAndEditImageAppTests.RepositoriesTest
{
	/// <summary>
	/// OpenaiImageRepositoryTestsのテストクラス
	/// </summary>
	public class OpenaiImageRepositoryTests
	{
		/// <summary>
		/// ImageBase64を取得するメソッドの正常系テスト。
		/// </summary>
		[Fact]
		public async void GetImageBase64Async_normal_returnBase64()
		{
			// Arrange
			var expectedBase64 = "base64string";
			var handlerMock = new Mock<HttpMessageHandler>();
			handlerMock.Protected()
				.Setup<Task<HttpResponseMessage>>(
					"SendAsync",
					ItExpr.IsAny<HttpRequestMessage>(),
					ItExpr.IsAny<CancellationToken>())
				.ReturnsAsync(new HttpResponseMessage
				{
					StatusCode = HttpStatusCode.OK,
					Content = JsonContent.Create(new ImageResponse { Image = expectedBase64 })
				});

			var httpClient = new HttpClient(handlerMock.Object)
			{
				BaseAddress = new Uri("https://localhost:7110")
			};

			var repo = new OpenaiImageRepositoryTestable(httpClient);

			// Act
			var result = await repo.GetImageBase64Async("prompt");

			// Assert
			Assert.Equal(expectedBase64, result);
		}

		/// <summary>
		/// GetImageBase64Asyncメソッドのエラー系テスト。
		/// </summary>
		[Fact]
		public async void GetImageBase64Async_error_throwException()
		{
			// Arrange
			var error = new ErrorResult { error = "エラー", suggestion = "サジェスト" };
			var handlerMock = new Mock<HttpMessageHandler>();
			handlerMock.Protected()
				.Setup<Task<HttpResponseMessage>>(
					"SendAsync",
					ItExpr.IsAny<HttpRequestMessage>(),
					ItExpr.IsAny<CancellationToken>())
				.ReturnsAsync(new HttpResponseMessage
				{
					StatusCode = HttpStatusCode.BadRequest,
					Content = JsonContent.Create(error)
				});

			var httpClient = new HttpClient(handlerMock.Object)
			{
				BaseAddress = new Uri("https://localhost:7110")
			};

			var repo = new OpenaiImageRepositoryTestable(httpClient);

			// Act & Assert
			var ex = await Assert.ThrowsAsync<Exception>(() => repo.GetImageBase64Async("prompt"));
			Assert.Contains(error.error, ex.Message);
			Assert.Contains(error.suggestion, ex.Message);
		}

		/// <summary>
		/// HttpClientを注入できるように継承クラスを作成
		/// </summary>
		private class OpenaiImageRepositoryTestable : OpenaiImageRepository
		{
			/// <summary>
			/// OpenaiImageRepositoryTestableのコンストラクタ。
			/// </summary>
			/// <param name="client">HttpClient</param>
			public OpenaiImageRepositoryTestable(HttpClient client)
			{
				// HttpClientを非公開フィールドに設定
				typeof(OpenaiImageRepository)
					.GetField("_httpClient", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
					?.SetValue(this, client);
			}
		}
	}
}
