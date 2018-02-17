using System;
using UnityEngine;

namespace Tools.Helper
{
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        private static T instance = null;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectsOfType(typeof(T)) as T;
                    if (instance == null)
                    {
                        instance = new GameObject("Singleton of " + typeof(T).ToString(), typeof(T)).GetComponent<T>();
                        instance.Init();
                    }
                }
                return instance;
            }
        }
        private void Awake()
        {
            if (instance == null)
            {
                instance = this as T;
            }
        }
        private void OnApplicationQuit()
        {
            instance = null;
        }
        public virtual void Init()
        {
        }
    }
}
