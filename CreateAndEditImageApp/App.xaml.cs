using CreateAndEditImageApp.Common;
using CreateAndEditImageApp.Services;
using CreateAndEditImageApp.ViewModels;
using CreateAndEditImageApp.Views;
using System.Windows;
using System.Windows.Controls;

namespace CreateAndEditImageApp
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : PrismApplication
	{
		protected override Window CreateShell()
		{
			return Container.Resolve<MainWindow>();
		}

		protected override void OnInitialized()
		{
			base.OnInitialized();

			var regionManager = Container.Resolve<IRegionManager>();
			regionManager.RequestNavigate("ContentRegion", "MainView");
		}

		protected override void RegisterTypes(IContainerRegistry containerRegistry)
		{
			containerRegistry.RegisterSingleton<IAppHostService, AppHostService>();

			// 這裡註冊你的依賴關係
			containerRegistry.RegisterForNavigation<MainView, MainViewModel>();
			containerRegistry.RegisterForNavigation<CreateAndEditImageView, CreateAndEditImageViewModel>();

			// 註冊服務
			containerRegistry.RegisterInstance<IOpenaiImageService>(new OpenaiImageService());
		}
	}
}
