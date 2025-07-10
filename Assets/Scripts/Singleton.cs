using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
    
    public Singleton() {}

    private static T _instance;

    public static T Instance()
    {
        if (_instance == null)
        {
            _instance = FindFirstObjectByType<T>();
            if (_instance == null)
            {
                GameObject obj = new GameObject(typeof(T).Name);
                _instance = obj.AddComponent<T>();
            }
        }
        return _instance;
    }

}
