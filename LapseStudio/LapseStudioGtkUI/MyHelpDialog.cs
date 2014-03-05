using System;

namespace LapseStudioGtkUI
{
	public partial class MyHelpDialog : Gtk.Dialog
	{
		public MyHelpDialog()
		{
			this.Build();
		}

		protected void OnButtonOKClicked(object sender, EventArgs e)
		{
			this.Hide();
		}
	}
}

