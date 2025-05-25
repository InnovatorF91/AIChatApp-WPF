using CreateAndEditImageApp.Common;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;

namespace CreateAndEditImageApp.Models
{
	/// <summary>
	/// チャットメッセージのモデル
	/// </summary>
	/// <remarks>
	/// チャットメッセージのコンストラクタ
	/// </remarks>
	/// <param name="regionManager">regionManager</param>
	public class ChatMessageModel(IRegionManager regionManager, IAppHostService navigationService) : ViewModelBase(regionManager, navigationService)
	{
		/// <summary>
		/// チャットメッセージのテキスト
		/// </summary>
		private string? text;

		/// <summary>
		/// チャットメッセージの画像
		/// </summary>
		private ImageSource? image;

		/// <summary>
		/// チャットメッセージのポップアップ色
		/// </summary>
		private Brush? popColor; // Initialize to avoid nullability warning

		/// <summary>
		/// チャットメッセージのテキストの配置
		/// </summary>
		private TextAlignment textAlignment; // Initialize to avoid nullability warning

		/// <summary>
		/// チャットメッセージのテキストのフォント色
		/// </summary>
		private Brush? textForeground; // Initialize to avoid nullability warning

		/// <summary>
		/// チャットメッセージのテキストがあるかどうか
		/// </summary>
		private bool hasText;

		/// <summary>
		/// チャットメッセージの画像があるかどうか
		/// </summary>
		private bool hasImage;

		/// <summary>
		/// チャットメッセージのテキスト
		/// </summary>
		public string Text
		{
			get { return text ?? string.Empty; } // Ensure a non-null value is returned
			set { SetProperty(ref text, value); }
		}

		/// <summary>
		/// チャットメッセージの画像
		/// </summary>
		public ImageSource Image
		{
			get { return image ?? new BitmapImage(); } // Ensure a non-null value is returned
			set { SetProperty(ref image, value); }
		}

		/// <summary>
		/// チャットメッセージのポップアップ色
		/// </summary>
		public Brush PopColor
		{
			get { return popColor ?? Brushes.Transparent; } // Ensure a non-null value is returned
			set { SetProperty(ref popColor, value); }
		}

		/// <summary>
		/// チャットメッセージのテキストの配置
		/// </summary>
		public TextAlignment TextAlignment
		{
			get { return textAlignment; }
			set { SetProperty(ref textAlignment, value); }
		}

		/// <summary>
		/// チャットメッセージのテキストのフォント色
		/// </summary>
		public Brush TextForeground
		{
			get { return textForeground ?? Brushes.Black; } // Ensure a non-null value is returned
			set { SetProperty(ref textForeground, value); }
		}

		/// <summary>
		/// チャットメッセージのテキストがあるかどうか
		/// </summary>
		public bool HasText
		{
			get { return hasText; }
			set { SetProperty(ref hasText, value); }
		}

		/// <summary>
		/// チャットメッセージの画像があるかどうか
		/// </summary>
		public bool HasImage
		{
			get { return hasImage; }
			set { SetProperty(ref hasImage, value); }
		}
	}

}
