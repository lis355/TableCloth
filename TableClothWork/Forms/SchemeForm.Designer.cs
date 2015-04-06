namespace BinaryCalc
{
    partial class SchemeForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SchemeForm));
            this.SPicture = new System.Windows.Forms.PictureBox();
            this.SPanel = new System.Windows.Forms.Panel();
            this.ToolMenu = new System.Windows.Forms.ToolStrip();
            this.SaveButton = new System.Windows.Forms.ToolStripButton();
            this.MinButton = new System.Windows.Forms.ToolStripButton();
            this.MaxButton = new System.Windows.Forms.ToolStripButton();
            this.FVal = new System.Windows.Forms.ToolStripLabel();
            ((System.ComponentModel.ISupportInitialize)(this.SPicture)).BeginInit();
            this.SPanel.SuspendLayout();
            this.ToolMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // SPicture
            // 
            this.SPicture.Location = new System.Drawing.Point(0, 0);
            this.SPicture.Margin = new System.Windows.Forms.Padding(0);
            this.SPicture.Name = "SPicture";
            this.SPicture.Size = new System.Drawing.Size(100, 100);
            this.SPicture.TabIndex = 0;
            this.SPicture.TabStop = false;
            this.SPicture.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            this.SPicture.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            this.SPicture.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
            // 
            // SPanel
            // 
            this.SPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.SPanel.AutoScroll = true;
            this.SPanel.Controls.Add(this.SPicture);
            this.SPanel.Location = new System.Drawing.Point(0, 25);
            this.SPanel.Name = "SPanel";
            this.SPanel.Size = new System.Drawing.Size(584, 338);
            this.SPanel.TabIndex = 1;
            // 
            // ToolMenu
            // 
            this.ToolMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SaveButton,
            this.MinButton,
            this.MaxButton,
            this.FVal});
            this.ToolMenu.Location = new System.Drawing.Point(0, 0);
            this.ToolMenu.Name = "ToolMenu";
            this.ToolMenu.Size = new System.Drawing.Size(584, 25);
            this.ToolMenu.TabIndex = 1;
            this.ToolMenu.Text = "toolStrip1";
            // 
            // SaveButton
            // 
            this.SaveButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.SaveButton.Image = ((System.Drawing.Image)(resources.GetObject("SaveButton.Image")));
            this.SaveButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(23, 22);
            this.SaveButton.Text = "&Сохранить";
            this.SaveButton.Click += new System.EventHandler(this.SaveToolStripButton_Click);
            // 
            // MinButton
            // 
            this.MinButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.MinButton.Image = ((System.Drawing.Image)(resources.GetObject("MinButton.Image")));
            this.MinButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.MinButton.Name = "MinButton";
            this.MinButton.Size = new System.Drawing.Size(23, 22);
            this.MinButton.Text = "Превью";
            this.MinButton.Click += new System.EventHandler(this.Preview_Click);
            // 
            // MaxButton
            // 
            this.MaxButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.MaxButton.Image = ((System.Drawing.Image)(resources.GetObject("MaxButton.Image")));
            this.MaxButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.MaxButton.Name = "MaxButton";
            this.MaxButton.Size = new System.Drawing.Size(23, 22);
            this.MaxButton.Text = "Исходный размер";
            this.MaxButton.Click += new System.EventHandler(this.NormView_Click);
            // 
            // FVal
            // 
            this.FVal.Font = new System.Drawing.Font("Courier New", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FVal.Name = "FVal";
            this.FVal.Size = new System.Drawing.Size(0, 22);
            // 
            // SchemeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(584, 362);
            this.Controls.Add(this.ToolMenu);
            this.Controls.Add(this.SPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(600, 400);
            this.Name = "SchemeForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Схема";
            ((System.ComponentModel.ISupportInitialize)(this.SPicture)).EndInit();
            this.SPanel.ResumeLayout(false);
            this.ToolMenu.ResumeLayout(false);
            this.ToolMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        private System.Windows.Forms.PictureBox SPicture;
        private System.Windows.Forms.Panel SPanel;
        private System.Windows.Forms.ToolStrip ToolMenu;
        private System.Windows.Forms.ToolStripButton MinButton;
        private System.Windows.Forms.ToolStripButton MaxButton;
        private System.Windows.Forms.ToolStripButton SaveButton;


        #endregion

        private System.Windows.Forms.ToolStripLabel FVal;
    }
}