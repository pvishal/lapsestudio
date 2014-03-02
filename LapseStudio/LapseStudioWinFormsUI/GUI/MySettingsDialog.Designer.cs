namespace LapseStudioWinFormsUI
{
    partial class MySettingsDialog
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.AbortButton = new System.Windows.Forms.Button();
            this.BrowseButton = new System.Windows.Forms.Button();
            this.AutoThreadChBox = new System.Windows.Forms.CheckBox();
            this.RunRTChBox = new System.Windows.Forms.CheckBox();
            this.KeepPP3ChBox = new System.Windows.Forms.CheckBox();
            this.LanguageCoBox = new System.Windows.Forms.ComboBox();
            this.ProgramCoBox = new System.Windows.Forms.ComboBox();
            this.OutputFormatCoBox = new System.Windows.Forms.ComboBox();
            this.BitDepthCoBox = new System.Windows.Forms.ComboBox();
            this.TiffCompCoBox = new System.Windows.Forms.ComboBox();
            this.JpgQualityTrackBar = new System.Windows.Forms.TrackBar();
            this.JpgQualityLabel = new System.Windows.Forms.Label();
            this.ThreadUpDo = new System.Windows.Forms.NumericUpDown();
            this.RTPathTextBox = new System.Windows.Forms.TextBox();
            this.OkButton = new System.Windows.Forms.Button();
            this.MainOpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.JpgQualityTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ThreadUpDo)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Threads:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Language:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Used Program:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 147);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(78, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "RawTherapee:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 173);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Output Format:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 198);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(54, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Bit Depth:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 222);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(62, 13);
            this.label7.TabIndex = 0;
            this.label7.Text = "Jpg Quality:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(13, 256);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(88, 13);
            this.label8.TabIndex = 0;
            this.label8.Text = "Tiff Compression:";
            // 
            // AbortButton
            // 
            this.AbortButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.AbortButton.Location = new System.Drawing.Point(127, 293);
            this.AbortButton.Name = "AbortButton";
            this.AbortButton.Size = new System.Drawing.Size(75, 23);
            this.AbortButton.TabIndex = 1;
            this.AbortButton.Text = "Cancel";
            this.AbortButton.UseVisualStyleBackColor = true;
            this.AbortButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // BrowseButton
            // 
            this.BrowseButton.Location = new System.Drawing.Point(258, 142);
            this.BrowseButton.Name = "BrowseButton";
            this.BrowseButton.Size = new System.Drawing.Size(25, 23);
            this.BrowseButton.TabIndex = 1;
            this.BrowseButton.Text = "...";
            this.BrowseButton.UseVisualStyleBackColor = true;
            this.BrowseButton.Click += new System.EventHandler(this.BrowseButton_Click);
            // 
            // AutoThreadChBox
            // 
            this.AutoThreadChBox.AutoSize = true;
            this.AutoThreadChBox.Location = new System.Drawing.Point(169, 19);
            this.AutoThreadChBox.Name = "AutoThreadChBox";
            this.AutoThreadChBox.Size = new System.Drawing.Size(83, 17);
            this.AutoThreadChBox.TabIndex = 2;
            this.AutoThreadChBox.Text = "Autothreads";
            this.AutoThreadChBox.UseVisualStyleBackColor = true;
            this.AutoThreadChBox.CheckedChanged += new System.EventHandler(this.AutoThreadChBox_CheckedChanged);
            // 
            // RunRTChBox
            // 
            this.RunRTChBox.AutoSize = true;
            this.RunRTChBox.Location = new System.Drawing.Point(135, 98);
            this.RunRTChBox.Name = "RunRTChBox";
            this.RunRTChBox.Size = new System.Drawing.Size(117, 17);
            this.RunRTChBox.TabIndex = 2;
            this.RunRTChBox.Text = "Run RawTherapee";
            this.RunRTChBox.UseVisualStyleBackColor = true;
            this.RunRTChBox.CheckedChanged += new System.EventHandler(this.RunRTChBox_CheckedChanged);
            // 
            // KeepPP3ChBox
            // 
            this.KeepPP3ChBox.AutoSize = true;
            this.KeepPP3ChBox.Location = new System.Drawing.Point(135, 121);
            this.KeepPP3ChBox.Name = "KeepPP3ChBox";
            this.KeepPP3ChBox.Size = new System.Drawing.Size(74, 17);
            this.KeepPP3ChBox.TabIndex = 2;
            this.KeepPP3ChBox.Text = "Keep PP3";
            this.KeepPP3ChBox.UseVisualStyleBackColor = true;
            // 
            // LanguageCoBox
            // 
            this.LanguageCoBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.LanguageCoBox.FormattingEnabled = true;
            this.LanguageCoBox.Location = new System.Drawing.Point(117, 44);
            this.LanguageCoBox.Name = "LanguageCoBox";
            this.LanguageCoBox.Size = new System.Drawing.Size(166, 21);
            this.LanguageCoBox.TabIndex = 3;
            this.LanguageCoBox.SelectedIndexChanged += new System.EventHandler(this.LanguageCoBox_SelectedIndexChanged);
            // 
            // ProgramCoBox
            // 
            this.ProgramCoBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ProgramCoBox.FormattingEnabled = true;
            this.ProgramCoBox.Items.AddRange(new object[] {
            "LapseStudio",
            "RawTherapee",
            "Adobe CameraRaw"});
            this.ProgramCoBox.Location = new System.Drawing.Point(117, 71);
            this.ProgramCoBox.Name = "ProgramCoBox";
            this.ProgramCoBox.Size = new System.Drawing.Size(166, 21);
            this.ProgramCoBox.TabIndex = 3;
            this.ProgramCoBox.SelectedIndexChanged += new System.EventHandler(this.ProgramCoBox_SelectedIndexChanged);
            // 
            // OutputFormatCoBox
            // 
            this.OutputFormatCoBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.OutputFormatCoBox.FormattingEnabled = true;
            this.OutputFormatCoBox.Items.AddRange(new object[] {
            "jpg",
            "png",
            "tiff"});
            this.OutputFormatCoBox.Location = new System.Drawing.Point(117, 170);
            this.OutputFormatCoBox.Name = "OutputFormatCoBox";
            this.OutputFormatCoBox.Size = new System.Drawing.Size(66, 21);
            this.OutputFormatCoBox.TabIndex = 3;
            this.OutputFormatCoBox.SelectedIndexChanged += new System.EventHandler(this.OutputFormatCoBox_SelectedIndexChanged);
            // 
            // BitDepthCoBox
            // 
            this.BitDepthCoBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.BitDepthCoBox.Enabled = false;
            this.BitDepthCoBox.FormattingEnabled = true;
            this.BitDepthCoBox.Items.AddRange(new object[] {
            "8 bit",
            "16 bit"});
            this.BitDepthCoBox.Location = new System.Drawing.Point(117, 195);
            this.BitDepthCoBox.Name = "BitDepthCoBox";
            this.BitDepthCoBox.Size = new System.Drawing.Size(66, 21);
            this.BitDepthCoBox.TabIndex = 3;
            // 
            // TiffCompCoBox
            // 
            this.TiffCompCoBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.TiffCompCoBox.FormattingEnabled = true;
            this.TiffCompCoBox.Items.AddRange(new object[] {
            "None",
            "LZW"});
            this.TiffCompCoBox.Location = new System.Drawing.Point(117, 253);
            this.TiffCompCoBox.Name = "TiffCompCoBox";
            this.TiffCompCoBox.Size = new System.Drawing.Size(66, 21);
            this.TiffCompCoBox.TabIndex = 3;
            // 
            // JpgQualityTrackBar
            // 
            this.JpgQualityTrackBar.Location = new System.Drawing.Point(145, 222);
            this.JpgQualityTrackBar.Maximum = 100;
            this.JpgQualityTrackBar.Minimum = 1;
            this.JpgQualityTrackBar.Name = "JpgQualityTrackBar";
            this.JpgQualityTrackBar.Size = new System.Drawing.Size(138, 45);
            this.JpgQualityTrackBar.TabIndex = 4;
            this.JpgQualityTrackBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.JpgQualityTrackBar.Value = 100;
            this.JpgQualityTrackBar.Scroll += new System.EventHandler(this.JpgQualityTrackBar_Scroll);
            // 
            // JpgQualityLabel
            // 
            this.JpgQualityLabel.AutoSize = true;
            this.JpgQualityLabel.Location = new System.Drawing.Point(114, 224);
            this.JpgQualityLabel.Name = "JpgQualityLabel";
            this.JpgQualityLabel.Size = new System.Drawing.Size(25, 13);
            this.JpgQualityLabel.TabIndex = 5;
            this.JpgQualityLabel.Text = "100";
            // 
            // ThreadUpDo
            // 
            this.ThreadUpDo.Location = new System.Drawing.Point(117, 18);
            this.ThreadUpDo.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ThreadUpDo.Name = "ThreadUpDo";
            this.ThreadUpDo.Size = new System.Drawing.Size(46, 20);
            this.ThreadUpDo.TabIndex = 6;
            this.ThreadUpDo.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // RTPathTextBox
            // 
            this.RTPathTextBox.Location = new System.Drawing.Point(117, 144);
            this.RTPathTextBox.Name = "RTPathTextBox";
            this.RTPathTextBox.ReadOnly = true;
            this.RTPathTextBox.Size = new System.Drawing.Size(135, 20);
            this.RTPathTextBox.TabIndex = 7;
            // 
            // OkButton
            // 
            this.OkButton.Location = new System.Drawing.Point(208, 293);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(75, 23);
            this.OkButton.TabIndex = 1;
            this.OkButton.Text = "OK";
            this.OkButton.UseVisualStyleBackColor = true;
            this.OkButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // MainOpenFileDialog
            // 
            this.MainOpenFileDialog.FileName = "rawtherapee.exe";
            this.MainOpenFileDialog.Filter = "Executable|*.exe";
            this.MainOpenFileDialog.Title = "RawTherapee Path";
            // 
            // MySettingsDialog
            // 
            this.AcceptButton = this.OkButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(296, 326);
            this.Controls.Add(this.RTPathTextBox);
            this.Controls.Add(this.ThreadUpDo);
            this.Controls.Add(this.JpgQualityLabel);
            this.Controls.Add(this.TiffCompCoBox);
            this.Controls.Add(this.BitDepthCoBox);
            this.Controls.Add(this.ProgramCoBox);
            this.Controls.Add(this.OutputFormatCoBox);
            this.Controls.Add(this.LanguageCoBox);
            this.Controls.Add(this.KeepPP3ChBox);
            this.Controls.Add(this.RunRTChBox);
            this.Controls.Add(this.AutoThreadChBox);
            this.Controls.Add(this.BrowseButton);
            this.Controls.Add(this.OkButton);
            this.Controls.Add(this.AbortButton);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.JpgQualityTrackBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MySettingsDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Settings";
            ((System.ComponentModel.ISupportInitialize)(this.JpgQualityTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ThreadUpDo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button AbortButton;
        private System.Windows.Forms.Button BrowseButton;
        private System.Windows.Forms.Label JpgQualityLabel;
        private System.Windows.Forms.Button OkButton;
        private System.Windows.Forms.OpenFileDialog MainOpenFileDialog;
        internal System.Windows.Forms.CheckBox AutoThreadChBox;
        internal System.Windows.Forms.CheckBox RunRTChBox;
        internal System.Windows.Forms.CheckBox KeepPP3ChBox;
        internal System.Windows.Forms.ComboBox LanguageCoBox;
        internal System.Windows.Forms.ComboBox ProgramCoBox;
        internal System.Windows.Forms.ComboBox OutputFormatCoBox;
        internal System.Windows.Forms.ComboBox BitDepthCoBox;
        internal System.Windows.Forms.ComboBox TiffCompCoBox;
        internal System.Windows.Forms.TrackBar JpgQualityTrackBar;
        internal System.Windows.Forms.NumericUpDown ThreadUpDo;
        internal System.Windows.Forms.TextBox RTPathTextBox;
    }
}