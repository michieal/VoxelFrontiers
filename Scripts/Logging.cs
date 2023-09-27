#region

using System;
using System.IO;
using Godot;

#endregion

namespace ApophisSoftware; 

public static class Logging {
	private static string       LogFile = "";
	private static StreamWriter swLogFile;

	private static void LogFileInit(string Log_File) {
		LogFile = Log_File;
		swLogFile = new StreamWriter(LogFile, true);
	}

	public static void Log(object message) {
		string timestamp = DateTime.Now.ToString("u");
		swLogFile.WriteLine(timestamp.ToString() + " - [INFO]: " + message);
		// swLogFile.WriteLine(); // put a blank line in there... 
		swLogFile.Flush();
		GD.Print(message);
	}

	public static void LogStartup(object message) {
		string timestamp = DateTime.Now.ToString("u");
		swLogFile.WriteLine("\n\r------------------------------");
		swLogFile.WriteLine(timestamp.ToString() + " - [SYSTEM]: " + message);
		swLogFile.WriteLine("------------------------------");
		swLogFile.Flush();
		GD.Print(message);
	}

	public static void Log(string loglevel, object message) {
		string timestamp = DateTime.Now.ToString("u");
		string loglevelpretty = "";

		switch (loglevel.ToLower()) {
			case "info":
				loglevelpretty = "[INFO]: ";
				break;
			case "verbose":
				loglevelpretty = "[VERBOSE]: ";
				break;
			case "action":
				loglevelpretty = "[ACTION]: ";
				break;
			case "warning":
				loglevelpretty = "[WARNING]: ";
				break;
			case "error":
				loglevelpretty = "[ERROR]: ";
				break;
			default:
				loglevelpretty = "[INFO]: ";
				break;
		}

		swLogFile.WriteLine(timestamp.ToString() + " - " + loglevelpretty + message);
		swLogFile.WriteLine(); // put a blank line in there... 
		swLogFile.Flush();
		GD.Print(message);
	}

	// CTOR
	static Logging() {
		if (LogFile != "")
			LogFileInit(LogFile);
		else
			LogFileInit(Utils.GetStoragePath() + "/debug.txt");
	}

	public static void CloseLogFile() {
		swLogFile?.Flush();
		swLogFile?.Dispose();
		swLogFile = null;
	}
}