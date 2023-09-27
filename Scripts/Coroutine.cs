#region

using Godot;
using System;
using System.Collections;

#endregion

namespace ApophisSoftware; 

public class Coroutine {
	private Action onComplete;
	private bool   isRunning = false;

	public event Action OnCleanup;

	public bool IsRunning => isRunning;

	internal IEnumerator Enumerator;

	public Coroutine(IEnumerator enumerator, Action onCompleteCallback = null) {
		Enumerator = enumerator;
		OnCleanup = onCompleteCallback;
	}

	internal void Start() {
		isRunning = true;
	}

	public void InvokeCleanup() {
		isRunning = false;
		OnCleanup?.Invoke();
	}

	// Called from the caller code, not the manager code, to stop the coroutine. Extra, and probably over-engineering.
	public void Stop() {
		isRunning = false;
	}
}