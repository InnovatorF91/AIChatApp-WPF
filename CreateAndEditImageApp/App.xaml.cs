using CreateAndEditImageApp.Common;
using CreateAndEditImageApp.Services;
using CreateAndEditImageApp.ViewModels;
using CreateAndEditImageApp.Views;
using System.Windows;

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
			containerRegistry.RegisterSingleton<IOpenaiImageService, OpenaiImageService>();
			containerRegistry.RegisterSingleton<IImageClientWrapper, ImageClientWrapper>();

			// 這裡註冊你的依賴關係
			containerRegistry.RegisterForNavigation<MainView, MainViewModel>();
			containerRegistry.RegisterForNavigation<CreateAndEditImageView, CreateAndEditImageViewModel>();
		}
	}
}
