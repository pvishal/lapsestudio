using Timelapse_UI;

namespace LapseStudioGtkUI
{
	public partial class MyAboutDialog : Gtk.Dialog
	{
		public MyAboutDialog()
		{
			this.Build();
			textview1.Buffer.Text = GeneralValues.AbouText;
		}

		protected void OnButtonOKClicked(object sender, System.EventArgs e)
		{
			this.Hide();
		}
	}
}

