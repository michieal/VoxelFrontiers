#region

using Godot;
using System;
using System.Collections;

#endregion

namespace ApophisSoftware; 

public class Coroutine {

	/// <summary>
	/// If assigned a function, this function will be called when the Coroutine stops.
	/// Note: it does not distinguish between completed and stopped prematurely.
	/// </summary>
	public Action onComplete;
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
		onComplete?.Invoke();
	}
}