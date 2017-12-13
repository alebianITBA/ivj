#define SAMPLE_OUTSIDE_EDITOR

using UnityEngine;
using UnityEngine.Profiling;

public class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviourSingleton<T>
{
	static MonoBehaviourSingleton<T> instance;

	// Used to select the behavior of the singleton when it's destroyed, if it's "true" then
	// the instance value will be set to "null" when the MonoBehavior is Destroyed, otherwise 
	// it will keep the last instance value.
	// If it's false, then Instance will try to find a new non destroyed instance of the singleton in the scene, if
	// it can't find one, it will return the last value. Can be useful for components that need to access the singleton
	// after it has been destroyed to remove references / set some values / etc..
	protected static bool setToNullAfterDestroy = true;

	// Used to detect if the singleton is destroyed or not. We can't rely on the value of "instance" because
	// it's value will be keep if "setToNullAfterDestroy" is false.
	static bool destroyed = true;

	// Used to detect if the singleton was found at least once, and if it wasn't then show 
	// an error message informing this when trying to access the Instance
	static bool initializedAtLeastOnce = false;

	// Used to detect if the singleton's Initialize() function needs to be called
	static bool needInitialization = true;

	protected bool Destroyed
	{
		get { return destroyed; }
	}

	public void Awake()
	{
		if (instance == null || destroyed) 
		{
			instance = this;
			destroyed = false;
		} 
		else if(instance != this)
		{
			Debug.LogError("Two instances of the same singleton '" + this + "'");
		}

		if (needInitialization)
		{
			needInitialization = false;
			initializedAtLeastOnce = true;

			#if UNITY_EDITOR || SAMPLE_OUTSIDE_EDITOR
			Profiler.BeginSample(instance.name + ".Initialize");
			#endif

			Initialize ();

			#if UNITY_EDITOR || SAMPLE_OUTSIDE_EDITOR
			Profiler.EndSample();
			#endif
		}
	}

	public void OnDestroy()
	{
		#if UNITY_EDITOR || SAMPLE_OUTSIDE_EDITOR
		Profiler.BeginSample(name + ".Destroy");
		#endif

		Destroy();

		#if UNITY_EDITOR || SAMPLE_OUTSIDE_EDITOR
		Profiler.EndSample();
		#endif
		destroyed = true;
		needInitialization = true;

		if (setToNullAfterDestroy)
			instance = null;
	}

	public static T Instance
	{
		get 
		{ 
			if (instance == null || destroyed || needInitialization)
			{
				if (instance == null || destroyed)
				{
					MonoBehaviourSingleton<T> newInstance = MonoBehaviour.FindObjectOfType<MonoBehaviourSingleton<T>>();

					if (newInstance != null)
					{
						instance = newInstance;
						destroyed = false;
					}
				}

				if (instance != null && !destroyed)
				{
					if (needInitialization)
					{
						needInitialization = false;
						initializedAtLeastOnce = true;
					}
				}
				else
				{
					if (!initializedAtLeastOnce)
						Debug.LogError("Missing Singleton '" + typeof(T).Name + "'");
				}
			}

			return (T) instance; 
		}
	}

	public static void Dispose()
	{
		if (instance != null && !destroyed)
		{
			Destroy(instance.gameObject);

			instance = null;
		}
	}

	protected virtual void Initialize()
	{
	}

	protected virtual void Destroy()
	{
	}

	static public bool IsInitialized()
	{
		return instance != null && !destroyed;
	}

	static public bool IsAvailable()
	{
		return (IsInitialized() || MonoBehaviour.FindObjectOfType<MonoBehaviourSingleton<T>>() != null);
	}
}
