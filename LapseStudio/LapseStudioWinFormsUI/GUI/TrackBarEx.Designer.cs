namespace LapseStudioWinFormsUI
{
    partial class TrackBarEx
    {
        /// <summary> 
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.ScrollPanel = new System.Windows.Forms.Panel();
            this.MainButton = new System.Windows.Forms.Button();
            this.MainLayout = new System.Windows.Forms.TableLayoutPanel();
            this.BGPanel = new System.Windows.Forms.Panel();
            this.PanLeftTop = new System.Windows.Forms.Panel();
            this.PanTop = new System.Windows.Forms.Panel();
            this.PanLeft = new System.Windows.Forms.Panel();
            this.PanLeftBottom = new System.Windows.Forms.Panel();
            this.PanBottom = new System.Windows.Forms.Panel();
            this.PanRightBottom = new System.Windows.Forms.Panel();
            this.PanRight = new System.Windows.Forms.Panel();
            this.PanRightTop = new System.Windows.Forms.Panel();
            this.MainLayout.SuspendLayout();
            this.BGPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // ScrollPanel
            // 
            this.ScrollPanel.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.ScrollPanel.Location = new System.Drawing.Point(2, 11);
            this.ScrollPanel.Margin = new System.Windows.Forms.Padding(6, 11, 6, 11);
            this.ScrollPanel.Name = "ScrollPanel";
            this.ScrollPanel.Size = new System.Drawing.Size(100, 2);
            this.ScrollPanel.TabIndex = 1;
            // 
            // MainButton
            // 
            this.MainButton.BackColor = System.Drawing.Color.Transparent;
            this.MainButton.Location = new System.Drawing.Point(2, 2);
            this.MainButton.Name = "MainButton";
            this.MainButton.Size = new System.Drawing.Size(12, 20);
            this.MainButton.TabIndex = 0;
            this.MainButton.TabStop = false;
            this.MainButton.UseVisualStyleBackColor = false;
            this.MainButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MainButton_MouseDown);
            this.MainButton.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainButton_MouseMove);
            this.MainButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MainButton_MouseUp);
            // 
            // MainLayout
            // 
            this.MainLayout.ColumnCount = 3;
            this.MainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.MainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.MainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.MainLayout.Controls.Add(this.BGPanel, 1, 1);
            this.MainLayout.Controls.Add(this.PanLeftTop, 0, 0);
            this.MainLayout.Controls.Add(this.PanTop, 1, 0);
            this.MainLayout.Controls.Add(this.PanLeft, 0, 1);
            this.MainLayout.Controls.Add(this.PanLeftBottom, 0, 2);
            this.MainLayout.Controls.Add(this.PanBottom, 1, 2);
            this.MainLayout.Controls.Add(this.PanRightBottom, 2, 2);
            this.MainLayout.Controls.Add(this.PanRight, 2, 1);
            this.MainLayout.Controls.Add(this.PanRightTop, 2, 0);
            this.MainLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainLayout.Location = new System.Drawing.Point(0, 0);
            this.MainLayout.Margin = new System.Windows.Forms.Padding(0);
            this.MainLayout.Name = "MainLayout";
            this.MainLayout.RowCount = 3;
            this.MainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.MainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.MainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.MainLayout.Size = new System.Drawing.Size(116, 34);
            this.MainLayout.TabIndex = 2;
            // 
            // BGPanel
            // 
            this.BGPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BGPanel.AutoSize = true;
            this.BGPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BGPanel.Controls.Add(this.MainButton);
            this.BGPanel.Controls.Add(this.ScrollPanel);
            this.BGPanel.Location = new System.Drawing.Point(3, 3);
            this.BGPanel.MinimumSize = new System.Drawing.Size(112, 24);
            this.BGPanel.Name = "BGPanel";
            this.BGPanel.Size = new System.Drawing.Size(112, 25);
            this.BGPanel.TabIndex = 0;
            // 
            // PanLeftTop
            // 
            this.PanLeftTop.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PanLeftTop.AutoSize = true;
            this.PanLeftTop.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.PanLeftTop.Location = new System.Drawing.Point(0, 0);
            this.PanLeftTop.Margin = new System.Windows.Forms.Padding(0);
            this.PanLeftTop.Name = "PanLeftTop";
            this.PanLeftTop.Size = new System.Drawing.Size(1, 1);
            this.PanLeftTop.TabIndex = 1;
            // 
            // PanTop
            // 
            this.PanTop.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PanTop.AutoSize = true;
            this.PanTop.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.PanTop.Location = new System.Drawing.Point(0, 0);
            this.PanTop.Margin = new System.Windows.Forms.Padding(0);
            this.PanTop.Name = "PanTop";
            this.PanTop.Size = new System.Drawing.Size(118, 1);
            this.PanTop.TabIndex = 1;
            // 
            // PanLeft
            // 
            this.PanLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanLeft.Location = new System.Drawing.Point(0, 0);
            this.PanLeft.Margin = new System.Windows.Forms.Padding(0);
            this.PanLeft.Name = "PanLeft";
            this.PanLeft.Size = new System.Drawing.Size(1, 31);
            this.PanLeft.TabIndex = 2;
            // 
            // PanLeftBottom
            // 
            this.PanLeftBottom.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PanLeftBottom.AutoSize = true;
            this.PanLeftBottom.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.PanLeftBottom.Location = new System.Drawing.Point(0, 31);
            this.PanLeftBottom.Margin = new System.Windows.Forms.Padding(0);
            this.PanLeftBottom.Name = "PanLeftBottom";
            this.PanLeftBottom.Size = new System.Drawing.Size(1, 3);
            this.PanLeftBottom.TabIndex = 2;
            // 
            // PanBottom
            // 
            this.PanBottom.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PanBottom.AutoSize = true;
            this.PanBottom.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.PanBottom.Location = new System.Drawing.Point(0, 31);
            this.PanBottom.Margin = new System.Windows.Forms.Padding(0);
            this.PanBottom.Name = "PanBottom";
            this.PanBottom.Size = new System.Drawing.Size(118, 3);
            this.PanBottom.TabIndex = 2;
            // 
            // PanRightBottom
            // 
            this.PanRightBottom.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PanRightBottom.AutoSize = true;
            this.PanRightBottom.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.PanRightBottom.Location = new System.Drawing.Point(118, 31);
            this.PanRightBottom.Margin = new System.Windows.Forms.Padding(0);
            this.PanRightBottom.Name = "PanRightBottom";
            this.PanRightBottom.Size = new System.Drawing.Size(1, 3);
            this.PanRightBottom.TabIndex = 2;
            // 
            // PanRight
            // 
            this.PanRight.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PanRight.AutoSize = true;
            this.PanRight.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.PanRight.Location = new System.Drawing.Point(118, 0);
            this.PanRight.Margin = new System.Windows.Forms.Padding(0);
            this.PanRight.Name = "PanRight";
            this.PanRight.Size = new System.Drawing.Size(1, 31);
            this.PanRight.TabIndex = 2;
            // 
            // PanRightTop
            // 
            this.PanRightTop.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PanRightTop.AutoSize = true;
            this.PanRightTop.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.PanRightTop.Location = new System.Drawing.Point(118, 0);
            this.PanRightTop.Margin = new System.Windows.Forms.Padding(0);
            this.PanRightTop.Name = "PanRightTop";
            this.PanRightTop.Size = new System.Drawing.Size(1, 1);
            this.PanRightTop.TabIndex = 2;
            // 
            // TrackBarEx
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.MainLayout);
            this.MinimumSize = new System.Drawing.Size(116, 34);
            this.Name = "TrackBarEx";
            this.Size = new System.Drawing.Size(116, 34);
            this.Resize += new System.EventHandler(this.TrackBarEx_Resize);
            this.MainLayout.ResumeLayout(false);
            this.MainLayout.PerformLayout();
            this.BGPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel ScrollPanel;
        private System.Windows.Forms.Button MainButton;
        private System.Windows.Forms.TableLayoutPanel MainLayout;
        private System.Windows.Forms.Panel BGPanel;
        private System.Windows.Forms.Panel PanLeftTop;
        private System.Windows.Forms.Panel PanTop;
        private System.Windows.Forms.Panel PanLeft;
        private System.Windows.Forms.Panel PanLeftBottom;
        private System.Windows.Forms.Panel PanBottom;
        private System.Windows.Forms.Panel PanRightBottom;
        private System.Windows.Forms.Panel PanRight;
        private System.Windows.Forms.Panel PanRightTop;
    }
}
