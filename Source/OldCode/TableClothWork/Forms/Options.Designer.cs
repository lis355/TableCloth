namespace BinaryCalc
{
    partial class Options
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
            this.User = new System.Windows.Forms.GroupBox();
            this.OpenLastSave = new System.Windows.Forms.CheckBox();
            this.SaveWindowsLocation = new System.Windows.Forms.CheckBox();
            this.OpenNewWhenStart = new System.Windows.Forms.CheckBox();
            this.User.SuspendLayout();
            this.SuspendLayout();
            // 
            // User
            // 
            this.User.Controls.Add(this.OpenLastSave);
            this.User.Controls.Add(this.SaveWindowsLocation);
            this.User.Controls.Add(this.OpenNewWhenStart);
            this.User.Location = new System.Drawing.Point(12, 12);
            this.User.Name = "User";
            this.User.Size = new System.Drawing.Size(333, 111);
            this.User.TabIndex = 0;
            this.User.TabStop = false;
            this.User.Text = "Пользовательские настройки";
            // 
            // OpenLastSave
            // 
            this.OpenLastSave.AutoSize = true;
            this.OpenLastSave.Location = new System.Drawing.Point(25, 77);
            this.OpenLastSave.Name = "OpenLastSave";
            this.OpenLastSave.Size = new System.Drawing.Size(260, 17);
            this.OpenLastSave.TabIndex = 2;
            this.OpenLastSave.Text = "Открывать последний сохраненный документ";
            this.OpenLastSave.UseVisualStyleBackColor = true;
            // 
            // SaveWindowsLocation
            // 
            this.SaveWindowsLocation.AutoSize = true;
            this.SaveWindowsLocation.Location = new System.Drawing.Point(25, 54);
            this.SaveWindowsLocation.Name = "SaveWindowsLocation";
            this.SaveWindowsLocation.Size = new System.Drawing.Size(183, 17);
            this.SaveWindowsLocation.TabIndex = 1;
            this.SaveWindowsLocation.Text = "Сохранять расположение окон";
            this.SaveWindowsLocation.UseVisualStyleBackColor = true;
            // 
            // OpenNewWhenStart
            // 
            this.OpenNewWhenStart.AutoSize = true;
            this.OpenNewWhenStart.Location = new System.Drawing.Point(25, 31);
            this.OpenNewWhenStart.Name = "OpenNewWhenStart";
            this.OpenNewWhenStart.Size = new System.Drawing.Size(288, 17);
            this.OpenNewWhenStart.TabIndex = 0;
            this.OpenNewWhenStart.Text = "Открывать новый документ при старте программы";
            this.OpenNewWhenStart.UseVisualStyleBackColor = true;
            // 
            // Options
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(359, 132);
            this.Controls.Add(this.User);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Options";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Настройки";
            this.User.ResumeLayout(false);
            this.User.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox User;
        private System.Windows.Forms.CheckBox OpenNewWhenStart;
        private System.Windows.Forms.CheckBox OpenLastSave;
        private System.Windows.Forms.CheckBox SaveWindowsLocation;
    }
}