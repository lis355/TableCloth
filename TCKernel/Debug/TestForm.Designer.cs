namespace TableClothKernel
{
	partial class TestForm
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
			this.InBox = new System.Windows.Forms.TextBox();
			this.ConstantType = new System.Windows.Forms.ComboBox();
			this.OperatorsType = new System.Windows.Forms.ComboBox();
			this.OutBox = new System.Windows.Forms.TextBox();
			this.TabControls = new System.Windows.Forms.TabControl();
			this.ParsePage = new System.Windows.Forms.TabPage();
			this.DotPage = new System.Windows.Forms.TabPage();
			this.ImgPanel = new System.Windows.Forms.Panel();
			this.DotGraphImage = new System.Windows.Forms.PictureBox();
			this.GenDotCheckBox = new System.Windows.Forms.CheckBox();
			this.VariablesPages = new System.Windows.Forms.TabPage();
			this.VariblesList = new System.Windows.Forms.TextBox();
			this.TabControls.SuspendLayout();
			this.ParsePage.SuspendLayout();
			this.DotPage.SuspendLayout();
			this.ImgPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.DotGraphImage)).BeginInit();
			this.VariablesPages.SuspendLayout();
			this.SuspendLayout();
			// 
			// InBox
			// 
			this.InBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.InBox.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.InBox.Location = new System.Drawing.Point(12, 12);
			this.InBox.Name = "InBox";
			this.InBox.Size = new System.Drawing.Size(518, 26);
			this.InBox.TabIndex = 0;
			this.InBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.InBox_KeyDown);
			this.InBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.InBox_MouseDoubleClick);
			// 
			// ConstantType
			// 
			this.ConstantType.Font = new System.Drawing.Font("Courier New", 12F);
			this.ConstantType.FormattingEnabled = true;
			this.ConstantType.Location = new System.Drawing.Point(12, 44);
			this.ConstantType.Name = "ConstantType";
			this.ConstantType.Size = new System.Drawing.Size(256, 26);
			this.ConstantType.TabIndex = 3;
			// 
			// OperatorsType
			// 
			this.OperatorsType.Font = new System.Drawing.Font("Courier New", 12F);
			this.OperatorsType.FormattingEnabled = true;
			this.OperatorsType.Location = new System.Drawing.Point(274, 44);
			this.OperatorsType.Name = "OperatorsType";
			this.OperatorsType.Size = new System.Drawing.Size(256, 26);
			this.OperatorsType.TabIndex = 4;
			// 
			// OutBox
			// 
			this.OutBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.OutBox.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.OutBox.Location = new System.Drawing.Point(3, 3);
			this.OutBox.Multiline = true;
			this.OutBox.Name = "OutBox";
			this.OutBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.OutBox.Size = new System.Drawing.Size(504, 357);
			this.OutBox.TabIndex = 5;
			// 
			// TabControls
			// 
			this.TabControls.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.TabControls.Controls.Add(this.ParsePage);
			this.TabControls.Controls.Add(this.DotPage);
			this.TabControls.Controls.Add(this.VariablesPages);
			this.TabControls.Location = new System.Drawing.Point(12, 99);
			this.TabControls.Name = "TabControls";
			this.TabControls.SelectedIndex = 0;
			this.TabControls.Size = new System.Drawing.Size(518, 389);
			this.TabControls.TabIndex = 6;
			// 
			// ParsePage
			// 
			this.ParsePage.Controls.Add(this.OutBox);
			this.ParsePage.Location = new System.Drawing.Point(4, 22);
			this.ParsePage.Name = "ParsePage";
			this.ParsePage.Padding = new System.Windows.Forms.Padding(3);
			this.ParsePage.Size = new System.Drawing.Size(510, 363);
			this.ParsePage.TabIndex = 0;
			this.ParsePage.Text = "ParsePage";
			this.ParsePage.UseVisualStyleBackColor = true;
			// 
			// DotPage
			// 
			this.DotPage.Controls.Add(this.ImgPanel);
			this.DotPage.Location = new System.Drawing.Point(4, 22);
			this.DotPage.Name = "DotPage";
			this.DotPage.Padding = new System.Windows.Forms.Padding(3);
			this.DotPage.Size = new System.Drawing.Size(510, 363);
			this.DotPage.TabIndex = 1;
			this.DotPage.Text = "DotPage";
			this.DotPage.UseVisualStyleBackColor = true;
			// 
			// ImgPanel
			// 
			this.ImgPanel.AutoScroll = true;
			this.ImgPanel.Controls.Add(this.DotGraphImage);
			this.ImgPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ImgPanel.Location = new System.Drawing.Point(3, 3);
			this.ImgPanel.Name = "ImgPanel";
			this.ImgPanel.Size = new System.Drawing.Size(504, 357);
			this.ImgPanel.TabIndex = 1;
			// 
			// DotGraphImage
			// 
			this.DotGraphImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.DotGraphImage.Location = new System.Drawing.Point(3, 3);
			this.DotGraphImage.Name = "DotGraphImage";
			this.DotGraphImage.Size = new System.Drawing.Size(544, 467);
			this.DotGraphImage.TabIndex = 0;
			this.DotGraphImage.TabStop = false;
			// 
			// GenDotCheckBox
			// 
			this.GenDotCheckBox.AutoSize = true;
			this.GenDotCheckBox.Location = new System.Drawing.Point(12, 76);
			this.GenDotCheckBox.Name = "GenDotCheckBox";
			this.GenDotCheckBox.Size = new System.Drawing.Size(90, 17);
			this.GenDotCheckBox.TabIndex = 7;
			this.GenDotCheckBox.Text = "Generate Dot";
			this.GenDotCheckBox.UseVisualStyleBackColor = true;
			// 
			// VariablesPages
			// 
			this.VariablesPages.Controls.Add(this.VariblesList);
			this.VariablesPages.Location = new System.Drawing.Point(4, 22);
			this.VariablesPages.Name = "VariablesPages";
			this.VariablesPages.Size = new System.Drawing.Size(510, 363);
			this.VariablesPages.TabIndex = 2;
			this.VariablesPages.Text = "Variables";
			this.VariablesPages.UseVisualStyleBackColor = true;
			// 
			// VariblesList
			// 
			this.VariblesList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.VariblesList.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.VariblesList.Location = new System.Drawing.Point(0, 0);
			this.VariblesList.Multiline = true;
			this.VariblesList.Name = "VariblesList";
			this.VariblesList.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.VariblesList.Size = new System.Drawing.Size(510, 363);
			this.VariblesList.TabIndex = 6;
			// 
			// TestForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(542, 500);
			this.Controls.Add(this.GenDotCheckBox);
			this.Controls.Add(this.TabControls);
			this.Controls.Add(this.OperatorsType);
			this.Controls.Add(this.ConstantType);
			this.Controls.Add(this.InBox);
			this.Name = "TestForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "TestForm";
			this.TabControls.ResumeLayout(false);
			this.ParsePage.ResumeLayout(false);
			this.ParsePage.PerformLayout();
			this.DotPage.ResumeLayout(false);
			this.ImgPanel.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.DotGraphImage)).EndInit();
			this.VariablesPages.ResumeLayout(false);
			this.VariablesPages.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		public System.Windows.Forms.TextBox InBox;
		public System.Windows.Forms.ComboBox ConstantType;
		public System.Windows.Forms.ComboBox OperatorsType;
		public System.Windows.Forms.TextBox OutBox;
		public System.Windows.Forms.TabControl TabControls;
		public System.Windows.Forms.TabPage ParsePage;
		public System.Windows.Forms.TabPage DotPage;
		public System.Windows.Forms.PictureBox DotGraphImage;
		private System.Windows.Forms.Panel ImgPanel;
		public System.Windows.Forms.CheckBox GenDotCheckBox;
		private System.Windows.Forms.TabPage VariablesPages;
		public System.Windows.Forms.TextBox VariblesList;
	}
}