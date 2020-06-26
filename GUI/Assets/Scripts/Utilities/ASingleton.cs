using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SingletonDestroyMode { GameObject, Script, Custom }

public abstract class ASingleton<T> : MonoBehaviour where T : Behaviour
{
	public static T Instance { get; protected set; }

	[Header("Singleton")]
	[SerializeField]
	protected SingletonDestroyMode _destroyMode = SingletonDestroyMode.GameObject;
	[SerializeField]
	protected bool _dontDestroyOnLoad = false;

	protected abstract void SingletonAwake();

    protected virtual void CustomDestroy(T oldInstance) { }

	private void Awake()
	{
		if (Instance != null)
		{
            if (_destroyMode == SingletonDestroyMode.GameObject)
                Destroy(gameObject);

            else if (_destroyMode == SingletonDestroyMode.Script)
                Destroy(this);

            else if (_destroyMode == SingletonDestroyMode.Custom)
                CustomDestroy(Instance);
        }

		else
		{
			if (_dontDestroyOnLoad)
				DontDestroyOnLoad(gameObject);

			Instance = this as T;
			SingletonAwake();
		}
	}
}
