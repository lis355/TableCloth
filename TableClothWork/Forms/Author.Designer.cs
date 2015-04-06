namespace BinaryCalc
{
    partial class Author
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
            this.PictureBox = new System.Windows.Forms.PictureBox();
            this.AText = new System.Windows.Forms.Label();
            this.AName = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // PictureBox
            // 
            this.PictureBox.Image = global::BinaryCalc.Properties.Resources.author;
            this.PictureBox.InitialImage = null;
            this.PictureBox.Location = new System.Drawing.Point(12, 12);
            this.PictureBox.Name = "PictureBox";
            this.PictureBox.Size = new System.Drawing.Size(228, 390);
            this.PictureBox.TabIndex = 0;
            this.PictureBox.TabStop = false;
            // 
            // AText
            // 
            this.AText.AutoSize = true;
            this.AText.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.AText.Location = new System.Drawing.Point(9, 438);
            this.AText.Name = "AText";
            this.AText.Size = new System.Drawing.Size(40, 18);
            this.AText.TabIndex = 1;
            this.AText.Text = "ABCD";
            // 
            // AName
            // 
            this.AName.AutoSize = true;
            this.AName.Font = new System.Drawing.Font("Consolas", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.AName.Location = new System.Drawing.Point(8, 410);
            this.AName.Name = "AName";
            this.AName.Size = new System.Drawing.Size(64, 28);
            this.AName.TabIndex = 2;
            this.AName.Text = "ABCD";
            // 
            // Author
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(253, 532);
            this.Controls.Add(this.AName);
            this.Controls.Add(this.AText);
            this.Controls.Add(this.PictureBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Author";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "О программе";
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox PictureBox;
        public System.Windows.Forms.Label AText;
        public System.Windows.Forms.Label AName;
    }
}