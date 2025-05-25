using CreateAndEditImageApp.Common;
using System.Windows;

namespace CreateAndEditImageApp.ViewModels
{
	/// <summary>
	/// MainViewのビューモデル
	/// </summary>
	public class MainViewModel : ViewModelBase
	{
		/// <summary>
		/// 画像の作成と編集を行うViewへ遷移するコマンド
		/// </summary>
		public DelegateCommand CreateAndEditImageViewCommand { get; private set; }

		/// <summary>
		/// アプリケーションを終了するコマンド
		/// </summary>
		public DelegateCommand QuitCommand { get; private set; }

		/// <summary>
		/// MainViewModel constructor
		/// </summary>
		/// <param name="regionManager">regionManager</param>
		public MainViewModel(IRegionManager regionManager,IAppHostService navigationService) : base(regionManager,navigationService)
		{
			// Initialize commands
			CreateAndEditImageViewCommand = new DelegateCommand(ExecuteCreateAndEditImageViewCommand);
			QuitCommand = new DelegateCommand(ExecuteQuitCommand);
		}

		/// <summary>
		/// 画像の作成と編集を行うViewへ遷移するコマンドの実行
		/// </summary>
		private void ExecuteCreateAndEditImageViewCommand()
		{
			_appHostService.NavigateTo("ContentRegion", "CreateAndEditImageView");
		}

		/// <summary>
		/// アプリケーションを終了するコマンドの実行
		/// </summary>
		private void ExecuteQuitCommand()
		{
			_appHostService.Shutdown();
		}
	}
}
