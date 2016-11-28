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


		private readonly string PATTERN = "Assembly-CSharp";
		private readonly string OPEN_BIN = "/usr/bin/open";
		private readonly string VS_PATH = "\"/Applications/Visual Studio.app\"";

		public AppDelegate()
		{
		}


		private void CallVisualStudio(string[] slnFilePath)
		{
			Process shell = new Process();
			shell.StartInfo.FileName = OPEN_BIN;
			shell.StartInfo.Arguments = VS_PATH + " -n --args " + slnFilePath[0] + " " + slnFilePath[1];
			shell.StartInfo.CreateNoWindow = true;
			shell.Start();
			NSApplication.SharedApplication.Terminate(this);
		}


		public override bool OpenFile(NSApplication sender, string filename)
		{
			string path = filename;
			if (!path.Equals(string.Empty))
			{
				int index = path.LastIndexOf('/');
				path = path.Substring(0, index);
				DirectoryInfo dir = new DirectoryInfo(path);
				FileInfo slnFile = null;
				FileInfo prefsFile = null;
				while (dir != null)
				{
					FileInfo[] files = dir.GetFiles();
					foreach (var f in files)
					{
						if (f.Name.Trim().Contains(PATTERN.Trim()))
						{
							if (f.Extension.Trim().Equals(".sln"))
							{
								slnFile = f;
							}
							else if (f.Extension.Trim().Equals(".userprefs"))
							{
								prefsFile = f;
							}
						}

					}
					if (slnFile != null)
					{
						break;
					}
					dir = dir.Parent;
				}
				if (dir == null)
				{
					NSApplication.SharedApplication.Terminate(this);
					return false;
				}
				if (prefsFile == null)
				{
					CallVisualStudio(new[] { filename, slnFile.FullName });
				}
				else
				{
					string workdir = slnFile.DirectoryName;
					FileInfo sourceFile = new FileInfo(filename);
					string sourceRelativePath = sourceFile.FullName.Substring(workdir.Length);
					if (sourceRelativePath.StartsWith("/"))
					{
						sourceRelativePath = sourceRelativePath.Substring(1);
					}
					XmlDocument prefXml = new XmlDocument();
					prefXml.Load(prefsFile.FullName);
					XmlNode root = prefXml.SelectSingleNode("Properties");
					XmlElement workbench = (XmlElement)root.SelectSingleNode("MonoDevelop.Ide.Workbench");
					if (workbench != null && workbench.HasChildNodes)
					{
						if (!workbench.GetAttribute("ActiveDocument").Equals(sourceRelativePath))
						{
							workbench.SetAttribute("ActiveDocument", sourceRelativePath);
							XmlNodeList fileList = workbench.SelectSingleNode("Files").SelectNodes("File");
							foreach (XmlElement n in fileList)
							{
								if (n.GetAttribute("FileName").Equals(sourceRelativePath))
								{
									prefXml.Save(prefsFile.FullName);
									CallVisualStudio(new[] { slnFile.FullName, string.Empty });
									return true;
								}
							}

							XmlElement newFile = prefXml.CreateElement("File");
							newFile.SetAttribute("FileName", sourceRelativePath);
							newFile.SetAttribute("Line", "1");
							newFile.SetAttribute("Column", "1");
							workbench.SelectSingleNode("Files").AppendChild(newFile);
							prefXml.Save(prefsFile.FullName);
							CallVisualStudio(new string[] { slnFile.FullName, string.Empty });
							return true;
						}
						else
						{
							CallVisualStudio(new string[] { slnFile.FullName, string.Empty });
							return true;
						}
					}

				}
			}
			NSApplication.SharedApplication.Terminate(this);
			return false;
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
