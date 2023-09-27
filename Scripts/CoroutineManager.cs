#region

using Godot;
using System;
using System.Collections;
using System.Collections.Generic;

#endregion

namespace ApophisSoftware; 

public partial class CoroutineManager : Node {
	// Singleton instance
	private static CoroutineManager instance;
	public static  CoroutineManager Instance => instance;

	public override void _Ready() {
		if (instance == null)
			instance = this;
		else
			// Ensure there's only one instance
			QueueFree();
	}

	public override void _Process(double delta) {
		base._Process(delta);
		UpdateCoroutines(delta);
	}

	private List<Coroutine> activeCoroutines = new();

	public Coroutine StartCoroutine(IEnumerator coroutine, Action onCompleteCallback = null) {
		var customCoroutine = new Coroutine(coroutine, onCompleteCallback);
		customCoroutine.Start();
		activeCoroutines.Add(customCoroutine);
		return customCoroutine;
	}

	public void StopCoroutine(Coroutine customCoroutine) {
		if (activeCoroutines.Contains(customCoroutine)) {
			activeCoroutines.Remove(customCoroutine);
			customCoroutine.InvokeCleanup();
		}
	}

	public void StopAllCoroutines() {
		foreach (var customCoroutine in activeCoroutines) customCoroutine.InvokeCleanup();

		activeCoroutines.Clear();
	}

	public void UpdateCoroutines(double delta) {
		for (int i = activeCoroutines.Count - 1; i >= 0; i--) {
			var customCoroutine = activeCoroutines[i];
			if (customCoroutine.IsRunning) {
				if (customCoroutine.Enumerator.MoveNext()) {
					if (customCoroutine.Enumerator.Current is WaitForSeconds) {
						var waitForSeconds = (WaitForSeconds) customCoroutine.Enumerator.Current;
						if (waitForSeconds.Update(delta)) {
							// Do not remove the Coroutine; just continue processing other Coroutines
						}
					} else if (customCoroutine.Enumerator.Current is WaitOneFrame) {
						var waitOneFrame = (WaitOneFrame) customCoroutine.Enumerator.Current;
						if (waitOneFrame.Update()) {
							// Do not remove the Coroutine; just continue processing other Coroutines
						}
					}
				} else {
					// The Coroutine has completed, remove it
					activeCoroutines.RemoveAt(i);
					customCoroutine.InvokeCleanup(); // Invoke cleanup event, if needed
				}
			} else {
				// Coroutine was manually stopped, remove it
				activeCoroutines.RemoveAt(i);
				customCoroutine.InvokeCleanup(); // Invoke cleanup event, if needed
			}
		}
	}
}

//-----------------------------------------------------------------------------------------
public class WaitForSeconds {
	private float duration;
	private float startTime;

	private double CurrentTime = 0;

	public WaitForSeconds(float seconds) {
		duration = seconds;
		startTime = (float) CurrentTime; // Get current time in seconds
	}

	public bool Update(double delta) {
		CurrentTime += delta;
		float currentTime = (float) CurrentTime; // Get current time in seconds
		return currentTime - startTime >= duration;
	}
}

public class WaitOneFrame {
	private bool hasWaited = false;

	public bool Update() {
		if (!hasWaited) {
			hasWaited = true;
			return false; // Continue processing in the next frame
		} else {
			return true; // Waiting is done; continue processing
		}
	}
}