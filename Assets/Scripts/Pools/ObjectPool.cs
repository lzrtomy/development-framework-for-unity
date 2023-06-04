using Company.NewApp;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Company.NewApp
{
    public class ObjectPool:UnitySingleton<ObjectPool>
    {
        private Dictionary<string, GameObject> m_GoMemoryDict;

        private Dictionary<string, List<GameObject>> m_ObjectPoolDict;

        public void Init() 
        {
            m_GoMemoryDict = new Dictionary<string, GameObject>();
            m_ObjectPoolDict = new Dictionary<string, List<GameObject>>();
        }

        /// <summary>
        /// 是否已经预加载
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool IsPreload(string path)
        {
            return m_GoMemoryDict.ContainsKey(path);
        }

        /// <summary>
        /// 资源手动预加载至内存
        /// </summary>
        /// <param name="path"></param>
        /// <param name="onLoad">加载资源至内存后的回调</param>
        public void Preload(string path, Action onLoad = null)
        {
            if (m_GoMemoryDict.ContainsKey(path))
            {
                onLoad?.Invoke();
            }
            else
            {
                ResourcesManager.Instance.LoadAsset(path,
                    (GameObject go) =>
                    {
                        m_GoMemoryDict[path] = go;
                        onLoad?.Invoke();
                    });
            }
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="path"> </param>
        public GameObject Get(string path)
        {
            if (!m_ObjectPoolDict.ContainsKey(path))
            {
                m_ObjectPoolDict[path] = new List<GameObject>();
            }

            if (m_ObjectPoolDict[path].Count == 0)
            {
                return InstantiateGo(path);
            }

            GameObject go = m_ObjectPoolDict[path][0];
            m_ObjectPoolDict[path].RemoveAt(0);

            try
            {
                go.SetActive(true);
            }
            catch (Exception e)
            {
                Debug.LogError("[ObjectPool] Exception:" + e);
            }

            return go;
        }

        private GameObject InstantiateGo(string key)
        {
            if (!m_GoMemoryDict.ContainsKey(key))
            {
                Debug.LogErrorFormat("[ObjectPool] Cannot Instaniate new GameObject with key -{0}-. Must be preload to memory at First!", key);
            }
            GameObject go = GameObject.Instantiate(m_GoMemoryDict[key]);
            return go;
        }

        /// <summary>
        /// 回收对象
        /// </summary>
        /// <param name="path"></param>
        /// <param name="go"></param>
        public void Return(string path, GameObject go)
        {
            if (go)
            {
                go.SetActive(false);
                if (!m_ObjectPoolDict.ContainsKey(path))
                {
                    m_ObjectPoolDict[path] = new List<GameObject>();
                    m_ObjectPoolDict[path].Add(go);
                    return;
                }
                if (!m_ObjectPoolDict[path].Contains(go))
                {
                    m_ObjectPoolDict[path].Add(go);
                    return;
                }
            }
        }

        /// <summary>
        /// 根据相对路径释放资源
        /// </summary>
        /// <param name="path"></param>
        public void Release(string path) 
        {
            if (m_ObjectPoolDict.ContainsKey(path)) 
            {
                m_ObjectPoolDict[path].Clear();
            }
            if (m_GoMemoryDict.ContainsKey(path)) 
            {
                m_GoMemoryDict.Remove(path);
            }
            ResourcesManager.Instance.Release(path);
        }

        /// <summary>
        /// 释放通过对象池加载到内存中的全部资源
        /// </summary>
        public void ReleaseAll() 
        {
            //释放池子
            if (m_ObjectPoolDict != null && m_ObjectPoolDict.Count > 0) 
            {
                var it = m_ObjectPoolDict.GetEnumerator();
                List<GameObject> tempList = null;
                while (it.MoveNext())
                {
                    tempList = it.Current.Value;
                    for (int index = 0; index < tempList.Count; index++)
                    {
                        Destroy(tempList[index]);
                    }
                }

                m_ObjectPoolDict.Clear();
            }

            //释放内存
            List<string> paths = new List<string>();
            if (m_GoMemoryDict != null && m_GoMemoryDict.Count > 0) 
            {
                foreach (var item in m_GoMemoryDict) 
                {
                    paths.Add(item.Key);
                }
                m_GoMemoryDict.Clear();
            }
            for (int index = 0; index < paths.Count; index++) 
            {
                ResourcesManager.Instance.Release(paths[index]);
            }
        }

    }
}