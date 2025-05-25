using OpenAI.Images;
using System.IO;
using System.Windows.Media.Imaging;

namespace CreateAndEditImageApp.Services
{
	/// <summary>
	/// OpenAIの画像生成クライアントラッパー
	/// </summary>
	public class ImageClientWrapper : IImageClientWrapper
	{
		/// <summary>
		/// OpenAIの画像生成クライアント
		/// </summary>
		private readonly ImageClient _imageClient;

		/// <summary>
		/// OpenAIの画像生成モデル
		/// </summary>
		private readonly string model = "gpt-image-1"; // Default model

		/// <summary>
		/// OpenAIのAPIキー
		/// </summary>
		private readonly string apiKey = "Your OpenAI API key"; // Your OpenAI API key

		/// <summary>
		/// OpenAIの画像生成クライアントラッパーのコンストラクタ
		/// </summary>
		public ImageClientWrapper()
		{
			_imageClient = new ImageClient(model, apiKey);
		}

		/// <summary>
		/// バイト配列をBitmapImageに変換します。
		/// </summary>
		/// <param name="imageBytes">imageBytes</param>
		/// <returns>imageBytes</returns>
		public BitmapImage Convert(byte[] imageBytes)
		{
			using var ms = new MemoryStream(imageBytes);
			var image = new BitmapImage();
			image.BeginInit();
			image.CacheOption = BitmapCacheOption.OnLoad;
			image.StreamSource = ms;
			image.EndInit();
			image.Freeze();
			return image;
		}

		/// <summary>
		/// 指定されたプロンプトに基づいて画像を生成します。
		/// </summary>
		/// <param name="prompt">入力したテキスト</param>
		/// <returns>ImageBytes</returns>
		public async Task<BinaryData?> GenerateImageAsync(string prompt)
		{
			var result = await _imageClient.GenerateImageAsync(prompt);
			return result?.Value?.ImageBytes;
		}
	}

	/// <summary>
	/// OpenAIの画像生成クライアントラッパーのインターフェース
	/// </summary>
	public interface IImageClientWrapper
	{
		/// <summary>
		/// 指定されたプロンプトに基づいて画像を生成します。
		/// </summary>
		/// <param name="prompt">入力したテキスト</param>
		/// <returns>ImageBytes</returns>
		Task<BinaryData?> GenerateImageAsync(string prompt);

		/// <summary>
		/// バイト配列をBitmapImageに変換します。
		/// </summary>
		/// <param name="imageBytes">imageBytes</param>
		/// <returns>imageBytes</returns>
		BitmapImage Convert(byte[] imageBytes);
	}
}
