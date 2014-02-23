using Gtk;
using LapseStudioGtkUI;
using MessageTranslation;
using System;
using Timelapse_API;
using Timelapse_UI;

public partial class MainWindow : Window
{
	GtkUI MainUI;
	GtkMessageBox MsgBox = new GtkMessageBox();

    #region Main

    public MainWindow()
        : base(Gtk.WindowType.Toplevel)
    {
        try
		{
			Build();
			MainUI = new GtkUI(this, new GtkMessageBox(), new GtkFileDialog());
			MainUI.MainGraph = new BrightnessGraph(MainGraph.Allocation.Width, MainGraph.Allocation.Height);
			MainGraph.Init(MainUI.MainGraph);
			MainUI.InitBaseUI();
        }
		catch (Exception ex) { Error.Report("Init", ex); }
    }
    
	public void OnDeleteEvent(object sender, DeleteEventArgs a)
    {
		try { a.RetVal = MainUI.Quit(ClosingReason.User); }
        catch (Exception ex) { Error.Report("Delete event", ex); }
    }

	public void OnExposeEvent(object sender, ExposeEventArgs a)
    {
		if (MainNotebook.CurrentPage == (int)TabLocation.Graph) { MainUI.MainGraph.RefreshGraph(); }
    }
    
    #endregion
    
    #region Menu

	public void OnNewActionActivated(object sender, EventArgs e)
    {
		try { MainUI.Click_NewProject(); }
        catch (Exception ex) { Error.Report("New button clicked", ex); }
    }

	public void OnOpenActionActivated(object sender, EventArgs e)
	{
		try { MainUI.Click_OpenProject(); }
        catch (Exception ex) { Error.Report("Open button clicked", ex); }
    }

	public void OnAboutActionActivated(object sender, EventArgs e)
    {
        try
        {
            MyAboutDialog dlg = new MyAboutDialog();
            dlg.Run();
            dlg.Destroy();
        }
        catch (Exception ex) { Error.Report("About button clicked", ex); }
    }

	public void OnHelpActionActivated(object sender, EventArgs e)
    {
        try
        {
            MyHelpDialog dlg = new MyHelpDialog();
            dlg.Run();
            dlg.Destroy();
        }
        catch (Exception ex) { Error.Report("Help button clicked", ex); }
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
        catch (Exception ex) { Error.Report("Preferences button clicked", ex); }
    }

	public void OnQuitActionActivated(object sender, EventArgs e)
    {
		try { MainUI.Quit(ClosingReason.User); }
        catch (Exception ex) { Error.Report("Quit button clicked", ex); }
    }

	public void OnSaveAsActionActivated(object sender, EventArgs e)
	{
		try { MainUI.Click_SaveProject(true); }
        catch (Exception ex) { Error.Report("Save as button clicked", ex); }
    }

	public void OnSaveActionActivated(object sender, EventArgs e)
	{
		try { MainUI.Click_SaveProject(false); }
        catch (Exception ex) { Error.Report("Save button clicked", ex); }
    }

    #endregion
    
    #region Toolbar

	public void OnCancelActionActivated(object sender, EventArgs e)
    {
		try { ProjectManager.Cancel(); }
        catch (Exception ex) { Error.Report("Cancel button clicked", ex); }
    }

	public void OnProcessActionActivated(object sender, EventArgs e)
	{
		try { MainUI.Click_Process(); }
        catch (Exception ex) { Error.Report("Process button clicked", ex); }
    }

	public void OnRefreshMetadataActionActivated(object sender, EventArgs e)
	{
		try { MainUI.Click_RefreshMetadata(); }
        catch (Exception ex) { Error.Report("Refresh metadata button clicked", ex); }
    }

	public void OnCalculateActionActivated(object sender, EventArgs e)
	{
		try { MainUI.Click_Calculate(); }
        catch (Exception ex) { Error.Report("Calculate button clicked", ex); }
    }

	public void OnAddFilesActionActivated(object sender, EventArgs e)
	{
		try { MainUI.Click_AddFrames(); }
        catch (Exception ex) { Error.Report("Add files button clicked", ex); }
    }

    #endregion
    
	#region General

	public void OnYtoEndButtonClicked(object sender, EventArgs e)
	{
		MainUI.MainGraph.YtoEnd();
	}

	public void OnYtoStartButtonClicked(object sender, EventArgs e)
    {
		MainUI.MainGraph.YtoStart();
	}

	public void OnAlignXButtonClicked(object sender, EventArgs e)
    {
		MainUI.MainGraph.AlignX();
	}

	public void OnResetGraphButtonClicked(object sender, EventArgs e)
    {
		MainUI.MainGraph.Reset();
	}

	public void OnXmovScaleValueChanged(object sender, EventArgs e)
    {
        if (fixThumb.Pixbuf != null && alignThumb.Pixbuf != null)
        {
            XmovLabel.Text = Message.GetString("X Movement:") + " " + ((int)XmovScale.Value).ToString() + "%";
            ProjectManager.SetXMovement((int)XmovScale.Value);
            int movementAbs = (int)(fixThumb.Pixbuf.Width * XmovScale.Value / 100);
            MoveAlignment.LeftPadding = (uint)((fixThumb.Pixbuf.Width * 75 / 100) + movementAbs);
        }
    }

	public void OnYmovScaleValueChanged(object sender, EventArgs e)
    {
        if (fixThumb.Pixbuf != null && alignThumb.Pixbuf != null)
        {
            YmovLabel.Text = Message.GetString("Y Movement:") + " " + ((int)YmovScale.Value).ToString() + "%";
            ProjectManager.SetYMovement((int)YmovScale.Value);
            int movementAbs = (int)(fixThumb.Pixbuf.Height * YmovScale.Value / 100);
            MoveAlignment.TopPadding = (uint)((fixThumb.Pixbuf.Height * 75 / 100) + movementAbs);
        }
    }

	public void OnCalcTypeCoBoxChanged(object sender, EventArgs e)
    {
        switch (CalcTypeCoBox.Active)
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
        }
    }
    
	public void OnFrameSelectScaleValueChanged(object sender, EventArgs e)
    {
        if (FrameSelectScale.Value >= 0 && FrameSelectScale.Value < ProjectManager.CurrentProject.Frames.Count)
        {
            ThumbEditView.Pixbuf = ProjectManager.CurrentProject.Frames[(int)FrameSelectScale.Value].ThumbEdited.Pixbuf.Copy();
            ThumbViewGraph.Pixbuf = ProjectManager.CurrentProject.Frames[(int)FrameSelectScale.Value].Thumb.Pixbuf.Copy();

            double factor = (double)ThumbViewGraph.Pixbuf.Width / (double)ThumbViewGraph.Pixbuf.Height;
            ThumbEditView.Pixbuf = ThumbEditView.Pixbuf.ScaleSimple(160, (int)(160 / factor), Gdk.InterpType.Bilinear);
            ThumbViewGraph.Pixbuf = ThumbViewGraph.Pixbuf.ScaleSimple(160, (int)(160 / factor), Gdk.InterpType.Bilinear);
        }
    }

	public void OnThumbEditButtonClicked(object sender, EventArgs e)
    {
		MainUI.Click_ThumbEdit();
    }
    
	public void OnBrightnessScaleValueChanged(object sender, EventArgs e)
    {
        MainUI.SetBrightnessScale(BrightnessScale.Value);
    }

	#endregion
}