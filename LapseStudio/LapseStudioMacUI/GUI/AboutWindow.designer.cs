// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoMac.Foundation;
using System.CodeDom.Compiler;

namespace LapseStudioMacUI
{
	[Register ("AboutWindowController")]
	partial class AboutWindowController
	{
		[Outlet]
		MonoMac.AppKit.NSTextField AboutTextLabel { get; set; }

		[Outlet]
		MonoMac.AppKit.NSButton OkButton { get; set; }

		[Action ("OkButton_Click:")]
		partial void OkButton_Click (MonoMac.Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (AboutTextLabel != null) {
				AboutTextLabel.Dispose ();
				AboutTextLabel = null;
			}

			if (OkButton != null) {
				OkButton.Dispose ();
				OkButton = null;
			}
		}
	}

	[Register ("AboutWindow")]
	partial class AboutWindow
	{
		
		void ReleaseDesignerOutlets ()
		{
		}
	}
}
