using System;
using Gtk;
using Timelapse_API;
using Timelapse_UI;
using MessageTranslation;
using Action = System.Action;

namespace LapseStudioGtkUI
{
	public class GtkUI : LapseStudioUI
	{
		MainWindow mw;

		public GtkUI(MainWindow win, MessageBox MsgBox, FileDialog FDialog) : base(MsgBox, FDialog)
		{
			mw = win;
			Init(Platform.Unix);

			ProjectManager.BrightnessCalculated += CurrentProject_BrightnessCalculated;
			ProjectManager.FramesLoaded += CurrentProject_FramesLoaded;
			ProjectManager.ProgressChanged += CurrentProject_ProgressChanged;
			ProjectManager.WorkDone += CurrentProject_WorkDone;
            
			mw.FileTree.CursorChanged += FileTree_CursorChanged;

			mw.ThumbEventBox.ButtonPressEvent += new ButtonPressEventHandler(fixThumb_ButtonPressEvent);
			mw.ThumbEventBox.ButtonReleaseEvent += new ButtonReleaseEventHandler(fixThumb_ButtonReleaseEvent);
			mw.ThumbEventBox.MotionNotifyEvent += new MotionNotifyEventHandler(fixThumb_MotionNotifyEvent);
		}

		#region Test:

		bool ismoving = false;
		int startx = 0;
		int starty = 0;

		void fixThumb_MotionNotifyEvent(object o, MotionNotifyEventArgs args)
		{
			/*if (ismoving)
        {
            using (Context MainGraph = Gdk.CairoHelper.Create(fixThumb.GdkWindow))
            {
                //Clear
                MainGraph.Operator = Operator.Clear;
                MainGraph.Paint();
                MainGraph.Operator = Operator.Add;
                //original thumbnail isn't repainted, it's black instead
                MainGraph.LineWidth = 1;
                MainGraph.SetSourceRGB(1, 0, 0);
                MainGraph.Rectangle(startx, starty, args.Event.X - startx, args.Event.Y - starty);
                MainGraph.Stroke();
            }
		}*/
		}

		void fixThumb_ButtonReleaseEvent(object o, ButtonReleaseEventArgs args)
		{
			ismoving = false;
			ProjectManager.CurrentProject.SimpleCalculationArea = new Rectangle(startx, starty, (int)args.Event.X - startx, (int)args.Event.Y - starty);
		}

		void fixThumb_ButtonPressEvent(object o, ButtonPressEventArgs args)
		{
			ismoving = true;
			startx = (int)args.Event.X;
			starty = (int)args.Event.Y;
		}

		#endregion

		#region Methods

		public override void Dispose()
		{
			if (mw.fixThumb.Pixbuf != null) mw.fixThumb.Pixbuf.Dispose();
			if (mw.alignThumb.Pixbuf != null) mw.alignThumb.Pixbuf.Dispose();
			if (mw.ThumbEditView.Pixbuf != null) mw.ThumbEditView.Pixbuf.Dispose();
			if (mw.ThumbViewList.Pixbuf != null) mw.ThumbViewList.Pixbuf.Dispose();
			if (mw.ThumbViewGraph.Pixbuf != null) mw.ThumbViewGraph.Pixbuf.Dispose();
		}

		public override void ResetMovement()
		{
			mw.XmovScale.Value = 0;
			mw.YmovScale.Value = 0;
			if (mw.alignThumb.Pixbuf != null) mw.alignThumb.Pixbuf.Dispose();
			if (mw.fixThumb.Pixbuf != null) mw.fixThumb.Pixbuf.Dispose();
		}

		public override void InitMovement()
		{
			mw.alignThumb.Pixbuf = ProjectManager.CurrentProject.Frames[1].Thumb.Pixbuf;
			mw.fixThumb.Pixbuf = ProjectManager.CurrentProject.Frames[0].Thumb.Pixbuf;

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
			mw.MoveAlignment.TopPadding = (uint)nh;
		}

		public override void InvokeUI(Action action)
		{
			Application.Invoke(delegate { action(); });
		}

		public override void QuitApplication()
		{
			Application.Quit();
		}

		public override void InitOpenedProject()
		{
			TreeViewHandler.UpdateTable();
			InitMovement();
			/*mw.FileTree.SetCursor(TreeViewHandler.GetFirstPath(), mw.FileTree.Columns[0], false);
			mw.FrameSelectScale.SetRange(0, ProjectManager.CurrentProject.Frames.Count - 1);
			OnFrameSelectScaleValueChanged(null, null);

			mw.MainNotebook.CurrentPage = (int)TabLocation.Graph;
			MainGraph.SetFromLoading(Storage.Points, Storage.SelectedPoint);
			mw.MainNotebook.CurrentPage = (int)TabLocation.Filelist;

			mw.CalcTypeCoBox.Active = Storage.CalcTypeCoBoxValue;
			mw.BrightnessScale.Value = Storage.BrightnessScaleValue;*/
		}

		public override void ResetProgress()
		{
			InvokeUI(() =>
			{
				mw.MainProgressBar.Fraction = 0;
				mw.MainProgressBar.Text = String.Empty;
			});
		}

		public override void ResetPictureBoxes()
		{
			mw.alignThumb.Pixbuf = null;
			mw.fixThumb.Pixbuf = null;
			mw.ThumbEditView.Pixbuf = null;
			mw.ThumbViewList.Pixbuf = null;
			mw.ThumbViewGraph.Pixbuf = null;
		}

		public override void InitUI()
		{
			mw.refreshMetadataAction.Sensitive = (LSSettings.UsedProgram != ProjectType.CameraRaw) ? false : true;
			mw.FileTree = TreeViewHandler.Init(mw.FileTree);
			mw.MainNotebook.CurrentPage = (int)TabLocation.Graph;
			mw.MainNotebook.CurrentPage = (int)TabLocation.Filelist;
			mw.CalcTypeCoBox.Active = (int)LSSettings.BrCalcType;
		}


		private void SetOpacity(Gdk.Pixbuf pxf, byte alpha)
		{
			unsafe
			{
				byte* pix1 = (byte*)pxf.Pixels;

				for (int y = 0; y < pxf.Height; y++)
				{
					for (int x = 0; x < pxf.Rowstride; x += pxf.NChannels)
					{
						int index = y * pxf.Rowstride + x;

						pix1[index + 3] = alpha;
					}
				}
			}
		}

		#endregion

		#region Eventhandling

		[GLib.ConnectBefore]
		protected void FileTree_CursorChanged(object sender, EventArgs e)
		{
			try
			{
				if (mw.FileTree.Selection.GetSelectedRows().Length > 0) TreeViewHandler.UpdateRow(mw.FileTree.Selection.GetSelectedRows()[0]);
				if (mw.MainNotebook.CurrentPage == (int)TabLocation.Filelist) { mw.ThumbViewList.Pixbuf = ProjectManager.CurrentProject.Frames[TreeViewHandler.SelectedRow].Thumb.Pixbuf.ScaleSimple(160, 120, Gdk.InterpType.Bilinear); }
			}
			catch (Exception ex) { Error.Report("FileTree CursorChanged", ex); }
		}

		private void CurrentProject_WorkDone(object sender, WorkFinishedEventArgs e)
		{
			try
			{
				Application.Invoke(delegate
				{
					if (!e.Cancelled)
					{
						switch (e.Topic)
						{
							case Work.ProcessThumbs:
								mw.OnFrameSelectScaleValueChanged(null, null);
								break;
							case Work.LoadProject:
								InitOpenedProject();
								break;
						}

						mw.StatusLabel.Text = Message.GetString(e.Topic.ToString()) + " " + Message.GetString("is done");
					}
					else { mw.StatusLabel.Text = Message.GetString(e.Topic.ToString()) + " " + Message.GetString("got cancelled"); }
					ResetProgress();
				});
			}
			catch (Exception ex) { Error.Report("Work finished", ex); }
		}

		private void CurrentProject_ProgressChanged(object sender, ProgressChangeEventArgs e)
		{
			try
			{
				Application.Invoke(delegate
				{
					mw.MainProgressBar.Fraction = e.ProgressPercentage / 100d;
					mw.MainProgressBar.Text = e.ProgressPercentage + "%";
					mw.StatusLabel.Text = Message.GetString(e.Topic.ToString());
				});
			}
			catch (Exception ex) { Error.Report("Progress changed", ex); }
		}

		private void CurrentProject_FramesLoaded(object sender, WorkFinishedEventArgs e)
		{
			try
			{
				Application.Invoke(delegate
				{
					mw.StatusLabel.Text = Message.GetString("Frames loaded");
					TreeViewHandler.UpdateTable();
					InitMovement();
					mw.FileTree.SetCursor(TreeViewHandler.GetFirstPath(), mw.FileTree.Columns[0], false);
					mw.FrameSelectScale.SetRange(0, ProjectManager.CurrentProject.Frames.Count - 1);
					mw.OnFrameSelectScaleValueChanged(null, null);
					ResetProgress();
				});
			}
			catch (Exception ex) { Error.Report("Frames loading finished", ex); }
		}

		private void CurrentProject_BrightnessCalculated(object sender, WorkFinishedEventArgs e)
		{
			try
			{
				Application.Invoke(delegate
				{
					mw.StatusLabel.Text = Message.GetString("Brightness calculated");
					TreeViewHandler.UpdateTable();
					ResetProgress();
				});
			}
			catch (Exception ex) { Error.Report("Brightness calculation finished", ex); }
		}

		#endregion
	}
}

