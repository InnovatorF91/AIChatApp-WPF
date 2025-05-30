using OpenAI.Images;
using System.ClientModel;

namespace CreateAndEditImageServer.Services
{
	/// <summary>
	/// OpenAI画像生成サービスの実装
	/// </summary>
	public class OpenaiImageService : IOpenaiImageService
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
		private readonly string apiKey = "Your OpenAI API key  "; // Your OpenAI API key  

		/// <summary>
		/// OpenaiImageServiceのコンストラクタ。
		/// </summary>
		public OpenaiImageService()
		{
			// Initialize the ImageClient with the model and API key.
			_imageClient = new ImageClient(model, apiKey);
		}

		/// <summary>
		/// OpenAIの画像生成サービスを使用して、プロンプトに基づいて画像を生成します。
		/// </summary>
		/// <param name="prompt">prompt</param>
		/// <returns>画像のバイナリデータ</returns>
		public async Task<BinaryData?> GenerateImageAsync(string prompt)
		{
			try
			{
				// OpenAIの画像生成クライアントを使用して、プロンプトに基づいて画像を生成します。
				var result = await _imageClient.GenerateImageAsync(prompt);

				// 生成された画像のバイナリデータを返します。
				return result?.Value?.ImageBytes;
			}
			catch (ClientResultException ex)when (ex.Message.Contains("billing_hard_limit_reached"))
			{
				// 明確拋出特殊錯誤給控制器
				throw new InvalidOperationException("OpenAI帳戶已達到使用上限，請檢查帳單或更換API Key。", ex);
			}
		}

		/// <summary>
		/// バイナリデータをBase64文字列に変換します。
		/// </summary>
		/// <param name="imageBytes">imageBytes</param>
		/// <returns>Base64文字列</returns>
		public string ConvertToBase64(byte[] imageBytes)
		{
			return Convert.ToBase64String(imageBytes);
		}
	}

	/// <summary>
	/// OpenAI画像生成サービスのインターフェース
	/// </summary>
	public interface IOpenaiImageService
	{
		/// <summary>
		/// OpenAIの画像生成サービスを使用して、プロンプトに基づいて画像を生成します。
		/// </summary>
		/// <param name="prompt">prompt</param>
		/// <returns>画像のバイナリデータ</returns>
		Task<BinaryData?> GenerateImageAsync(string prompt);

		/// <summary>
		/// バイナリデータをBase64文字列に変換します。
		/// </summary>
		/// <param name="imageBytes">imageBytes</param>
		/// <returns>Base64文字列</returns>
		string ConvertToBase64(byte[] imageBytes);
	}
}
