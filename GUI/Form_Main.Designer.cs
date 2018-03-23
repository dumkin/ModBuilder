namespace ModBuilder.GUI
{
    partial class Form_Main
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
            this.PictureBox_Main = new System.Windows.Forms.PictureBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.ListBox_Main = new System.Windows.Forms.ListBox();
            this.Label_Type = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox_Main)).BeginInit();
            this.SuspendLayout();
            // 
            // PictureBox_Main
            // 
            this.PictureBox_Main.Location = new System.Drawing.Point(247, 12);
            this.PictureBox_Main.Name = "PictureBox_Main";
            this.PictureBox_Main.Size = new System.Drawing.Size(128, 128);
            this.PictureBox_Main.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PictureBox_Main.TabIndex = 1;
            this.PictureBox_Main.TabStop = false;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(405, 276);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(175, 20);
            this.textBox1.TabIndex = 2;
            this.textBox1.Text = "chest";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(586, 274);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(26, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "S";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(405, 303);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(207, 108);
            this.listBox1.TabIndex = 4;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // ListBox_Main
            // 
            this.ListBox_Main.FormattingEnabled = true;
            this.ListBox_Main.Location = new System.Drawing.Point(12, 12);
            this.ListBox_Main.Name = "ListBox_Main";
            this.ListBox_Main.Size = new System.Drawing.Size(229, 420);
            this.ListBox_Main.Sorted = true;
            this.ListBox_Main.TabIndex = 5;
            this.ListBox_Main.SelectedIndexChanged += new System.EventHandler(this.ListBox_Main_SelectedIndexChanged);
            // 
            // Label_Type
            // 
            this.Label_Type.AutoSize = true;
            this.Label_Type.Location = new System.Drawing.Point(381, 12);
            this.Label_Type.Name = "Label_Type";
            this.Label_Type.Size = new System.Drawing.Size(31, 13);
            this.Label_Type.TabIndex = 6;
            this.Label_Type.Text = "Type";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(537, 417);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 7;
            this.button2.Text = "Add";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Form_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 441);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.Label_Type);
            this.Controls.Add(this.ListBox_Main);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.PictureBox_Main);
            this.Name = "Form_Main";
            this.Text = "Main - ModBuilder";
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox_Main)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox PictureBox_Main;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.ListBox ListBox_Main;
        private System.Windows.Forms.Label Label_Type;
        private System.Windows.Forms.Button button2;
    }
}