using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

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
	}
}
