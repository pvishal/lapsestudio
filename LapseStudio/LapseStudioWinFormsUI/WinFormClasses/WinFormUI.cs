using System;
using System.Windows.Forms;
using Timelapse_API;
using Timelapse_UI;
using System.Collections;
using FileDialog = Timelapse_UI.FileDialog;
using MessageBox = Timelapse_UI.MessageBox;
using Message = MessageTranslation.Message;

namespace LapseStudioWinFormsUI
{
	public class WinFormUI : LapseStudioUI
	{
		MainForm mw;

        public WinFormUI(MainForm win, MessageBox MsgBox, FileDialog FDialog)
            : base(MsgBox, FDialog)
		{
			mw = win;
			Init(Platform.Windows);
            WinFormFileDialog.InitOpenFolderDialog(mw);
            mw.AddFileToolButton.Image = Timelapse_UI.Properties.Resources.Add32;
            mw.CalculateToolButton.Image = Timelapse_UI.Properties.Resources.Calculate32;
            mw.MetadataToolButton.Image = Timelapse_UI.Properties.Resources.Reload32;
            mw.ProcessToolButton.Image = Timelapse_UI.Properties.Resources.Save32;
            mw.CancelToolButton.Image = Timelapse_UI.Properties.Resources.Cancel32;

            MsgBox.InfoTextChanged += MsgBox_InfoTextChanged;
            this.TitleChanged += WinFormUI_TitleChanged;
		}

		#region Methods

		public override void Dispose()
		{

		}

		public override void ResetMovement()
		{
			/*mw.XmovScale.Value = 0;
			mw.YmovScale.Value = 0;
			if (mw.fixThumb.Pixbuf != null) { mw.fixThumb.Pixbuf.Dispose(); }
			if (mw.fixThumb.Pixbuf != null) { mw.fixThumb.Pixbuf.Dispose(); }*/
		}

		public override void InitMovement()
		{
			/*mw.alignThumb.Pixbuf = ProjectManager.CurrentProject.Frames[1].Thumb;
			mw.fixThumb.Pixbuf = ProjectManager.CurrentProject.Frames[0].Thumb;

			double factor = (double)mw.fixThumb.Pixbuf.Width / (double)mw.fixThumb.Pixbuf.Height;
			mw.fixThumb.Pixbuf = mw.fixThumb.Pixbuf.ScaleSimple(160, (int)(160 / factor), Gdk.InterpType.Bilinear);
			mw.alignThumb.Pixbuf = mw.alignThumb.Pixbuf.ScaleSimple(160, (int)(160 / factor), Gdk.InterpType.Bilinear);

			mw.alignThumb.Pixbuf = mw.alignThumb.Pixbuf.AddAlpha(false, 0, 0, 0);
			SetOpacity(mw.alignThumb.Pixbuf, 128);

			int nl = (mw.fixThumb.Pixbuf.Width * 25 / 100) + (mw.fixThumb.Pixbuf.Width / 2);
			int nh = (mw.fixThumb.Pixbuf.Height * 25 / 100) + (mw.fixThumb.Pixbuf.Height / 2);

			mw.FixAlignment.LeftPadding = (uint)nl;
			mw.FixAlignment.TopPadding = (uint)nh;
			mw.MoveAlignment.LeftPadding = (uint)nl;
			mw.MoveAlignment.TopPadding = (uint)nh;*/
		}

		public override void InvokeUI(Action action)
		{
			mw.Invoke(action);
		}

		public override void QuitApplication()
		{
            Application.Exit();
		}

		public override void InitOpenedProject()
        {
            UpdateTable();
			InitMovement();
            mw.MainTable.Rows[0].Selected = true;
            mw.FrameSelectScale_Scroll(null, null);

			//MainGraph.SetFromLoading(Storage.Points, Storage.SelectedPoint);
            mw.MainNotebook.SelectedIndex = (int)TabLocation.Filelist;

			//mw.CalcTypeCoBox.Active = Storage.CalcTypeCoBoxValue;
			//mw.BrightnessScale.Value = Storage.BrightnessScaleValue;
		}

        public override void ResetProgress()
        {
            mw.MainProgressBar.Value = 0;
            mw.MainProgressBar.Text = String.Empty;
        }

		public override void ResetPictureBoxes()
		{
			//mw.alignThumb.Pixbuf = null;
			//mw.fixThumb.Pixbuf = null;
			mw.ThumbEditView.Image = null;
            mw.ThumbViewList.Image = null;
            mw.ThumbViewGraph.Image = null;
		}

		public override void InitUI()
		{
            mw.MetadataToolButton.Enabled = (LSSettings.UsedProgram != ProjectType.CameraRaw) ? false : true;
            UpdateTable();
            mw.CalcTypeCoBox.Items.Clear();
            mw.CalcTypeCoBox.Items.AddRange(Enum.GetNames(typeof(BrightnessCalcType)));
            mw.CalcTypeCoBox.SelectedIndex = 0;
		}

        public override void ClearTable()
        {
            if (ProjectManager.CurrentProject.Type == ProjectType.LapseStudio) mw.MainTable.Columns[(int)TableLocation.Keyframe].Visible = false;
            else mw.MainTable.Columns[(int)TableLocation.Keyframe].Visible = true;
            mw.MainTable.Rows.Clear();
            int count = ProjectManager.CurrentProject.Frames.Count;
            if (count > 0) mw.MainTable.Rows.Add(count);
        }

        public override void SetTableRow(int Index, ArrayList Values)
        {
            mw.MainTable.Rows[Index].SetValues(Values[0], Values[1], Values[2], Values[3], Values[4], Values[5], Values[6]);
        }

		#endregion

		#region Eventhandling
        
        protected override void CurrentProject_WorkDone(object sender, WorkFinishedEventArgs e)
        {
            try
            {
                if (!e.Cancelled)
                {
                    switch (e.Topic)
                    {
                        case Work.ProcessThumbs:
                            mw.FrameSelectScale_Scroll(null, null);
                            break;
                        case Work.LoadProject:
                            InitOpenedProject();
                            break;
                    }

                    mw.StatusLabel.Text = Message.GetString(e.Topic.ToString()) + " " + Message.GetString("is done");
                }
                else { mw.StatusLabel.Text = Message.GetString(e.Topic.ToString()) + " " + Message.GetString("got cancelled"); }
                ResetProgress();
            }
            catch (Exception ex) { Error.Report("Work finished", ex); }
        }

        protected override void CurrentProject_ProgressChanged(object sender, ProgressChangeEventArgs e)
		{
            try
            {
                mw.MainProgressBar.Value = e.ProgressPercentage;
                mw.MainProgressBar.Text = e.ProgressPercentage + "%";
                mw.StatusLabel.Text = Message.GetString(e.Topic.ToString());
                if (e.Topic == ProgressType.LoadFrames) UpdateTable();
            }
            catch (Exception ex) { Error.Report("Progress changed", ex); }
		}

        protected override void CurrentProject_FramesLoaded(object sender, WorkFinishedEventArgs e)
		{
            try
            {
                mw.StatusLabel.Text = Message.GetString("Frames loaded");
                UpdateTable();
                InitMovement();
                mw.MainTable.Rows[0].Selected = true;
                mw.FrameSelectScale.SetRange(0, ProjectManager.CurrentProject.Frames.Count - 1);
                mw.FrameSelectScale_Scroll(null, null);
                ResetProgress();
            }
            catch (Exception ex) { Error.Report("Frames loading finished", ex); }
		}

        protected override void CurrentProject_BrightnessCalculated(object sender, WorkFinishedEventArgs e)
		{
            try
            {
                mw.StatusLabel.Text = Message.GetString("Brightness calculated");
                ResetProgress();
                UpdateTable();
                MainGraph.RefreshGraph();
            }
            catch (Exception ex) { Error.Report("Brightness calculation finished", ex); }
		}

        private void WinFormUI_TitleChanged(string Value)
        {
            mw.Text = Value;
        }

        private void MsgBox_InfoTextChanged(string Value)
        {
            mw.StatusLabel.Text = Value;
        }

		#endregion
	}
}