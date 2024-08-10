namespace VotVImageConverter
{
	partial class ConverterForm
	{
		/// <summary>
		///  Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		///  Clean up any resources being used.
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
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			btnSelectDir = new Button();
			btnConvert = new Button();
			txtSelectedDir = new TextBox();
			labelInfo = new Label();
			SuspendLayout();
			// 
			// btnSelectDir
			// 
			btnSelectDir.Location = new Point(397, 12);
			btnSelectDir.Name = "btnSelectDir";
			btnSelectDir.Size = new Size(75, 23);
			btnSelectDir.TabIndex = 0;
			btnSelectDir.Text = "Browse";
			btnSelectDir.UseVisualStyleBackColor = true;
			btnSelectDir.Click += BtnSelectDir_Click;
			// 
			// btnConvert
			// 
			btnConvert.Location = new Point(397, 41);
			btnConvert.Name = "btnConvert";
			btnConvert.Size = new Size(75, 23);
			btnConvert.TabIndex = 1;
			btnConvert.Text = "Convert";
			btnConvert.UseVisualStyleBackColor = true;
			btnConvert.Click += BtnConvert_Click;
			// 
			// txtSelectedDir
			// 
			txtSelectedDir.ForeColor = Color.Black;
			txtSelectedDir.Location = new Point(12, 12);
			txtSelectedDir.Name = "txtSelectedDir";
			txtSelectedDir.PlaceholderText = "Select a directory to convert images from.";
			txtSelectedDir.Size = new Size(379, 23);
			txtSelectedDir.TabIndex = 2;
			// 
			// labelInfo
			// 
			labelInfo.AutoSize = true;
			labelInfo.Location = new Point(12, 45);
			labelInfo.Name = "labelInfo";
			labelInfo.Size = new Size(277, 15);
			labelInfo.TabIndex = 4;
			labelInfo.Text = "Once a directory is chosen, press \"Convert\" to start.";
			// 
			// ConverterForm
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(484, 71);
			Controls.Add(labelInfo);
			Controls.Add(txtSelectedDir);
			Controls.Add(btnConvert);
			Controls.Add(btnSelectDir);
			Name = "ConverterForm";
			Text = "VotV Image Converter";
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private Button btnSelectDir;
		private Button btnConvert;
		private TextBox txtSelectedDir;
		private Label labelInfo;
	}
}
