using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BinaryCalc
{
    public partial class Options : Form
    {
        public Options()
        {
            InitializeComponent();

            OpenNewWhenStart.CheckState = (Properties.Settings.Default.OpenNewNote == true) ?
                CheckState.Checked : CheckState.Unchecked;
            OpenNewWhenStart.CheckStateChanged += new System.EventHandler(this.OpenNewWhenStart_CheckStateChanged);

            SaveWindowsLocation.CheckState =  (Properties.Settings.Default.SaveWindowsLocation == true) ?
                CheckState.Checked : CheckState.Unchecked;
            SaveWindowsLocation.CheckedChanged += new System.EventHandler(this.SaveWindowsLocation_CheckedChanged);

            OpenLastSave.CheckState = (Properties.Settings.Default.OpenLastSavedNote == true) ?
                CheckState.Checked : CheckState.Unchecked;
            OpenLastSave.CheckedChanged += new System.EventHandler(this.OpenLastSave_CheckedChanged);
        }

        #region Изменения чекбоксов
        private void OpenNewWhenStart_CheckStateChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.OpenNewNote = (OpenNewWhenStart.CheckState == CheckState.Checked) ?
                true : false;
            Properties.Settings.Default.Save();
            //MessageBox.Show(Convert.ToString(Properties.Settings.Default.OpenNewNote));
        }

        private void SaveWindowsLocation_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.SaveWindowsLocation =
                (SaveWindowsLocation.CheckState == CheckState.Checked) ? true : false;
            Properties.Settings.Default.Save();
        }

        private void OpenLastSave_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.OpenLastSavedNote = 
                (OpenLastSave.CheckState == CheckState.Checked) ? true : false;
            Properties.Settings.Default.Save();
        }
        #endregion
    }
}
