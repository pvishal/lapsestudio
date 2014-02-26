using MessageTranslation;
using Gtk;
using Stetic;

public partial class MainWindow
{
    #region Container

    public UIManager UIManager;
    public Statusbar MainStatusbar;
    public Notebook MainNotebook;
    public MenuBar MainMenuBar;
    public Toolbar MainToolbar;
    public EventBox ThumbEventBox;
    public ScrolledWindow GtkScrolledWindow;

    #endregion

    #region Important Components

    //TreeViews
    public TreeView FileTree;
    //ProgressBars
    public ProgressBar MainProgressBar;
    //Graphs
	public LapseStudioGtkUI.Graph MainGraph;
    //Images
    public Image ThumbViewList;
    public Image fixThumb;
    public Image alignThumb;
    public Image ThumbEditView;
    public Image ThumbViewGraph;
    //Labels
    public Label XmovLabel;
    public Label YmovLabel;
    public Label StatusLabel;
    //ComboBoxes
    public ComboBox CalcTypeCoBox;
    //Scales
    public VScale YmovScale;
    public HScale XmovScale;
    public HScale BrightnessScale;
    public HScale FrameSelectScale;
    //Alignments
    public Alignment FixAlignment;
    public Alignment MoveAlignment;
    //Buttons
    public Button YtoEndButton;
    public Button YtoStartButton;
    public Button ThumbEditButton;
    public Button AlignXButton;
    public Button ResetGraphButton;

    #endregion

    #region Actions

    public Action MenuAction;
	public Action newAction;
	public Action openAction;
	public Action saveAction;
	public Action saveAsAction;
    public Action quitAction;
    public Action HelpAction;
	public Action helpAction;
	public Action aboutAction;
	public Action SettingsAction;
	public Action preferencesAction;
	public Action calculateAction;
	public Action addFilesAction;
	public Action processAction;
	public Action cancelAction;
	public Action refreshMetadataAction;

    #endregion

    #region Unimportant stuff

    public VBox vbox1;
    public VBox vbox2;
    public VBox vbox3;
    public HBox hbox1;
    public HBox hbox2;

    public Fixed fixed1;
    public Fixed fixed2;
    public Fixed fixed3;

    public Label label1;
    public Label label2;
    public Label label3;
    public Label label4;

    public HSeparator hseparator1;
    public HSeparator hseparator2;
    public VSeparator vseparator1;

    public HButtonBox hbuttonbox1;
    public HButtonBox hbuttonbox2;

    #endregion

    protected virtual void Build()
    {
        Gui.Initialize(this);

        #region MainWindow

        UIManager = new UIManager();
        ActionGroup w1 = new ActionGroup("Default");
        MenuAction = new Action("MenuAction", Message.GetString("Menu"), null, null);
        newAction = new Action("newAction", Message.GetString("New Project"), null, "gtk-new");
        openAction = new Action("openAction", Message.GetString("Open Project"), null, "gtk-open");
        saveAction = new Action("saveAction", Message.GetString("Save Project"), null, "gtk-save");
        saveAsAction = new Action("saveAsAction", Message.GetString("Save Project As"), null, "gtk-save-as");
        quitAction = new Action("quitAction", Message.GetString("Quit"), null, "gtk-quit");
        HelpAction = new Action("HelpAction", Message.GetString("Help"), null, null);
        helpAction = new Action("helpAction", Message.GetString("Help"), null, "gtk-help");
        aboutAction = new Action("aboutAction", Message.GetString("About"), null, "gtk-about");
        SettingsAction = new Action("SettingsAction", Message.GetString("Settings"), null, null);
        preferencesAction = new Action("preferencesAction", Message.GetString("General Settings"), null, "gtk-preferences");
        calculateAction = new Action("calculateAction", null, null, "gtk-execute");
        addFilesAction = new Action("addFilesAction", null, null, "gtk-open");
        processAction = new Action("processAction", null, null, "gtk-harddisk");
        cancelAction = new Action("cancelAction", null, null, "gtk-stop");
        refreshMetadataAction = new Action("refreshMetadataAction", null, null, "gtk-refresh");

        MenuAction.ShortLabel = Message.GetString("Menu");
        newAction.ShortLabel = Message.GetString("New Project");
        openAction.ShortLabel = Message.GetString("Open Project");
        saveAction.ShortLabel = Message.GetString("Save Project");
        saveAsAction.ShortLabel = Message.GetString("Save Project As");
        quitAction.ShortLabel = Message.GetString("Quit");
        helpAction.ShortLabel = Message.GetString("Help");
        aboutAction.ShortLabel = Message.GetString("About");
        SettingsAction.ShortLabel = Message.GetString("Settings");
        preferencesAction.ShortLabel = Message.GetString("General Settings");

        w1.Add(MenuAction, null);
        w1.Add(newAction, null);
        w1.Add(openAction, null);
        w1.Add(saveAction, null);
        w1.Add(saveAsAction, null);
        w1.Add(quitAction, null);
        w1.Add(HelpAction, null);
        w1.Add(helpAction, null);
        w1.Add(aboutAction, null);
        w1.Add(SettingsAction, null);
        w1.Add(preferencesAction, null);
        w1.Add(calculateAction, null);
        w1.Add(addFilesAction, null);
        w1.Add(processAction, null);
        w1.Add(cancelAction, null);
        w1.Add(refreshMetadataAction, null);

        UIManager.InsertActionGroup(w1, 0);
        AddAccelGroup(UIManager.AccelGroup);
        Name = "MainWindow";
        Title = "LapseStudio";
        WindowPosition = ((WindowPosition)(4));

        #endregion

        #region Container
        
        //MainMenu
        UIManager.AddUiFromString(@"<ui><menubar name='MainMenuBar'><menu name='MenuAction' action='MenuAction'><menuitem name='newAction' action='newAction'/><menuitem name='openAction' action='openAction'/><separator/><menuitem name='saveAction' action='saveAction'/><menuitem name='saveAsAction' action='saveAsAction'/><separator/><menuitem name='quitAction' action='quitAction'/></menu><menu name='SettingsAction' action='SettingsAction'><menuitem name='preferencesAction' action='preferencesAction'/></menu><menu name='HelpAction' action='HelpAction'><menuitem name='helpAction' action='helpAction'/><menuitem name='aboutAction' action='aboutAction'/></menu></menubar></ui>");
        MainMenuBar = ((MenuBar)(UIManager.GetWidget("/MainMenuBar")));
        MainMenuBar.Name = "MainMenuBar";

        //MainToolbar
        UIManager.AddUiFromString(@"<ui><toolbar name='MainToolbar'><toolitem name='addFilesAction' action='addFilesAction'/><toolitem name='calculateAction' action='calculateAction'/><toolitem name='refreshMetadataAction' action='refreshMetadataAction'/><toolitem name='processAction' action='processAction'/><toolitem name='cancelAction' action='cancelAction'/><toolitem/><toolitem/></toolbar></ui>");
        MainToolbar = ((Toolbar)(UIManager.GetWidget("/MainToolbar")));
        MainToolbar.Name = "MainToolbar";
        MainToolbar.ShowArrow = false;
        MainToolbar.ToolbarStyle = ((ToolbarStyle)(0));
        MainToolbar.IconSize = ((IconSize)(2));

        //MainNotebook
        MainNotebook = new Notebook();
        MainNotebook.CanFocus = true;
        MainNotebook.Name = "MainNotebook";
        MainNotebook.CurrentPage = 2;
        
        //ThumbEventBox
        ThumbEventBox = new EventBox();
        ThumbEventBox.Name = "ThumbEventBox";
        
        //ScrolledWindow
        GtkScrolledWindow = new ScrolledWindow();
        GtkScrolledWindow.Name = "GtkScrolledWindow";
        GtkScrolledWindow.ShadowType = ((ShadowType)(1));

        //hbox2
        hbox1 = new HBox();
        hbox1.Name = "hbox1";
        hbox1.Spacing = 6;

        //hbox2
        hbox2 = new HBox();
        hbox2.Name = "hbox2";
        hbox2.Spacing = 6;

        //vbox1
        vbox1 = new VBox();
        vbox1.Name = "vbox1";
        vbox1.Spacing = 6;

        //vbox2
        vbox2 = new VBox();
        vbox2.Name = "vbox2";
        vbox2.Spacing = 6;

        //vbox3
        vbox3 = new VBox();
        vbox3.WidthRequest = 0;
        vbox3.Name = "vbox3";
        vbox3.Spacing = 6;
        
        //fixed1
        fixed1 = new Fixed();
        fixed1.Name = "fixed1";
        fixed1.HasWindow = false;

        //fixed2
        fixed2 = new Fixed();
        fixed2.Name = "fixed2";
        fixed2.HasWindow = false;

        //fixed3
        fixed3 = new Fixed();
        fixed3.Name = "fixed10";
        fixed3.HasWindow = false;

        //FixAlignment
        FixAlignment = new Alignment(0.5F, 0.5F, 1F, 1F);
        FixAlignment.WidthRequest = 0;
        FixAlignment.HeightRequest = 0;
        FixAlignment.Name = "FixAlignment";
        FixAlignment.LeftPadding = ((uint)(200));
        FixAlignment.TopPadding = ((uint)(100));

        //MoveAlignment
        MoveAlignment = new Alignment(0.5F, 0.5F, 1F, 1F);
        MoveAlignment.Name = "MoveAlignment";
        MoveAlignment.LeftPadding = ((uint)(200));
        MoveAlignment.TopPadding = ((uint)(100));

        //hbuttonbox1
        hbuttonbox1 = new HButtonBox();
        hbuttonbox1.LayoutStyle = ((ButtonBoxStyle)(3));

        //hbuttonbox2
        hbuttonbox2 = new HButtonBox();
        hbuttonbox2.Name = "hbuttonbox2";

        //MainStatusbar
        MainStatusbar = new Statusbar();
        MainStatusbar.Name = "MainStatusbar";
        MainStatusbar.Spacing = 20;
        MainStatusbar.HasResizeGrip = false;
        
        #endregion

        #region Important Components

        #region Notebook Tab 1 (Files)

        //FileTree
        FileTree = new TreeView();
        FileTree.CanFocus = true;
        FileTree.Name = "FileTree";
        FileTree.EnableSearch = false;

        //ThumbViewList
        ThumbViewList = new Image();
        ThumbViewList.WidthRequest = 160;
        ThumbViewList.HeightRequest = 120;
        ThumbViewList.Name = "ThumbViewList";

        #endregion

        #region Notebook Tab 2 (Calculation)

        //XmovLabel
        XmovLabel = new Label();
        XmovLabel.Name = "XmovLabel";
        XmovLabel.LabelProp = Message.GetString("X Movement:") + " 0%";

        //YmovLabel
        YmovLabel = new Label();
        YmovLabel.Name = "YmovLabel";
        YmovLabel.LabelProp = Message.GetString("Y Movement:") + " 0%";

        //CalcTypeCoBox
        CalcTypeCoBox = ComboBox.NewText();
        CalcTypeCoBox.AppendText(Message.GetString("Advanced"));
        CalcTypeCoBox.AppendText(Message.GetString("Simple"));
        CalcTypeCoBox.AppendText(Message.GetString("Exif only"));
        CalcTypeCoBox.Name = "CalcTypeCoBox";
        CalcTypeCoBox.Active = 0;

        //YmovScale
        YmovScale = new VScale(null);
        YmovScale.HeightRequest = 300;
        YmovScale.CanFocus = true;
        YmovScale.Name = "YmovScale";
        YmovScale.Adjustment.Lower = -75D;
        YmovScale.Adjustment.Upper = 75D;
        YmovScale.Adjustment.PageIncrement = 10D;
        YmovScale.Adjustment.StepIncrement = 1D;
        YmovScale.DrawValue = false;
        YmovScale.Digits = 0;
        YmovScale.ValuePos = ((PositionType)(2));

        //XmovScale
        XmovScale = new HScale(null);
        XmovScale.WidthRequest = 400;
        XmovScale.CanFocus = true;
        XmovScale.Name = "XmovScale";
        XmovScale.Adjustment.Lower = -75D;
        XmovScale.Adjustment.Upper = 75D;
        XmovScale.Adjustment.PageIncrement = 10D;
        XmovScale.Adjustment.StepIncrement = 1D;
        XmovScale.DrawValue = false;
        XmovScale.Digits = 0;
        XmovScale.ValuePos = ((PositionType)(2));

        //fixThumb
        fixThumb = new Image();
        fixThumb.Name = "fixThumb";

        //alignThumb
        alignThumb = new Image();
        alignThumb.Name = "alignThumb";

        #endregion

        #region Notebook Tab 3 (Graph)

        //MainGraph
		MainGraph = new global::LapseStudioGtkUI.Graph();
        MainGraph.Events = ((global::Gdk.EventMask)(256));
        MainGraph.Name = "MainGraph";

        //YtoEndButton
        YtoEndButton = new Button();
        YtoEndButton.CanFocus = true;
        YtoEndButton.Name = "YtoEndButton";
        YtoEndButton.UseUnderline = true;
        YtoEndButton.Label = Message.GetString("Y to End");

        //YtoStartButton
        YtoStartButton = new Button();
        YtoStartButton.CanFocus = true;
        YtoStartButton.Name = "YtoStartButton";
        YtoStartButton.UseUnderline = true;
        YtoStartButton.Label = Message.GetString("Y to Start");

        //AlignXButton
        AlignXButton = new Button();
        AlignXButton.CanFocus = true;
        AlignXButton.Name = "AlignXButton";
        AlignXButton.UseUnderline = true;
        AlignXButton.Label = Message.GetString("Align X");

		//ResetGraphButton
        ResetGraphButton = new Button();
        ResetGraphButton.CanFocus = true;
        ResetGraphButton.Name = "ResetGraphButton";
        ResetGraphButton.UseUnderline = true;
        ResetGraphButton.Label = Message.GetString("Reset");

        //ThumbEditButton
        ThumbEditButton = new Button();
        ThumbEditButton.CanFocus = true;
        ThumbEditButton.Name = "ThumbEditButton";
        ThumbEditButton.UseUnderline = true;
        ThumbEditButton.Label = Message.GetString("Edit Thumbs");

        //BrightnessScale
        BrightnessScale = new HScale(null);
        BrightnessScale.CanFocus = true;
        BrightnessScale.Name = "BrightnessScale";
        BrightnessScale.Adjustment.Lower = -100D;
        BrightnessScale.Adjustment.Upper = 100D;
        BrightnessScale.Adjustment.PageIncrement = 10D;
        BrightnessScale.Adjustment.StepIncrement = 0.5D;
        BrightnessScale.DrawValue = false;
        BrightnessScale.Digits = 0;
        BrightnessScale.ValuePos = ((PositionType)(2));

        //ThumbEditView
        ThumbEditView = new Image();
        ThumbEditView.HeightRequest = 120;
        ThumbEditView.Name = "ThumbEditView";

        //ThumbViewGraph
        ThumbViewGraph = new Image();
        ThumbViewGraph.WidthRequest = 160;
        ThumbViewGraph.HeightRequest = 120;
        ThumbViewGraph.Name = "ThumbViewGraph";

        //FrameSelectScale
        FrameSelectScale = new HScale(null);
        FrameSelectScale.CanFocus = true;
        FrameSelectScale.Name = "FrameSelectScale";
        FrameSelectScale.Adjustment.Upper = 100D;
        FrameSelectScale.Adjustment.PageIncrement = 10D;
        FrameSelectScale.Adjustment.StepIncrement = 1D;
        FrameSelectScale.DrawValue = true;
        FrameSelectScale.Digits = 0;
        FrameSelectScale.ValuePos = ((PositionType)(3));


        #endregion

        //MainProgressBar
        MainProgressBar = new ProgressBar();
        MainProgressBar.WidthRequest = 150;
        MainProgressBar.HeightRequest = 25;
        MainProgressBar.Name = "MainProgressBar";
        MainStatusbar.Add(MainProgressBar);

        //StatusLabel
        StatusLabel = new Label();
        StatusLabel.WidthRequest = 250;
        StatusLabel.Name = "StatusLabel";
        StatusLabel.LabelProp = Message.GetString("Everything ok");
        StatusLabel.Wrap = true;

        #endregion

        #region Unimportant Stuff

        //label1
        label1 = new Label();
        label1.Name = "label1";
        label1.LabelProp = Message.GetString("Files");
        label1.ShowAll();

        //label2
        label2 = new Label();
        label2.Name = "label2";
        label2.LabelProp = Message.GetString("Calculation");
        label2.ShowAll();

        //label3
        label3 = new Label();
        label3.Name = "label3";
        label3.LabelProp = Message.GetString("Graph");
        label3.ShowAll();

        //label4
        label4 = new Label();
        label4.Name = "label4";
        label4.LabelProp = Message.GetString("Brightness Calculation:");

        //hseparator1
        hseparator1 = new HSeparator();
        hseparator1.Name = "hseparator1";

        //hseparator2
        hseparator2 = new HSeparator();
        hseparator2.Name = "hseparator2";

        //vseparator1
        vseparator1 = new VSeparator();
        vseparator1.Name = "vseparator1";


        #endregion
        
        #region Container Adding

        #region Add

        vbox1.Add(MainMenuBar);
        vbox1.Add(MainToolbar);
        GtkScrolledWindow.Add(FileTree);
        hbox1.Add(GtkScrolledWindow);
        vbox2.Add(ThumbViewList);
        vbox2.Add(fixed2);
        hbox1.Add(vbox2);
        MainNotebook.Add(hbox1);
        fixed1.Add(XmovLabel);
        fixed1.Add(YmovLabel);
        fixed1.Add(label4);
        fixed1.Add(CalcTypeCoBox);
        fixed1.Add(YmovScale);
        FixAlignment.Add(ThumbEventBox);
        FixAlignment.Add(fixThumb);
        fixed1.Add(FixAlignment);
        ThumbEventBox.Add(fixThumb);
        MoveAlignment.Add(alignThumb);
        fixed1.Add(MoveAlignment);
        fixed1.Add(XmovScale);
        MainNotebook.Add(fixed1);
        vbox3.Add(hseparator1);
        hbuttonbox1.Add(YtoEndButton);
        hbuttonbox1.Add(YtoStartButton);
        vbox3.Add(hbuttonbox1);
        vbox3.Add(fixed3);
        vbox3.Add(BrightnessScale);
        vbox3.Add(ThumbEditView);
        vbox3.Add(FrameSelectScale);
        vbox3.Add(ThumbEditButton);
        vbox3.Add(ThumbViewGraph);
        vbox3.Add(hseparator2);
        hbuttonbox2.Add(AlignXButton);
        hbuttonbox2.Add(ResetGraphButton);
        vbox3.Add(hbuttonbox2);
        hbox2.Add(vbox3);
        hbox2.Add(vseparator1);
        hbox2.Add(MainGraph);
        MainNotebook.Add(hbox2);
        vbox1.Add(MainNotebook);
        MainStatusbar.Add(StatusLabel);
        vbox1.Add(MainStatusbar);

        #endregion

        #region Child Settings

        //MainMenuBar
        Box.BoxChild w2 = ((Box.BoxChild)(vbox1[MainMenuBar]));
        w2.Position = 0;
        w2.Expand = false;
        w2.Fill = false;
        //MainToolbar
        Box.BoxChild w3 = ((Box.BoxChild)(vbox1[MainToolbar]));
        w3.Position = 1;
        w3.Expand = false;
        w3.Fill = false;
        //GtkScrolledWindow
        Box.BoxChild w5 = ((Box.BoxChild)(hbox1[GtkScrolledWindow]));
        w5.Position = 0;
        //ThumbViewList
        Box.BoxChild w6 = ((Box.BoxChild)(vbox2[ThumbViewList]));
        w6.Position = 0;
        w6.Expand = false;
        w6.Fill = false;
        //fixed2
        Box.BoxChild w7 = ((Box.BoxChild)(vbox2[fixed2]));
        w7.Position = 1;
        //vbox2
        Box.BoxChild w8 = ((Box.BoxChild)(hbox1[vbox2]));
        w8.Position = 1;
        w8.Expand = false;
        w8.Fill = false;
        //XmovLabel
        Fixed.FixedChild w10 = ((Fixed.FixedChild)(fixed1[XmovLabel]));
        w10.X = 16;
        w10.Y = 20;
        //YmovLabel
        Fixed.FixedChild w11 = ((Fixed.FixedChild)(fixed1[YmovLabel]));
        w11.X = 180;
        w11.Y = 20;
        //label4
        Fixed.FixedChild w12 = ((Fixed.FixedChild)(fixed1[label4]));
        w12.X = 360;
        w12.Y = 20;
        //CalcTypeCoBox
        Fixed.FixedChild w13 = ((Fixed.FixedChild)(fixed1[CalcTypeCoBox]));
        w13.X = 518;
        w13.Y = 15;
        //YmovScale
        Fixed.FixedChild w14 = ((Fixed.FixedChild)(fixed1[YmovScale]));
        w14.X = 35;
        w14.Y = 90;
        //FixAlignment
        Fixed.FixedChild w17 = ((Fixed.FixedChild)(fixed1[FixAlignment]));
        w17.X = 70;
        w17.Y = 95;
        //MoveAlignment
        Fixed.FixedChild w18 = ((Fixed.FixedChild)(fixed1[MoveAlignment]));
        w18.X = 70;
        w18.Y = 95;
        //XmovScale
        Fixed.FixedChild w19 = ((Fixed.FixedChild)(fixed1[XmovScale]));
        w19.X = 70;
        w19.Y = 60;
        //fixed1
        Notebook.NotebookChild w20 = ((Notebook.NotebookChild)(MainNotebook[fixed1]));
        w20.Position = 1;
        //hseparator1
        Box.BoxChild w21 = ((Box.BoxChild)(vbox3[hseparator1]));
        w21.Position = 0;
        w21.Expand = false;
        w21.Fill = false;
        //YtoEndButton
        ButtonBox.ButtonBoxChild w22 = ((ButtonBox.ButtonBoxChild)(hbuttonbox1[YtoEndButton]));
        w22.Expand = false;
        w22.Fill = false;
        //YtoStartButton
        ButtonBox.ButtonBoxChild w23 = ((ButtonBox.ButtonBoxChild)(hbuttonbox1[YtoStartButton]));
        w23.Position = 1;
        w23.Expand = false;
        w23.Fill = false;
        //hbuttonbox1
        Box.BoxChild w24 = ((Box.BoxChild)(vbox3[hbuttonbox1]));
        w24.Position = 1;
        w24.Expand = false;
        w24.Fill = false;
        //fixed3
        Box.BoxChild w25 = ((Box.BoxChild)(vbox3[fixed3]));
        w25.PackType = ((PackType)(1));
        w25.Position = 2;
        //BrightnessScale
        Box.BoxChild w26 = ((Box.BoxChild)(vbox3[BrightnessScale]));
        w26.PackType = ((PackType)(1));
        w26.Position = 3;
        w26.Expand = false;
        w26.Fill = false;
        //ThumbEditView
        Box.BoxChild w27 = ((Box.BoxChild)(vbox3[ThumbEditView]));
        w27.PackType = ((PackType)(1));
        w27.Position = 4;
        w27.Expand = false;
        w27.Fill = false;
        //FrameSelectScale
        Box.BoxChild w28 = ((Box.BoxChild)(vbox3[FrameSelectScale]));
        w28.PackType = ((PackType)(1));
        w28.Position = 5;
        w28.Expand = false;
        w28.Fill = false;
        //ThumbEditButton
        Box.BoxChild w29 = ((Box.BoxChild)(vbox3[ThumbEditButton]));
        w29.PackType = ((PackType)(1));
        w29.Position = 6;
        w29.Expand = false;
        w29.Fill = false;
        Box.BoxChild w30 = ((Box.BoxChild)(vbox3[ThumbViewGraph]));
        w30.PackType = ((PackType)(1));
        w30.Position = 7;
        w30.Expand = false;
        w30.Fill = false;
        //hseparator2
        Box.BoxChild w31 = ((Box.BoxChild)(vbox3[hseparator2]));
        w31.PackType = ((PackType)(1));
        w31.Position = 8;
        w31.Expand = false;
        w31.Fill = false;
        //AlignXButton
        ButtonBox.ButtonBoxChild w32 = ((ButtonBox.ButtonBoxChild)(hbuttonbox2[AlignXButton]));
        w32.Expand = false;
        w32.Fill = false;
        //ResetGraphButton
        ButtonBox.ButtonBoxChild w33 = ((ButtonBox.ButtonBoxChild)(hbuttonbox2[ResetGraphButton]));
        w33.Position = 1;
        w33.Expand = false;
        w33.Fill = false;
        //hbuttonbox2
        Box.BoxChild w34 = ((Box.BoxChild)(vbox3[hbuttonbox2]));
        w34.PackType = ((PackType)(1));
        w34.Position = 9;
        w34.Expand = false;
        w34.Fill = false;
        Box.BoxChild w35 = ((Box.BoxChild)(hbox2[vbox3]));
        w35.Position = 0;
        w35.Expand = false;
        w35.Fill = false;
        //vseparator1
        Box.BoxChild w36 = ((Box.BoxChild)(hbox2[vseparator1]));
        w36.Position = 1;
        w36.Expand = false;
        w36.Fill = false;
        //MainGraph
        Box.BoxChild w37 = ((Box.BoxChild)(hbox2[MainGraph]));
        w37.Position = 2;
        //hbox2
        Notebook.NotebookChild w38 = ((Notebook.NotebookChild)(MainNotebook[hbox2]));
        w38.Position = 2;
        //MainNotebook
        Box.BoxChild w39 = ((Box.BoxChild)(vbox1[MainNotebook]));
        w39.Position = 2;
        //MainProgressBar
        Box.BoxChild w40 = ((Box.BoxChild)(MainStatusbar[MainProgressBar]));
        w40.Position = 0;
        //StatusLabel
        Box.BoxChild w41 = ((Box.BoxChild)(MainStatusbar[StatusLabel]));
        w41.Position = 1;
        w41.Expand = false;
        w41.Fill = false;
        //MainStatusbar
        Box.BoxChild w42 = ((Box.BoxChild)(vbox1[MainStatusbar]));
        w42.PackType = ((PackType)(1));
        w42.Position = 3;
        w42.Expand = false;
        w42.Fill = false;

        #endregion

        #endregion

        #region Final

        MainNotebook.SetTabLabel(hbox1, label1);
        MainNotebook.SetTabLabel(fixed1, label2);
        MainNotebook.SetTabLabel(hbox2, label3);
        Add(vbox1);
        if ((Child != null)) { Child.ShowAll(); }
        DefaultWidth = 737;
        DefaultHeight = 606;
        Show();

        #endregion

        #region Events

        DeleteEvent += OnDeleteEvent;
        ExposeEvent += OnExposeEvent;
        newAction.Activated += OnNewActionActivated;
        openAction.Activated += OnOpenActionActivated;
        saveAction.Activated += OnSaveActionActivated;
        saveAsAction.Activated += OnSaveAsActionActivated;
        quitAction.Activated += OnQuitActionActivated;
        helpAction.Activated += OnHelpActionActivated;
        aboutAction.Activated += OnAboutActionActivated;
        preferencesAction.Activated += OnPreferencesActionActivated;
        calculateAction.Activated += OnCalculateActionActivated;
        addFilesAction.Activated += OnAddFilesActionActivated;
        processAction.Activated += OnProcessActionActivated;
        cancelAction.Activated += OnCancelActionActivated;
        refreshMetadataAction.Activated += OnRefreshMetadataActionActivated;
        CalcTypeCoBox.Changed += OnCalcTypeCoBoxChanged;
        YmovScale.ValueChanged += OnYmovScaleValueChanged;
        XmovScale.ValueChanged += OnXmovScaleValueChanged;
        YtoEndButton.Clicked += OnYtoEndButtonClicked;
        YtoStartButton.Clicked += OnYtoStartButtonClicked;
        AlignXButton.Clicked += OnAlignXButtonClicked;
        ResetGraphButton.Clicked += OnResetGraphButtonClicked;
        ThumbEditButton.Clicked += OnThumbEditButtonClicked;
        FrameSelectScale.ValueChanged += OnFrameSelectScaleValueChanged;
        BrightnessScale.ValueChanged += OnBrightnessScaleValueChanged;

        #endregion
    }
}
