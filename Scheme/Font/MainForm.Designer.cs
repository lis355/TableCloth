namespace Scheme
{
    partial class MainForm
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.SPicture = new System.Windows.Forms.PictureBox();
            this.SPanel = new System.Windows.Forms.Panel();
            this.ToolMenu = new System.Windows.Forms.ToolStrip();
            this.SaveStripButton = new System.Windows.Forms.ToolStripButton();
            this.MinButton = new System.Windows.Forms.ToolStripButton();
            this.MaxButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.Timer = new System.Windows.Forms.Timer(this.components);
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
            this.SPicture.Size = new System.Drawing.Size(115, 111);
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
            this.SaveStripButton,
            this.MinButton,
            this.MaxButton,
            this.toolStripButton2,
            this.toolStripButton3,
            this.toolStripButton4,
            this.toolStripButton1});
            this.ToolMenu.Location = new System.Drawing.Point(0, 0);
            this.ToolMenu.Name = "ToolMenu";
            this.ToolMenu.Size = new System.Drawing.Size(584, 25);
            this.ToolMenu.TabIndex = 1;
            this.ToolMenu.Text = "toolStrip1";
            // 
            // SaveStripButton
            // 
            this.SaveStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.SaveStripButton.Image = ((System.Drawing.Image)(resources.GetObject("SaveStripButton.Image")));
            this.SaveStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SaveStripButton.Name = "SaveStripButton";
            this.SaveStripButton.Size = new System.Drawing.Size(23, 22);
            this.SaveStripButton.Text = "&Сохранить";
            this.SaveStripButton.Click += new System.EventHandler(this.SaveToolStripButton_Click);
            // 
            // MinButton
            // 
            this.MinButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.MinButton.Image = ((System.Drawing.Image)(resources.GetObject("MinButton.Image")));
            this.MinButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.MinButton.Name = "MinButton";
            this.MinButton.Size = new System.Drawing.Size(23, 22);
            this.MinButton.Text = "toolStripButton1";
            this.MinButton.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // MaxButton
            // 
            this.MaxButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.MaxButton.Image = ((System.Drawing.Image)(resources.GetObject("MaxButton.Image")));
            this.MaxButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.MaxButton.Name = "MaxButton";
            this.MaxButton.Size = new System.Drawing.Size(23, 22);
            this.MaxButton.Text = "toolStripButton2";
            this.MaxButton.Click += new System.EventHandler(this.DrawOriginalSheme);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton2.Text = "toolStripButton2";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton3.Image")));
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton3.Text = "toolStripButton3";
            this.toolStripButton3.Click += new System.EventHandler(this.toolStripButton3_Click);
            // 
            // toolStripButton4
            // 
            this.toolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton4.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton4.Image")));
            this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton4.Text = "toolStripButton4";
            this.toolStripButton4.Click += new System.EventHandler(this.toolStripButton4_Click);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.Text = "toolStripButton1";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click_1);
            // 
            // Timer
            // 
            this.Timer.Interval = 500;
            this.Timer.Tick += new System.EventHandler(this.Timer_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(584, 362);
            this.Controls.Add(this.ToolMenu);
            this.Controls.Add(this.SPanel);
            this.MinimumSize = new System.Drawing.Size(600, 400);
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Scheme";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.SPicture)).EndInit();
            this.SPanel.ResumeLayout(false);
            this.ToolMenu.ResumeLayout(false);
            this.ToolMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox SPicture;
        private System.Windows.Forms.Panel SPanel;
        private System.Windows.Forms.ToolStrip ToolMenu;
        private System.Windows.Forms.ToolStripButton SaveStripButton;
        private System.Windows.Forms.ToolStripButton MinButton;
        private System.Windows.Forms.ToolStripButton MaxButton;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.ToolStripButton toolStripButton4;
        private System.Windows.Forms.Timer Timer;
        private System.Windows.Forms.ToolStripButton toolStripButton1;




    }
}

