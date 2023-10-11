#region

using System;
using System.Collections;
using System.Collections.Generic;
using Godot;

#endregion

#region License / Copyright

/*
The MIT License (MIT)

Copyright © 2023 Michieal of Apophis Software.
Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the “Software”), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

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