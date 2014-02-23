using MessageTranslation;
using Gtk;
using Stetic;

namespace LapseStudioGtkUI
{
	public partial class MyHelpDialog
	{
		private HBox hbox1;
		private Expander TopicExpander;
		private Label GtkLabel1;
		private ScrolledWindow GtkScrolledWindow;
		private TextView HelpTextView;
		private Button buttonCancel;

		protected virtual void Build ()
		{
			Gui.Initialize (this);
			// Widget LapseStudioSub.MyHelpDialog
			Name = "LapseStudioSub.MyHelpDialog";
			Title = Message.GetString ("LapseStudio Help");
			Icon = global::Stetic.IconLoader.LoadIcon (this, "gtk-help", IconSize.Menu);
			WindowPosition = ((WindowPosition)(4));
			DefaultWidth = 450;
			DefaultHeight = 500;
			DestroyWithParent = true;
			// Internal child LapseStudioSub.MyHelpDialog.VBox
			VBox w1 = VBox;
			w1.Name = "dialog1_VBox";
			w1.BorderWidth = ((uint)(2));
			// Container child dialog1_VBox.Gtk.Box+BoxChild
			hbox1 = new HBox ();
			hbox1.Name = "hbox1";
			hbox1.Spacing = 6;
			// Container child hbox1.Gtk.Box+BoxChild
			TopicExpander = new Expander (null);
			TopicExpander.WidthRequest = 150;
			TopicExpander.CanFocus = true;
			TopicExpander.Name = "TopicExpander";
			GtkLabel1 = new Label ();
			GtkLabel1.Name = "GtkLabel1";
			GtkLabel1.LabelProp = Message.GetString ("GtkExpander");
			GtkLabel1.UseUnderline = true;
			TopicExpander.LabelWidget = GtkLabel1;
			hbox1.Add (TopicExpander);
			Box.BoxChild w2 = ((Box.BoxChild)(hbox1 [TopicExpander]));
			w2.Position = 0;
			w2.Expand = false;
			w2.Fill = false;
			// Container child hbox1.Gtk.Box+BoxChild
			GtkScrolledWindow = new ScrolledWindow ();
			GtkScrolledWindow.Name = "GtkScrolledWindow";
			GtkScrolledWindow.ShadowType = ((ShadowType)(1));
			// Container child GtkScrolledWindow.Gtk.Container+ContainerChild
			HelpTextView = new TextView ();
			HelpTextView.Buffer.Text = "No help here yet, sorry.\n\nContact me instead:\n\tbildstein.johannes@gmail.com";
			HelpTextView.CanFocus = true;
			HelpTextView.Name = "HelpTextView";
			HelpTextView.Editable = false;
			HelpTextView.CursorVisible = false;
			GtkScrolledWindow.Add (HelpTextView);
			hbox1.Add (GtkScrolledWindow);
			Box.BoxChild w4 = ((Box.BoxChild)(hbox1 [GtkScrolledWindow]));
			w4.Position = 1;
			w1.Add (hbox1);
			Box.BoxChild w5 = ((Box.BoxChild)(w1 [hbox1]));
			w5.Position = 0;
			// Internal child LapseStudioSub.MyHelpDialog.ActionArea
			HButtonBox w6 = ActionArea;
			w6.Name = "dialog1_ActionArea";
			w6.Spacing = 10;
			w6.BorderWidth = ((uint)(5));
			w6.LayoutStyle = ((ButtonBoxStyle)(4));
			// Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
			buttonCancel = new Button ();
			buttonCancel.CanDefault = true;
			buttonCancel.CanFocus = true;
			buttonCancel.Name = "buttonCancel";
			buttonCancel.UseStock = true;
			buttonCancel.UseUnderline = true;
			buttonCancel.Label = "gtk-ok";
			AddActionWidget (buttonCancel, -5);
			ButtonBox.ButtonBoxChild w7 = ((ButtonBox.ButtonBoxChild)(w6 [buttonCancel]));
			w7.Expand = false;
			w7.Fill = false;
			if ((Child != null)) {
				Child.ShowAll ();
			}
			Show ();
			buttonCancel.Clicked += OnButtonCancelClicked;
		}
	}
}
