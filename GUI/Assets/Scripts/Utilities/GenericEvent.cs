using System;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Wrapper for <see cref="UnityEvent"/> to enable better debugging.
/// Removing generic <see cref="T"/> by inheritance enables Unity to serialize it in the inspector.
/// </summary>
public class GenericEvent<T> : UnityEvent<T>
{
#if UNITY_EDITOR || DEVELOPMENT_BUILD

    /// <summary>Is a custom name for this event when logging it to the console.</summary>
	public string EventName = "";
    /// <summary> If true, <see cref="AddEventListener(UnityAction{T}, object)"/> will be logged to the console. </summary>
	public bool LogAddListener = false;
    /// <summary> If true, <see cref="RemoveEventListener(UnityAction{T}, object)"/> will be logged to the console. </summary>
    public bool LogRemoveListeners = false;
    /// <summary> If true, <see cref="InvokeEvent(T, object)"/> will be logged to the console. </summary>
    public bool LogInvokes = false;

#endif

    /// <summary> Makes sure that the given listener is only added once. </summary>
    public void AddSingleEventListener(UnityAction<T> listener, object listeneObject = null)
    {
        RemoveEventListener(listener);
        AddEventListener(listener);
    }

	public virtual void AddEventListener(UnityAction<T> listener, object listenerObject = null)
	{
#if UNITY_EDITOR || DEVELOPMENT_BUILD
		if (LogAddListener)
			DebugLog("Listener added to " + EventName, listenerObject, new System.Diagnostics.StackFrame(1, true));
#endif
		base.AddListener(listener);
	}

	public virtual void RemoveEventListener(UnityAction<T> listener, object listenerObject = null)
	{
#if UNITY_EDITOR || DEVELOPMENT_BUILD
		if (LogRemoveListeners)
			DebugLog("Listener removed from " + EventName, listenerObject, new System.Diagnostics.StackFrame(1, true));
#endif
		base.RemoveListener(listener);
	}

	public virtual void InvokeEvent(T eventArgs, object eventRaiser = null)
	{
#if UNITY_EDITOR || DEVELOPMENT_BUILD
		if (LogInvokes)
			DebugLog("Event " + EventName + " invoked.", eventRaiser, new System.Diagnostics.StackFrame(1, true));
#endif

		base.Invoke(eventArgs);
	}

#if UNITY_EDITOR || DEVELOPMENT_BUILD
	private void DebugLog(string msg, object listenerObject, System.Diagnostics.StackFrame stack)
	{
		var path = stack.GetFileName();
		var lastIndex = path.LastIndexOf(System.IO.Path.DirectorySeparatorChar);
		path = path.Substring(lastIndex + 1, path.Length - lastIndex - 1);

		int line = stack.GetFileLineNumber();

        string listenerTypeName = "";
		UnityEngine.Object unityContext = null;

		if (listenerObject != null)
		{
			unityContext = listenerObject as UnityEngine.Object;

			if (!unityContext)
				listenerTypeName = listenerObject.GetType().Name;
			else
				listenerTypeName = unityContext.name;
		}

		Debug.Log((msg + " from '" + listenerTypeName + "' at " + path + ", line: " + line), unityContext);
	}
#endif

	[Obsolete("Use AddEventListener instead!", true)]
	public new void AddListener(UnityAction<T> listener)
	{
		throw new NotImplementedException("Don't use this. See Obsolete!");
	}

	[Obsolete("Use RemoveEventListener instead!", true)]
	public new void RemoveListener(UnityAction<T> listener)
	{
		throw new NotImplementedException("Don't use this. See Obsolete!");
	}

	[Obsolete("Use InvokeEvent instead!", true)]
	public new void Invoke(T args)
	{
		throw new NotImplementedException("Don't use this. See Obsolete!");
	}

#if UNITY_EDITOR || DEVELOPMENT_BUILD

	public void SetDebugging(string eventName, bool logAddListeners, bool logRemoveListeners, bool logInvokes)
    {
        EventName = eventName;
        SetDebugging(logAddListeners, logRemoveListeners, logInvokes);
    }

    public void SetDebugging(bool logAddListeners, bool logRemoveListeners, bool logInvokes)
    {
        LogAddListener = logAddListeners;
        LogRemoveListeners = logRemoveListeners;
        LogInvokes = logInvokes;
    }

    public void EnableDebuggingAll(string eventName)
	{
		SetDebugging(eventName, true, true, true);
	}

    public void DisableDebuggingAll()
    {
        SetDebugging(false, false, false);
    }
#endif
}
