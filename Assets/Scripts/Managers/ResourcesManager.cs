/*
 Load进行管理
 */

using System;
using System.Collections.Generic;
using UnityEngine;
using Company.Tools;
using System.Collections;

namespace Company.NewApp
{
    public class ResourcesManager : UnitySingleton<ResourcesManager>
    {
        private AddressablesManager m_AddressablesManager;

        //加载至内存中的资源
        private Dictionary<string, UnityEngine.Object> m_LoadedAssetDict = new Dictionary<string, UnityEngine.Object>();

        private bool m_LogEnabled = true;
        private bool m_IsUseAssetDatabaseModeInEditor = true;

        /// <summary>
        /// 是否使用AssetDatabase模式加载资源
        /// </summary>
        /// <returns></returns>
        private bool IsAssetDatabaseMode
        {
            get
            {
#if UNITY_EDITOR
                return m_IsUseAssetDatabaseModeInEditor;
#endif
                return false;
            }
        }

        public void Init()
        {
            m_AddressablesManager = AddressablesManager.Instance;
            m_LogEnabled = AppSettings.Instance.LogEnabled;
            m_IsUseAssetDatabaseModeInEditor = AppSettings.Instance.Resources.IsUseAssetDatabaseModeInEditor;
        }

        #region 加载资源

        /// <summary>
        /// 泛型加载Asset
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public void LoadAsset<T>(string path, Action<T> onLoad) where T : UnityEngine.Object
        {
            UnityEngine.Object asset = null;
            if (m_LoadedAssetDict.TryGetValue(path, out asset))
            {
                onLoad?.Invoke(asset as T);
            }
            else
            {
                GetAssetCache<T>(path, 
                    (T t) => 
                    {
                        m_LoadedAssetDict[path] = t;
                        onLoad?.Invoke(t);
                    });
            }
        }

        public void LoadAsset<T>(string path) where T : UnityEngine.Object
        {
            LoadAsset<T>(path, null);
        }

        
        /// <summary>
        /// 检查预加载资源
        /// </summary>
        /// <param name="path"></param>
        /// <param name="onLoad"></param>
        public void CheckPreloadPrefab(string path, Action onLoad = null)
        {
            UnityEngine.Object prefab = null;
            if (m_LoadedAssetDict.TryGetValue(path, out prefab))
            {
                onLoad?.Invoke();
            }
            else
            {
                GetAssetCache<GameObject>(path,
                    (GameObject go) =>
                    {
                        m_LoadedAssetDict[path] = go;
                        onLoad?.Invoke();
                    });
            }
        }

        /// <summary>
        /// 批量检查预加载资源，前一项检查开始后就会继续开始检查下一项
        /// </summary>
        /// <param name="pathList"></param>
        /// <param name="onLoad"></param>
        public void CheckPreloadPrefabs(List<string> pathList, Action onLoad = null)
        {
            int count = 0;
            for (int i = 0; i < pathList.Count; i++)
            {
                CheckPreloadPrefab(pathList[i],
                    () => 
                    {
                        count++;
                        if (count >= pathList.Count)
                        {
                            onLoad?.Invoke();
                        }
                    });
            }
        }

        /// <summary>
        /// 批量检查预加载资源，前一项预加载结束之后才会开始检查下一项
        /// </summary>
        /// <param name="pathList"></param>
        /// <param name="onLoad"></param>
        public void CheckPreloadPrefabsInSeq(List<string> pathList, Action onLoad = null)
        {
            StartCoroutine(IECheckPreloadPrefabsInSeq(pathList, onLoad));
        }

        private IEnumerator IECheckPreloadPrefabsInSeq(List<string> pathList, Action onLoad = null)
        {
            bool isPreload = false;
            WaitUntil waitLoad = new WaitUntil(() => isPreload);

            for (int i = 0; i < pathList.Count; i++)
            {
                isPreload = false;
                CheckPreloadPrefab(pathList[i], () => isPreload = true);
                yield return waitLoad;
            }

            onLoad?.Invoke();
        }



        private void GetAssetCache<T>(string path, Action<T> onLoad) where T : UnityEngine.Object
        {
            if (IsAssetDatabaseMode)
            {
#if UNITY_EDITOR
                //string path = AddressablesUtility.PackagePathToAssetPath(name);

                string fullPath = "Assets/AssetsPackage/" + path;

                T asset = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(fullPath);

                if (asset == null && m_LogEnabled)
                    Debug.LogError("[ResourcesManager] Cannot get asset cache at path: " + fullPath);

                onLoad?.Invoke(asset);
#else
                Debug.LogError("[ResourcesManager] Should be used in Editor platform");
#endif
            }
            else
            {
                m_AddressablesManager.LoadAsset<T>(AddressablesUtility.AssetPathToName(path), onLoad);
            }
        }

        #endregion

        #region 克隆资源

        /// <summary>
        /// 克隆对象
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="ponCloneath">回调</param>
        /// <param name="parent">父物体</param>
        /// <returns></returns>
        public void Clone(string path, Action<GameObject> onClone, Transform parent = null)
        {
            if (m_LoadedAssetDict.ContainsKey(path))
            {
                onClone?.Invoke(Instantiate(m_LoadedAssetDict[path] as GameObject, parent));
            }
            else
            {
                GetAssetCache<GameObject>(path,
                    (GameObject go) =>
                    {
                        m_LoadedAssetDict[path] = go;
                        onClone?.Invoke(Instantiate(m_LoadedAssetDict[path] as GameObject, parent));
                    });
            }
        }

        public GameObject Clone(GameObject origin, Transform parent = null)
        {
            return Instantiate(origin, parent);
        }

#endregion

#region 卸载资源

        /// <summary>
        /// 根据key卸载资源
        /// </summary>
        /// <param name="path"></param>
        public void Release(string path)
        {
            if (m_LoadedAssetDict.ContainsKey(path))
            {
                m_LoadedAssetDict.Remove(path);
            }

            if (!IsAssetDatabaseMode)
            {
                m_AddressablesManager.Release(AddressablesUtility.AssetPathToName(path));
            }
        }

        public void ReleaseInstance(GameObject addressable)
        {
            if (addressable)
            {
                m_AddressablesManager.ReleaseInstance(addressable);
            }
        }

        /// <summary>
        /// 切换场景时清理内存
        /// </summary>
        public void ReleaseAll()
        {
            m_LoadedAssetDict.Clear();

            if (!IsAssetDatabaseMode)
            {
                m_AddressablesManager.ReleaseAll();
            }

            Resources.UnloadUnusedAssets();
            ObjectPool.Instance.ReleaseAll();
            CachePool.Instance.ReleaseAll();
        }

        #endregion

        private void OnDestroy()
        {
            StopAllCoroutines();
        }
    }
}