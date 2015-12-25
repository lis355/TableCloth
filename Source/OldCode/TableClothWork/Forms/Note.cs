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
    public partial class Note : Form
    {
        static int GlobalNoteCounter = 0;

        public BCalcProgram BinarySpace;

        MainForm MF;
        VariablesForms VF;

        public Note(ref VariablesForms A, string NoteName, int w, int h, bool show, MainForm M)
        {
            InitializeComponent();
            MF = M;
            MdiParent = M;
            Location = new Point(15 * GlobalNoteCounter, 35 * GlobalNoteCounter);
            if (NoteName != "") Text = NoteName; 
            else Text = "NewNote"+Convert.ToString(++GlobalNoteCounter);
            if (w != 0 && h != 0) Size = new System.Drawing.Size(w, h);

            BinarySpace = new BCalcProgram(ref MainTablePanel, ref A, Text);

            VF = A; VF.AddNewVarSpace(Text, ref BinarySpace.FVariables);

            // делаем фокус ввода на первое пространство ввода
            SelectedInBox.Ref = BinarySpace.FSpaceList[0].InBox; SelectedInBox.Ref.TabIndex = 0;

            // показываем форму
            if (show) Show();
        }

        private void Panel_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            BinarySpace.FSpaceList.AddBack();
        }

        private void Note_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult v = MessageBox.Show("Сохранить изменения в "+Text+"?", this.Text,
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (v == DialogResult.Cancel)
            {
                e.Cancel = true;
                return;
            }
            else if (v == DialogResult.Yes)
            {
                MF.SaveToolStripButton_Click(MF.saveToolStripMenuItem, null);
                VF.DeleteVarSpace(Text);
                MF.NoteCollection.Remove(this);
            }
            else if (v == DialogResult.No)
            {
                VF.DeleteVarSpace(Text);
                MF.NoteCollection.Remove(this);
            }
        }

        private void Note_Activated(object sender, EventArgs e)
        {
            SelectedNote.Ref = this;
        }
    }

    public static class SelectedNote
    {
        public static Note Ref;
    }
}
