using AppKit;
using System;
using System.IO;
using System.Diagnostics;
namespace UnitySolutionLauncher
{
	static class MainClass
	{

		static void Main(string[] args)
		{
			NSApplication.Init();
			NSApplication.Main(args);
		}
	}
}
