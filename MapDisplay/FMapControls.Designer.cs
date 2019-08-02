namespace MapDisplay
{
    partial class FMapControls
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
            this.MapHolder = new System.Windows.Forms.Panel();
            this.TokenPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.BNewToken = new System.Windows.Forms.Button();
            this.BLoadMap = new System.Windows.Forms.Button();
            this.BTrash = new System.Windows.Forms.Button();
            this.DLoadMap = new System.Windows.Forms.OpenFileDialog();
            this.DNewToken = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // MapHolder
            // 
            this.MapHolder.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MapHolder.AutoScroll = true;
            this.MapHolder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.MapHolder.Location = new System.Drawing.Point(3, 50);
            this.MapHolder.Name = "MapHolder";
            this.MapHolder.Size = new System.Drawing.Size(629, 395);
            this.MapHolder.TabIndex = 0;
            // 
            // TokenPanel
            // 
            this.TokenPanel.AllowDrop = true;
            this.TokenPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TokenPanel.AutoScroll = true;
            this.TokenPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TokenPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.TokenPanel.Location = new System.Drawing.Point(644, 86);
            this.TokenPanel.Name = "TokenPanel";
            this.TokenPanel.Size = new System.Drawing.Size(153, 358);
            this.TokenPanel.TabIndex = 1;
            this.TokenPanel.DragDrop += new System.Windows.Forms.DragEventHandler(this.TokenPanel_DragDrop);
            this.TokenPanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.TokenPanel_MouseUp);
            // 
            // BNewToken
            // 
            this.BNewToken.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BNewToken.Location = new System.Drawing.Point(642, 50);
            this.BNewToken.Name = "BNewToken";
            this.BNewToken.Size = new System.Drawing.Size(110, 30);
            this.BNewToken.TabIndex = 2;
            this.BNewToken.Text = "New Token";
            this.BNewToken.UseVisualStyleBackColor = true;
            this.BNewToken.Click += new System.EventHandler(this.BNewToken_Click);
            // 
            // BLoadMap
            // 
            this.BLoadMap.Location = new System.Drawing.Point(3, 12);
            this.BLoadMap.Name = "BLoadMap";
            this.BLoadMap.Size = new System.Drawing.Size(111, 30);
            this.BLoadMap.TabIndex = 3;
            this.BLoadMap.Text = "Load Map";
            this.BLoadMap.UseVisualStyleBackColor = true;
            this.BLoadMap.Click += new System.EventHandler(this.BLoadMap_Click);
            // 
            // BTrash
            // 
            this.BTrash.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BTrash.Location = new System.Drawing.Point(760, 50);
            this.BTrash.Name = "BTrash";
            this.BTrash.Size = new System.Drawing.Size(37, 29);
            this.BTrash.TabIndex = 4;
            this.BTrash.UseVisualStyleBackColor = true;
            this.BTrash.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BTrash_MouseUp);
            // 
            // DLoadMap
            // 
            this.DLoadMap.FileOk += new System.ComponentModel.CancelEventHandler(this.DLoadMap_FileOk);
            // 
            // DNewToken
            // 
            this.DNewToken.FileOk += new System.ComponentModel.CancelEventHandler(this.DNewToken_FileOk);
            // 
            // FMapControls
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.BTrash);
            this.Controls.Add(this.BLoadMap);
            this.Controls.Add(this.BNewToken);
            this.Controls.Add(this.TokenPanel);
            this.Controls.Add(this.MapHolder);
            this.Name = "FMapControls";
            this.Text = "FMapControls";
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.FMapControls_MouseUp);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel MapHolder;
        private System.Windows.Forms.FlowLayoutPanel TokenPanel;
        private System.Windows.Forms.Button BNewToken;
        private System.Windows.Forms.Button BLoadMap;
        private System.Windows.Forms.Button BTrash;
        private System.Windows.Forms.OpenFileDialog DLoadMap;
        private System.Windows.Forms.OpenFileDialog DNewToken;
    }
}