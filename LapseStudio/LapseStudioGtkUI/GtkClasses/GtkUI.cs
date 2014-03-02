using System;
using Gtk;
using System.Collections;
using Timelapse_API;
using Timelapse_UI;
using MessageTranslation;
using Action = System.Action;

namespace LapseStudioGtkUI
{
	public class GtkUI : LapseStudioUI
	{
        MainWindow mw;
        int SelectedRow;
        ListStore TreeViewTable;

		public GtkUI(MainWindow win, MessageBox MsgBox, FileDialog FDialog) : base(MsgBox, FDialog)
		{
			mw = win;
			Init(Platform.Unix);

			ProjectManager.BrightnessCalculated += CurrentProject_BrightnessCalculated;
			ProjectManager.FramesLoaded += CurrentProject_FramesLoaded;
			ProjectManager.ProgressChanged += CurrentProject_ProgressChanged;
			ProjectManager.WorkDone += CurrentProject_WorkDone;
            this.TitleChanged += GtkUI_TitleChanged;
            this.InfoTextChanged += GtkUI_InfoTextChanged;
            
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
			UpdateTable();
			InitMovement();
			mw.FileTree.SetCursor(GetFirstPath(), mw.FileTree.Columns[0], false);
			mw.FrameSelectScale.SetRange(0, ProjectManager.CurrentProject.Frames.Count - 1);
			/*OnFrameSelectScaleValueChanged(null, null);

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
            InitTreeView();
			mw.CalcTypeCoBox.Active = (int)LSSettings.BrCalcType;
		}

        public override void ClearTable()
        {
            TreeViewTable.Clear();
        }

        public override void SetTableRow(int Index, ArrayList Values)
        {
            SetTableRow(Values);
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

        #region TreeView Methods
        
        private void InitTreeView()
        {
            foreach (TreeViewColumn col in mw.FileTree.Columns) { mw.FileTree.RemoveColumn(col); }
            TreeViewTable = new ListStore(typeof(string), typeof(bool), typeof(string), typeof(string), typeof(string), typeof(string), typeof(string));

            TreeViewColumn NrColumn = new TreeViewColumn();
            TreeViewColumn FileColumn = new TreeViewColumn();
            TreeViewColumn BrightnessColumn = new TreeViewColumn();
            TreeViewColumn AVColumn = new TreeViewColumn();
            TreeViewColumn TVColumn = new TreeViewColumn();
            TreeViewColumn ISOColumn = new TreeViewColumn();
            TreeViewColumn KeyframeColumn = new TreeViewColumn();

            CellRendererText NrCell = new CellRendererText();
            CellRendererText FileCell = new CellRendererText();
            CellRendererText BrightnessCell = new CellRendererText();
            CellRendererText AVCell = new CellRendererText();
            CellRendererText TVCell = new CellRendererText();
            CellRendererText ISOCell = new CellRendererText();
            CellRendererToggle KeyframeCell = new CellRendererToggle();

            NrColumn.Title = Message.GetString("Nr");
            NrColumn.MinWidth = 35;
            NrColumn.Resizable = false;
            NrColumn.PackStart(NrCell, true);

            FileColumn.Title = Message.GetString("Filename");
            FileColumn.MinWidth = 100;
            FileColumn.Resizable = true;
            FileColumn.PackStart(FileCell, true);

            BrightnessColumn.Title = Message.GetString("Brightness");
            BrightnessColumn.MinWidth = 70;
            BrightnessColumn.Resizable = true;
            BrightnessColumn.PackStart(BrightnessCell, true);
            BrightnessCell.Editable = true;
            BrightnessCell.Edited += CellEdited;

            AVColumn.Title = Message.GetString("AV");
            AVColumn.MinWidth = 40;
            AVColumn.Resizable = true;
            AVColumn.PackStart(AVCell, true);

            TVColumn.Title = Message.GetString("TV");
            TVColumn.MinWidth = 40;
            TVColumn.Resizable = true;
            TVColumn.PackStart(TVCell, true);

            ISOColumn.Title = Message.GetString("ISO");
            ISOColumn.MinWidth = 40;
            ISOColumn.Resizable = true;
            ISOColumn.PackStart(ISOCell, true);

            KeyframeColumn.Title = Message.GetString("KF");
            KeyframeColumn.MinWidth = 30;
            KeyframeColumn.MaxWidth = 40;
            KeyframeColumn.PackStart(KeyframeCell, true);
            KeyframeColumn.AddAttribute(KeyframeCell, "active", (int)TableLocation.Keyframe);
            KeyframeCell.Activatable = true;
            KeyframeCell.Active = false;
            KeyframeCell.Toggled += KeyframeCell_Toggled;

            NrColumn.AddAttribute(NrCell, "text", (int)TableLocation.Nr);
            FileColumn.AddAttribute(FileCell, "text", (int)TableLocation.Filename);
            BrightnessColumn.AddAttribute(BrightnessCell, "text", (int)TableLocation.Brightness);
            AVColumn.AddAttribute(AVCell, "text", (int)TableLocation.AV);
            TVColumn.AddAttribute(TVCell, "text", (int)TableLocation.TV);
            ISOColumn.AddAttribute(ISOCell, "text", (int)TableLocation.ISO);
            KeyframeColumn.AddAttribute(KeyframeCell, "toggle", (int)TableLocation.Keyframe);


            mw.FileTree.AppendColumn(NrColumn);
            mw.FileTree.AppendColumn(KeyframeColumn);
            mw.FileTree.AppendColumn(FileColumn);
            mw.FileTree.AppendColumn(BrightnessColumn);
            mw.FileTree.AppendColumn(AVColumn);
            mw.FileTree.AppendColumn(TVColumn);
            mw.FileTree.AppendColumn(ISOColumn);

            mw.FileTree.Model = TreeViewTable;
        }

        private void UpdateRow(TreePath path)
        {
            SelectedRow = GetIndex(path);
        }

        private int GetIndex(TreePath path)
        {
            TreeIter iter;
            TreeViewTable.GetIter(out iter, path);
            return TreeViewTable.GetPath(iter).Indices[0];
        }

        private TreePath GetFirstPath()
        {
            TreeIter iter;
            TreeViewTable.GetIterFirst(out iter);
            return TreeViewTable.GetPath(iter);
        }

        private bool GetToggleState(TreePath path, TableLocation Column)
        {
            try
            {
                TreeIter iter;
                TreeViewTable.GetIter(out iter, path);
                return (bool)TreeViewTable.GetValue(iter, (int)Column);
            }
            catch { return false; }
        }
        
        private void SetTableRow(ArrayList Values)
        {
            TreeViewTable.AppendValues(Values[0], Values[1], Values[2], Values[3], Values[4], Values[5], Values[6]);
        }

        #endregion

        #region Eventhandling

        [GLib.ConnectBefore]
		protected void FileTree_CursorChanged(object sender, EventArgs e)
		{
			try
			{
				if (mw.FileTree.Selection.GetSelectedRows().Length > 0) UpdateRow(mw.FileTree.Selection.GetSelectedRows()[0]);
				if (mw.MainNotebook.CurrentPage == (int)TabLocation.Filelist) { mw.ThumbViewList.Pixbuf = ProjectManager.CurrentProject.Frames[SelectedRow].Thumb.Pixbuf.ScaleSimple(160, 120, Gdk.InterpType.Bilinear); }
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
					UpdateTable();
					InitMovement();
					mw.FileTree.SetCursor(GetFirstPath(), mw.FileTree.Columns[0], false);
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
					UpdateTable();
					ResetProgress();
				});
			}
			catch (Exception ex) { Error.Report("Brightness calculation finished", ex); }
		}

        private void CellEdited(object o, EditedArgs args)
        {
            try { UpdateBrightness(GetIndex(new TreePath(args.Path)), args.NewText); }
            catch (Exception ex) { Error.Report("Cell edited", ex); }
        }

        private void KeyframeCell_Toggled(object o, ToggledArgs args)
        {
            try { Click_KeyframeToggle(GetIndex(new TreePath(args.Path)), GetToggleState(new TreePath(args.Path), TableLocation.Keyframe)); }
            catch (Exception ex) { Error.Report("Keyframecell toggled", ex); }
        }

        private void GtkUI_InfoTextChanged(string Value)
        {
            mw.StatusLabel.Text = Value;
        }

        private void GtkUI_TitleChanged(string Value)
        {
            mw.Title = Value;
        }

		#endregion
	}
}