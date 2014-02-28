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
            catch (Exception ex) { Error.Report("New button clicked", ex); }
        }

        internal void openProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try { MainUI.Click_OpenProject(); }
            catch (Exception ex) { Error.Report("Open button clicked", ex); }
        }

        internal void saveProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try { MainUI.Click_SaveProject(false); }
            catch (Exception ex) { Error.Report("Save button clicked", ex); }
        }

        internal void saveProjectAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try { MainUI.Click_SaveProject(true); }
            catch (Exception ex) { Error.Report("Save as button clicked", ex); }
        }

        internal void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try { MainUI.Quit(ClosingReason.User); }
            catch (Exception ex) { Error.Report("Quit button clicked", ex); }
        }

        internal void generalSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                MySettingsDialog dlg = new MySettingsDialog();
                dlg.ShowDialog();
                MainUI.SettingsChanged();
            }
            catch (Exception ex) { Error.Report("Preferences button clicked", ex); }
        }

        internal void helpToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                MyHelpDialog dlg = new MyHelpDialog();
                dlg.Show();
            }
            catch (Exception ex) { Error.Report("Help button clicked", ex); }
        }

        internal void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                MyAboutDialog dlg = new MyAboutDialog();
                dlg.ShowDialog();
            }
            catch (Exception ex) { Error.Report("About button clicked", ex); }
        }

        #endregion
        
        #region Toolbar

        internal void AddFileToolButton_Click(object sender, EventArgs e)
        {
            try { MainUI.Click_AddFrames(); }
            catch (Exception ex) { Error.Report("Add files button clicked", ex); }
        }

        internal void CalculateToolButton_Click(object sender, EventArgs e)
        {
            try { MainUI.Click_Calculate(); }
            catch (Exception ex) { Error.Report("Calculate button clicked", ex); }
        }

        internal void MetadataToolButton_Click(object sender, EventArgs e)
        {
            try { MainUI.Click_RefreshMetadata(); }
            catch (Exception ex) { Error.Report("Refresh metadata button clicked", ex); }
        }

        internal void ProcessToolButton_Click(object sender, EventArgs e)
        {
            try { MainUI.Click_Process(); }
            catch (Exception ex) { Error.Report("Process button clicked", ex); }
        }

        internal void CancelToolButton_Click(object sender, EventArgs e)
        {
            try { ProjectManager.Cancel(); }
            catch (Exception ex) { Error.Report("Cancel button clicked", ex); }
        }

        #endregion

        #region General

        internal void YToEndButton_Click(object sender, EventArgs e)
        {
            MainUI.MainGraph.YtoEnd();
        }

        internal void YToStartButton_Click(object sender, EventArgs e)
        {
            MainUI.MainGraph.YtoStart();
        }

        internal void AlignXButtton_Click(object sender, EventArgs e)
        {
            MainUI.MainGraph.AlignX();
        }

        internal void ResetGraphButton_Click(object sender, EventArgs e)
        {
            MainUI.MainGraph.Reset();
        }

        internal void EditThumbsButton_Click(object sender, EventArgs e)
        {
            MainUI.Click_ThumbEdit();
        }

        internal void FrameSelectScale_Scroll(object sender, EventArgs e)
        {
            if (FrameSelectScale.Value >= 0 && FrameSelectScale.Value < ProjectManager.CurrentProject.Frames.Count)
            {
                ThumbEditView.Image = ProjectManager.CurrentProject.Frames[(int)FrameSelectScale.Value].ThumbEdited.Bitmap;
                ThumbViewGraph.Image = ProjectManager.CurrentProject.Frames[(int)FrameSelectScale.Value].Thumb.Bitmap;
                FrameSelectLabel.Text = FrameSelectScale.Value.ToString();
            }
        }

        internal void BrightnessScale_Scroll(object sender, EventArgs e)
        {
            MainUI.SetBrightnessScale(BrightnessScale.Value);
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
            catch (Exception ex) { Error.Report("MainTable cell value changed", ex); }
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
            catch (Exception ex) { Error.Report("MainTable cell mouse click", ex); }
        }

        internal void MainTable_SelectionChanged(object sender, EventArgs e)
        {
            try { if (MainTable.SelectedRows.Count > 0) ThumbViewList.Image = ProjectManager.CurrentProject.Frames[MainTable.SelectedRows[0].Index].Thumb.Bitmap; }
            catch (Exception ex) { Error.Report("FileTree CursorChanged", ex); }
        }

        internal void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try { if (e.CloseReason != CloseReason.ApplicationExitCall && e.CloseReason != CloseReason.WindowsShutDown) e.Cancel = MainUI.Quit(ClosingReason.User); }
            catch (Exception ex) { Error.Report("Delete event", ex); }
        }

        #endregion
    }
}
