using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Xml.Linq;

namespace CreateAndEditImageApp.Common
{
	/// <summary>
	/// アプリケーションホストサービス
	/// </summary>
	public class AppHostService : IAppHostService
	{
		/// <summary>
		/// アプリケーションのナビゲーションを管理するためのRegionManager
		/// </summary>
		private readonly IRegionManager _regionManager;

		/// <summary>
		/// AppHostServiceのコンストラクタ
		/// </summary>
		/// <param name="regionManager">regionManager</param>
		public AppHostService(IRegionManager regionManager)
		{
			_regionManager = regionManager;
		}

		/// <summary>
		/// マウスを指定されたUI要素にキャプチャします。
		/// </summary>
		/// <param name="element">UIElement</param>
		public void CaptureMouse(UIElement element)
		{
			element?.CaptureMouse();
		}

		/// <summary>
		/// マウスの位置を指定されたUI要素に対して取得します。
		/// </summary>
		/// <param name="relativeTo">UIElement</param>
		/// <returns>マウスの位置</returns>
		public Point GetMousePosition(UIElement relativeTo)
		{
			return Mouse.GetPosition(relativeTo);
		}

		/// <summary>
		/// 指定されたScrollViewerの垂直オフセットを取得します。
		/// </summary>
		/// <param name="scrollViewer">UIElement</param>
		/// <returns>指定されたScrollViewerの垂直オフセット</returns>
		public double GetVerticalOffset(UIElement scrollViewer)
		{
			return (scrollViewer as ScrollViewer)?.VerticalOffset ?? 0;
		}

		/// <summary>
		/// 指定されたリージョンに指定されたビューをナビゲートします。
		/// </summary>
		/// <param name="regionName">regionName</param>
		/// <param name="viewName">viewName</param>
		public void NavigateTo(string regionName, string viewName)
		{
			_regionManager.RequestNavigate(regionName, viewName);
		}

		/// <summary>
		/// マウスキャプチャを解除します。
		/// </summary>
		/// <param name="element">UIElement</param>
		public void ReleaseMouse(UIElement element)
		{
			element?.ReleaseMouseCapture();
		}

		/// <summary>
		/// BitmapImageを指定されたフォルダにファイルとして保存します。
		/// </summary>
		/// <param name="bitmapImage">bitmapImage</param>
		/// <param name="folderPath">フォルダのパス</param>
		/// <param name="fileName">ファイル名</param>
		/// <exception cref="InvalidOperationException">InvalidOperationException</exception>
		public void SaveImageBytesAsFile(BitmapImage bitmapImage, string folderPath, string fileName)
		{
			// Check if the folder exists, if not, create it
			if (!Directory.Exists(folderPath))
			{
				Directory.CreateDirectory(folderPath);
			}

			var filePath = Path.Combine(folderPath, fileName);

			// Check if the file already exists
			BitmapEncoder encoder = Path.GetExtension(filePath).ToLower() switch
			{
				".jpg" or ".jpeg" => new JpegBitmapEncoder(),
				".png" => new PngBitmapEncoder(),
				_ => throw new InvalidOperationException("Unsupported image format.")
			};

			encoder.Frames.Add(BitmapFrame.Create(bitmapImage));

			using FileStream stream = new FileStream(filePath, FileMode.Create);
			encoder.Save(stream);
		}

		/// <summary>
		/// 指定されたScrollViewerをスクロールして、最後の位置に移動します。
		/// </summary>
		/// <param name="scrollViewer">scrollViewer</param>
		public void ScrollToEnd(UIElement scrollViewer)
		{
			if (scrollViewer is ScrollViewer sv)
			{
				sv.ScrollToEnd();
			}
		}

		/// <summary>
		/// 指定されたScrollViewerをスクロールして、指定された垂直オフセットに移動します。
		/// </summary>
		/// <param name="scrollViewer">UIElement</param>
		/// <param name="offset">指定された垂直オフセット</param>
		public void ScrollToVerticalOffset(UIElement scrollViewer, double offset)
		{
			if (scrollViewer is ScrollViewer sv)
			{
				sv.ScrollToVerticalOffset(offset);
			}
		}

		/// <summary>
		/// アプリケーションをシャットダウンします。
		/// </summary>
		public void Shutdown()
		{
			Application.Current.Shutdown();
		}
	}

	/// <summary>
	/// アプリケーションホストサービスのインターフェース
	/// </summary>
	public interface IAppHostService
	{
		/// <summary>
		/// 指定されたリージョンに指定されたビューをナビゲートします。
		/// </summary>
		/// <param name="regionName">regionName</param>
		/// <param name="viewName">viewName</param>
		void NavigateTo(string regionName, string viewName);

		/// <summary>
		/// アプリケーションをシャットダウンします。
		/// </summary>
		void Shutdown();

		/// <summary>
		/// 指定されたScrollViewerをスクロールして、最後の位置に移動します。
		/// </summary>
		/// <param name="scrollViewer">scrollViewer</param>
		void ScrollToEnd(UIElement scrollViewer);

		/// <summary>
		/// BitmapImageを指定されたフォルダにファイルとして保存します。
		/// </summary>
		/// <param name="bitmapImage">bitmapImage</param>
		/// <param name="folderPath">フォルダのパス</param>
		/// <param name="fileName">ファイル名</param>
		/// <exception cref="InvalidOperationException">InvalidOperationException</exception>
		void SaveImageBytesAsFile(BitmapImage bitmapImage, string folderPath, string fileName);

		/// <summary>
		/// 指定されたScrollViewerをスクロールして、指定された垂直オフセットに移動します。
		/// </summary>
		/// <param name="scrollViewer">UIElement</param>
		/// <param name="offset">指定された垂直オフセット</param>
		void ScrollToVerticalOffset(UIElement scrollViewer, double offset);

		/// <summary>
		/// マウスキャプチャを解除します。
		/// </summary>
		/// <param name="element">UIElement</param>
		void ReleaseMouse(UIElement element);

		/// <summary>
		/// マウスを指定されたUI要素にキャプチャします。
		/// </summary>
		/// <param name="element">UIElement</param>
		void CaptureMouse(UIElement element);

		/// <summary>
		/// マウスの位置を指定されたUI要素に対して取得します。
		/// </summary>
		/// <param name="relativeTo">UIElement</param>
		/// <returns>マウスの位置</returns>
		Point GetMousePosition(UIElement relativeTo);

		/// <summary>
		/// 指定されたScrollViewerの垂直オフセットを取得します。
		/// </summary>
		/// <param name="scrollViewer">UIElement</param>
		/// <returns>指定されたScrollViewerの垂直オフセット</returns>
		double GetVerticalOffset(UIElement scrollViewer);
	}
}
