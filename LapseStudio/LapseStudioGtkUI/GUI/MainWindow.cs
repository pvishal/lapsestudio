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

	public void OnSaveAsActionActivated(object sender, EventArgs e)
	{
		try { MainUI.Click_SaveProject(true); }
		catch (Exception ex) { Error.Report("OnSaveAsActionActivated", ex); }
    }

	public void OnSaveActionActivated(object sender, EventArgs e)
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

	public void OnRefreshMetadataActionActivated(object sender, EventArgs e)
	{
		try { MainUI.Click_RefreshMetadata(); }
		catch (Exception ex) { Error.Report("OnRefreshMetadataActionActivated", ex); }
    }

	public void OnCalculateActionActivated(object sender, EventArgs e)
	{
		try { MainUI.Click_Calculate(); }
		catch (Exception ex) { Error.Report("OnCalculateActionActivated", ex); }
    }

	public void OnAddFilesActionActivated(object sender, EventArgs e)
	{
		try { MainUI.Click_AddFrames(); }
		catch (Exception ex) { Error.Report("OnAddFilesActionActivated", ex); }
    }

    #endregion
    
	#region General

	public void OnYtoEndButtonClicked(object sender, EventArgs e)
	{
		try { MainUI.MainGraph.YtoEnd(); }
		catch (Exception ex) { Error.Report("OnYtoEndButtonClicked", ex); }
	}

	public void OnYtoStartButtonClicked(object sender, EventArgs e)
    {
		try { MainUI.MainGraph.YtoStart(); }
		catch (Exception ex) { Error.Report("OnYtoStartButtonClicked", ex); }
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
	        if (fixThumb.Pixbuf != null && alignThumb.Pixbuf != null)
	        {
	            XmovLabel.Text = Message.GetString("X Movement:") + " " + ((int)XmovScale.Value).ToString() + "%";
	            ProjectManager.SetXMovement((int)XmovScale.Value);
	            int movementAbs = (int)(fixThumb.Pixbuf.Width * XmovScale.Value / 100);
	            MoveAlignment.LeftPadding = (uint)((fixThumb.Pixbuf.Width * 75 / 100) + movementAbs);
			}
		}
		catch (Exception ex) { Error.Report("OnXmovScaleValueChanged", ex); }
    }

	public void OnYmovScaleValueChanged(object sender, EventArgs e)
    {
		try
		{
	        if (fixThumb.Pixbuf != null && alignThumb.Pixbuf != null)
	        {
	            YmovLabel.Text = Message.GetString("Y Movement:") + " " + ((int)YmovScale.Value).ToString() + "%";
	            ProjectManager.SetYMovement((int)YmovScale.Value);
	            int movementAbs = (int)(fixThumb.Pixbuf.Height * YmovScale.Value / 100);
	            MoveAlignment.TopPadding = (uint)((fixThumb.Pixbuf.Height * 75 / 100) + movementAbs);
			} 
		}
		catch (Exception ex) { Error.Report("OnYmovScaleValueChanged", ex); }
    }

	public void OnCalcTypeCoBoxChanged(object sender, EventArgs e)
    {
		try
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
		catch (Exception ex) { Error.Report("OnCalcTypeCoBoxChanged", ex); }
    }
    
	public void OnFrameSelectScaleValueChanged(object sender, EventArgs e)
    {
		try
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

	#endregion
}