using System;
using System.Collections;
using System.Windows.Forms;
using Timelapse_UI;
using Timelapse_API;
using System.Data;
using Message = MessageTranslation.Message;

namespace LapseStudioWinFormsUI
{
    public partial class MainForm : Form, IUIHandler
    {
        WinFormMessageBox MsgBox;
        WinFormFileDialog FDialog;
        LapseStudioUI MainUI;
        DataTable TableData = new DataTable();

        public MainForm()
        {
            try
            {
                InitializeComponent();
                MsgBox = new WinFormMessageBox();
                FDialog = new WinFormFileDialog();
                MainUI = new LapseStudioUI(Platform.Windows, this, MsgBox, FDialog);

                WinFormFileDialog.InitOpenFolderDialog(this);
                AddFileToolButton.Image = Timelapse_UI.Properties.Resources.Add32;
                CalculateToolButton.Image = Timelapse_UI.Properties.Resources.Calculate32;
                MetadataToolButton.Image = Timelapse_UI.Properties.Resources.Reload32;
                ProcessToolButton.Image = Timelapse_UI.Properties.Resources.Save32;
                CancelToolButton.Image = Timelapse_UI.Properties.Resources.Cancel32;

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
            try { MainUI.Click_Calculate((BrightnessCalcType)CalcTypeCoBox.SelectedIndex); }
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
            try { RefreshImages(); }
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
            try
            {
                if (MainTable.SelectedRows.Count > 0)
                {
                    BitmapEx tmpBmp = ProjectManager.CurrentProject.GetThumb(MainTable.SelectedRows[0].Index);
                    if (tmpBmp != null) ThumbViewList.Image = WinFormHelper.ConvertToBitmap(tmpBmp);
                }
            }
            catch (Exception ex) { Error.Report("MainTable_SelectionChanged", ex); }
        }

        internal void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try { if (e.CloseReason != CloseReason.ApplicationExitCall && e.CloseReason != CloseReason.WindowsShutDown) e.Cancel = MainUI.Quit(ClosingReason.User); }
			catch (Exception ex) { Error.Report("MainForm_FormClosing", ex); }
        }

        #endregion
        
        #region UI Methods

        public void ReleaseUIData()
        {
            ResetPictureBoxes();
        }
        
        public void InvokeUI(Action action)
        {
            Invoke((MethodInvoker)delegate { action(); });
        }

        public void QuitApplication()
        {
            Application.Exit();
        }

        public void InitOpenedProject()
        {
            MainUI.UpdateTable(false);
            SelectTableRow(0);
            RefreshImages();
            FrameSelectScale.SetRange(0, ProjectManager.CurrentProject.Frames.Count - 1);

            //MainGraph.SetFromLoading(Storage.Points, Storage.SelectedPoint);
            MainNotebook.SelectedIndex = (int)TabLocation.Filelist;

            //mw.CalcTypeCoBox.Active = Storage.CalcTypeCoBoxValue;
            //mw.BrightnessScale.Value = Storage.BrightnessScaleValue;
        }

        public void SetProgress(int percent)
        {
            MainProgressBar.Value = percent;
        }

        public void ResetPictureBoxes()
        {
            ThumbEditView.Image = null;
            ThumbViewList.Image = null;
            ThumbViewGraph.Image = null;
        }

        public void RefreshImages()
        {
            if (ProjectManager.CurrentProject.Frames.Count > 0)
            {
                if (FrameSelectScale.Value >= 0 && FrameSelectScale.Value < ProjectManager.CurrentProject.Frames.Count
                    && ProjectManager.CurrentProject.GetThumb(FrameSelectScale.Value) != null)
                {
                    ThumbEditView.Image = WinFormHelper.ConvertToBitmap(ProjectManager.CurrentProject.GetThumbEdited((int)FrameSelectScale.Value));
                    ThumbViewGraph.Image = WinFormHelper.ConvertToBitmap(ProjectManager.CurrentProject.GetThumb((int)FrameSelectScale.Value));
                    FrameSelectLabel.Text = FrameSelectScale.Value.ToString();
                }
            }
            else
            {
                ThumbEditView.Image = null;
                ThumbViewGraph.Image = null;
                ThumbViewList.Image = null;
            }
        }

        public void InitUI()
        {
            MetadataToolButton.Enabled = (LSSettings.UsedProgram != ProjectType.CameraRaw) ? false : true;
            TableData = new DataTable();
            CalcTypeCoBox.Items.Clear();
            CalcTypeCoBox.Items.AddRange(Enum.GetNames(typeof(BrightnessCalcType)));
            CalcTypeCoBox.SelectedIndex = 0;
        }
        
        public void InitAfterFrameLoad()
        {
            FrameSelectScale.SetRange(0, ProjectManager.CurrentProject.Frames.Count - 1);
        }

        public void SetStatusLabel(string Text)
        {
            StatusLabel.Text = Text;
        }

        public void SetWindowTitle(string Text)
        {
            this.Text = Text;
        }
        
        public void SetTableRow(int Index, ArrayList Values, bool fill)
        {
            if (fill) { TableData.Rows.Add(Values.ToArray()); TableData.AcceptChanges(); }
            else
            {
                TableData.Rows[Index].ItemArray = Values.ToArray();
                TableData.AcceptChanges();
            }
        }

        public void SelectTableRow(int index)
        {
            MainTable.Rows[index].Selected = true;
        }

        public void InitTable()
        {
            TableData.Columns.Add("Nr", typeof(string));
            TableData.Columns[0].DefaultValue = "0";
            TableData.Columns.Add("KF", typeof(bool));
            TableData.Columns[1].DefaultValue = false;
            TableData.Columns.Add("Filename", typeof(string));
            TableData.Columns[2].DefaultValue = "N/A";
            TableData.Columns.Add("Brightness", typeof(string));
            TableData.Columns[3].DefaultValue = "0.000";
            TableData.Columns.Add("AV", typeof(string));
            TableData.Columns[4].DefaultValue = "N/A";
            TableData.Columns.Add("TV", typeof(string));
            TableData.Columns[5].DefaultValue = "N/A";
            TableData.Columns.Add("ISO", typeof(string));
            TableData.Columns[6].DefaultValue = "N/A";
            MainTable.DataSource = TableData;

            MainTable.Columns[(int)TableLocation.Nr].ReadOnly = true;
            MainTable.Columns[(int)TableLocation.Nr].Width = 30;
            MainTable.Columns[(int)TableLocation.Nr].SortMode = DataGridViewColumnSortMode.NotSortable;
            MainTable.Columns[(int)TableLocation.Nr].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            MainTable.Columns[(int)TableLocation.Nr].Resizable = DataGridViewTriState.False;

            MainTable.Columns[(int)TableLocation.Keyframe].ReadOnly = false;
            MainTable.Columns[(int)TableLocation.Keyframe].Width = 30;
            MainTable.Columns[(int)TableLocation.Keyframe].SortMode = DataGridViewColumnSortMode.NotSortable;
            MainTable.Columns[(int)TableLocation.Keyframe].Resizable = DataGridViewTriState.False;

            MainTable.Columns[(int)TableLocation.Filename].ReadOnly = true;
            MainTable.Columns[(int)TableLocation.Filename].Width = 120;
            MainTable.Columns[(int)TableLocation.Filename].SortMode = DataGridViewColumnSortMode.NotSortable;
            MainTable.Columns[(int)TableLocation.Filename].Resizable = DataGridViewTriState.True;

            MainTable.Columns[(int)TableLocation.Brightness].ReadOnly = false;
            MainTable.Columns[(int)TableLocation.Brightness].Width = 100;
            MainTable.Columns[(int)TableLocation.Brightness].SortMode = DataGridViewColumnSortMode.NotSortable;
            MainTable.Columns[(int)TableLocation.Brightness].Resizable = DataGridViewTriState.True;

            MainTable.Columns[(int)TableLocation.AV].ReadOnly = true;
            MainTable.Columns[(int)TableLocation.AV].Width = 60;
            MainTable.Columns[(int)TableLocation.AV].SortMode = DataGridViewColumnSortMode.NotSortable;
            MainTable.Columns[(int)TableLocation.AV].Resizable = DataGridViewTriState.True;

            MainTable.Columns[(int)TableLocation.TV].ReadOnly = true;
            MainTable.Columns[(int)TableLocation.TV].Width = 60;
            MainTable.Columns[(int)TableLocation.TV].SortMode = DataGridViewColumnSortMode.NotSortable;
            MainTable.Columns[(int)TableLocation.TV].Resizable = DataGridViewTriState.True;

            MainTable.Columns[(int)TableLocation.ISO].ReadOnly = true;
            MainTable.Columns[(int)TableLocation.ISO].SortMode = DataGridViewColumnSortMode.NotSortable;
            MainTable.Columns[(int)TableLocation.ISO].Resizable = DataGridViewTriState.True;
            MainTable.Columns[(int)TableLocation.ISO].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            if (ProjectManager.CurrentProject.Type == ProjectType.LapseStudio) MainTable.Columns[(int)TableLocation.Keyframe].Visible = false;
            else MainTable.Columns[(int)TableLocation.Keyframe].Visible = true;
        }
        
        #endregion

        private void CalcTypeCoBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            CalcTypeExplanationLabel.Text = MainUI.Click_CalcTypeChanged((BrightnessCalcType)CalcTypeCoBox.SelectedIndex);
        }

        private void CalcSettingsPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void CalcSettingsPanel_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void CalcSettingsPanel_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void CalcSettingsPanel_MouseUp(object sender, MouseEventArgs e)
        {

        }
    }
}
