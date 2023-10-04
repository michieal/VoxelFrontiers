#region

using System;
using System.Collections.Generic;
using System.IO;
using Godot;
using Environment = System.Environment;
using FileAccess = Godot.FileAccess;
using System.Globalization;

#endregion


namespace ApophisSoftware {
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

		// --------------------------------------------
		// Localization dictionaries.		
		// format: key = English, Value = Translated.
		private static Dictionary<string, string> EngineLangDE = new Dictionary<string, string>();
		private static Dictionary<string, string> EngineLangFR = new Dictionary<string, string>();
		private static Dictionary<string, string> EngineLangJA = new Dictionary<string, string>();
		private static Dictionary<string, string> EngineLangES = new Dictionary<string, string>();
		private static Dictionary<string, string> GameLangDE   = new Dictionary<string, string>();
		private static Dictionary<string, string> GameLangFR   = new Dictionary<string, string>();
		private static Dictionary<string, string> GameLangJA   = new Dictionary<string, string>();
		private static Dictionary<string, string> GameLangES   = new Dictionary<string, string>();

		// --------------------------------------------

		public static T ConvertTo<T>(object source) {
			return (T) Convert.ChangeType(source, typeof(T));
		}

		public static string GetStoragePath() {
			string PathString = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
			if (!ANDROID && !WEBDEPLOY) {
				PathString += "/.Apophis Software/.mineclonepp";
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

		public static void SaveSettingsToFile(string settingsConf, Dictionary<string, object> SettingsDict) {
			StreamWriter swWriter = new StreamWriter(settingsConf);
			string data;

			foreach (KeyValuePair<string, object> setting in SettingsDict) {
				data = setting.Key + "=" + setting.Value.ToString();
				swWriter.WriteLine(data.Trim());
			}

			swWriter.Flush();
			swWriter.Close();
			GC.Collect();
		}

		public static Dictionary<string, object> ProcessSavedSettingsFile(string ConfFilename) {
			if (File.Exists(ConfFilename) == false) {
				Logging.Log("warning", "Tried to get settings from a non-existent file:\n" + ConfFilename);

				return null; // not there? exit.
			}

			Dictionary<string, object> settings = new Dictionary<string, object>();

			StreamReader srReader = new StreamReader(ConfFilename);
			string data;
			string[] setdata;
			while (srReader.EndOfStream == false) {
				data = srReader.ReadLine();
				if (data.Contains('=') && data.StartsWith("#") == false) {
					setdata = data.Split("=");
					if (!settings.ContainsKey(setdata[0].ToLower().Trim())) {
						settings.Add(setdata[0].ToLower().Trim(), setdata[1].ToLower().Trim());
					}
				}
			}

			srReader.Close();

			return settings;
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
							if (setstrData.Length > 2)
								setting.minValue = ConvertTo<int>(setstrData[2].Trim()) as object;

							if (setstrData.Length > 3)
								setting.maxValue = ConvertTo<int>(setstrData[3].Trim()) as object;

							break;
						case "float":
							setting.DefaultValue = ConvertTo<float>(setstrData[1].Trim()) as object;
							// check for, and handle min/max values.
							if (setstrData.Length > 2)
								setting.minValue = ConvertTo<float>(setstrData[2].Trim()) as object;

							if (setstrData.Length > 3)
								setting.maxValue = ConvertTo<float>(setstrData[3].Trim()) as object;

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

		static Utils() {
			GetLocalization(); // initialize the language.
		}

		internal static string GetLocalization() {
			// Get the current culture
			CultureInfo currentCulture = CultureInfo.CurrentCulture;

			// Get the current language code (two-letter ISO language name)
			string languageCode = currentCulture.TwoLetterISOLanguageName;

			switch (languageCode.ToUpper()) {
				case "EN":
					CurrentLocale = LanguageCodes.en;
					break;
				case "ES":
					CurrentLocale = LanguageCodes.es;
					LoadLocalizationFile("res://locale/locale.es.tr", LanguageCodes.en);
					break;
				case "FR":
					CurrentLocale = LanguageCodes.fr;
					break;
				case "DE":
					CurrentLocale = LanguageCodes.de;
					break;
				case "JA":
					CurrentLocale = LanguageCodes.ja;
					break;

				
			}
			
			return languageCode;
		}

		public static LanguageCodes CurrentLocale = LanguageCodes.en;

		public static string S(string strDisplayString) {
			return LocalizeString(strDisplayString, CurrentLocale);
		}
		
		public static string GS(string strDisplayString) {
			return LocalizeString(strDisplayString, CurrentLocale, true);
		}
		
		public static string LocalizeString(string DisplayString, LanguageCodes Lang, bool ForGame = false) {
			// This switch block uses fall-through logic. 
			switch (Lang) {
				case LanguageCodes.de:
					if (ForGame) {
						if (GameLangDE.ContainsKey(DisplayString) ) {
							return GameLangDE[DisplayString];
						} 
					} else {
						if (EngineLangDE.ContainsKey(DisplayString)) {
							return EngineLangDE[DisplayString];
						} 
					}
					break;
				case LanguageCodes.fr:
					if (ForGame) {
						if (GameLangFR.ContainsKey(DisplayString) ) {
							return GameLangFR[DisplayString];
						} 
					} else {
						if (EngineLangFR.ContainsKey(DisplayString)) {
							return EngineLangFR[DisplayString];
						} 
					}
					break;
				case LanguageCodes.es:
					if (ForGame) {
						if (GameLangES.ContainsKey(DisplayString) ) {
							return GameLangES[DisplayString];
						}
					} else {
						if (EngineLangES.ContainsKey(DisplayString)) {
							return EngineLangES[DisplayString];
						}
					}
					break;
				case LanguageCodes.ja:
					if (ForGame) {
						if (GameLangJA.ContainsKey(DisplayString) ) {
							return GameLangJA[DisplayString];
						}
					} else {
						if (EngineLangJA.ContainsKey(DisplayString)) {
							return EngineLangJA[DisplayString];
						} 
					}
					break;

				default:
					break;

			}
			return DisplayString;
		}

		public static void LoadLocalizationFile(string Filename, LanguageCodes LangCode, bool ForGame = false) {
			if (File.Exists(Filename) == false)
				return;
			
			StreamReader srTRFile = new StreamReader(Filename);

			string[] data;
			string temp;
			while (srTRFile.EndOfStream == false) {
				temp = srTRFile.ReadLine();
				if (temp.StartsWith("#"))
					continue; // skip lines with # as the starting character. aka, skip remarks.
				
				data = temp.Split("=");

				switch (LangCode) {
					case LanguageCodes.de:
						if (ForGame) {
							if (GameLangDE.ContainsKey(data[0].Trim()) == false) {
								GameLangDE.Add(data[0].Trim(), data[1].Trim());
							}
						} else {
							if (EngineLangDE.ContainsKey(data[0].Trim()) == false) {
								EngineLangDE.Add(data[0].Trim(), data[1].Trim());
							}
						}

						break;
					case LanguageCodes.es:
						if (ForGame) {
							if (GameLangES.ContainsKey(data[0].Trim()) == false) {
								GameLangES.Add(data[0].Trim(), data[1].Trim());
							}
						} else {
							if (EngineLangES.ContainsKey(data[0].Trim()) == false) {
								EngineLangES.Add(data[0].Trim(), data[1].Trim());
							}
						}

						break;
					case LanguageCodes.fr:
						if (ForGame) {
							if (GameLangFR.ContainsKey(data[0].Trim()) == false) {
								GameLangFR.Add(data[0].Trim(), data[1].Trim());
							}
						} else {
							if (EngineLangFR.ContainsKey(data[0].Trim()) == false) {
								EngineLangFR.Add(data[0].Trim(), data[1].Trim());
							}
						}

						break;
					case LanguageCodes.ja:
						if (ForGame) {
							if (GameLangJA.ContainsKey(data[0].Trim()) == false) {
								GameLangJA.Add(data[0].Trim(), data[1].Trim());
							}
						} else {
							if (EngineLangJA.ContainsKey(data[0].Trim()) == false) {
								EngineLangJA.Add(data[0].Trim(), data[1].Trim());
							}
						}

						break;
					default:
						Logging.Log("error", "Unknown Language file");
						break;
				}
			}

			srTRFile.Close();
		}
	} // class Utils.

	public enum LanguageCodes {
		en = 0, // project default language.
		de = 1,
		fr = 2,
		ja = 3,
		es = 4,
	}
} // Namespace.