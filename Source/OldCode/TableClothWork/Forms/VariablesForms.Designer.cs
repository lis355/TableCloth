namespace BinaryCalc
{
    partial class VariablesForms
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VariablesForms));
            this.TreeView = new System.Windows.Forms.TreeView();
            this.FormulaText = new System.Windows.Forms.Label();
            this.LayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.DeleteVar = new System.Windows.Forms.PictureBox();
            this.LayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DeleteVar)).BeginInit();
            this.SuspendLayout();
            // 
            // TreeView
            // 
            this.TreeView.BackColor = System.Drawing.Color.White;
            this.LayoutPanel.SetColumnSpan(this.TreeView, 2);
            this.TreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TreeView.Location = new System.Drawing.Point(3, 21);
            this.TreeView.Name = "TreeView";
            this.TreeView.ShowNodeToolTips = true;
            this.TreeView.Size = new System.Drawing.Size(328, 138);
            this.TreeView.TabIndex = 0;
            this.TreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeView_AfterSelect);
            this.TreeView.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.FuncTree_NodeMouseDoubleClick);
            // 
            // FormulaText
            // 
            this.FormulaText.AutoSize = true;
            this.FormulaText.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormulaText.Location = new System.Drawing.Point(18, 0);
            this.FormulaText.Name = "FormulaText";
            this.FormulaText.Size = new System.Drawing.Size(0, 18);
            this.FormulaText.TabIndex = 1;
            // 
            // LayoutPanel
            // 
            this.LayoutPanel.AutoSize = true;
            this.LayoutPanel.BackColor = System.Drawing.Color.White;
            this.LayoutPanel.ColumnCount = 2;
            this.LayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.LayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.LayoutPanel.Controls.Add(this.DeleteVar, 0, 0);
            this.LayoutPanel.Controls.Add(this.TreeView, 1, 1);
            this.LayoutPanel.Controls.Add(this.FormulaText, 1, 0);
            this.LayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.LayoutPanel.Name = "LayoutPanel";
            this.LayoutPanel.RowCount = 2;
            this.LayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.LayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.LayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.LayoutPanel.Size = new System.Drawing.Size(234, 162);
            this.LayoutPanel.TabIndex = 2;
            // 
            // DeleteVar
            // 
            this.DeleteVar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.DeleteVar.Image = global::BinaryCalc.Properties.Resources.all0;
            this.DeleteVar.Location = new System.Drawing.Point(3, 3);
            this.DeleteVar.Margin = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.DeleteVar.Name = "DeleteVar";
            this.DeleteVar.Size = new System.Drawing.Size(12, 12);
            this.DeleteVar.TabIndex = 2;
            this.DeleteVar.TabStop = false;
            this.DeleteVar.Visible = false;
            this.DeleteVar.Click += new System.EventHandler(this.DeleteVar_Click);
            this.DeleteVar.MouseEnter += new System.EventHandler(this.DeleteVarMouseEnter);
            this.DeleteVar.MouseLeave += new System.EventHandler(this.DeleteVarMouseLeave);
            this.DeleteVar.MouseHover += new System.EventHandler(this.DeleteVarMouseHover);
            // 
            // VariablesForms
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(234, 162);
            this.Controls.Add(this.LayoutPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(250, 200);
            this.Name = "VariablesForms";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Переменные";
            this.Activated += new System.EventHandler(this.VariablesForms_Activated);
            this.Deactivate += new System.EventHandler(this.VariablesForms_Deactivate);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.VariableFormClosing);
            this.LayoutPanel.ResumeLayout(false);
            this.LayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DeleteVar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView TreeView;
        private System.Windows.Forms.Label FormulaText;
        private System.Windows.Forms.TableLayoutPanel LayoutPanel;
        private System.Windows.Forms.PictureBox DeleteVar;
    }
}