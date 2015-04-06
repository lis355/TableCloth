using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

using TableClothGUI;

namespace TestGUI
{
    public partial class MForm : Form
    {
        MFormController _controller;

        public MForm()
        {
            InitializeComponent();

            _controller = new MFormController( this );
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Form1_Load( object sender, EventArgs e )
        {
            _controller.CreateNewNote();
        }

        private void newToolStripMenuItem_Click( object sender, EventArgs e )
        {
            _controller.CreateNewNote();
        }

        private void saveToolStripMenuItem_Click( object sender, EventArgs e )
        {
            _controller.SaveActiveNote();
        }

        private void openToolStripMenuItem_Click( object sender, EventArgs e )
        {
            _controller.OpenNote();
        }
    }
}
