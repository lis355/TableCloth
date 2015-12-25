namespace TableClothGUI
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Note));
            this.Status = new System.Windows.Forms.StatusStrip();
            this.toolStripMashtabPlus = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripMashtabLess = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripStatusLabelMashtab = new System.Windows.Forms.ToolStripStatusLabel();
            this.NoteMainPanel = new System.Windows.Forms.Panel();
            this.Status.SuspendLayout();
            this.SuspendLayout();
            // 
            // Status
            // 
            this.Status.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMashtabPlus,
            this.toolStripMashtabLess,
            this.toolStripStatusLabelMashtab});
            this.Status.Location = new System.Drawing.Point(0, 340);
            this.Status.Name = "Status";
            this.Status.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Status.Size = new System.Drawing.Size(684, 22);
            this.Status.SizingGrip = false;
            this.Status.TabIndex = 0;
            // 
            // toolStripMashtabPlus
            // 
            this.toolStripMashtabPlus.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripMashtabPlus.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMashtabPlus.Image")));
            this.toolStripMashtabPlus.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripMashtabPlus.Name = "toolStripMashtabPlus";
            this.toolStripMashtabPlus.ShowDropDownArrow = false;
            this.toolStripMashtabPlus.Size = new System.Drawing.Size(20, 20);
            this.toolStripMashtabPlus.Text = "toolStripDropDownButton1";
            this.toolStripMashtabPlus.Click += new System.EventHandler(this.toolStripMashtabPlus_Click);
            // 
            // toolStripMashtabLess
            // 
            this.toolStripMashtabLess.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripMashtabLess.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMashtabLess.Image")));
            this.toolStripMashtabLess.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripMashtabLess.Name = "toolStripMashtabLess";
            this.toolStripMashtabLess.ShowDropDownArrow = false;
            this.toolStripMashtabLess.Size = new System.Drawing.Size(20, 20);
            this.toolStripMashtabLess.Text = "toolStripDropDownButton2";
            this.toolStripMashtabLess.Click += new System.EventHandler(this.toolStripMashtabLess_Click);
            // 
            // toolStripStatusLabelMashtab
            // 
            this.toolStripStatusLabelMashtab.Name = "toolStripStatusLabelMashtab";
            this.toolStripStatusLabelMashtab.Size = new System.Drawing.Size(38, 17);
            this.toolStripStatusLabelMashtab.Text = "MSTB";
            // 
            // NoteMainPanel
            // 
            this.NoteMainPanel.AutoScroll = true;
            this.NoteMainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.NoteMainPanel.Location = new System.Drawing.Point(0, 0);
            this.NoteMainPanel.Margin = new System.Windows.Forms.Padding(0);
            this.NoteMainPanel.Name = "NoteMainPanel";
            this.NoteMainPanel.Size = new System.Drawing.Size(684, 340);
            this.NoteMainPanel.TabIndex = 1;
            this.NoteMainPanel.DoubleClick += new System.EventHandler(this.NoteMainPanel_DoubleClick);
            // 
            // Note
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(684, 362);
            this.Controls.Add(this.NoteMainPanel);
            this.Controls.Add(this.Status);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(600, 300);
            this.Name = "Note";
            this.ShowInTaskbar = false;
            this.Text = "Note";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Note_FormClosed);
            this.Resize += new System.EventHandler(this.Note_Resize);
            this.Status.ResumeLayout(false);
            this.Status.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip Status;
        private System.Windows.Forms.ToolStripDropDownButton toolStripMashtabPlus;
        private System.Windows.Forms.ToolStripDropDownButton toolStripMashtabLess;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelMashtab;
        private System.Windows.Forms.Panel NoteMainPanel;



    }
}