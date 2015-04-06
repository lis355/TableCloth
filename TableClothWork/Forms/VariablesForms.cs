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
    public partial class VariablesForms : Form
    {
        MainForm MForm;
        
        /// <summary>
        /// Содержит указатели на списки переменных, чтобы удалять нужную переменную
        /// </summary>
        List<Variables> V;

        public VariablesForms(MainForm ApplicationForm)
        {
            InitializeComponent();
            MForm = ApplicationForm;
            FormulaText.Font = new Font(MathFont.CourierMathFont.Families[0], 11.75F);
            V = new List<Variables>();
        }

        /// <summary>
        /// Добавление уровня документа
        /// </summary>
        public void AddNewVarSpace(string S, ref Variables VarRef)
        {
            TreeView.Nodes.Add(S);
            V.Add(VarRef);
        }

        /// <summary>
        /// Изменяет имя уровня переменных документа
        /// </summary>
        public void SetVarSpace(string S, Variables VarRef)
        {
            foreach (TreeNode x in TreeView.Nodes)
            {
                if (x.Text == VarRef.VariablesSpaceName)
                {
                    x.Text = S;
                }
            } 
        }

        /// <summary>
        /// Удаление уровня документа
        /// </summary>
        public void DeleteVarSpace(string S)
        {
            foreach (TreeNode x in TreeView.Nodes)
            {
                if (x.Parent == null && x.Text == S)
                {
                    x.Remove();
                    return;
                }
            }
        }

        /// <summary>
        /// Добавление переменной
        /// </summary>
        public void SetVarFromNote(string S, string Name, string Tip)
        {
            // получаем ссылку на узел "переменные данного документа"
            foreach (TreeNode x in TreeView.Nodes)
            {
                if (x.Text == S)
                {
                    // добавляем или изменяем
                    foreach (TreeNode xc in x.Nodes)
                    {
                        if (xc.Text == Name)
                        {
                            // уже существует, нужно только обновить tip
                            xc.ToolTipText = Tip;
                            //обновляем если переменную изменили и она была выделена
                            if (TreeView.SelectedNode != null)
                                FormulaText.Text = TreeView.SelectedNode.ToolTipText;
                            return;
                        }
                    }
                    // не нашли, значит нужно добавить
                    x.Nodes.Add(Name);
                    x.Nodes[x.Nodes.Count - 1].ToolTipText = Tip;
                }
            }
        }

        void AddTextToSelectedInBox(string s)
        {
            int t = SelectedInBox.Ref.SelectionStart;
            SelectedInBox.Ref.Text = SelectedInBox.Ref.Text.Insert(t, s);
            SelectedInBox.Ref.SelectionStart = t + s.Length;
        }

        private void FuncTree_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (TreeView.SelectedNode != null)
            {
                if (TreeView.SelectedNode.Parent != null)
                {
                    AddTextToSelectedInBox(TreeView.SelectedNode.Text);
                }
            }
        }

        private void TreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (TreeView.SelectedNode != null)
            {
                FormulaText.Text = TreeView.SelectedNode.ToolTipText;
                DeleteVar.Visible = (FormulaText.Text != "") ? true : false;
            }
        }

        /// <summary>
        /// Удаление переменной
        /// </summary>
        private void DeleteVar_Click(object sender, EventArgs e)
        {
            foreach (Variables x in V)
            {
                if (x.VariablesSpaceName == TreeView.SelectedNode.Parent.Text)
                {
                    x.Delete(TreeView.SelectedNode.Text);
                    TreeView.SelectedNode.Remove();
                    return;
                }
            }

        }

        #region Fading
        private void DeleteVarMouseEnter(object sender, EventArgs e)
        {
            DeleteVar.Image = BinaryCalc.Properties.Resources.delete1;
        }

        private void DeleteVarMouseHover(object sender, EventArgs e)
        {
            DeleteVar.Image = BinaryCalc.Properties.Resources.delete2;
        }

        private void DeleteVarMouseLeave(object sender, EventArgs e)
        {
            DeleteVar.Image = BinaryCalc.Properties.Resources.all0;
        }
        #endregion

        bool OnFormFocus = false;
        private void VariableFormClosing(object sender, FormClosingEventArgs e)
        {
            if (OnFormFocus)
            {
                MForm.varsToolStripMenuItem.Checked = false;
                Hide();
                e.Cancel = true;
            }
        }

        private void VariablesForms_Activated(object sender, EventArgs e)
        {
            OnFormFocus = true;
        }

        private void VariablesForms_Deactivate(object sender, EventArgs e)
        {
            OnFormFocus = false;
        }
    }
}
