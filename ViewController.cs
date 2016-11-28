using System;

using AppKit;
using Foundation;

namespace UnitySolutionLauncher
{
	public partial class ViewController : NSViewController
	{

		public bool inVS = true;

		public bool inUnity = true;

		public string fullPath = string.Empty;

		partial void btnBrowseClick(NSObject sender)
		{
			NSOpenPanel panel = NSOpenPanel.OpenPanel;

			panel.Title = "Choose Unity Project Root Folder";

			panel.Message = "";

			panel.Prompt = "OK";

			panel.CanChooseFiles = false;

			panel.CanChooseDirectories = true;

			panel.CanCreateDirectories = false;

			panel.AllowsMultipleSelection = false;

			nint res = panel.RunModal("~/","");

			if (res == 1)
			{
				fullPath = panel.Url.AbsoluteString.Substring(7);
				folderText.StringValue = fullPath;
			}


		}

		partial void btnInUnityClick(NSObject sender)
		{
			inUnity = btnInUnity.Cell.State == NSCellStateValue.On;
		}

		partial void btnInVSClick(NSObject sender)
		{
			inVS = btnInVS.Cell.State == NSCellStateValue.On;
		}

		partial void btnLaunchClick(NSObject sender)
		{
			FileResolver.OpenProjectFolder(fullPath, inUnity, inVS);
		}

		public ViewController(IntPtr handle) : base(handle)
		{
			
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			// Do any additional setup after loading the view.
		}

		public override NSObject RepresentedObject
		{
			get
			{
				return base.RepresentedObject;
			}
			set
			{
				base.RepresentedObject = value;
				// Update the view, if already loaded.
			}
		}
	}
}
