using System;
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

		protected void OnButtonCancelClicked(object sender, EventArgs e)
		{
			this.HideAll();
		}
	}
}

