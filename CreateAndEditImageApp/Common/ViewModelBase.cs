namespace CreateAndEditImageApp.Common
{
	/// <summary>
	/// ViewModelの基底クラス
	/// </summary>
	public class ViewModelBase : BindableBase, INavigationAware, IDestructible
	{
		/// <summary>
		/// アプリケーションのナビゲーションを管理するためのRegionManager
		/// </summary>
		protected readonly IRegionManager _regionManager;

		/// <summary>
		/// アプリケーションのホストサービス
		/// </summary>
		protected readonly IAppHostService _appHostService;

		/// <summary>
		/// ViewModelBaseのコンストラクタ
		/// </summary>
		/// <param name="regionManager">regionManager</param>
		/// <param name="appHostService">appHostService</param>
		public ViewModelBase(IRegionManager regionManager, IAppHostService appHostService)
		{
			// 共通初始化
			_regionManager = regionManager;
			_appHostService = appHostService;
		}

		public virtual void OnNavigatedTo(NavigationContext navigationContext)
		{
		}

		public virtual bool IsNavigationTarget(NavigationContext navigationContext)
		{
			return false;
		}

		public virtual void OnNavigatedFrom(NavigationContext navigationContext)
		{
		}

		public virtual void Destroy()
		{
		}
	}
}
