using CreateAndEditImageApp.Common;
using CreateAndEditImageApp.Repositories;
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
			// 註冊服務
			containerRegistry.RegisterSingleton<IAppHostService, AppHostService>();
			containerRegistry.RegisterSingleton<IOpenaiImageRepository, OpenaiImageRepository>();

			// 這裡註冊你的依賴關係
			containerRegistry.RegisterForNavigation<MainView, MainViewModel>();
			containerRegistry.RegisterForNavigation<CreateAndEditImageView, CreateAndEditImageViewModel>();
		}
	}
}
