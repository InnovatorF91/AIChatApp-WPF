using OpenAI.Images;
using System.IO;
using System.Windows.Media.Imaging;

namespace CreateAndEditImageApp.Services
{
	/// <summary>
	/// OpenAIの画像生成サービス
	/// </summary>
	public class OpenaiImageService : IOpenaiImageService
	{
		/// <summary>
		/// OpenAIの画像生成クライアント
		/// </summary>
		private ImageClient imageClient; // Use ImageClient instead of OpenAIClient

		/// <summary>
		/// OpenAIの画像生成モデル
		/// </summary>
		private readonly string model = "gpt-image-1"; // Default model

		/// <summary>
		/// OpenAIのAPIキー
		/// </summary>
		private readonly string apiKey = "Your OpenAI API key"; // Your OpenAI API key

		/// <summary>
		/// OpenaiImageServiceのコンストラクタ
		/// </summary>
		public OpenaiImageService()
		{
			imageClient = new ImageClient(model, apiKey); // Initialize ImageClient
		}

		/// <summary>
		/// 指定されたプロンプトに基づいて画像を生成します。
		/// </summary>
		/// <param name="prompt">prompt</param>
		/// <returns>BitmapImage</returns>
		public async Task<BitmapImage?> GenerateImageAsync(string prompt) // Fix: Use nullable BitmapImage
		{
			var result = await imageClient.GenerateImageAsync(prompt);

			if (result?.Value?.ImageBytes != null && result.Value.ImageBytes.ToArray().Length > 0) // Fix: Use ToArray() to access the byte array
			{
				using var ms = new MemoryStream(result.Value.ImageBytes.ToArray()); // Fix: Convert BinaryData to byte array
				var image = new BitmapImage();
				image.BeginInit();
				image.CacheOption = BitmapCacheOption.OnLoad;
				image.StreamSource = ms;
				image.EndInit();
				image.Freeze();
				return image;
			}

			return null; // Fix: Explicitly return null for nullable BitmapImage
		}
	}

	/// <summary>
	/// OpenAIの画像生成サービスのインターフェース
	/// </summary>
	public interface IOpenaiImageService
	{
		/// <summary>
		/// 指定されたプロンプトに基づいて画像を生成します。
		/// </summary>
		/// <param name="prompt">prompt</param>
		/// <returns>BitmapImage</returns>
		Task<BitmapImage?> GenerateImageAsync(string prompt); // Fix: Update interface to match nullable return type
	}
}
