using CreateAndEditImageApp.Common;
using CreateAndEditImageApp.Models;
using CreateAndEditImageApp.Services;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CreateAndEditImageApp.ViewModels
{
	/// <summary>
	/// 画像の作成と編集を行うViewModel
	/// </summary>
	public class CreateAndEditImageViewModel : ViewModelBase
	{
		/// <summary>
		/// プロンプトテキスト
		/// </summary>
		private readonly string promptText = "Please enter your message here...";

		/// <summary>
		/// 待機中のテキスト
		/// </summary>
		private readonly string waitingText = "Please wait..."; // Waiting text

		/// <summary>
		/// 保存する画像のフォルダパス
		/// </summary>
		private readonly string saveImageFolderPath = "./SaveImages/"; // Save image folder path

		/// <summary>
		/// 保存する画像の名前
		/// </summary>
		private string imageName = $"image_{DateTime.Now:yyyyMMdd_HHmmss}.png";

		/// <summary>
		/// OpenAIサービス
		/// </summary>
		private readonly IOpenaiImageService _openaiService;

		/// <summary>
		/// マウスダウンフラグ
		/// </summary>
		private bool isMouseDown = false;

		/// <summary>
		/// マウスの開始位置
		/// </summary>
		private Point mouseStartPoint;

		/// <summary>
		/// スクロール開始位置
		/// </summary>
		private double startOffset;

		/// <summary>
		/// 入力されたテキスト
		/// </summary>
		private string inputText = string.Empty; // Initialize to avoid nullability warning

		/// <summary>
		/// 入力されたテキストのフォントスタイル
		/// </summary>
		private string inputTextFontStyle = "Normal"; // Initialize to avoid nullability warning

		/// <summary>
		/// 入力されたテキストのフォント色
		/// </summary>
		private string inputTextForeground = "Black"; // Initialize to avoid nullability warning

		/// <summary>
		/// 入力テキストボックスの有効状態
		/// </summary>
		private bool isInputTextEnable;

		/// <summary>
		/// 入力テキストボックスの無効状態
		/// </summary>
		private bool isInputTextDisable;

		/// <summary>
		/// 入力ボタンの有効状態
		/// </summary>
		private bool isInputBtnEnable;

		/// <summary>
		/// 入力ボタンの無効状態
		/// </summary>
		private bool isInputBtnDisable;

		/// <summary>
		/// 戻るボタンの有効状態
		/// </summary>
		private bool isReturnBtnEnable;

		/// <summary>
		/// 戻るボタンの無効状態
		/// </summary>
		private bool isReturnBtnDisable;

		/// <summary>
		/// 入力されたテキスト
		/// </summary>
		public string InputText
		{
			get { return inputText; }
			set { SetProperty(ref inputText, value); }
		}

		/// <summary>
		/// 入力されたテキストのフォントスタイル
		/// </summary>
		public string InputTextFontStyle
		{
			get { return inputTextFontStyle; }
			set { SetProperty(ref inputTextFontStyle, value); }
		}

		/// <summary>
		/// 入力されたテキストのフォント色
		/// </summary>
		public string InputTextForeground
		{
			get { return inputTextForeground; }
			set { SetProperty(ref inputTextForeground, value); }
		}

		/// <summary>
		/// 入力テキストボックスの有効状態
		/// </summary>
		public bool IsInputTextEnable
		{
			get { return isInputTextEnable; }
			set { SetProperty(ref isInputTextEnable, value); }
		}

		/// <summary>
		/// 入力テキストボックスの無効状態
		/// </summary>
		public bool IsInputTextDisable
		{
			get { return isInputTextDisable; }
			set { SetProperty(ref isInputTextDisable, value); }
		}

		/// <summary>
		/// 入力ボタンの有効状態
		/// </summary>
		public bool IsInputBtnEnable
		{
			get { return isInputBtnEnable; }
			set { SetProperty(ref isInputBtnEnable, value); }
		}

		/// <summary>
		/// 入力ボタンの無効状態
		/// </summary>
		public bool IsInputBtnDisable
		{
			get { return isInputBtnDisable; }
			set { SetProperty(ref isInputBtnDisable, value); }
		}

		/// <summary>
		/// 戻るボタンの有効状態
		/// </summary>
		public bool IsReturnBtnEnable
		{
			get { return isReturnBtnEnable; }
			set { SetProperty(ref isReturnBtnEnable, value); }
		}

		/// <summary>
		/// 戻るボタンの無効状態
		/// </summary>
		public bool IsReturnBtnDisable
		{
			get { return isReturnBtnDisable; }
			set { SetProperty(ref isReturnBtnDisable, value); }
		}

		/// <summary>
		/// 入力テキストボックスにフォーカスが当たったときのコマンド
		/// </summary>
		public DelegateCommand InputTextGotFocus { get; private set; }

		/// <summary>
		/// 入力テキストボックスからフォーカスが外れたときのコマンド
		/// </summary>
		public DelegateCommand InputTextLostFocus { get; private set; }

		/// <summary>
		/// 入力コマンド
		/// </summary>
		public DelegateCommand<UIElement> InputCommand { get; private set; }

		/// <summary>
		/// 戻るコマンド
		/// </summary>
		public DelegateCommand ReturnCommand { get; private set; }

		/// <summary>
		/// チャットメッセージのマウスダウンコマンド
		/// </summary>
		public DelegateCommand<UIElement> ChatMessagesMouseDown { get; private set; }

		/// <summary>
		/// チャットメッセージのマウスアップコマンド
		/// </summary>
		public DelegateCommand<UIElement> ChatMessagesMouseUp { get; private set; }

		/// <summary>
		/// チャットメッセージのマウスムーブコマンド
		/// </summary>
		public DelegateCommand<UIElement> ChatMessagesMouseMove { get; private set; }

		/// <summary>
		/// スクロールビューアのロードコマンド
		/// </summary>
		public DelegateCommand<UIElement> ScrollViewerLoaded { get; private set; }

		/// <summary>
		/// チャットメッセージのコレクション
		/// </summary>
		public ObservableCollection<ChatMessageModel> ChatMessages { get; private set; }

		/// <summary>
		/// CreateAndEditImageViewModel constructor
		/// </summary>
		/// <param name="regionManager">regionManager</param>
		public CreateAndEditImageViewModel(IRegionManager regionManager,IAppHostService appHostService ,IOpenaiImageService openaiService) : base(regionManager, appHostService)
		{
			_openaiService = openaiService;

			// Initialize commands
			InputTextGotFocus = new DelegateCommand(GotFocus);

			InputTextLostFocus = new DelegateCommand(LostFocus);

			InputCommand = new DelegateCommand<UIElement>(OnInputAsync);

			ReturnCommand = new DelegateCommand(OnReturn);

			ChatMessages = new ObservableCollection<ChatMessageModel>();

			ChatMessagesMouseDown = new DelegateCommand<UIElement>(OnChatMessagesMouseDown);

			ChatMessagesMouseUp = new DelegateCommand<UIElement>(OnChatMessagesMouseUp);

			ChatMessagesMouseMove = new DelegateCommand<UIElement>(OnChatMessagesMouseMove);

			ScrollViewerLoaded = new DelegateCommand<UIElement>(OnScrollViewerLoaded);

			// Initialize the default input text style
			Initalize();
		}

		/// <summary>
		/// 初期化処理
		/// </summary>
		private void Initalize()
		{
			SetDefaultInputTextStyle();

			// test
			ChatMessages.Add(new ChatMessageModel(_regionManager, base._appHostService) { Text = "Hello, this is a test message.", PopColor = Brushes.LightBlue, TextAlignment = TextAlignment.Left, TextForeground = Brushes.White, HasText = true, HasImage = false });
			// Add a test message


			// Replace the problematic line with the following:
			//test image path
			var imagePath = System.IO.Path.Combine(
	@"F:\CodingProjects\C#Projects\OpenAI-Create&Edit_Image\CreateAndEditImageApp\CreateAndEditImageApp\Images",
	"Test.jpg");

			ChatMessages.Add(new ChatMessageModel(_regionManager, base._appHostService)
			{
				Text = "Hello,this is a test image.",
				Image = new BitmapImage(new Uri(imagePath, UriKind.Absolute)),
				PopColor = Brushes.LightBlue,
				TextAlignment = TextAlignment.Left,
				TextForeground = Brushes.White,
				HasText = true,
				HasImage = true
			});
			// Add a test image message
			// Add prompt text as a message
		}

		/// <summary>
		/// 入力テキストボックスにフォーカスが当たったときの処理
		/// </summary>
		private void GotFocus()
		{
			if (InputText.Equals(promptText))
			{
				InputText = string.Empty; // Clear the input text when focused
				InputTextFontStyle = "Normal"; // Set font style to Normal
				InputTextForeground = "Black"; // Set font color to Black
			}
		}

		/// <summary>
		/// 入力テキストボックスからフォーカスが外れたときの処理
		/// </summary>
		private void LostFocus()
		{
			SetDefaultInputTextStyle();
		}

		/// <summary>
		/// 入力コマンドの実行
		/// </summary>
		/// <param name="element">UIElement</param>
		private async void OnInputAsync(UIElement element)
		{
			if (string.IsNullOrWhiteSpace(InputText) ||
				string.IsNullOrEmpty(InputText) ||
				InputText.Equals(promptText))
			{
				return;
			}

			ChatMessages.Add(new ChatMessageModel(_regionManager, base._appHostService)
			{
				Text = InputText,
			    PopColor = Brushes.LightGreen, 
				TextAlignment = TextAlignment.Right, 
				TextForeground = Brushes.Black,
				HasText = true,
				HasImage = false }); // Add the input text as a message

			var inputText = InputText; // Store the input text

			// Set the waiting text style while generating the image
			SetWaitingTextStyle();

			try
			{
				var bitmap = await _openaiService.GenerateImageAsync(inputText);

				if (bitmap != null)
				{
					ChatMessages.Add(new ChatMessageModel(_regionManager, base._appHostService)
					{
						Image = bitmap,
						PopColor = Brushes.LightBlue,
						TextAlignment = TextAlignment.Left,
						TextForeground = Brushes.White,
						HasText = false,
						HasImage = true
					});

					_appHostService.SaveImageBytesAsFile(bitmap, saveImageFolderPath, imageName); // Save the generated image to a file
				}
			}
			catch (Exception ex)
			{
				// 建議加上日誌或提示用戶
				MessageBox.Show($"画像生成中にエラーが発生しました: {ex.Message}");
			}

			InputText = string.Empty; // Clear the input text after adding the message
			SetDefaultInputTextStyle(); // Reset the input text style

			if (element != null)
			{
				_appHostService.ScrollToEnd(element); // Scroll to the end of the ScrollViewer
			}
		}

		/// <summary>
		/// 戻るコマンドの実行
		/// </summary>
		private void OnReturn()
		{
			// Implement the logic for the ReturnCommand here
			_appHostService.NavigateTo("ContentRegion", "MainView");
		}

		/// <summary>
		/// デフォルトの入力テキストスタイルを設定する
		/// </summary>
		private void SetDefaultInputTextStyle()
		{
			// Set the default input text style
			CanInputOrClick(true);

			if (string.IsNullOrWhiteSpace(InputText) || string.IsNullOrEmpty(InputText))
			{
				InputText = promptText; // Set the prompt text if input is empty

				InputTextFontStyle = "Italic"; // Set font style to Italic

				InputTextForeground = "Gray"; // Set font color to Gray
			}
		}

		/// <summary>
		/// 待機中のテキストスタイルを設定する
		/// </summary>
		private void SetWaitingTextStyle()
		{
			CanInputOrClick(false); // Disable input and click actions

			// Set the waiting text style
			InputText = waitingText; // Set the waiting text
			InputTextFontStyle = "Italic"; // Set font style to Italic
			InputTextForeground = "Gray"; // Set font color to Gray
		}

		/// <summary>
		/// 入力とクリックを有効または無効にする
		/// </summary>
		/// <param name="canInputOrClick">インプットキーがクリックできるか</param>
		private void CanInputOrClick(bool canInputOrClick)
		{
			IsInputBtnDisable = !canInputOrClick; // Disable the input button
			IsInputTextDisable = !canInputOrClick; // Disable the input text box
			IsReturnBtnDisable = !canInputOrClick; // Disable the return button

			IsInputTextEnable = canInputOrClick; // Enable the input text box
			IsInputBtnEnable = canInputOrClick; // Enable the input button
			IsReturnBtnEnable = canInputOrClick; // Enable the return button
		}

		/// <summary>
		/// チャットメッセージのマウスダウンイベント
		/// </summary>
		/// <param name="element">UIElement</param>
		private void OnChatMessagesMouseMove(UIElement element)
		{
			if (isMouseDown && element != null)
			{
				Point current = _appHostService.GetMousePosition(element);
				double delta = mouseStartPoint.Y - current.Y;
				_appHostService.ScrollToVerticalOffset(element, startOffset + delta);
			}
		}

		/// <summary>
		/// チャットメッセージのマウスアップイベント
		/// </summary>
		/// <param name="element">UIElement</param>
		private void OnChatMessagesMouseUp(UIElement element)
		{
			if (element != null)
			{
				isMouseDown = false;
				_appHostService.ReleaseMouse(element);
			}
		}

		/// <summary>
		/// チャットメッセージのマウスダウンイベント
		/// </summary>
		/// <param name="element">UIElement</param>
		private void OnChatMessagesMouseDown(UIElement element)
		{
			if (element != null)
			{
				isMouseDown = true;
				mouseStartPoint = _appHostService.GetMousePosition(element);
				startOffset = _appHostService.GetVerticalOffset(element);
				_appHostService.CaptureMouse(element);
			}
		}

		/// <summary>
		/// スクロールビューアのロードイベント
		/// </summary>
		/// <param name="element">UIElement</param>
		private void OnScrollViewerLoaded(UIElement element)
		{
			if (element != null)
			{
				// Scroll to the end of the ScrollViewer when it is loaded
				_appHostService.ScrollToEnd(element);
			}
		}
	}
}
