using System;
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
		private readonly IImageClientWrapper _client;

		/// <summary>
		/// OpenaiImageServiceのコンストラクタ
		/// </summary>
		public OpenaiImageService(IImageClientWrapper client)
		{
			_client = client;
		}

		/// <summary>
		/// 指定されたプロンプトに基づいて画像を生成します。
		/// </summary>
		/// <param name="prompt">prompt</param>
		/// <returns>BitmapImage</returns>
		public async Task<BitmapImage?> GenerateImageAsync(string prompt) // Fix: Use nullable BitmapImage
		{
			// 指定されたプロンプトに基づいて画像を生成するメソッドを呼び出します。
			var bytes = await _client.GenerateImageAsync(prompt);

			// 生成された画像データがnullでない場合、BitmapImageを作成して返します。
			if (bytes != null && bytes.ToArray().Length > 0)
			{
				return _client.Convert(bytes.ToArray());
			}

			// 生成された画像データがnullまたは空の場合、nullを返します。
			return null;
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
