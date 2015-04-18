using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TableClothWork
{
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			var assemblyName = Assembly.GetExecutingAssembly().GetName();
			Title = String.Format( "{0} {1}.{2}",
				assemblyName.Name,
				assemblyName.Version.Major,
				assemblyName.Version.MajorRevision );
		}

		void Control_OnMouseDoubleClick( object sender, MouseButtonEventArgs e )
		{
			StackInput.Children.Add( new UserInput() );
		}
	}
}
