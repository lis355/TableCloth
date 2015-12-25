using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace TableClothWork
{
	public partial class MainWindow
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		void MainWindow_OnInitialized( object sender, EventArgs e )
		{
			var model = DataContext as MainWindowViewModel;
			if ( model != null 
				&& model.OnInitializeCommand != null 
				&& model.OnInitializeCommand.CanExecute( null ) )
			{
				model.OnInitializeCommand.Execute( null );
			}
		}

		void MainWindow_OnDataContextChanged( object sender, DependencyPropertyChangedEventArgs e )
		{
			var model = DataContext as MainWindowViewModel;
			if ( model != null )
			{
				model.View = this;

				model.RecentFiles.OnRecentFilesChanged += RecentFiles_OnRecentFilesChanged;
			}
		}

		void RecentFiles_OnRecentFilesChanged( object sender, RecentFilesManager.RecentFiles e )
		{
			RecentFilesMenuItem.IsEnabled = e.RecentFileList.Count > 0;
			if ( !RecentFilesMenuItem.IsEnabled )
				return;

			for ( int i = 0; i < Math.Min( RecentFilesMenuItem.Items.Count, e.RecentFileList.Count ); ++i )
			{
				( ( MenuItem )RecentFilesMenuItem.Items[i] ).Header = String.Format( "{0}. {1}",
					i + 1, e.RecentFileList[i].Path );
			}

			while ( RecentFilesMenuItem.Items.Count > e.RecentFileList.Count )
			{
				RecentFilesMenuItem.Items.RemoveAt( RecentFilesMenuItem.Items.Count - 1 );
			}

			while ( RecentFilesMenuItem.Items.Count < e.RecentFileList.Count )
			{
				var num = RecentFilesMenuItem.Items.Count;
				var item = new MenuItem();
				item.Header = String.Format( "{0}. {1}",
					num + 1, e.RecentFileList[num].Path );
			}
		}

		void MainWindow_OnClosing( object sender, CancelEventArgs e )
		{
			var model = DataContext as MainWindowViewModel;
			if ( model != null 
				&& model.OnClosingCommand != null 
				&& model.OnClosingCommand.CanExecute( null ) )
			{
				model.OnClosingCommand.Execute( null );
			}
		}
	}
}
