using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleUnitySingleton<T> : MonoBehaviour where T : SimpleUnitySingleton<T>
{
    private static T _instance;

    public static T Instance
    {
        get { return _instance; }
    }

    public virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}