using Godot;
using System;
using System.IO;
using ApophisSoftware;

namespace ApophisSoftware {

	public partial class LogDisplayerNode : Control {
		[Export] public Button         btnCloseLogView;
		[Export] public CodeEdit       LogViewDisplay;
		[Export] public MenuController MainMenuController;



		// Called when the node enters the scene tree for the first time.
		public override void _Ready() {
			this.VisibilityChanged += OnVisibilityChanged;
			btnCloseLogView.Pressed += BtnCloseLogViewOnPressed;
		}

		private void BtnCloseLogViewOnPressed() {
			LogViewDisplay.Text = "";
			MainMenuController.BtnExitLogDisplayerOnPressed();
		}

		private void OnVisibilityChanged() {

			if (!this.Visible) {
				LogViewDisplay.Text = "";
				return;
			}

			string LogFile = Utils.GetStoragePath() + "/debug.txt";
			if (!File.Exists(LogFile)) {
				LogViewDisplay.Text = "Logfile Not Found.";
				return;
			}
			StreamReader srLogFile = new StreamReader(LogFile);
			LogViewDisplay.Text = srLogFile.ReadToEnd();
			srLogFile.Close();

		}
	}
}