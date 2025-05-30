using System.Net.Http;
using System.Net.Http.Json;

namespace CreateAndEditImageApp.Repositories
{
	/// <summary>
	/// Repository for handling OpenAI image generation requests.
	/// </summary>
	public class OpenaiImageRepository : IOpenaiImageRepository
	{
		/// <summary>
		/// HTTP client for making requests to the OpenAI image generation API.
		/// </summary>
		private readonly HttpClient _httpClient;

		/// <summary>
		/// OpenaiImageRepositoryのコンストラクタ。
		/// </summary>
		public OpenaiImageRepository()
		{
			// Initialize the HttpClient with the base address of the OpenAI image generation API.
			_httpClient = new HttpClient
			{
				BaseAddress = new Uri("https://localhost:7110"),
				Timeout = TimeSpan.FromMinutes(5)
			};
		}

		/// <summary>
		/// Generates an image based on the provided prompt and returns the image as a base64 string.
		/// </summary>
		/// <param name="prompt">prompt</param>
		/// <returns>Base64</returns>
		public async Task<string?> GetImageBase64Async(string prompt)
		{
			// Validate the prompt to ensure it is not null or empty.
			var response = await _httpClient.PostAsJsonAsync("/api/OpenaiImage/GetImage", new { prompt });

			if (!response.IsSuccessStatusCode)
			{
				// 讀取伺服器傳來的 JSON 錯誤資訊
				var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResult>();

				// 拋出帶有錯誤訊息的例外
				throw new Exception(errorResponse?.error + "\n" + errorResponse?.suggestion);
			}

			// Read the response content as JSON and deserialize it into an ImageResponse object.
			var result = await response.Content.ReadFromJsonAsync<ImageResponse>();
			return result?.Image;
		}
	}

	/// <summary>
	/// Interface for the OpenAI image repository to handle image generation requests.
	/// </summary>
	public interface IOpenaiImageRepository
	{
		/// <summary>
		/// Generates an image based on the provided prompt and returns the image as a base64 string.
		/// </summary>
		/// <param name="prompt">prompt</param>
		/// <returns>Base64</returns>
		Task<string?> GetImageBase64Async(string prompt);
	}

	/// <summary>
	/// エラーリザルト
	/// </summary>
	public class ErrorResult
	{
		/// <summary>
		/// エラー
		/// </summary>
		public string? error { get; set; }

		/// <summary>
		/// サジェスチョン
		/// </summary>
		public string? suggestion { get; set; }
	}

	/// <summary>
	/// Represents the response from the OpenAI image generation API.
	/// </summary>
	public class ImageResponse
	{
		/// <summary>
		/// The base64 encoded image string returned by the OpenAI API.
		/// </summary>
		public string Image { get; set; } = string.Empty;
	}
}
