#region

using System;
using System.Collections.Generic;
using System.Text;

#endregion

namespace ApophisSoftware;

public class Setting {
	internal List<string> Description;  // A text Description of the Setting, usually shown to the user.
	internal string       SettingName;  // The Name of the setting. Used as the main Identifier.
	internal object       DefaultValue; // The setting's Default Value. Actual data type determined by the SettingType.
	internal object       CurrentValue; // The setting's Actual Value.
	internal string       SettingType;  // type of Setting. IE, Int, Float, String, Bool, etc. 
	internal string       DisplayName;  // The visible name of the setting shown to user, contained in ()'s.

	internal object
		minValue; // Min value for the range. Set for a range value. ie, 0.0 to 1.0 (float) or 1 to 100 for int, etc. 

	internal object   maxValue;          // Max value of the range.
	internal string   SettingsFile;      // the file location that stores the setting.
	internal string   SettingsHeader;    // Defines the settings header (Category) for the setting.
	internal string[] EnumerationValues; // Holds the Enumeration values from the Enum type in settings.

	// Define a constructor to initialize the properties
	public Setting() {
		// Initialize properties here if needed
		Description = new List<string>();
		DefaultValue = (object) 0;
		SettingName = "";
		SettingType = "";
		minValue = (object) 0;
		maxValue = (object) 0;
		EnumerationValues = new[] {""};
	}

	public Setting(string settingName, object Val, string settingType) {
		SettingName = settingName;
		DefaultValue = Val;
		CurrentValue = Val;
		SettingType = settingType;
		EnumerationValues = new[] {""};
	}

	public Setting(string[] description, string settingName) {
		Description.Clear();
		Description.AddRange(description);
		SettingName = settingName;
	}

	// Define a strongly typed GetValue() function
	public object GetMinValue() {
		try {
			// Determine the type based on the SettingType property
			Type targetType;
			switch (SettingType.ToLower()) {
				case "int":
					targetType = typeof(int);
					break;
				case "float":
					targetType = typeof(float);
					break;
				// Add more cases for other supported types
				default:
					throw new InvalidOperationException("Unsupported setting type: " + SettingType);
			}

			// Perform type conversion
			return Convert.ChangeType(minValue, targetType);
		} catch (InvalidCastException ex) {
			// Handle type conversion errors here
			throw new InvalidCastException("Error converting setting value.", ex);
		}
	}

	public object GetMaxValue() {
		try {
			// Determine the type based on the SettingType property
			Type targetType;
			switch (SettingType.ToLower()) {
				case "int":
					targetType = typeof(int);
					break;
				case "float":
					targetType = typeof(float);
					break;
				// Add more cases for other supported types
				default:
					throw new InvalidOperationException("Unsupported setting type: " + SettingType);
			}

			// Perform type conversion
			return Convert.ChangeType(maxValue, targetType);
		} catch (InvalidCastException ex) {
			// Handle type conversion errors here
			throw new InvalidCastException("Error converting setting value.", ex);
		}
	}

	public object GetValue() {
		try {
			// Determine the type based on the SettingType property
			Type targetType;
			switch (SettingType.ToLower()) {
				case "int":
					targetType = typeof(int);
					break;
				case "float":
					targetType = typeof(float);
					break;
				case "bool":
					targetType = typeof(bool);
					break;
				case "string":
					targetType = typeof(string);
					break;
				// Add more cases for other supported types
				default:
					throw new InvalidOperationException("Unsupported setting type: " + SettingType);
			}

			// Perform type conversion
			return Convert.ChangeType(DefaultValue, targetType);
		} catch (InvalidCastException ex) {
			// Handle type conversion errors here
			throw new InvalidCastException("Error converting setting value.", ex);
		}
	}

	public void SetValue(object Val, string ValType) {
		DefaultValue = Val;
		SettingType = ValType;
	}

	public void SetMinMax(object minVal, object maxVal) {
		minValue = minVal;
		maxValue = maxVal;
	}

	public override string ToString() {
		StringBuilder str = new StringBuilder();
		str.Clear();
		str.AppendLine("Filename for setting: " + SettingsFile);
		str.AppendLine("Category: " + SettingsHeader);
		str.AppendLine("Setting Identifier: " + SettingName);
		str.AppendLine("Displayed Name: " + DisplayName);
		for (int i = 0; i < Description.Count; i++)
			str.AppendLine(Description[i]);
		str.AppendLine("Default Value: " + DefaultValue.ToString());
		str.AppendLine("Current Value: " + CurrentValue.ToString());
		str.AppendLine("Setting Data Type: " + SettingType);
		str.AppendLine(minValue.ToString());
		str.AppendLine(maxValue.ToString());

		return str.ToString();
	}
}