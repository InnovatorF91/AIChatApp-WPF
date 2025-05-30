using CreateAndEditImageServer.Services;
namespace CreateAndEditImageServer.Logics
{
	/// <summary>
	/// OpenAI画像生成ロジック
	/// </summary>
	public class OpenaiImageLogic : IOpenaiImageLogic
	{
		/// <summary>
		/// OpenAI画像生成サービス
		/// </summary>
		private readonly IOpenaiImageService _openaiImageService;

		/// <summary>
		/// OpenaiImageLogicのコンストラクタ。
		/// </summary>
		/// <param name="openaiImageService">openaiImageService</param>
		public OpenaiImageLogic(IOpenaiImageService openaiImageService)
		{
			_openaiImageService = openaiImageService;
		}

		/// <summary>
		/// OpenAIの画像生成サービスを使用して、プロンプトに基づいて画像を生成します。
		/// </summary>
		/// <param name="prompt">prompt</param>
		/// <returns>data URIスキーム</returns>
		public async Task<string?> GenerateImageAsync(string prompt)
		{
			// openaiImageServiceを使用して、プロンプトに基づいて画像を生成します。
			var bytes = await _openaiImageService.GenerateImageAsync(prompt);
			if (bytes != null && bytes.ToArray().Length > 0)
			{
				// 生成された画像をBase64文字列に変換します。
				string base64 = _openaiImageService.ConvertToBase64(bytes.ToArray());

				// Base64文字列をdata URIスキームに変換して返します。
				return $"data:image/png;base64,{base64}";
			}

			// 画像生成に失敗した場合はnullを返します。
			return null;
		}
	}

	/// <summary>
	/// Interface for OpenAI image generation logic.
	/// </summary>
	public interface IOpenaiImageLogic
	{
		/// <summary>
		/// OpenAIの画像生成サービスを使用して、プロンプトに基づいて画像を生成します。
		/// </summary>
		/// <param name="prompt">prompt</param>
		/// <returns>data URIスキーム</returns>
		Task<string?> GenerateImageAsync(string prompt);
	}
}
