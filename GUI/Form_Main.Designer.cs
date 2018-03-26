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
            this.Selected_Image = new System.Windows.Forms.PictureBox();
            this.Search_Edit = new System.Windows.Forms.TextBox();
            this.Search_Find = new System.Windows.Forms.Button();
            this.Search_List = new System.Windows.Forms.ListBox();
            this.Build_List = new System.Windows.Forms.ListBox();
            this.Selected_Type = new System.Windows.Forms.Label();
            this.Search_Add = new System.Windows.Forms.Button();
            this.Download_FolderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.Dependencies_List = new System.Windows.Forms.ListBox();
            this.Search_Group = new System.Windows.Forms.GroupBox();
            this.Selected_Group = new System.Windows.Forms.GroupBox();
            this.Selected_Control_Group = new System.Windows.Forms.GroupBox();
            this.Selected_Control_Delete = new System.Windows.Forms.Button();
            this.Selected_Available_Group = new System.Windows.Forms.GroupBox();
            this.Selected_Available_List = new System.Windows.Forms.ListBox();
            this.Selected_Name = new System.Windows.Forms.Label();
            this.Build_Group = new System.Windows.Forms.GroupBox();
            this.Dependencies_Group = new System.Windows.Forms.GroupBox();
            this.Control_Group = new System.Windows.Forms.GroupBox();
            this.Control_Download = new System.Windows.Forms.Button();
            this.Control_Available = new System.Windows.Forms.ComboBox();
            this.Status = new System.Windows.Forms.StatusStrip();
            this.Status_Label = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.Selected_Image)).BeginInit();
            this.Search_Group.SuspendLayout();
            this.Selected_Group.SuspendLayout();
            this.Selected_Control_Group.SuspendLayout();
            this.Selected_Available_Group.SuspendLayout();
            this.Build_Group.SuspendLayout();
            this.Dependencies_Group.SuspendLayout();
            this.Control_Group.SuspendLayout();
            this.Status.SuspendLayout();
            this.SuspendLayout();
            // 
            // Selected_Image
            // 
            this.Selected_Image.Location = new System.Drawing.Point(6, 19);
            this.Selected_Image.Name = "Selected_Image";
            this.Selected_Image.Size = new System.Drawing.Size(128, 128);
            this.Selected_Image.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.Selected_Image.TabIndex = 1;
            this.Selected_Image.TabStop = false;
            // 
            // Search_Edit
            // 
            this.Search_Edit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Search_Edit.Location = new System.Drawing.Point(6, 18);
            this.Search_Edit.Name = "Search_Edit";
            this.Search_Edit.Size = new System.Drawing.Size(182, 20);
            this.Search_Edit.TabIndex = 2;
            // 
            // Search_Find
            // 
            this.Search_Find.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Search_Find.Location = new System.Drawing.Point(194, 16);
            this.Search_Find.Name = "Search_Find";
            this.Search_Find.Size = new System.Drawing.Size(50, 23);
            this.Search_Find.TabIndex = 3;
            this.Search_Find.Text = "Find";
            this.Search_Find.UseVisualStyleBackColor = true;
            this.Search_Find.Click += new System.EventHandler(this.Search_Find_Click);
            // 
            // Search_List
            // 
            this.Search_List.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Search_List.FormattingEnabled = true;
            this.Search_List.Location = new System.Drawing.Point(6, 45);
            this.Search_List.Name = "Search_List";
            this.Search_List.Size = new System.Drawing.Size(238, 108);
            this.Search_List.Sorted = true;
            this.Search_List.TabIndex = 4;
            this.Search_List.SelectedIndexChanged += new System.EventHandler(this.Search_List_SelectedIndexChanged);
            // 
            // Build_List
            // 
            this.Build_List.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Build_List.FormattingEnabled = true;
            this.Build_List.Location = new System.Drawing.Point(8, 21);
            this.Build_List.Margin = new System.Windows.Forms.Padding(5);
            this.Build_List.Name = "Build_List";
            this.Build_List.Size = new System.Drawing.Size(234, 303);
            this.Build_List.Sorted = true;
            this.Build_List.TabIndex = 5;
            this.Build_List.SelectedIndexChanged += new System.EventHandler(this.Build_List_SelectedIndexChanged);
            // 
            // Selected_Type
            // 
            this.Selected_Type.AutoSize = true;
            this.Selected_Type.Location = new System.Drawing.Point(141, 39);
            this.Selected_Type.Name = "Selected_Type";
            this.Selected_Type.Size = new System.Drawing.Size(31, 13);
            this.Selected_Type.TabIndex = 6;
            this.Selected_Type.Text = "Type";
            // 
            // Search_Add
            // 
            this.Search_Add.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Search_Add.Enabled = false;
            this.Search_Add.Location = new System.Drawing.Point(169, 158);
            this.Search_Add.Name = "Search_Add";
            this.Search_Add.Size = new System.Drawing.Size(75, 23);
            this.Search_Add.TabIndex = 7;
            this.Search_Add.Text = "Add";
            this.Search_Add.UseVisualStyleBackColor = true;
            this.Search_Add.Click += new System.EventHandler(this.Search_Add_Click);
            // 
            // Dependencies_List
            // 
            this.Dependencies_List.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Dependencies_List.FormattingEnabled = true;
            this.Dependencies_List.Location = new System.Drawing.Point(8, 21);
            this.Dependencies_List.Margin = new System.Windows.Forms.Padding(5);
            this.Dependencies_List.Name = "Dependencies_List";
            this.Dependencies_List.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.Dependencies_List.Size = new System.Drawing.Size(234, 160);
            this.Dependencies_List.Sorted = true;
            this.Dependencies_List.TabIndex = 12;
            // 
            // Search_Group
            // 
            this.Search_Group.Controls.Add(this.Search_Add);
            this.Search_Group.Controls.Add(this.Search_Edit);
            this.Search_Group.Controls.Add(this.Search_Find);
            this.Search_Group.Controls.Add(this.Search_List);
            this.Search_Group.Location = new System.Drawing.Point(268, 357);
            this.Search_Group.Name = "Search_Group";
            this.Search_Group.Size = new System.Drawing.Size(250, 192);
            this.Search_Group.TabIndex = 13;
            this.Search_Group.TabStop = false;
            this.Search_Group.Text = "Add extension:";
            // 
            // Selected_Group
            // 
            this.Selected_Group.Controls.Add(this.Selected_Control_Group);
            this.Selected_Group.Controls.Add(this.Selected_Available_Group);
            this.Selected_Group.Controls.Add(this.Selected_Name);
            this.Selected_Group.Controls.Add(this.Selected_Image);
            this.Selected_Group.Controls.Add(this.Selected_Type);
            this.Selected_Group.Location = new System.Drawing.Point(268, 12);
            this.Selected_Group.Name = "Selected_Group";
            this.Selected_Group.Size = new System.Drawing.Size(507, 339);
            this.Selected_Group.TabIndex = 14;
            this.Selected_Group.TabStop = false;
            this.Selected_Group.Text = "Selected extension:";
            // 
            // Selected_Control_Group
            // 
            this.Selected_Control_Group.Controls.Add(this.Selected_Control_Delete);
            this.Selected_Control_Group.Location = new System.Drawing.Point(346, 153);
            this.Selected_Control_Group.Name = "Selected_Control_Group";
            this.Selected_Control_Group.Size = new System.Drawing.Size(155, 180);
            this.Selected_Control_Group.TabIndex = 10;
            this.Selected_Control_Group.TabStop = false;
            this.Selected_Control_Group.Text = "Selected control:";
            // 
            // Selected_Control_Delete
            // 
            this.Selected_Control_Delete.Enabled = false;
            this.Selected_Control_Delete.Location = new System.Drawing.Point(6, 19);
            this.Selected_Control_Delete.Name = "Selected_Control_Delete";
            this.Selected_Control_Delete.Size = new System.Drawing.Size(75, 23);
            this.Selected_Control_Delete.TabIndex = 0;
            this.Selected_Control_Delete.Text = "Delete";
            this.Selected_Control_Delete.UseVisualStyleBackColor = true;
            this.Selected_Control_Delete.Click += new System.EventHandler(this.Selected_Control_Delete_Click);
            // 
            // Selected_Available_Group
            // 
            this.Selected_Available_Group.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.Selected_Available_Group.Controls.Add(this.Selected_Available_List);
            this.Selected_Available_Group.Location = new System.Drawing.Point(6, 153);
            this.Selected_Available_Group.Name = "Selected_Available_Group";
            this.Selected_Available_Group.Size = new System.Drawing.Size(128, 180);
            this.Selected_Available_Group.TabIndex = 9;
            this.Selected_Available_Group.TabStop = false;
            this.Selected_Available_Group.Text = "Available versions:";
            // 
            // Selected_Available_List
            // 
            this.Selected_Available_List.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Selected_Available_List.FormattingEnabled = true;
            this.Selected_Available_List.Location = new System.Drawing.Point(10, 19);
            this.Selected_Available_List.Name = "Selected_Available_List";
            this.Selected_Available_List.Size = new System.Drawing.Size(112, 147);
            this.Selected_Available_List.Sorted = true;
            this.Selected_Available_List.TabIndex = 8;
            // 
            // Selected_Name
            // 
            this.Selected_Name.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Selected_Name.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Selected_Name.Location = new System.Drawing.Point(140, 19);
            this.Selected_Name.Name = "Selected_Name";
            this.Selected_Name.Size = new System.Drawing.Size(361, 20);
            this.Selected_Name.TabIndex = 7;
            this.Selected_Name.Text = "Name";
            // 
            // Build_Group
            // 
            this.Build_Group.Controls.Add(this.Build_List);
            this.Build_Group.Location = new System.Drawing.Point(12, 12);
            this.Build_Group.Name = "Build_Group";
            this.Build_Group.Size = new System.Drawing.Size(250, 339);
            this.Build_Group.TabIndex = 15;
            this.Build_Group.TabStop = false;
            this.Build_Group.Text = "Extension:";
            // 
            // Dependencies_Group
            // 
            this.Dependencies_Group.Controls.Add(this.Dependencies_List);
            this.Dependencies_Group.Location = new System.Drawing.Point(12, 357);
            this.Dependencies_Group.Name = "Dependencies_Group";
            this.Dependencies_Group.Size = new System.Drawing.Size(250, 192);
            this.Dependencies_Group.TabIndex = 16;
            this.Dependencies_Group.TabStop = false;
            this.Dependencies_Group.Text = "Dependencies:";
            // 
            // Control_Group
            // 
            this.Control_Group.Controls.Add(this.Control_Download);
            this.Control_Group.Controls.Add(this.Control_Available);
            this.Control_Group.Location = new System.Drawing.Point(524, 357);
            this.Control_Group.Name = "Control_Group";
            this.Control_Group.Size = new System.Drawing.Size(251, 192);
            this.Control_Group.TabIndex = 17;
            this.Control_Group.TabStop = false;
            this.Control_Group.Text = "Build control:";
            // 
            // Control_Download
            // 
            this.Control_Download.Enabled = false;
            this.Control_Download.Location = new System.Drawing.Point(170, 19);
            this.Control_Download.Name = "Control_Download";
            this.Control_Download.Size = new System.Drawing.Size(75, 23);
            this.Control_Download.TabIndex = 13;
            this.Control_Download.Text = "Download";
            this.Control_Download.UseVisualStyleBackColor = true;
            this.Control_Download.Click += new System.EventHandler(this.Control_Download_Click);
            // 
            // Control_Available
            // 
            this.Control_Available.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Control_Available.FormattingEnabled = true;
            this.Control_Available.Location = new System.Drawing.Point(6, 20);
            this.Control_Available.Name = "Control_Available";
            this.Control_Available.Size = new System.Drawing.Size(158, 21);
            this.Control_Available.TabIndex = 12;
            this.Control_Available.SelectedIndexChanged += new System.EventHandler(this.Control_Available_SelectedIndexChanged);
            // 
            // Status
            // 
            this.Status.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Status_Label});
            this.Status.Location = new System.Drawing.Point(0, 556);
            this.Status.Name = "Status";
            this.Status.Size = new System.Drawing.Size(787, 22);
            this.Status.SizingGrip = false;
            this.Status.TabIndex = 18;
            // 
            // Status_Label
            // 
            this.Status_Label.Name = "Status_Label";
            this.Status_Label.Size = new System.Drawing.Size(72, 17);
            this.Status_Label.Text = "Status_Label";
            // 
            // Form_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(787, 578);
            this.Controls.Add(this.Status);
            this.Controls.Add(this.Control_Group);
            this.Controls.Add(this.Dependencies_Group);
            this.Controls.Add(this.Build_Group);
            this.Controls.Add(this.Selected_Group);
            this.Controls.Add(this.Search_Group);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Form_Main";
            this.Text = "Main - ModBuilder";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_Main_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.Selected_Image)).EndInit();
            this.Search_Group.ResumeLayout(false);
            this.Search_Group.PerformLayout();
            this.Selected_Group.ResumeLayout(false);
            this.Selected_Group.PerformLayout();
            this.Selected_Control_Group.ResumeLayout(false);
            this.Selected_Available_Group.ResumeLayout(false);
            this.Build_Group.ResumeLayout(false);
            this.Dependencies_Group.ResumeLayout(false);
            this.Control_Group.ResumeLayout(false);
            this.Status.ResumeLayout(false);
            this.Status.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox Selected_Image;
        private System.Windows.Forms.TextBox Search_Edit;
        private System.Windows.Forms.Button Search_Find;
        private System.Windows.Forms.ListBox Search_List;
        private System.Windows.Forms.ListBox Build_List;
        private System.Windows.Forms.Label Selected_Type;
        private System.Windows.Forms.Button Search_Add;
        private System.Windows.Forms.FolderBrowserDialog Download_FolderBrowserDialog;
        private System.Windows.Forms.ListBox Dependencies_List;
        private System.Windows.Forms.GroupBox Search_Group;
        private System.Windows.Forms.GroupBox Selected_Group;
        private System.Windows.Forms.Label Selected_Name;
        private System.Windows.Forms.GroupBox Build_Group;
        private System.Windows.Forms.GroupBox Dependencies_Group;
        private System.Windows.Forms.GroupBox Control_Group;
        private System.Windows.Forms.ListBox Selected_Available_List;
        private System.Windows.Forms.GroupBox Selected_Available_Group;
        private System.Windows.Forms.GroupBox Selected_Control_Group;
        private System.Windows.Forms.Button Selected_Control_Delete;
        private System.Windows.Forms.ComboBox Control_Available;
        private System.Windows.Forms.StatusStrip Status;
        private System.Windows.Forms.ToolStripStatusLabel Status_Label;
        private System.Windows.Forms.Button Control_Download;
    }
}