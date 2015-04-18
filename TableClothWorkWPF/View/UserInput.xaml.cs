using System;
using System.Collections.Generic;
using System.Linq;
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
	public partial class UserInput : UserControl
	{
		// temp
		readonly NoteController _controller = new NoteController();

		public UserInput()
		{
			InitializeComponent();

			Input.Text = String.Empty;
			Output.Text = String.Empty;
		}

		private void Input_KeyDown(object sender, KeyEventArgs e)
		{
			if ( e.Key != Key.Enter )
				return;

			Output.Text = _controller.Calc( Input.Text );
		}
	}
}
