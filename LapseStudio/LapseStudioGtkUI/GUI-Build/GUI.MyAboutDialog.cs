using MessageTranslation;
using Gtk;
using Stetic;

namespace LapseStudioGtkUI
{
    public partial class MyAboutDialog
    {
        private TextView textview1;
        private Button buttonCancel;

        protected virtual void Build()
        {
            Gui.Initialize(this);

            //Main
            WidthRequest = 500;
            Name = "LapseStudioSub.MyAboutDialog";
            Title = Message.GetString("About LapseStudio");
            Icon = global::Stetic.IconLoader.LoadIcon(this, "gtk-about", IconSize.Menu);
            WindowPosition = ((WindowPosition)(4));
            Modal = true;
            Resizable = false;
            AllowGrow = false;
            DestroyWithParent = true;

            //VBox
            VBox w1 = VBox;
            w1.Name = "dialog1_VBox";
            w1.BorderWidth = ((uint)(4));

            //textview1
            textview1 = new TextView();
            textview1.CanFocus = true;
            textview1.Name = "textview1";
            textview1.Editable = false;
            textview1.CursorVisible = false;
            textview1.WrapMode = ((WrapMode)(2));
            textview1.LeftMargin = 15;
            textview1.RightMargin = 15;
            w1.Add(textview1);
            Box.BoxChild w2 = ((Box.BoxChild)(w1[textview1]));
            w2.Position = 0;

            //ActionArea
            HButtonBox w3 = ActionArea;
            w3.Name = "dialog1_ActionArea";
            w3.Spacing = 10;
            w3.BorderWidth = ((uint)(5));
            w3.LayoutStyle = ((ButtonBoxStyle)(4));

            //buttonCancel
            buttonCancel = new Button();
            buttonCancel.CanDefault = true;
            buttonCancel.CanFocus = true;
            buttonCancel.Name = "buttonCancel";
            buttonCancel.UseStock = true;
            buttonCancel.UseUnderline = true;
            buttonCancel.Label = "gtk-ok";
            AddActionWidget(buttonCancel, -5);
            ButtonBox.ButtonBoxChild w4 = ((ButtonBox.ButtonBoxChild)(w3[buttonCancel]));
            w4.Expand = false;
            w4.Fill = false;
            
            //Final
            if ((Child != null)) { Child.ShowAll(); }
            DefaultWidth = 500;
            DefaultHeight = 323;
            Show();

            //Events
			buttonCancel.Clicked += OnButtonCancelClicked;
        }
    }
}
