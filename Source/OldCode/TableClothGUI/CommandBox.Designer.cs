namespace TableClothGUI
{
    partial class CommandBox
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

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Обязательный метод для поддержки конструктора - не изменяйте 
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // CommandBox
            // 
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.DoubleBuffered = true;
            this.Name = "CommandBox";
            this.Size = new System.Drawing.Size(100, 30);
            this.Enter += new System.EventHandler(this.CommandBox_Enter);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CommandBox_KeyDown);
            this.Leave += new System.EventHandler(this.CommandBox_Leave);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CommandBox_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.CommandBox_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.CommandBox_MouseUp);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
