using MessageTranslation;
using Gtk;
using Stetic;

namespace LapseStudioGtkUI
{
    public partial class MySettingsDialog
    {
        #region Container

        private Fixed fixed1;
        private Fixed fixed2;

        private HBox threadhbox;
        private HBox langhbox;
        private HBox proghbox;
        private HBox keepPP3hbox;
        private HBox RTpathhbox;
        private HBox bitdepthhbox;
        private HBox runrthbox;
        private HBox outforhbox;
        private HBox jpgqualhbox;
        private HBox compressionhbox;

        #endregion

        #region Important Components

        internal ComboBox LanguageCoBox;
        internal ComboBox ProgramCoBox;
        internal ComboBox FormatCoBox;
        internal ComboBox BitdepthCoBox;
        internal ComboBox CompressionCoBox;
        internal CheckButton RunRTChBox;
        internal CheckButton KeepPP3ChBox;
        internal Entry RTPathBox;
        internal Button RTBrowseButton;
        internal HScale JpgQualityScale;
        internal SpinButton ThreadSpin;
        internal CheckButton AutothreadsChBox;

        internal Button buttonCancel;
        internal Button buttonOk;

        #endregion

        #region Unimportant Components

        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;

        #endregion

        protected virtual void Build()
        {
            Gui.Initialize(this);

            #region Main

            Name = "LapseStudioSub.MySettingsDialog";
            Title = Message.GetString("Settings");
            Icon = global::Stetic.IconLoader.LoadIcon(this, "gtk-preferences", IconSize.Menu);
            WindowPosition = ((WindowPosition)(4));
            Resizable = false;
            AllowGrow = false;
            DestroyWithParent = true;

            #endregion

            #region Container

            VBox w1 = VBox;
            w1.Name = "dialog1_VBox";
            w1.Spacing = 2;
            w1.BorderWidth = ((uint)(5));

            threadhbox = new HBox();
            threadhbox.Name = "threadhbox";
            threadhbox.Spacing = 6;

            langhbox = new HBox();
            langhbox.Name = "langhbox";
            langhbox.Spacing = 6;

            proghbox = new HBox();
            proghbox.Name = "proghbox";
            proghbox.Spacing = 6;

            runrthbox = new HBox();
            runrthbox.Name = "runrthbox";
            runrthbox.Spacing = 6;

            keepPP3hbox = new HBox();
            keepPP3hbox.Name = "keepPP3hbox";
            keepPP3hbox.Spacing = 6;

            RTpathhbox = new HBox();
            RTpathhbox.Name = "RTpathhbox";
            RTpathhbox.Spacing = 6;

            outforhbox = new HBox();
            outforhbox.Name = "outforhbox";
            outforhbox.Spacing = 6;

            bitdepthhbox = new HBox();
            bitdepthhbox.Name = "bitdepthhbox";
            bitdepthhbox.Spacing = 6;

            jpgqualhbox = new HBox();
            jpgqualhbox.Name = "jpgqualhbox";
            jpgqualhbox.Spacing = 6;

            compressionhbox = new HBox();
            compressionhbox.Name = "compressionhbox";
            compressionhbox.Spacing = 6;

            fixed1 = new Fixed();
            fixed1.WidthRequest = 69;
            fixed1.Name = "fixed1";
            fixed1.HasWindow = false;
            keepPP3hbox.Add(fixed1);

            fixed2 = new Fixed();
            fixed2.WidthRequest = 114;
            fixed2.Name = "fixed2";
            fixed2.HasWindow = false;

            HButtonBox w33 = ActionArea;
            w33.Name = "dialog1_ActionArea";
            w33.Spacing = 10;
            w33.BorderWidth = ((uint)(5));
            w33.LayoutStyle = ((ButtonBoxStyle)(4));

            #endregion

            #region Important Components

            //ThreadSpin
            ThreadSpin = new SpinButton(1D, 100D, 1D);
            ThreadSpin.CanFocus = true;
            ThreadSpin.Name = "ThreadSpin";
            ThreadSpin.Adjustment.PageIncrement = 2D;
            ThreadSpin.ClimbRate = 1D;
            ThreadSpin.Numeric = true;
            ThreadSpin.Value = 1D;

            //AutothreadsChBox
            AutothreadsChBox = new CheckButton();
            AutothreadsChBox.CanFocus = true;
            AutothreadsChBox.Name = "AutothreadsChBox";
            AutothreadsChBox.Label = Message.GetString("Autothreads");
            AutothreadsChBox.DrawIndicator = true;
            AutothreadsChBox.UseUnderline = true;

            //LanguageCoBox
            LanguageCoBox = ComboBox.NewText();
            LanguageCoBox.Name = "LanguageCoBox";

            //ProgramCoBox
            ProgramCoBox = ComboBox.NewText();
            ProgramCoBox.Name = "ProgramCoBox";
            ProgramCoBox.Active = 0;

            //RunRTChBox
            RunRTChBox = new CheckButton();
            RunRTChBox.CanFocus = true;
            RunRTChBox.Name = "RunRTChBox";
            RunRTChBox.Label = Message.GetString("Run RawTherapee");
            RunRTChBox.DrawIndicator = true;
            RunRTChBox.UseUnderline = true;

            //KeepPP3ChBox
            KeepPP3ChBox = new CheckButton();
            KeepPP3ChBox.CanFocus = true;
            KeepPP3ChBox.Name = "KeepPP3ChBox";
            KeepPP3ChBox.Label = Message.GetString("Keep PP3");
            KeepPP3ChBox.DrawIndicator = true;
            KeepPP3ChBox.UseUnderline = true;

            //RTPathBox
            RTPathBox = new Entry();
            RTPathBox.WidthRequest = 100;
            RTPathBox.IsEditable = false;
            RTPathBox.Name = "RTPathBox";

            //RTBrowseButton
            RTBrowseButton = new Button("...");
            RTBrowseButton.WidthRequest = 20;
            RTBrowseButton.CanDefault = true;
            RTBrowseButton.CanFocus = true;
            RTBrowseButton.Name = "RTBrowseButton";

            //FormatCoBox
            FormatCoBox = ComboBox.NewText();
            FormatCoBox.Name = "FormatCoBox";
            FormatCoBox.Active = 0;

            //BitdepthCoBox
            BitdepthCoBox = ComboBox.NewText();
            BitdepthCoBox.Name = "BitdepthCoBox";
            BitdepthCoBox.Active = 0;

            //JpgQualityScale
            JpgQualityScale = new HScale(null);
            JpgQualityScale.WidthRequest = 120;
            JpgQualityScale.CanFocus = true;
            JpgQualityScale.Name = "JpgQualityScale";
            JpgQualityScale.Adjustment.Lower = 1D;
            JpgQualityScale.Adjustment.Upper = 100D;
            JpgQualityScale.Adjustment.PageIncrement = 10D;
            JpgQualityScale.Adjustment.StepIncrement = 1D;
            JpgQualityScale.Adjustment.Value = 100D;
            JpgQualityScale.DrawValue = true;
            JpgQualityScale.Digits = 0;
            JpgQualityScale.ValuePos = ((PositionType)(0));

            //CompressionCoBox
            CompressionCoBox = ComboBox.NewText();
            CompressionCoBox.Name = "CompressionCoBox";
            CompressionCoBox.Active = 0;

            //buttonCancel
            buttonCancel = new Button();
            buttonCancel.CanDefault = true;
            buttonCancel.CanFocus = true;
            buttonCancel.Name = "buttonCancel";
            buttonCancel.UseStock = true;
            buttonCancel.UseUnderline = true;
            buttonCancel.Label = "gtk-cancel";
            AddActionWidget(buttonCancel, -6);

            //buttonOk
            buttonOk = new Button();
            buttonOk.CanDefault = true;
            buttonOk.CanFocus = true;
            buttonOk.Name = "buttonOk";
            buttonOk.UseStock = true;
            buttonOk.UseUnderline = true;
            buttonOk.Label = "gtk-ok";
            AddActionWidget(buttonOk, -5);

            #endregion

            #region Unimportant Components

            label1 = new Label();
            label1.WidthRequest = 150;
            label1.Name = "label1";
            label1.LabelProp = Message.GetString("Threads:");
            label1.Wrap = true;

            label2 = new Label();
            label2.WidthRequest = 150;
            label2.Name = "label2";
            label2.LabelProp = Message.GetString("Output Format:");
            label2.Wrap = true;

            label3 = new Label();
            label3.WidthRequest = 150;
            label3.Name = "label3";
            label3.LabelProp = Message.GetString("Bit Depth:");
            label3.Wrap = true;

            label4 = new Label();
            label4.WidthRequest = 150;
            label4.Name = "label4";
            label4.LabelProp = Message.GetString("Jpg Quality:");
            label4.Wrap = true;

            label5 = new Label();
            label5.WidthRequest = 150;
            label5.Name = "label5";
            label5.LabelProp = Message.GetString("Used Program:");
            label5.Wrap = true;

            label6 = new Label();
            label6.WidthRequest = 150;
            label6.Name = "label6";
            label6.LabelProp = Message.GetString("Tiff Compression:");
            label6.Wrap = true;

            label7 = new Label();
            label7.WidthRequest = 150;
            label7.Name = "label7";
            label7.LabelProp = Message.GetString("Language:");
            label7.Wrap = true;

            label8 = new Label();
            label8.WidthRequest = 150;
            label8.Name = "label8";
            label8.LabelProp = Message.GetString("RawTherapee:");
            label8.Wrap = true;

            #endregion

            #region Container Adding

            #region Add

            threadhbox.Add(label1);
            threadhbox.Add(ThreadSpin);
            threadhbox.Add(AutothreadsChBox);
            w1.Add(threadhbox);
            langhbox.Add(label7);
            langhbox.Add(LanguageCoBox);
            w1.Add(langhbox);
            proghbox.Add(label5);
            proghbox.Add(ProgramCoBox);
            w1.Add(proghbox);
            runrthbox.Add(fixed2);
            runrthbox.Add(RunRTChBox);
            w1.Add(runrthbox);
            keepPP3hbox.Add(KeepPP3ChBox);
            w1.Add(keepPP3hbox);
            RTpathhbox.Add(label8);
            RTpathhbox.Add(RTPathBox);
            RTpathhbox.Add(RTBrowseButton);
            w1.Add(RTpathhbox);
            outforhbox.Add(label2);
            outforhbox.Add(FormatCoBox);
            w1.Add(outforhbox);
            bitdepthhbox.Add(label3);
            bitdepthhbox.Add(BitdepthCoBox);
            w1.Add(bitdepthhbox);
            jpgqualhbox.Add(label4);
            jpgqualhbox.Add(JpgQualityScale);
            w1.Add(jpgqualhbox);
            compressionhbox.Add(label6);
            compressionhbox.Add(CompressionCoBox);
            w1.Add(compressionhbox);

            #endregion

            #region Child Settings

            //label1
            Box.BoxChild w2 = ((Box.BoxChild)(threadhbox[label1]));
            w2.Position = 0;
            w2.Expand = false;
            w2.Fill = false;
            //ThreadSpin
            Box.BoxChild w3 = ((Box.BoxChild)(threadhbox[ThreadSpin]));
            w3.Position = 1;
            w3.Expand = false;
            w3.Fill = false;
            //AutothreadsChBox
            Box.BoxChild w4 = ((Box.BoxChild)(threadhbox[AutothreadsChBox]));
            w4.Position = 2;
            //threadhbox
            Box.BoxChild w5 = ((Box.BoxChild)(w1[threadhbox]));
            w5.Position = 0;
            w5.Expand = false;
            w5.Fill = false;
            w5.Padding = ((uint)(5));
            //label7
            Box.BoxChild w6 = ((Box.BoxChild)(langhbox[label7]));
            w6.Position = 0;
            w6.Expand = false;
            w6.Fill = false;
            //LanguageCoBox
            Box.BoxChild w7 = ((Box.BoxChild)(langhbox[LanguageCoBox]));
            w7.Position = 1;
            w7.Expand = false;
            w7.Fill = false;
            //langhbox
            Box.BoxChild w8 = ((Box.BoxChild)(w1[langhbox]));
            w8.Position = 1;
            w8.Expand = false;
            w8.Fill = false;
            //label5
            Box.BoxChild w9 = ((Box.BoxChild)(proghbox[label5]));
            w9.Position = 0;
            w9.Expand = false;
            w9.Fill = false;
            //ProgramCoBox
            Box.BoxChild w10 = ((Box.BoxChild)(proghbox[ProgramCoBox]));
            w10.Position = 1;
            w10.Expand = false;
            w10.Fill = false;
            //proghbox
            Box.BoxChild w11 = ((Box.BoxChild)(w1[proghbox]));
            w11.Position = 2;
            w11.Expand = false;
            w11.Fill = false;
            w11.Padding = ((uint)(5));
            //fixed2
            Box.BoxChild w12 = ((Box.BoxChild)(runrthbox[fixed2]));
            w12.Position = 0;
            //RunRTChBox
            Box.BoxChild w13 = ((Box.BoxChild)(runrthbox[RunRTChBox]));
            w13.Position = 1;
            //runrthbox
            Box.BoxChild w14 = ((Box.BoxChild)(w1[runrthbox]));
            w14.Position = 3;
            w14.Expand = false;
            w14.Fill = false;
            //fixed1
            Box.BoxChild w15 = ((Box.BoxChild)(keepPP3hbox[fixed1]));
            w15.Position = 0;
            //KeepPP3ChBox
            Box.BoxChild w16 = ((Box.BoxChild)(keepPP3hbox[KeepPP3ChBox]));
            w16.Position = 1;
            //keepPP3hbox
            Box.BoxChild w17 = ((Box.BoxChild)(w1[keepPP3hbox]));
            w17.Position = 4;
            w17.Expand = false;
            w17.Fill = false;
            //label8
            Box.BoxChild w18 = ((Box.BoxChild)(RTpathhbox[label8]));
            w18.Position = 0;
            w18.Expand = false;
            w18.Fill = false;
            //RTchooseButton
            Box.BoxChild w19 = ((Box.BoxChild)(RTpathhbox[RTPathBox]));
            w19.Position = 1;
            //RTchooseButton
            Box.BoxChild w36 = ((Box.BoxChild)(RTpathhbox[RTBrowseButton]));
            w36.Position = 2;
            //RTpathhbox
            Box.BoxChild w20 = ((Box.BoxChild)(w1[RTpathhbox]));
            w20.Position = 5;
            w20.Expand = false;
            w20.Fill = false;
            //label2
            Box.BoxChild w21 = ((Box.BoxChild)(outforhbox[label2]));
            w21.Position = 0;
            w21.Expand = false;
            w21.Fill = false;
            //FormatCoBox
            Box.BoxChild w22 = ((Box.BoxChild)(outforhbox[FormatCoBox]));
            w22.Position = 1;
            w22.Expand = false;
            w22.Fill = false;
            //outforhbox
            Box.BoxChild w23 = ((Box.BoxChild)(w1[outforhbox]));
            w23.Position = 6;
            w23.Expand = false;
            w23.Fill = false;
            w23.Padding = ((uint)(5));
            //label3
            Box.BoxChild w24 = ((Box.BoxChild)(bitdepthhbox[label3]));
            w24.Position = 0;
            w24.Expand = false;
            w24.Fill = false;
            //BitdepthCoBox
            Box.BoxChild w25 = ((Box.BoxChild)(bitdepthhbox[BitdepthCoBox]));
            w25.Position = 1;
            w25.Expand = false;
            w25.Fill = false;
            //bitdepthhbox
            Box.BoxChild w26 = ((Box.BoxChild)(w1[bitdepthhbox]));
            w26.Position = 7;
            w26.Expand = false;
            w26.Fill = false;
            w26.Padding = ((uint)(5));
            //label4
            Box.BoxChild w27 = ((Box.BoxChild)(jpgqualhbox[label4]));
            w27.Position = 0;
            w27.Expand = false;
            w27.Fill = false;
            //JpgQualityScale
            Box.BoxChild w28 = ((Box.BoxChild)(jpgqualhbox[JpgQualityScale]));
            w28.Position = 1;
            //jpgqualhbox
            Box.BoxChild w29 = ((Box.BoxChild)(w1[jpgqualhbox]));
            w29.Position = 8;
            w29.Expand = false;
            w29.Fill = false;
            w29.Padding = ((uint)(5));
            //label6
            Box.BoxChild w30 = ((Box.BoxChild)(compressionhbox[label6]));
            w30.Position = 0;
            w30.Expand = false;
            w30.Fill = false;
            w30.Padding = ((uint)(1));
            Box.BoxChild w31 = ((Box.BoxChild)(compressionhbox[CompressionCoBox]));
            w31.Position = 1;
            w31.Expand = false;
            w31.Fill = false;
            //compressionhbox
            Box.BoxChild w32 = ((Box.BoxChild)(w1[compressionhbox]));
            w32.Position = 9;
            w32.Expand = false;
            w32.Fill = false;
            w32.Padding = ((uint)(5));
            //buttonCancel
            ButtonBox.ButtonBoxChild w34 = ((ButtonBox.ButtonBoxChild)(w33[buttonCancel]));
            w34.Expand = false;
            w34.Fill = false;
            //buttonOk
            ButtonBox.ButtonBoxChild w35 = ((ButtonBox.ButtonBoxChild)(w33[buttonOk]));
            w35.Position = 1;
            w35.Expand = false;
            w35.Fill = false;

            #endregion

            #endregion

            #region Final
            
            if ((Child != null)) { Child.ShowAll(); }
            DefaultWidth = 369;
            DefaultHeight = 411;
            Show();

            #endregion

            #region Events

            AutothreadsChBox.Toggled += OnAutothreadsChBoxToggled;
            LanguageCoBox.Changed += OnLanguageCoBoxChanged;
            ProgramCoBox.Changed += OnProgramCoBoxChanged;
            RunRTChBox.Toggled += OnRunRTChBoxToggled;
            FormatCoBox.Changed += OnFormatCoBoxChanged;
            buttonCancel.Clicked += OnButtonCancelClicked;
            buttonOk.Clicked += OnButtonOkClicked;
            RTBrowseButton.Clicked += RTBrowseButton_Clicked;

            #endregion
        }
    }
}
