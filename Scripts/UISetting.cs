#region usings

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Godot;

#endregion

namespace ApophisSoftware;

public partial class UISetting : Node {
	[ExportGroup("UI Setting Properties")] [ExportCategory("Linkages")] [Export]
	public Label Description;

	[Export] public Label        SettingName;
	[Export] public Label        DefaultValue;
	[Export] public LineEdit     ValueInput;
	[Export] public OptionButton ValueDropdown;
	[Export] public CheckBox     ValueToggle;
	[Export] public Label        ValueRange;
	[Export] public bool         DEBUG = false;
	internal        Setting      ThisSetting;

	public UISetting() {
	}

	private string   settingType;
	private string[] values;
	private bool     togVal = false;
	private bool     UseRange;

	internal void InitializeSetting(Setting SettingDefault) {
		ThisSetting = SettingDefault;

		settingType = ThisSetting.SettingType;
		SettingName.Text = ThisSetting.DisplayName;

		Description.Text = ""; // Clear out the placeholder text, from designing the prefab.
		Description.TooltipText = "";
		foreach (string s in ThisSetting.Description) {
			Description.Text += s + "\n";
			Description.TooltipText += s + "\n";
		}

		// hide the edit area.
		ValueDropdown.Visible = false;
		ValueInput.Visible = false;
		ValueToggle.Visible = false;

		if (ThisSetting.minValue.Equals(ThisSetting.maxValue))
			UseRange = false;
		else
			UseRange = true;

		if (UseRange) {
			ValueRange.TooltipText += "Minimum Value = " + ThisSetting.minValue + "\n";
			ValueRange.TooltipText += "Maximum Value = " + ThisSetting.maxValue + "\n";
			ValueRange.Text = "Range: " + ThisSetting.minValue + " - " + ThisSetting.maxValue;
		}

		// set the current value in case we need to edit it.
		switch (ThisSetting.SettingType) {
			case "bool":
				if (ValueToggle != null)
					ValueToggle.Toggled += ValueToggleOnToggled;
				if ((bool) ThisSetting.CurrentValue) {
					togVal = true;
					ValueToggle.Text = "TRUE";
					ValueToggle.ButtonPressed = true;
				} else {
					togVal = false;
					ValueToggle.Text = "FALSE";
					ValueToggle.ButtonPressed = false;
				}

				break;
			case "enum":
				if (ValueDropdown == null) return;

				ValueDropdown.TooltipText = ThisSetting.DisplayName + "\n";
				ValueDropdown.TooltipText += "Default Value = " + ThisSetting.DefaultValue + "\n";
				ValueDropdown.TooltipText += "Options:\n";
				values = ThisSetting.EnumerationValues;
				foreach (string opts in values) {
					ValueDropdown.AddItem(opts.Trim());
					ValueDropdown.TooltipText += opts + "\n";
				}

				int index = 0;
				for (int i = 0; i < values.Length; i++)
					if (values[i] == ThisSetting.CurrentValue.ToString()) {
						index = i;
						break;
					}

				ValueDropdown.Selected = index;
				ValueDropdown.ItemSelected += ValueDropdownOnItemSelected;

				break;
			default:
				if (ValueInput != null) {
					ValueInput.Text = ThisSetting.CurrentValue.ToString();
					ValueInput.TextSubmitted += ValueInputOnTextSubmitted;
					ValueInput.SelectAllOnFocus = true;
					ValueInput.FocusExited += ValueInputOnFocusExit;
					ValueInput.TooltipText = ThisSetting.DisplayName + "\n";
					ValueInput.TooltipText = "Any Changes will be validated and saved automatically.\n";
					ValueInput.TooltipText += "Default Value = " + ThisSetting.DefaultValue + "\n";
					ValueInput.TooltipText += "Current Value = " + ThisSetting.CurrentValue + "\n";
					if (UseRange) {
						ValueInput.TooltipText += "Minimum Value = " + ThisSetting.minValue + "\n";
						ValueInput.TooltipText += "Maximum Value = " + ThisSetting.maxValue + "\n";
					}
				}

				break;
		}

		DefaultValue.Text = ThisSetting.DefaultValue.ToString();

		DisplaySetting();
	}

	private void ValueInputOnFocusExit() {
		ValueInputOnTextSubmitted(ValueInput.Text);
	}

	private void ValueInputOnTextSubmitted(string newtext) {
		ValueInput.TooltipText = "Any Changes will be validated and saved automatically.\n";
		ValueInput.TooltipText += "Default Value = " + ThisSetting.DefaultValue + "\n";
		ValueInput.TooltipText += "Current Value = " + ThisSetting.CurrentValue + "\n";

		if (UseRange) {
			ValueInput.TooltipText += "Minimum Value = " + ThisSetting.minValue + "\n";
			ValueInput.TooltipText += "Maximum Value = " + ThisSetting.maxValue + "\n";
			UseRange = true;
		}

		switch (ThisSetting.SettingType) {
			case "int":
				try {
					int val = newtext.ToInt();
					if (UseRange) {
						if (val <= (int) ThisSetting.maxValue && val >= (int) ThisSetting.minValue) {
							ThisSetting.CurrentValue = val;
						} else {
							ValueInput.Text = ThisSetting.CurrentValue.ToString();
						}
					} else {
						ThisSetting.CurrentValue = val;
					}
				} catch (Exception e) {
					ValueInput.Text = ThisSetting.CurrentValue.ToString();
				}

				break;
			case "float":
				try {
					float val = newtext.ToInt();
					if (UseRange) {
						if (val <= (float) ThisSetting.maxValue && val >= (float) ThisSetting.minValue) {
							ThisSetting.CurrentValue = val;
						} else {
							ValueInput.Text = ThisSetting.CurrentValue.ToString();
						}
					} else {
						ThisSetting.CurrentValue = val;
					}
				} catch (Exception e) {
					ValueInput.Text = ThisSetting.CurrentValue.ToString();
				}

				break;
			case "string":
				ThisSetting.CurrentValue = newtext.ToLower();
				break;
		}
	}

	private void ValueToggleOnToggled(bool buttonpressed) {
		togVal = buttonpressed;
		if (togVal)
			ValueToggle.Text = "TRUE";
		else
			ValueToggle.Text = "FALSE";
	}

	private void ValueDropdownOnItemSelected(long index) {
		ThisSetting.CurrentValue = values[index];
		if (DEBUG) Logging.Log("Dropdown Value changed to: " + ThisSetting.CurrentValue);
	}

	public void DisplaySetting() {
		// this is simple - hide the display value and show the editable version.

		if (UseRange) {
			ValueRange.Visible = true;
		} else {
			ValueRange.Visible = false;
		}

		switch (ThisSetting.SettingType) {
			case "bool":
				ValueToggle.Visible = true;
				break;
			case "enum":
				ValueDropdown.Visible = true;
				break;
			default:
				ValueInput.Visible = true;
				break;
		}
	}

	internal void Remove() {
		Free();
	}
}