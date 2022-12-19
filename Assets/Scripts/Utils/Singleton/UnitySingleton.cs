using UnityEngine;

public class UnitySingleton<T> : MonoBehaviour where T : UnitySingleton<T>
{
    private static T _instance;
    private static readonly object syslock = new object();

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (syslock)
                {
                    _instance = FindObjectOfType<T>();
                    if (_instance == null)
                    {
                        string instanceName = string.Format(typeof(T).Name);
                        GameObject go = new GameObject(instanceName);
                        _instance = go.AddComponent<T>();
                    }
                }
            }
            return _instance;
        }
    }

    public virtual void Awake()
    {
        DontDestroyOnLoad(gameObject);
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