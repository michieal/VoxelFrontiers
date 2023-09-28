#region

using System;
using System.Collections.Generic;
using System.IO;
using Godot;
using Environment = System.Environment;
using FileAccess = Godot.FileAccess;

#endregion


namespace ApophisSoftware; 

public class Utils {
	/*public static Texture2D LoadImageToTexture2D(string filename) {
		Texture2D RetImage = new Texture2D(1, 1);

		if (File.Exists(filename) == false)
			return null;

		byte[] imagedata = File.ReadAllBytes(filename);

		RetImage.LoadImage(imagedata);

		return RetImage;
	}*/

	internal static bool ANDROID   { get; set; }
	internal static bool WEBDEPLOY { get; set; }

	public static T ConvertTo<T>(object source) {
		return (T) Convert.ChangeType(source, typeof(T));
	}

	public static string GetStoragePath() {
		string PathString = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
		if (!ANDROID && !WEBDEPLOY) {
			PathString += "/.Apophis Software/mineclonepp";
			Directory.CreateDirectory(PathString); // make the path string, if it doesn't exist.
			return PathString;
		} else {
			// When not ran on a Desktop system, use the built in user data dir. Which, points to the data path for
			// the storage device. For Android, it's the app's storage; for WebGL, it's the section of the Browser
			// storage that the webgl app has access to.
			PathString = OS.GetUserDataDir();
			PathString += "/mineclonepp";
			Directory.CreateDirectory(PathString); // make the path string, if it doesn't exist.

			return PathString;
		}
	}

	public static string ProcessConfSettingFromFile(string filename, string setting) {
		StreamReader srStream = File.OpenText(filename);
		string value = "";

		while (!srStream.EndOfStream) {
			value = srStream.ReadLine().ToLower();
			if (value.StartsWith(setting)) { // if found, strip out the bs. should end up with a "mcl_" something.
				value = value.Replace(setting, ""); // "=\"{}#"
				value = value.Replace(" ", "");
				value = value.Replace("=", "");
				value = value.Replace("\"", "");
				value = value.Replace("#", "");

				break;
			}
		}

		return value;
	}

	public static Dictionary<string, Setting> ProcessSettingFromTextFile(string filename) {
		Dictionary<string, Setting> settings = new Dictionary<string, Setting>();
		Setting setting = new Setting();
		StreamReader srStream = File.OpenText(filename);
		string value = "";

		string SettingsCategory = "General";

		while (!srStream.EndOfStream) {
			setting.SettingsFile = filename;
			value = srStream.ReadLine().ToLower().Trim();

			if (value == "") // handle a new setting.
				if (settings.ContainsKey(setting.SettingName) == false) {
					// prevent adding 2 of the same settings. Also, no blank settings.
					settings.Add(setting.SettingName, setting); // Add to existing setting. 

					if (settings.ContainsKey("")) settings.Remove("");

					setting = new Setting();
				}

			if (value.StartsWith("[")) { // Handle Category Specifier.
				setting.SettingsHeader = value.Remove(0, 1).Replace("]", "");
				value = ""; // A nothing value.
				SettingsCategory = setting.SettingsHeader;
			} else {
				setting.SettingsHeader = SettingsCategory;
			}

			if (value.StartsWith("#")) {    // handle Description
				value = value.Remove(0, 1); // Remove the first char (#) from the string.
				value = value.Trim();
				value = value.Replace("Minetest", "[old engine]");
				value = value.Replace("minetest", "crap");
				if (value != "")                          // don't put in blank lines. 
					setting.Description.Add(value + " "); // add the new line to the string description.
			} else if (value.StartsWith("#") == false && value != "") {
				// here is the bulk of the setting...
				char[] splitter = {'(', ')'};
				string[] setstr = value.Split(splitter);
				string[] setstrData = {""};
				try {
					setstrData = setstr[2].Trim().Split(' ');
				} catch (Exception e) {
					Logging.Log("error", "Malformed Setting.\n\r" + e);
					return null;
				}

				// Example Format: "tsm_railcorridors_probability_chest (Chest probability) float 0.05 0.0 1.0"
				// extract the setting name.
				setting.SettingName = setstr[0].Trim(); // first part of the line.
				Logging.Log("Processing Setting: " + setting.SettingName);
				setting.DisplayName = setstr[1].Trim(); // second part of the line.
				setting.SettingType = setstrData[0].Trim();

				// Defining Defaults:
				// mcl_weather_rain_particles (Rain particles) int 500 0
				// the 500 is the Default value, 0 is the minimum value.

				switch (setting.SettingType.ToLower()) {
					case "enum":
						Logging.Log(value);
						//[INFO]: mcl_node_particles (block particles detail level) enum none high,medium,low,none
						setting.DefaultValue = ConvertTo<string>(setstrData[1]) as object;
						setting.EnumerationValues = setstrData[2].Trim().Split(',');
						break;
					case "string":
						break;
					case "bool":
						setting.DefaultValue = ConvertTo<bool>(setstrData[1].Trim()) as object;
						break;
					case "int":
						setting.DefaultValue = ConvertTo<int>(setstrData[1].Trim()) as object;
						// check for, and handle min/max values.
						if (setstrData.Length > 2) setting.minValue = ConvertTo<int>(setstrData[2].Trim()) as object;

						if (setstrData.Length > 3) setting.maxValue = ConvertTo<int>(setstrData[3].Trim()) as object;

						break;
					case "float":
						setting.DefaultValue = ConvertTo<float>(setstrData[1].Trim()) as object;
						// check for, and handle min/max values.
						if (setstrData.Length > 2) setting.minValue = ConvertTo<float>(setstrData[2].Trim()) as object;

						if (setstrData.Length > 3) setting.maxValue = ConvertTo<float>(setstrData[3].Trim()) as object;

						break;
					default:
						Logging.Log("error",
							"Unknown Setting Type DefaultValue in Settings File: " + setting.SettingsFile +
							"; Setting:" +
							setting.SettingName + "\n " + value);
						break;
				} // switch

				setting.CurrentValue = setting.DefaultValue;
			} // if
		}     // while.

		// Handle the last (or in some cases the ONLY) setting.
		if (settings.ContainsKey(setting.SettingName) == false) {
			// prevent adding 2 of the same settings. Also, no blank settings.

			settings.Add(setting.SettingName, setting); // Add to existing setting. 

			if (settings.ContainsKey("")) settings.Remove("");
		}

		return settings; // return the defined settings from this file.
	}                    // function.


	public Utils() {
		// Have a basic Ctor.
	}
} // class Utils.
// Namespace.