using AppKit;
using Foundation;
using System.IO;
using System.Diagnostics;
using System.Xml;
namespace UnitySolutionLauncher
{
	[Register("AppDelegate")]
	public partial class AppDelegate : NSApplicationDelegate
	{

		public AppDelegate()
		{
			
		}





		public override bool OpenFile(NSApplication sender, string filename)
		{
			var ret = FileResolver.OpenSourceFile(filename);
			NSApplication.SharedApplication.Terminate(this);
			return ret;
		}

		public override void DidFinishLaunching(NSNotification notification)
		{
			// Insert code here to initialize your application
		}

		public override void WillTerminate(NSNotification notification)
		{
			// Insert code here to tear down your application
		}
	}
}
