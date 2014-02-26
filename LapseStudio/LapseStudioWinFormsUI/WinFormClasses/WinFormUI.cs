using System;
using System.Windows.Forms;
using Timelapse_API;
using Timelapse_UI;
using MessageTranslation;
using System.Collections.Generic;
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
            mw.AddFileToolButton.Image = Timelapse_UI.Properties.Resources.Add_32x32;
            mw.CalculateToolButton.Image = Timelapse_UI.Properties.Resources.Calculate_32x32;
            mw.MetadataToolButton.Image = Timelapse_UI.Properties.Resources.Reload_32x32;
            mw.ProcessToolButton.Image = Timelapse_UI.Properties.Resources.Save_32x32;
            mw.CancelToolButton.Image = Timelapse_UI.Properties.Resources.Cancel_32x32;

			ProjectManager.BrightnessCalculated += CurrentProject_BrightnessCalculated;
			ProjectManager.FramesLoaded += CurrentProject_FramesLoaded;
			ProjectManager.ProgressChanged += CurrentProject_ProgressChanged;
			ProjectManager.WorkDone += CurrentProject_WorkDone;
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
			mw.MainNotebook.SelectedIndex = (int)TabLocation.Graph;
            mw.MainNotebook.SelectedIndex = (int)TabLocation.Filelist;
			//mw.CalcTypeCoBox.Active = (int)LSSettings.BrCalcType;
		}

        //TODO: put that on the LapseStudioUI class somehow (it's redundant)
        public void UpdateTable()
        {
            List<Frame> Framelist = ProjectManager.CurrentProject.Frames;
            if (ProjectManager.CurrentProject.Type == ProjectType.LapseStudio) mw.MainTable.Columns[(int)TableLocation.Keyframe].Visible = false;
            else mw.MainTable.Columns[(int)TableLocation.Keyframe].Visible = true;
            mw.MainTable.Rows.Clear();
            mw.MainTable.Rows.Add(ProjectManager.CurrentProject.Frames.Count);

            ArrayList LScontent = new ArrayList();
            int index;
            for (int i = 0; i < mw.MainTable.Columns.Count; i++) { LScontent.Add("N/A"); }

            for (int i = 0; i < Framelist.Count; i++)
            {
                //Nr
                index = (int)TableLocation.Nr;
                LScontent[index] = Convert.ToString(i + 1);
                //Filenames
                index = (int)TableLocation.Filename;
                LScontent[index] = Framelist[i].Filename;
                //Brightness
                index = (int)TableLocation.Brightness;
                LScontent[index] = Framelist[i].OriginalBrightness.ToString("N3");
                //AV
                index = (int)TableLocation.AV;
                if (Framelist[i].AVstring != null) { LScontent[index] = Framelist[i].AVstring; }
                else { LScontent[index] = "N/A"; }
                //TV
                index = (int)TableLocation.TV;
                if (Framelist[i].TVstring != null) { LScontent[index] = Framelist[i].TVstring; }
                else { LScontent[index] = "N/A"; }
                //ISO
                index = (int)TableLocation.ISO;
                if (Framelist[i].SVstring != null) { LScontent[index] = Framelist[i].SVstring; }
                else { LScontent[index] = "N/A"; }
                //Keyframes
                index = (int)TableLocation.Keyframe;
                if (Framelist[i].IsKeyframe) { LScontent[index] = true; }
                else { LScontent[index] = false; }

                //filling the table
                mw.MainTable.Rows[i].SetValues(LScontent[0], LScontent[1], LScontent[2], LScontent[3], LScontent[4], LScontent[5], LScontent[6]);
            }
        }
        
		#endregion

		#region Eventhandling
        
        private void CurrentProject_WorkDone(object sender, WorkFinishedEventArgs e)
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

		private void CurrentProject_ProgressChanged(object sender, ProgressChangeEventArgs e)
		{
            try
            {
                mw.MainProgressBar.Value = e.ProgressPercentage;
                mw.MainProgressBar.Text = e.ProgressPercentage + "%";
                mw.StatusLabel.Text = Message.GetString(e.Topic.ToString());
            }
            catch (Exception ex) { Error.Report("Progress changed", ex); }
		}

		private void CurrentProject_FramesLoaded(object sender, WorkFinishedEventArgs e)
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

		private void CurrentProject_BrightnessCalculated(object sender, WorkFinishedEventArgs e)
		{
            try
            {
                mw.StatusLabel.Text = Message.GetString("Brightness calculated");
                UpdateTable();
                ResetProgress();
            }
            catch (Exception ex) { Error.Report("Brightness calculation finished", ex); }
		}

		#endregion
	}
}

