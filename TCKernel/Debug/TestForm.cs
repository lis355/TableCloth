using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TableClothKernel
{
	public partial class TestForm : Form
	{
		TestFormController _controller;

		public TestForm()
		{
			InitializeComponent();

			_controller = new TestFormController( this );
		}

		private void InBox_KeyDown(object sender, KeyEventArgs e)
		{
			if ( e.KeyCode == Keys.Return )
			{
				_controller.InBoxReturnDown();
			}
		}

		private void InBox_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			_controller.InBoxMouseDoubleClick();
		}
	}
}
