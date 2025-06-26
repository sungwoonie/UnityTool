using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarCloudgamesLibrary
{
	public class SingleTon<T> : MonoBehaviour
	{
		public bool dontDestroy = true;

		private static T _instance;

		public static T instance
		{
			get
			{
				return _instance;
			}
		}

		protected virtual void Awake()
		{
			if (instance != null)
			{
				Destroy(gameObject);
			}
			else
			{
				_instance = this.GetComponent<T>();

				if (dontDestroy)
				{
					DontDestroyOnLoad(this);
				}
			}
		}
	}
}