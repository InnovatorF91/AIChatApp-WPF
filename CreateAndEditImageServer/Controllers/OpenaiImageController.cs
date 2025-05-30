using CreateAndEditImageServer.Logics;
using Microsoft.AspNetCore.Mvc;

namespace CreateAndEditImageServer.Controllers
{
	/// <summary>
	/// OpenAI画像生成コントローラー
	/// </summary>
	[ApiController]
	[Route("api/[controller]")]
	public class OpenaiImageController : ControllerBase
	{
		/// <summary>
		/// OpenAI画像生成ロジック
		/// </summary>
		private readonly IOpenaiImageLogic _openaiImageLogic;

		/// <summary>
		/// OpenaiImageControllerのコンストラクタ。
		/// </summary>
		/// <param name="openaiImageLogic">openaiImageLogic</param>
		public OpenaiImageController(IOpenaiImageLogic openaiImageLogic)
		{
			_openaiImageLogic = openaiImageLogic;
		}

		/// <summary>
		/// Generates an image based on the provided prompt and returns the image as a base64 string.
		/// </summary>
		/// <param name="dto">Prompt</param>
		/// <returns>ActionResult</returns>
		[HttpPost("GetImage")]
		public async Task<IActionResult> GenerateImageAsync([FromBody] PromptDto dto)
		{
			// Validate the prompt to ensure it is not null or empty.
			if (string.IsNullOrWhiteSpace(dto?.Prompt))
			{
				return BadRequest(new { error = "Prompt cannot be null or empty." });
			}

			try
			{
				// Generate the image using the OpenAI image logic.
				var base64 = await _openaiImageLogic.GenerateImageAsync(dto.Prompt);

				if (base64 == null)
				{
					// If the image generation fails, return a NotFound result with an error message.
					return NotFound(new { error = "Failed to generate image." });
				}

				// Return the generated image as a base64 string in the response.
				return Ok(new ImageResponse { Image = base64 });
			}
			catch (InvalidOperationException ex)
			{
				return BadRequest(new { error = ex.Message });
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { error = "Server Error", detail = ex.Message });
			}
		}
	}

	/// <summary>
	/// DTO for the prompt used in image generation.
	/// </summary>
	public class PromptDto
	{
		public string? Prompt { get; set; }
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
