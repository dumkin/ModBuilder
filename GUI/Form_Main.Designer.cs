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
            this.components = new System.ComponentModel.Container();
            this.ListView_Main = new System.Windows.Forms.ListView();
            this.ImageList_Main = new System.Windows.Forms.ImageList(this.components);
            this.ContextMenuStrip_Main = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ContextMenuStrip_Main.SuspendLayout();
            this.SuspendLayout();
            // 
            // ListView_Main
            // 
            this.ListView_Main.LargeImageList = this.ImageList_Main;
            this.ListView_Main.Location = new System.Drawing.Point(12, 12);
            this.ListView_Main.Name = "ListView_Main";
            this.ListView_Main.Size = new System.Drawing.Size(294, 417);
            this.ListView_Main.SmallImageList = this.ImageList_Main;
            this.ListView_Main.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.ListView_Main.StateImageList = this.ImageList_Main;
            this.ListView_Main.TabIndex = 0;
            this.ListView_Main.UseCompatibleStateImageBehavior = false;
            this.ListView_Main.View = System.Windows.Forms.View.List;
            this.ListView_Main.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ListView_Main_MouseClick);
            // 
            // ImageList_Main
            // 
            this.ImageList_Main.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.ImageList_Main.ImageSize = new System.Drawing.Size(64, 64);
            this.ImageList_Main.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // ContextMenuStrip_Main
            // 
            this.ContextMenuStrip_Main.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteToolStripMenuItem});
            this.ContextMenuStrip_Main.Name = "ContextMenuStrip_Main";
            this.ContextMenuStrip_Main.Size = new System.Drawing.Size(181, 48);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            // 
            // Form_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 441);
            this.Controls.Add(this.ListView_Main);
            this.Name = "Form_Main";
            this.Text = "Main - ModBuilder";
            this.ContextMenuStrip_Main.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView ListView_Main;
        private System.Windows.Forms.ImageList ImageList_Main;
        private System.Windows.Forms.ContextMenuStrip ContextMenuStrip_Main;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
    }
}