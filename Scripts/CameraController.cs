using Godot;
using System;

namespace ApophisSoftware {
	public enum ViewCamera {
		CameraStateNormal = 0,
		CameraStateFront  = 1,
		CameraStateBehind = 2,
	}

	public partial class CameraController : Node3D {
		[ExportGroup("Character Camera Properties")] [ExportCategory("Camera Controller Properties")] [Export]
		public Camera3D FirstPersonCamera;

		[Export] public Camera3D ThirdPersonCameraFront;
		[Export] public Camera3D ThirdPersonCameraBehind;

		[Export] public ViewCamera CurrentState;


		//-----------------------------------------------------------------------
		// Private static instance to hold the singleton instance
		private static CameraController _instance;

		// Public property to access the singleton instance
		public static CameraController Instance {
			get {
				if (_instance == null) {
					_instance = new CameraController();
				}

				return _instance;
			}
		}

		// Private constructor to prevent external instantiation
		private CameraController() {
		}

		// Called when the node enters the scene tree for the first time.
		public override void _Ready() {
			FirstPersonCamera.Visible = true;
			ThirdPersonCameraBehind.Visible = false;
			ThirdPersonCameraFront.Visible = false;
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta) {
		}

		internal void ChangeCurrentCamera(ViewCamera NewState) {
			switch (NewState) {
				case ViewCamera.CameraStateNormal:
					FirstPersonCamera.Visible = true;
					ThirdPersonCameraBehind.Visible = false;
					ThirdPersonCameraFront.Visible = false;
					break;

				case ViewCamera.CameraStateFront:
					FirstPersonCamera.Visible = false;
					ThirdPersonCameraBehind.Visible = false;
					ThirdPersonCameraFront.Visible = true;
					break;

				case ViewCamera.CameraStateBehind:
					FirstPersonCamera.Visible = false;
					ThirdPersonCameraBehind.Visible = true;
					ThirdPersonCameraFront.Visible = false;
					break;

				default:
					FirstPersonCamera.Visible = true;
					ThirdPersonCameraBehind.Visible = false;
					ThirdPersonCameraFront.Visible = false;
					break;
			}

			CurrentState = NewState;
		}

		internal void ChangeCurrentCamera() {
			switch (CurrentState) {
				case ViewCamera.CameraStateNormal:
					FirstPersonCamera.Visible = true;
					ChangeCurrentCamera(ViewCamera.CameraStateBehind);
					CurrentState = ViewCamera.CameraStateBehind;
					break;

				case ViewCamera.CameraStateFront:
					ChangeCurrentCamera(ViewCamera.CameraStateNormal);
					CurrentState = ViewCamera.CameraStateNormal;
					break;

				case ViewCamera.CameraStateBehind:
					ChangeCurrentCamera(ViewCamera.CameraStateFront);
					CurrentState = ViewCamera.CameraStateFront;
					break;

				default:
					ChangeCurrentCamera(ViewCamera.CameraStateNormal);
					break;
			}
		}
	}
}