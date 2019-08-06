namespace MapDisplay
{
    partial class FMirrorWindow
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
            this.MirrorFrame = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // MirrorFrame
            // 
            this.MirrorFrame.AutoScroll = true;
            this.MirrorFrame.AutoScrollMinSize = new System.Drawing.Size(400, 400);
            this.MirrorFrame.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MirrorFrame.Location = new System.Drawing.Point(0, 0);
            this.MirrorFrame.Name = "MirrorFrame";
            this.MirrorFrame.Size = new System.Drawing.Size(800, 450);
            this.MirrorFrame.TabIndex = 0;
            this.MirrorFrame.Scroll += new System.Windows.Forms.ScrollEventHandler(this.MirrorFrame_Scroll);
            // 
            // FMirrorWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.MirrorFrame);
            this.Name = "FMirrorWindow";
            this.Text = "Map Mirror";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FMirrorWindow_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel MirrorFrame;
    }
}