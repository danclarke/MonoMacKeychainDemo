// WARNING
//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoMac.Foundation;

namespace KeyChainDemo
{
	[Register ("MainWindowController")]
	partial class MainWindowController
	{
		[Outlet]
		MonoMac.AppKit.NSTextField UsernameTextField { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField PasswordTextField { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField SavePasswordLabel { get; set; }

		[Action ("SetPasswordClicked:")]
		partial void SetPasswordClicked (MonoMac.AppKit.NSButton sender);

		[Action ("FetchClicked:")]
		partial void FetchClicked (MonoMac.AppKit.NSButton sender);

		[Action ("DeleteClicked:")]
		partial void DeleteClicked (MonoMac.AppKit.NSButton sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (UsernameTextField != null) {
				UsernameTextField.Dispose ();
				UsernameTextField = null;
			}

			if (PasswordTextField != null) {
				PasswordTextField.Dispose ();
				PasswordTextField = null;
			}

			if (SavePasswordLabel != null) {
				SavePasswordLabel.Dispose ();
				SavePasswordLabel = null;
			}
		}
	}

	[Register ("MainWindow")]
	partial class MainWindow
	{
		
		void ReleaseDesignerOutlets ()
		{
		}
	}
}
