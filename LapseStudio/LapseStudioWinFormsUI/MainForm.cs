using System;
using System.Windows.Forms;
using Timelapse_UI;
using Timelapse_API;

namespace LapseStudioWinFormsUI
{
    public partial class MainForm : Form
    {
        WinFormUI MainUI;
        WinFormMessageBox MsgBox;
        WinFormFileDialog FDialog;

        public MainForm()
        {
            try
            {
                InitializeComponent();
                MsgBox = new WinFormMessageBox();
                FDialog = new WinFormFileDialog();
                MainUI = new WinFormUI(this, MsgBox, FDialog);
                MainUI.MainGraph = new BrightnessGraph(MainGraph.Width, MainGraph.Height);
                MainGraph.Init(MainUI.MainGraph);
                MainUI.InitBaseUI();
            }
            catch (Exception ex) { Error.Report("Init", ex); }
        }
        
        #region Menu

        internal void newProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try { MainUI.Click_NewProject(); }
			catch (Exception ex) { Error.Report("newProjectToolStripMenuItem_Click", ex); }
        }

        internal void openProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try { MainUI.Click_OpenProject(); }
			catch (Exception ex) { Error.Report("openProjectToolStripMenuItem_Click", ex); }
        }

        internal void saveProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try { MainUI.Click_SaveProject(false); }
			catch (Exception ex) { Error.Report("saveProjectToolStripMenuItem_Click", ex); }
        }

        internal void saveProjectAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try { MainUI.Click_SaveProject(true); }
			catch (Exception ex) { Error.Report("saveProjectAsToolStripMenuItem_Click", ex); }
        }

        internal void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try { MainUI.Quit(ClosingReason.User); }
			catch (Exception ex) { Error.Report("quitToolStripMenuItem_Click", ex); }
        }

        internal void generalSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                MySettingsDialog dlg = new MySettingsDialog();
                dlg.ShowDialog();
                MainUI.SettingsChanged();
            }
			catch (Exception ex) { Error.Report("generalSettingsToolStripMenuItem_Click", ex); }
        }

        internal void helpToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                MyHelpDialog dlg = new MyHelpDialog();
                dlg.Show();
            }
			catch (Exception ex) { Error.Report("helpToolStripMenuItem1_Click", ex); }
        }

        internal void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                MyAboutDialog dlg = new MyAboutDialog();
                dlg.ShowDialog();
            }
			catch (Exception ex) { Error.Report("aboutToolStripMenuItem_Click", ex); }
        }

        #endregion
        
        #region Toolbar

        internal void AddFileToolButton_Click(object sender, EventArgs e)
        {
            try { MainUI.Click_AddFrames(); }
			catch (Exception ex) { Error.Report("AddFileToolButton_Click", ex); }
        }

        internal void CalculateToolButton_Click(object sender, EventArgs e)
        {
            try { MainUI.Click_Calculate(); }
			catch (Exception ex) { Error.Report("CalculateToolButton_Click", ex); }
        }

        internal void MetadataToolButton_Click(object sender, EventArgs e)
        {
            try { MainUI.Click_RefreshMetadata(); }
			catch (Exception ex) { Error.Report("MetadataToolButton_Click", ex); }
        }

        internal void ProcessToolButton_Click(object sender, EventArgs e)
        {
            try { MainUI.Click_Process(); }
			catch (Exception ex) { Error.Report("ProcessToolButton_Click", ex); }
        }

        internal void CancelToolButton_Click(object sender, EventArgs e)
        {
            try { ProjectManager.Cancel(); }
			catch (Exception ex) { Error.Report("CancelToolButton_Click", ex); }
        }

        #endregion

        #region General

        internal void YToEndButton_Click(object sender, EventArgs e)
        {
			try { MainUI.MainGraph.YtoEnd(); }
			catch (Exception ex) { Error.Report("YToEndButton_Click", ex); }
        }

        internal void YToStartButton_Click(object sender, EventArgs e)
        {
			try { MainUI.MainGraph.YtoStart(); }
			catch (Exception ex) { Error.Report("YToStartButton_Click", ex); }
        }

        internal void AlignXButtton_Click(object sender, EventArgs e)
        {
			try { MainUI.MainGraph.AlignX(); }
			catch (Exception ex) { Error.Report("AlignXButtton_Click", ex); }
        }

        internal void ResetGraphButton_Click(object sender, EventArgs e)
        {
			try { MainUI.MainGraph.Reset(); }
			catch (Exception ex) { Error.Report("ResetGraphButton_Click", ex); }
        }

        internal void EditThumbsButton_Click(object sender, EventArgs e)
        {
			try { MainUI.Click_ThumbEdit(); }
			catch (Exception ex) { Error.Report("EditThumbsButton_Click", ex); }
        }

        internal void FrameSelectScale_Scroll(object sender, EventArgs e)
        {
			try
			{
	            if (FrameSelectScale.Value >= 0 && FrameSelectScale.Value < ProjectManager.CurrentProject.Frames.Count)
	            {
	                ThumbEditView.Image = ProjectManager.CurrentProject.Frames[(int)FrameSelectScale.Value].ThumbEdited.Bitmap;
	                ThumbViewGraph.Image = ProjectManager.CurrentProject.Frames[(int)FrameSelectScale.Value].Thumb.Bitmap;
	                FrameSelectLabel.Text = FrameSelectScale.Value.ToString();
				}
			}
			catch (Exception ex) { Error.Report("FrameSelectScale_Scroll", ex); }
        }

        internal void BrightnessScale_Scroll(object sender, EventArgs e)
		{
			try { MainUI.Click_BrightnessSlider(BrightnessScale.Value); }
			catch (Exception ex) { Error.Report("BrightnessScale_Scroll", ex); }
        }

        internal void MainTable_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0 && e.ColumnIndex == (int)TableLocation.Brightness)
                {
                    MainUI.UpdateBrightness(e.RowIndex, (string)MainTable.Rows[e.RowIndex].Cells[(int)TableLocation.Brightness].Value);
                }
            }
			catch (Exception ex) { Error.Report("MainTable_CellValueChanged", ex); }
        }

        internal void MainTable_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0 && e.ColumnIndex == (int)TableLocation.Keyframe)
                {
                    MainUI.Click_KeyframeToggle(e.RowIndex, (bool)MainTable.Rows[e.RowIndex].Cells[(int)TableLocation.Keyframe].Value);
                }
            }
			catch (Exception ex) { Error.Report("MainTable_CellMouseClick", ex); }
        }

        internal void MainTable_SelectionChanged(object sender, EventArgs e)
        {
            try { if (MainTable.SelectedRows.Count > 0) ThumbViewList.Image = ProjectManager.CurrentProject.Frames[MainTable.SelectedRows[0].Index].Thumb.Bitmap; }
			catch (Exception ex) { Error.Report("MainTable_SelectionChanged", ex); }
        }

        internal void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try { if (e.CloseReason != CloseReason.ApplicationExitCall && e.CloseReason != CloseReason.WindowsShutDown) e.Cancel = MainUI.Quit(ClosingReason.User); }
			catch (Exception ex) { Error.Report("MainForm_FormClosing", ex); }
        }

        #endregion
    }
}
