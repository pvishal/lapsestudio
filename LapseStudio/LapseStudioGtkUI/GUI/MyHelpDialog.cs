using System;

namespace LapseStudioGtkUI
{
	public partial class MyHelpDialog : Gtk.Dialog
	{
		public MyHelpDialog()
		{
			this.Build();
		}

		protected void OnButtonCancelClicked(object sender, EventArgs e)
		{
			this.HideAll();
		}
	}
}

