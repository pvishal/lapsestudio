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
	[Register ("AppDelegate")]
	partial class AppDelegate
	{
		[Outlet]
		MonoMac.AppKit.NSMenuItem MenuAboutItem { get; set; }

		[Outlet]
		MonoMac.AppKit.NSMenuItem MenuCloseItem { get; set; }

		[Outlet]
		MonoMac.AppKit.NSMenuItem MenuHelpItem { get; set; }

		[Outlet]
		MonoMac.AppKit.NSMenuItem MenuNewItem { get; set; }

		[Outlet]
		MonoMac.AppKit.NSMenuItem MenuOpenItem { get; set; }

		[Outlet]
		MonoMac.AppKit.NSMenuItem MenuPreferencesItem { get; set; }

		[Outlet]
		MonoMac.AppKit.NSMenuItem MenuQuitItem { get; set; }

		[Outlet]
		MonoMac.AppKit.NSMenuItem MenuSaveAsItem { get; set; }

		[Outlet]
		MonoMac.AppKit.NSMenuItem MenuSaveItem { get; set; }

		[Action ("MenuAboutItem_Click:")]
		partial void MenuAboutItem_Click (MonoMac.Foundation.NSObject sender);

		[Action ("MenuCloseItem_Click:")]
		partial void MenuCloseItem_Click (MonoMac.Foundation.NSObject sender);

		[Action ("MenuHelpItem_Click:")]
		partial void MenuHelpItem_Click (MonoMac.Foundation.NSObject sender);

		[Action ("MenuNewItem_Click:")]
		partial void MenuNewItem_Click (MonoMac.Foundation.NSObject sender);

		[Action ("MenuOpenItem_Click:")]
		partial void MenuOpenItem_Click (MonoMac.Foundation.NSObject sender);

		[Action ("MenuPreferencesItem_Click:")]
		partial void MenuPreferencesItem_Click (MonoMac.Foundation.NSObject sender);

		[Action ("MenuQuitItem_Click:")]
		partial void MenuQuitItem_Click (MonoMac.Foundation.NSObject sender);

		[Action ("MenuSaveAsItem_Click:")]
		partial void MenuSaveAsItem_Click (MonoMac.Foundation.NSObject sender);

		[Action ("MenuSaveItem_Click:")]
		partial void MenuSaveItem_Click (MonoMac.Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (MenuAboutItem != null) {
				MenuAboutItem.Dispose ();
				MenuAboutItem = null;
			}

			if (MenuPreferencesItem != null) {
				MenuPreferencesItem.Dispose ();
				MenuPreferencesItem = null;
			}

			if (MenuNewItem != null) {
				MenuNewItem.Dispose ();
				MenuNewItem = null;
			}

			if (MenuOpenItem != null) {
				MenuOpenItem.Dispose ();
				MenuOpenItem = null;
			}

			if (MenuCloseItem != null) {
				MenuCloseItem.Dispose ();
				MenuCloseItem = null;
			}

			if (MenuSaveItem != null) {
				MenuSaveItem.Dispose ();
				MenuSaveItem = null;
			}

			if (MenuSaveAsItem != null) {
				MenuSaveAsItem.Dispose ();
				MenuSaveAsItem = null;
			}

			if (MenuHelpItem != null) {
				MenuHelpItem.Dispose ();
				MenuHelpItem = null;
			}

			if (MenuQuitItem != null) {
				MenuQuitItem.Dispose ();
				MenuQuitItem = null;
			}
		}
	}
}
