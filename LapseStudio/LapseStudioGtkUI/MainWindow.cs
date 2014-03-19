using Gdk;
using Gtk;
using System;
using System.Collections;
using Timelapse_API;
using Timelapse_UI;
using Error = Timelapse_UI.Error;
using Message = MessageTranslation.Message;
using Window = Gtk.Window;

namespace LapseStudioGtkUI
{
	public partial class MainWindow : Window, IUIHandler
	{
		LapseStudioUI MainUI;
        GtkMessageBox MsgBox = new GtkMessageBox();
        int SelectedRow;
        ListStore TreeViewTable;

		#region Main

		public MainWindow()
			: base(Gtk.WindowType.Toplevel)
		{
			try
			{
				this.Build();
                MainUI = new LapseStudioUI(Platform.Unix, this, MsgBox, new GtkFileDialog());

                FileTree.CursorChanged += FileTree_CursorChanged;
                Pixbuf[] iconlist = new Pixbuf[5];
                iconlist[0] = new Pixbuf("Icons/Icon16.png");
                iconlist[1] = new Pixbuf("Icons/Icon32.png");
                iconlist[2] = new Pixbuf("Icons/Icon64.png");
                iconlist[3] = new Pixbuf("Icons/Icon128.png");
                iconlist[4] = new Pixbuf("Icons/Icon256.png");
                this.IconList = iconlist;

				MainUI.MainGraph = new BrightnessGraph(MainGraph.Allocation.Width, MainGraph.Allocation.Height);
				MainGraph.Init(MainUI.MainGraph);
				MainUI.InitBaseUI();
			}
			catch (Exception ex) { Error.Report("Init", ex); }
		}

		public void OnDeleteEvent(object sender, DeleteEventArgs a)
		{
			try { a.RetVal = MainUI.Quit(ClosingReason.User); }
			catch (Exception ex) { Error.Report("OnDeleteEvent", ex); }
		}

		public void OnExposeEvent(object sender, ExposeEventArgs a)
		{
			try { if (MainNotebook.CurrentPage == (int)TabLocation.Graph) { MainUI.MainGraph.RefreshGraph(); } }
			catch (Exception ex) { Error.Report("OnExposeEvent", ex); }
		}
        
		#endregion

		#region Menu

		public void OnNewActionActivated(object sender, EventArgs e)
		{
			try { MainUI.Click_NewProject(); }
			catch (Exception ex) { Error.Report("OnNewActionActivated", ex); }
		}

		public void OnOpenActionActivated(object sender, EventArgs e)
		{
			try { MainUI.Click_OpenProject(); }
			catch (Exception ex) { Error.Report("OnOpenActionActivated", ex); }
		}

		public void OnAboutActionActivated(object sender, EventArgs e)
		{
			try
			{
				MyAboutDialog dlg = new MyAboutDialog();
				dlg.Run();
				dlg.Destroy();
			}
			catch (Exception ex) { Error.Report("OnAboutActionActivated", ex); }
		}

		public void OnHelpActionActivated(object sender, EventArgs e)
		{
			try
			{
				MyHelpDialog dlg = new MyHelpDialog();
				dlg.Run();
				dlg.Destroy();
			}
			catch (Exception ex) { Error.Report("OnHelpActionActivated", ex); }
		}

		public void OnPreferencesActionActivated(object sender, EventArgs e)
		{
			try
			{
				MySettingsDialog dlg = new MySettingsDialog();
				dlg.Run();
				dlg.Destroy();
				MainUI.SettingsChanged();
			}
			catch (Exception ex) { Error.Report("OnPreferencesActionActivated", ex); }
		}

		public void OnQuitActionActivated(object sender, EventArgs e)
		{
			try { MainUI.Quit(ClosingReason.User); }
			catch (Exception ex) { Error.Report("OnQuitActionActivated", ex); }
		}

		public void OnSaveActionActivated(object sender, EventArgs e)
		{
			try { MainUI.Click_SaveProject(false); }
			catch (Exception ex) { Error.Report("OnSaveActionActivated", ex); }
		}

		public void OnSaveAsActionActivated(object sender, EventArgs e)
		{
			try { MainUI.Click_SaveProject(true); }
			catch (Exception ex) { Error.Report("OnSaveAsActionActivated", ex); }
		}

		public void OnProcessActionActivatedted(object sender, EventArgs e)
		{
			try { MainUI.Click_SaveProject(false); }
			catch (Exception ex) { Error.Report("OnSaveActionActivated", ex); }
		}

		#endregion

		#region Toolbar

		public void OnCancelActionActivated(object sender, EventArgs e)
		{
			try { ProjectManager.Cancel(); }
			catch (Exception ex) { Error.Report("OnCancelActionActivated", ex); }
		}

		public void OnProcessActionActivated(object sender, EventArgs e)
		{
			try { MainUI.Click_Process(); }
			catch (Exception ex) { Error.Report("OnProcessActionActivated", ex); }
		}

		public void OnReloadActionActivated(object sender, EventArgs e)
		{
			try { MainUI.Click_RefreshMetadata(); }
			catch (Exception ex) { Error.Report("OnReloadActionActivated", ex); }
		}

		public void OnCalculateActionActivated(object sender, EventArgs e)
		{
			try { MainUI.Click_Calculate(BrightnessCalcType.Advanced); }    //TODO: replace that with actual user selection
			catch (Exception ex) { Error.Report("OnCalculateActionActivated", ex); }
		}

		public void OnAddActionActivated(object sender, EventArgs e)
		{
			try { MainUI.Click_AddFrames(); }
			catch (Exception ex) { Error.Report("OnAddActionActivated", ex); }
		}

		#endregion

		#region General

		public void OnYToEndButtonClicked(object sender, EventArgs e)
		{
			try { MainUI.MainGraph.YtoEnd(); }
			catch (Exception ex) { Error.Report("OnYToEndButtonClicked", ex); }
		}

		public void OnYToStartButtonClicked(object sender, EventArgs e)
		{
			try { MainUI.MainGraph.YtoStart(); }
			catch (Exception ex) { Error.Report("OnYToStartButtonClicked", ex); }
		}

		public void OnAlignXButtonClicked(object sender, EventArgs e)
		{
			try { MainUI.MainGraph.AlignX(); }
			catch (Exception ex) { Error.Report("OnAlignXButtonClicked", ex); }
		}

		public void OnResetGraphButtonClicked(object sender, EventArgs e)
		{
			try { MainUI.MainGraph.Reset(); }
			catch (Exception ex) { Error.Report("OnResetGraphButtonClicked", ex); }
		}

		public void OnXmovScaleValueChanged(object sender, EventArgs e)
		{
			try
			{
				/*if (fixThumb.Pixbuf != null && alignThumb.Pixbuf != null)
				{
					XmovLabel.Text = Message.GetString("X Movement:") + " " + ((int)XmovScale.Value).ToString() + "%";
					ProjectManager.SetXMovement((int)XmovScale.Value);
					int movementAbs = (int)(fixThumb.Pixbuf.Width * XmovScale.Value / 100);
					MoveAlignment.LeftPadding = (uint)((fixThumb.Pixbuf.Width * 75 / 100) + movementAbs);
				}*/
			}
			catch (Exception ex) { Error.Report("OnXmovScaleValueChanged", ex); }
		}

		public void OnYmovScaleValueChanged(object sender, EventArgs e)
		{
			try
			{
				/*if (fixThumb.Pixbuf != null && alignThumb.Pixbuf != null)
				{
					YmovLabel.Text = Message.GetString("Y Movement:") + " " + ((int)YmovScale.Value).ToString() + "%";
					ProjectManager.SetYMovement((int)YmovScale.Value);
					int movementAbs = (int)(fixThumb.Pixbuf.Height * YmovScale.Value / 100);
					MoveAlignment.TopPadding = (uint)((fixThumb.Pixbuf.Height * 75 / 100) + movementAbs);
				}*/
			}
			catch (Exception ex) { Error.Report("OnYmovScaleValueChanged", ex); }
		}

		public void OnCalcTypeCoBoxChanged(object sender, EventArgs e)
		{
			try
			{
				/*switch (CalcTypeCoBox.Active)
				{
					case 0:
						LSSettings.BrCalcType = BrightnessCalcType.Advanced;
						break;
					case 1:
						LSSettings.BrCalcType = BrightnessCalcType.Simple;
						break;
					case 2:
						LSSettings.BrCalcType = BrightnessCalcType.Exif;
						break;
				}*/
			}
			catch (Exception ex) { Error.Report("OnCalcTypeCoBoxChanged", ex); }
		}

		public void OnFrameSelectScaleValueChanged(object sender, EventArgs e)
		{
            try { RefreshImages(); }
			catch (Exception ex) { Error.Report("OnFrameSelectScaleValueChanged", ex); }
		}

		public void OnThumbEditButtonClicked(object sender, EventArgs e)
		{
			try { MainUI.Click_ThumbEdit(); }
			catch (Exception ex) { Error.Report("OnThumbEditButtonClicked", ex); }
		}

		public void OnBrightnessScaleValueChanged(object sender, EventArgs e)
		{
			try { MainUI.Click_BrightnessSlider(BrightnessScale.Value); }
			catch (Exception ex) { Error.Report("OnBrightnessScaleValueChanged", ex); }
		}

        [GLib.ConnectBefore]
        public void FileTree_CursorChanged(object sender, EventArgs e)
        {
            try
            {
                if (FileTree.Selection.GetSelectedRows().Length > 0) UpdateRow(FileTree.Selection.GetSelectedRows()[0]);
                if (MainNotebook.CurrentPage == (int)TabLocation.Filelist) { ThumbViewList.Pixbuf = GtkHelper.ConvertToPixbuf(ProjectManager.CurrentProject.GetThumb(SelectedRow).ScaleW(160)); }
            }
            catch (Exception ex) { Error.Report("FileTree CursorChanged", ex); }
        }

        private void CellEdited(object o, EditedArgs args)
        {
            try { MainUI.UpdateBrightness(GetIndex(new TreePath(args.Path)), args.NewText); }
            catch (Exception ex) { Error.Report("Cell edited", ex); }
        }

        private void KeyframeCell_Toggled(object o, ToggledArgs args)
        {
            try { MainUI.Click_KeyframeToggle(GetIndex(new TreePath(args.Path)), GetToggleState(new TreePath(args.Path), TableLocation.Keyframe)); }
            catch (Exception ex) { Error.Report("Keyframecell toggled", ex); }
        }

		#endregion
        
        //TODO: loading a large amount of images breaks the UI

        #region UI Methods

        public void ReleaseUIData()
        {
            ResetPictureBoxes();
        }

        public void InvokeUI(System.Action action)
        {
            Application.Invoke(delegate { action(); });
        }

        public void QuitApplication()
        {
            Application.Quit();
        }

        public void InitOpenedProject()
        {
            MainUI.UpdateTable(false);
            SelectTableRow(0);
            RefreshImages();
            FrameSelectScale.SetRange(0, ProjectManager.CurrentProject.Frames.Count - 1);
        }

        public void SetProgress(int percent)
        {
            InvokeUI(() =>
            {
                MainProgressBar.Fraction = percent / 100f;
                MainProgressBar.Text = String.Empty;
            });
        }

        public void ResetPictureBoxes()
        {
            if (ThumbEditView.Pixbuf != null) ThumbEditView.Pixbuf.Dispose();
            if (ThumbViewList.Pixbuf != null) ThumbViewList.Pixbuf.Dispose();
            if (ThumbViewGraph.Pixbuf != null) ThumbViewGraph.Pixbuf.Dispose();
        }

        public void InitTable()
        {
            foreach (TreeViewColumn col in FileTree.Columns) { FileTree.RemoveColumn(col); }
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


            FileTree.AppendColumn(NrColumn);
            FileTree.AppendColumn(KeyframeColumn);
            FileTree.AppendColumn(FileColumn);
            FileTree.AppendColumn(BrightnessColumn);
            FileTree.AppendColumn(AVColumn);
            FileTree.AppendColumn(TVColumn);
            FileTree.AppendColumn(ISOColumn);

            FileTree.Model = TreeViewTable;
        }
        
        public void InitUI()
        {
            ReloadAction.Sensitive = LSSettings.UsedProgram == ProjectType.CameraRaw;
            if (TreeViewTable != null) TreeViewTable.Clear();
            InitTable();
        }

        public void SelectTableRow(int index)
        {
            FileTree.SetCursor(new TreePath(new int[] { index }), FileTree.Columns[0], false);
        }

        public void RefreshImages()
        {
            if (FrameSelectScale.Value >= 0 && FrameSelectScale.Value < ProjectManager.CurrentProject.Frames.Count)
            {
                ThumbEditView.Pixbuf = GtkHelper.ConvertToPixbuf(ProjectManager.CurrentProject.GetThumbEdited((int)FrameSelectScale.Value));
                ThumbViewGraph.Pixbuf = GtkHelper.ConvertToPixbuf(ProjectManager.CurrentProject.GetThumb((int)FrameSelectScale.Value));

                double factor = (double)ThumbViewGraph.Pixbuf.Width / (double)ThumbViewGraph.Pixbuf.Height;
                ThumbEditView.Pixbuf = ThumbEditView.Pixbuf.ScaleSimple(160, (int)(160 / factor), Gdk.InterpType.Bilinear);
                ThumbViewGraph.Pixbuf = ThumbViewGraph.Pixbuf.ScaleSimple(160, (int)(160 / factor), Gdk.InterpType.Bilinear);
            }
        }

        public void SetTableRow(int index, ArrayList Values, bool fill)
        {
            if (fill) TreeViewTable.AppendValues(Values[0], Values[1], Values[2], Values[3], Values[4], Values[5], Values[6]);
            else
            {
                TreeIter it;
                TreeViewTable.GetIter(out it, new TreePath(new int[] { index }));
                TreeViewTable.SetValues(it, Values[0], Values[1], Values[2], Values[3], Values[4], Values[5], Values[6]);
            }
        }

        public void SetStatusLabel(string Text)
        {
            StatusLabel.Text = Text;
        }

        public void SetWindowTitle(string Text)
        {
            this.Title = Text;
        }

        public void InitAfterFrameLoad()
        {
            FrameSelectScale.SetRange(0, ProjectManager.CurrentProject.Frames.Count - 1);
        }

        #endregion

        #region TreeView Methods

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
        
        #endregion
    }
}
