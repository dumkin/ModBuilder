namespace ModBuilder
{
    partial class Form_Project
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
            this.ListBox_Projects = new System.Windows.Forms.ListBox();
            this.Button_Delete = new System.Windows.Forms.Button();
            this.Button_Load = new System.Windows.Forms.Button();
            this.Button_New = new System.Windows.Forms.Button();
            this.SaveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.SuspendLayout();
            // 
            // ListBox_Projects
            // 
            this.ListBox_Projects.FormattingEnabled = true;
            this.ListBox_Projects.Location = new System.Drawing.Point(12, 12);
            this.ListBox_Projects.Name = "ListBox_Projects";
            this.ListBox_Projects.Size = new System.Drawing.Size(360, 277);
            this.ListBox_Projects.TabIndex = 0;
            this.ListBox_Projects.SelectedIndexChanged += new System.EventHandler(this.ListBox_Projects_SelectedIndexChanged);
            // 
            // Button_Delete
            // 
            this.Button_Delete.Enabled = false;
            this.Button_Delete.Location = new System.Drawing.Point(12, 310);
            this.Button_Delete.Name = "Button_Delete";
            this.Button_Delete.Size = new System.Drawing.Size(112, 39);
            this.Button_Delete.TabIndex = 3;
            this.Button_Delete.Text = "Delete";
            this.Button_Delete.UseVisualStyleBackColor = true;
            this.Button_Delete.Click += new System.EventHandler(this.Button_Delete_Click);
            // 
            // Button_Load
            // 
            this.Button_Load.Enabled = false;
            this.Button_Load.Location = new System.Drawing.Point(130, 310);
            this.Button_Load.Name = "Button_Load";
            this.Button_Load.Size = new System.Drawing.Size(124, 39);
            this.Button_Load.TabIndex = 4;
            this.Button_Load.Text = "Load";
            this.Button_Load.UseVisualStyleBackColor = true;
            this.Button_Load.Click += new System.EventHandler(this.Button_Load_Click);
            // 
            // Button_New
            // 
            this.Button_New.Location = new System.Drawing.Point(260, 310);
            this.Button_New.Name = "Button_New";
            this.Button_New.Size = new System.Drawing.Size(112, 39);
            this.Button_New.TabIndex = 5;
            this.Button_New.Text = "New";
            this.Button_New.UseVisualStyleBackColor = true;
            this.Button_New.Click += new System.EventHandler(this.Button_New_Click);
            // 
            // Form_Project
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 361);
            this.Controls.Add(this.Button_New);
            this.Controls.Add(this.Button_Load);
            this.Controls.Add(this.Button_Delete);
            this.Controls.Add(this.ListBox_Projects);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Form_Project";
            this.Text = "Projects - ModBuilder";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox ListBox_Projects;
        private System.Windows.Forms.Button Button_Delete;
        private System.Windows.Forms.Button Button_Load;
        private System.Windows.Forms.Button Button_New;
        private System.Windows.Forms.SaveFileDialog SaveFileDialog;
    }
}