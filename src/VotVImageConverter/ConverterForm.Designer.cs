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
			button1 = new Button();
			button2 = new Button();
			textBox1 = new TextBox();
			label1 = new Label();
			label2 = new Label();
			SuspendLayout();
			// 
			// button1
			// 
			button1.Location = new Point(397, 27);
			button1.Name = "button1";
			button1.Size = new Size(75, 23);
			button1.TabIndex = 0;
			button1.Text = "Browse";
			button1.UseVisualStyleBackColor = true;
			// 
			// button2
			// 
			button2.Location = new Point(397, 56);
			button2.Name = "button2";
			button2.Size = new Size(75, 23);
			button2.TabIndex = 1;
			button2.Text = "Search";
			button2.UseVisualStyleBackColor = true;
			// 
			// textBox1
			// 
			textBox1.Location = new Point(12, 27);
			textBox1.Name = "textBox1";
			textBox1.Size = new Size(379, 23);
			textBox1.TabIndex = 2;
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Location = new Point(12, 9);
			label1.Name = "label1";
			label1.Size = new Size(227, 15);
			label1.TabIndex = 3;
			label1.Text = "Select a directory to convert images from.";
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.Location = new Point(12, 60);
			label2.Name = "label2";
			label2.Size = new Size(371, 15);
			label2.TabIndex = 4;
			label2.Text = "Once a directory is chosen, press \"Search\" to find game Assets folder.";
			// 
			// Form1
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(484, 161);
			Controls.Add(label2);
			Controls.Add(label1);
			Controls.Add(textBox1);
			Controls.Add(button2);
			Controls.Add(button1);
			Name = "Form1";
			Text = "Form1";
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private Button button1;
		private Button button2;
		private TextBox textBox1;
		private Label label1;
		private Label label2;
	}
}
