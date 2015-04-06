namespace BinaryCalc
{
    partial class Note
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

        #region 
        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Note));
            this.Panel = new System.Windows.Forms.Panel();
            this.SpecialPanel = new System.Windows.Forms.Panel();
            this.MainTablePanel = new System.Windows.Forms.TableLayoutPanel();
            this.Panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // Panel
            // 
            this.Panel.AutoScroll = true;
            this.Panel.AutoScrollMargin = new System.Drawing.Size(20, 20);
            this.Panel.AutoSize = true;
            this.Panel.BackColor = System.Drawing.Color.White;
            this.Panel.Controls.Add(this.SpecialPanel);
            this.Panel.Controls.Add(this.MainTablePanel);
            this.Panel.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Panel.Location = new System.Drawing.Point(0, 0);
            this.Panel.Name = "Panel";
            this.Panel.Size = new System.Drawing.Size(584, 362);
            this.Panel.TabIndex = 4;
            this.Panel.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Panel_MouseDoubleClick);
            // 
            // SpecialPanel
            // 
            this.SpecialPanel.BackColor = System.Drawing.Color.White;
            this.SpecialPanel.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.SpecialPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.SpecialPanel.Location = new System.Drawing.Point(0, 0);
            this.SpecialPanel.Name = "SpecialPanel";
            this.SpecialPanel.Size = new System.Drawing.Size(584, 120);
            this.SpecialPanel.TabIndex = 19;
            this.SpecialPanel.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Panel_MouseDoubleClick);
            // 
            // MainTablePanel
            // 
            this.MainTablePanel.AutoSize = true;
            this.MainTablePanel.ColumnCount = 1;
            this.MainTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.MainTablePanel.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.MainTablePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.MainTablePanel.Location = new System.Drawing.Point(0, 0);
            this.MainTablePanel.Name = "MainTablePanel";
            this.MainTablePanel.RowCount = 1;
            this.MainTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.MainTablePanel.Size = new System.Drawing.Size(584, 0);
            this.MainTablePanel.TabIndex = 15;
            // 
            // Note
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 362);
            this.Controls.Add(this.Panel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(600, 400);
            this.Name = "Note";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Activated += new System.EventHandler(this.Note_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Note_FormClosing);
            this.Panel.ResumeLayout(false);
            this.Panel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.Panel Panel;
        private System.Windows.Forms.TableLayoutPanel MainTablePanel;
        private System.Windows.Forms.Panel SpecialPanel;
    }
}