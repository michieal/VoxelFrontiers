#region usings

using Godot;
using System;
using ApophisSoftware;

#endregion

public partial class MenuController : Control {
	[ExportGroup("Main Menu Properties")] [ExportCategory("Main Menu")] [Export]
	public Control MainMenu;

	[Export] public Control SettingsMenu;

	[Export] public Panel        MenuBackground;
	[Export] public Control      UpdateMessage;
	[Export] public AcceptDialog DownloadError;

	[Export] public SCC SourceControl;

	[ExportCategory("GameState vars")] [Export]
	public bool InGame;

	[Export] public bool InGameScreen;
	[Export] public bool InMainMenu = true;
	[Export] public bool InSettings;
	[Export] public bool InUpdate;

	[ExportCategory("Button Settings")] [Export]
	public Button btnSettings;

	[Export] public Button btnUpdate;
	[Export] public Button btnExit;
	[Export] public Button BtnExitSettings;

	[ExportCategory("Debugging")] [Export] public bool DEBUG;

	public override void _Ready() {
		Visible = true; // prep everything for initial game start.
		// set up the menus
		MainMenu.Visible = true;
		SettingsMenu.Visible = false;
		MenuBackground.Visible = true;

		InMainMenu = true;
		InSettings = false;
		InGame = false;


		btnUpdate.Pressed += SourceControl.DownloadGameSource;
		btnSettings.Pressed += BtnSettingsOnPressed;
		btnExit.Pressed += BtnExitOnPressed;
		BtnExitSettings.Pressed += BtnExitSettingsOnPressed;
		base._Ready();
	}

	private void BtnExitSettingsOnPressed() {
		AcceptEvent();
		if (DEBUG) Logging.Log("Exit Settings Button Pressed.");

		SourceControl.GatherAndSaveSettings();
		
		SettingsMenu.Visible = false;
		if (!InGame) {
			MainMenu.Visible = true;
			InMainMenu = true;
		}

		InSettings = false;
	}

	private void BtnSettingsOnPressed() {
		AcceptEvent();
		if (btnSettings.Visible == false || InMainMenu == false) return;
		if (InUpdate) return;

		InMainMenu = false;
		InSettings = true;

		if (DEBUG) Logging.Log("Settings Button Pressed.");

		if (MainMenu != null)
			MainMenu.Visible = false;

		if (SettingsMenu != null) SettingsMenu.Visible = true;
	}

	public override void _Input(InputEvent @event) {
		if (Visible == false) return;

		if (@event.IsActionPressed("ui_cancel")) { // this is mapped to the Esc key.
			AcceptEvent();                         // clear out the event so nothing else receives it. 
			if (InMainMenu) {
				BtnExitOnPressed(); // do the exit button pressed.
			} else if (InSettings) {
				BtnExitSettingsOnPressed();
			} else if (InGame) {
				// show settings
				BtnSettingsOnPressed();
			} else if (InGameScreen) {
				// close game screen
			}
		}

		base._Input(@event);
	}

	private void BtnExitOnPressed() {
		if (Visible == false) return; //do nothing.
		if (InUpdate) return;

		if (DEBUG) Logging.Log("Exit Button Pressed.");

		System.Environment.Exit(0);
	}

	internal void ShowUpdateNotice() {
		btnUpdate.Disabled = true;
		UpdateMessage.Visible = true;
		InUpdate = true;
	}

	internal void HideUpdateNotice() {
		btnUpdate.Disabled = false;
		UpdateMessage.Visible = false;
		InUpdate = false;
	}

	internal void ShowDownloadError(string message) {
		DownloadError.DialogText = message;
		DownloadError.Show();
	}
}