using Moq;
using CreateAndEditImageApp.ViewModels;
using CreateAndEditImageApp.Services;
using CreateAndEditImageApp.Common;
using System.Windows.Media.Imaging;
using System.Windows;

namespace CreateAndEditImageAppTests
{
	/// <summary>
	/// CreateAndEditImageViewModelのテストクラス
	/// </summary>
	public class CreateAndEditImageViewModelTests
	{
		/// <summary>
		/// RegionManagerのモック
		/// </summary>
		private readonly Mock<IRegionManager> _regionManagerMock;

		/// <summary>
		/// AppHostServiceのモック
		/// </summary>
		private readonly Mock<IAppHostService> _appHostServiceMock;

		/// <summary>
		/// OpenaiImageServiceのモック
		/// </summary>
		private readonly Mock<IOpenaiImageService> _openaiServiceMock;

		/// <summary>
		/// CreateAndEditImageViewModelTestsのコンストラクタ
		/// </summary>
		public CreateAndEditImageViewModelTests()
		{
			_regionManagerMock = new Mock<IRegionManager>();
			_appHostServiceMock = new Mock<IAppHostService>();
			_openaiServiceMock = new Mock<IOpenaiImageService>();
		}

		/// <summary>
		/// CreateAndEditImageViewModelのコンストラクタが正しくプロパティとコマンドを初期化することを確認します。
		/// </summary>
		[Fact]
		public void Constructor_InitializesPropertiesAndCommands()
		{
			// Arrange & Act
			var vm = new CreateAndEditImageViewModel(
				_regionManagerMock.Object,
				_appHostServiceMock.Object,
				_openaiServiceMock.Object);

			// Assert
			Assert.NotNull(vm.InputTextGotFocus);
			Assert.NotNull(vm.InputTextLostFocus);
			Assert.NotNull(vm.InputCommand);
			Assert.NotNull(vm.ReturnCommand);
			Assert.NotNull(vm.ChatMessages);
			Assert.True(vm.ChatMessages.Count >= 2); // 預設有測試訊息
		}

		/// <summary>
		/// GotFocusメソッドがプロンプトテキストをクリアすることを確認します。
		/// </summary>
		[Fact]
		public void GotFocus_ClearsPromptText()
		{
			// Arrange
			var vm = new CreateAndEditImageViewModel(
				_regionManagerMock.Object,
				_appHostServiceMock.Object,
				_openaiServiceMock.Object);

			vm.InputText = "Please enter your message here...";

			// Act
			vm.InputTextGotFocus.Execute();

			// Assert
			Assert.Equal(string.Empty, vm.InputText);
			Assert.Equal("Normal", vm.InputTextFontStyle);
			Assert.Equal("Black", vm.InputTextForeground);
		}

		/// <summary>
		/// LostFocusメソッドがプロンプトテキストをデフォルトのスタイルに設定することを確認します。
		/// </summary>
		[Fact]
		public void LostFocus_SetsDefaultInputTextStyle()
		{
			// Arrange
			var vm = new CreateAndEditImageViewModel(
				_regionManagerMock.Object,
				_appHostServiceMock.Object,
				_openaiServiceMock.Object);

			vm.InputText = "";

			// Act
			vm.InputTextLostFocus.Execute();

			// Assert
			Assert.Equal("Please enter your message here...", vm.InputText);
			Assert.Equal("Italic", vm.InputTextFontStyle);
			Assert.Equal("Gray", vm.InputTextForeground);
		}

		/// <summary>
		/// InputCommandが有効な入力を追加し、OpenaiImageServiceのGenerateImageAsyncメソッドを呼び出すことを確認します。
		/// </summary>
		[Fact]
		public async void InputCommandValidInputAddsMessageAndCallsOpenaiServiceAsync()
		{
			// Arrange
			var vm = new CreateAndEditImageViewModel(
			_regionManagerMock.Object,
			_appHostServiceMock.Object,
			_openaiServiceMock.Object);

			vm.InputText = "Hello world";
			var bitmap = new BitmapImage();
			_openaiServiceMock.Setup(s => s.GenerateImageAsync("Hello world"))
			.ReturnsAsync(bitmap);

			// Create a mock UIElement to pass as the parameter
			var mockUIElement = new Mock<UIElement>().Object;

			// Act
			await Task.Run(() => vm.InputCommand.Execute(mockUIElement)); // Pass a valid UIElement object

			// Assert
			Assert.Contains(vm.ChatMessages, m => m.Text == "Hello world");
			_openaiServiceMock.Verify(s => s.GenerateImageAsync("Hello world"), Times.Once);
		}

		/// <summary>
		/// ReturnCommandが実行されると、AppHostServiceのNavigateToメソッドを呼び出してMainViewにナビゲートすることを確認します。
		/// </summary>
		[Fact]
		public void ReturnCommand_NavigatesToMainView()
		{
			// Arrange
			var vm = new CreateAndEditImageViewModel(
				_regionManagerMock.Object,
				_appHostServiceMock.Object,
				_openaiServiceMock.Object);

			// Act
			vm.ReturnCommand.Execute();

			// Assert
			_appHostServiceMock.Verify(s => s.NavigateTo("ContentRegion", "MainView"), Times.Once);
		}

		/// <summary>
		/// ChatMessagesMouseDownがマウスの位置と垂直オフセットを取得し、マウスをキャプチャすることを確認します。
		/// </summary>
		[Fact]
		public void OnChatMessagesMouseDown_UpdatesStateAndCallsService()
		{
			var vm = new CreateAndEditImageViewModel(
				_regionManagerMock.Object,
				_appHostServiceMock.Object,
				_openaiServiceMock.Object);

			var element = new UIElement();
			var expectedPoint = new Point(10, 20);
			var expectedOffset = 123.45;

			_appHostServiceMock.Setup(s => s.GetMousePosition(element)).Returns(expectedPoint);
			_appHostServiceMock.Setup(s => s.GetVerticalOffset(element)).Returns(expectedOffset);

			// Execute
			vm.ChatMessagesMouseDown.Execute(element);

			// Verify
			_appHostServiceMock.Verify(s => s.GetMousePosition(element), Times.Once);
			_appHostServiceMock.Verify(s => s.GetVerticalOffset(element), Times.Once);
			_appHostServiceMock.Verify(s => s.CaptureMouse(element), Times.Once);

			// Verify isMouseDown, mouseStartPoint, startOffset
			var isMouseDownField = typeof(CreateAndEditImageViewModel)
				.GetField("isMouseDown", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
			Assert.NotNull(isMouseDownField); // Ensure the field exists
			var isMouseDown = isMouseDownField?.GetValue(vm) as bool?; // Add null-conditional operator
			Assert.True(isMouseDown.HasValue && isMouseDown.Value);

			var mouseStartPointField = typeof(CreateAndEditImageViewModel)
				.GetField("mouseStartPoint", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
			Assert.NotNull(mouseStartPointField); // Ensure the field exists
			var mouseStartPoint = mouseStartPointField?.GetValue(vm) as Point?; // Add null-conditional operator
			Assert.True(mouseStartPoint.HasValue && mouseStartPoint.Value == expectedPoint);

			var startOffsetField = typeof(CreateAndEditImageViewModel)
				.GetField("startOffset", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
			Assert.NotNull(startOffsetField); // Ensure the field exists
			var startOffset = startOffsetField?.GetValue(vm) as double?; // Add null-conditional operator
			Assert.True(startOffset.HasValue && startOffset.Value == expectedOffset);
		}

		/// <summary>
		/// ChatMessagesMouseUpがマウスを解放し、isMouseDownをfalseに設定することを確認します。
		/// </summary>
		[Fact]
		public void OnChatMessagesMouseUp_UpdatesStateAndCallsService()
		{
			var vm = new CreateAndEditImageViewModel(
				_regionManagerMock.Object,
				_appHostServiceMock.Object,
				_openaiServiceMock.Object);

			var element = new UIElement();

			// 先設為 true
			typeof(CreateAndEditImageViewModel)
				.GetField("isMouseDown", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
				?.SetValue(vm, true); // Add null-conditional operator

			vm.ChatMessagesMouseUp.Execute(element);

			// 驗證
			_appHostServiceMock.Verify(s => s.ReleaseMouse(element), Times.Once);

			var isMouseDown = typeof(CreateAndEditImageViewModel)
				.GetField("isMouseDown", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
				?.GetValue(vm); // Add null-conditional operator
			Assert.False((bool?)isMouseDown ?? false); // Use null-coalescing operator
		}

		/// <summary>
		/// ChatMessagesMouseMoveがマウスが押されている場合、垂直オフセットを更新することを確認します。
		/// </summary>
		[Fact]
		public void OnChatMessagesMouseMove_WhenIsMouseDown_CallsService()
		{
			var vm = new CreateAndEditImageViewModel(
				_regionManagerMock.Object,
				_appHostServiceMock.Object,
				_openaiServiceMock.Object);

			var element = new UIElement();

			// 設定 isMouseDown = true, mouseStartPoint, startOffset
			typeof(CreateAndEditImageViewModel)
				.GetField("isMouseDown", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
				?.SetValue(vm, true);
			typeof(CreateAndEditImageViewModel)
				.GetField("mouseStartPoint", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
				?.SetValue(vm, new Point(0, 100));
			typeof(CreateAndEditImageViewModel)
				.GetField("startOffset", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
				?.SetValue(vm, 50.0);

			_appHostServiceMock.Setup(s => s.GetMousePosition(element)).Returns(new Point(0, 80));

			vm.ChatMessagesMouseMove.Execute(element);

			// delta = 100 - 80 = 20, offset = 50 + 20 = 70
			_appHostServiceMock.Verify(s => s.GetMousePosition(element), Times.Once);
			_appHostServiceMock.Verify(s => s.ScrollToVerticalOffset(element, 70.0), Times.Once);
		}

		/// <summary>
		/// OnScrollViewerLoadedがScrollViewerの垂直オフセットを最後にスクロールすることを確認します。
		/// </summary>
		[Fact]
		public void OnScrollViewerLoaded_CallsScrollToEnd()
		{
			var vm = new CreateAndEditImageViewModel(
				_regionManagerMock.Object,
				_appHostServiceMock.Object,
				_openaiServiceMock.Object);

			var element = new UIElement();

			vm.ScrollViewerLoaded.Execute(element);

			_appHostServiceMock.Verify(s => s.ScrollToEnd(element), Times.Once);
		}
	}
}
