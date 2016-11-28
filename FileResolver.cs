using System;
using System.Xml;
using AppKit;
using Foundation;
using System.Diagnostics;
using System.IO;

namespace UnitySolutionLauncher
{
	public class FileResolver
	{




		private static readonly string PATTERN = "Assembly-CSharp";
		private static readonly string OPEN_BIN = "/usr/bin/open";
		private static readonly string VS_PATH = "\"/Applications/Visual Studio.app\"";
		private static readonly string UNITY_PATH = "\"/Applications/Unity/Unity.app\"";
		private static readonly string UNITY_ARGS = " -projectPath ";

		private static void CallUnity(string projectPath)
		{
			Process shell = new Process();
			shell.StartInfo.FileName = OPEN_BIN;
			shell.StartInfo.Arguments = UNITY_PATH + " -n --args " + UNITY_ARGS + projectPath;
			shell.StartInfo.CreateNoWindow = true;
			shell.Start();
		}


		private static void CallVisualStudio(string[] slnFilePath)
		{
			Process shell = new Process();
			shell.StartInfo.FileName = OPEN_BIN;
			shell.StartInfo.Arguments = VS_PATH + " -n --args " + slnFilePath[0] + " " + slnFilePath[1];
			shell.StartInfo.CreateNoWindow = true;
			shell.Start();
		}

		public static bool OpenProjectFolder(string folderpath, bool isUnity, bool isVS)
		{
			DirectoryInfo dir = new DirectoryInfo(folderpath);

			FileInfo[] files = dir.GetFiles();
			FileInfo slnFile = null;
			foreach (var f in files)
			{
				if (f.Name.Trim().StartsWith(PATTERN.Trim()) && f.Extension.Trim().Equals(".sln"))
				{
					slnFile = f;
					break;
				}
			}
			if (slnFile != null)
			{
				if (isUnity)
				{
					CallUnity(folderpath);
				}
				if (isVS) 
				{ 
					CallVisualStudio(new[] { slnFile.FullName, "" }); 
				}
				return true;
			}

			return false;

		}

		public static bool OpenSourceFile(string filename)
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
			return false;
		}
	}
}
