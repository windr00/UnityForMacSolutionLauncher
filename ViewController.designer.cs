// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace UnitySolutionLauncher
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		AppKit.NSButton btnInUnity { get; set; }

		[Outlet]
		AppKit.NSButton btnInVS { get; set; }

		[Outlet]
		AppKit.NSTextField folderText { get; set; }

		[Outlet]
		AppKit.NSButton launchButton { get; set; }

		[Outlet]
		AppKit.NSButton openFolderButton { get; set; }

		[Action ("btnBrowseClick:")]
		partial void btnBrowseClick (Foundation.NSObject sender);

		[Action ("btnInUnityClick:")]
		partial void btnInUnityClick (Foundation.NSObject sender);

		[Action ("btnInVSClick:")]
		partial void btnInVSClick (Foundation.NSObject sender);

		[Action ("btnLaunchClick:")]
		partial void btnLaunchClick (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (openFolderButton != null) {
				openFolderButton.Dispose ();
				openFolderButton = null;
			}

			if (folderText != null) {
				folderText.Dispose ();
				folderText = null;
			}

			if (btnInUnity != null) {
				btnInUnity.Dispose ();
				btnInUnity = null;
			}

			if (btnInVS != null) {
				btnInVS.Dispose ();
				btnInVS = null;
			}

			if (launchButton != null) {
				launchButton.Dispose ();
				launchButton = null;
			}
		}
	}
}
