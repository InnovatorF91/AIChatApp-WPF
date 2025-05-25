using CreateAndEditImageApp.Common;
using CreateAndEditImageApp.ViewModels;
using Moq;

namespace CreateAndEditImageAppTests
{
	/// <summary>
	/// MainViewModelのテストクラス
	/// </summary>
	public class MainViewModelTests
	{
		/// <summary>
		/// CreateAndEditImageViewCommandのExecuteメソッドがIAppHostServiceのNavigateToメソッドを呼び出すことを確認するテスト
		/// </summary>
		[Fact]
		public void CreateAndEditImageViewCommand_Execute_CallsRequestNavigate()
		{
			// Arrange
			var regionManagerMock = new Mock<IRegionManager>();
			var navigationServiceMock = new Mock<IAppHostService>();
			var vm = new MainViewModel(regionManagerMock.Object, navigationServiceMock.Object);

			// Act
			vm.CreateAndEditImageViewCommand.Execute();

			// Assert
			navigationServiceMock.Verify(
				m => m.NavigateTo("ContentRegion", "CreateAndEditImageView"),
				Times.Once);
		}

		/// <summary>
		/// QuitCommandのExecuteメソッドがIAppHostServiceのShutdownメソッドを呼び出すことを確認するテスト
		/// </summary>
		[Fact]
		public void QuitCommand_Execute_CallsApplicationShutdown()
		{
			// Arrange
			var regionManagerMock = new Mock<IRegionManager>();
			var navigationServiceMock = new Mock<IAppHostService>();
			var vm = new MainViewModel(regionManagerMock.Object, navigationServiceMock.Object);

			// Application.Current.Shutdown() をテストで呼ばないようにするため、Application をモック化するか、リフレクション等で差し替える必要があります。
			// ここでは単純にコマンドが呼べることのみを確認します。

			// Act & Assert
			// 実際のアプリ終了はテストしません
			vm.QuitCommand.Execute();

			navigationServiceMock.Verify(
				m => m.Shutdown(),
				Times.Once);
		}
	}
}