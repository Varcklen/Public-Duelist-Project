using UnityEngine;
using System;

namespace Project.Singleton.MonoBehaviourSingleton
{
	/// <summary>
	/// Creates an Instance field for the child class.
	/// </summary>
	/// <typeparam name="T">Сhild class</typeparam>
	public abstract class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviourSingleton<T>
    {
		private static T _instance;
		private static readonly object _instanceLock = new object();
		private static bool _quitting = false;

		public static T Instance
		{
			get
			{
				lock (_instanceLock)
				{
					if (_instance == null && !_quitting)
					{
						_instance = FindObjectOfType<T>();
						if (_instance == null)
						{
							GameObject go = new GameObject(typeof(T).ToString());
							_instance = go.AddComponent<T>();
							//DontDestroyOnLoad(_instance.gameObject);
						}
					}

					return _instance;
				}
			}
		}

		protected virtual void Awake()
		{
			if (_instance == null)
            {
				_instance = gameObject.GetComponent<T>();
			}
			else if (_instance.GetInstanceID() != GetInstanceID())
			{
				Destroy(gameObject);
				throw new Exception($"Instance of {GetType().FullName} already exists, removing {ToString()}.");
			}
		}

		protected virtual void OnApplicationQuit()
		{
			_quitting = true;
		}
	}
}
