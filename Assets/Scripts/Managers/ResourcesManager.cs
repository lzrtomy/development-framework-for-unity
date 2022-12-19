/*
 Load进行管理
 */

using System;
using System.Collections.Generic;
using UnityEngine;
using Company.Tools;

namespace Company.NewApp
{
    public class ResourcesManager : UnitySingleton<ResourcesManager>
    {
        private AddressablesManager m_AddressablesManager;

        //加载至内存中的非GameObject类型资源
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
        public T LoadAsset<T>(string path) where T : UnityEngine.Object
        {
            if (!m_LoadedAssetDict.ContainsKey(path))
            {
                m_LoadedAssetDict[path] = GetAssetCache<T>(path);
            }
            return m_LoadedAssetDict[path] as T;
        }

        /// <summary>
        /// 获取Asset对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        private T GetAssetCache<T>(string path) where T : UnityEngine.Object
        {
            if (IsAssetDatabaseMode)
            {
#if UNITY_EDITOR
                //string path = AddressablesUtility.PackagePathToAssetPath(name);

                string fullPath = "Assets/AssetsPackage/" + path;

                T asset = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(fullPath);

                if (asset == null && m_LogEnabled)
                    Debug.LogError("[ResourcesManager] Cannot get asset cache at path: " + fullPath);

                return asset;
#endif
                Debug.LogError("[ResourcesManager] Should be used in Editor platform");
                return null;
            }
            else
            {
                return m_AddressablesManager.LoadAsset<T>(AddressablesUtility.AssetPathToName(path));
            }
        }

        #endregion

        #region 克隆资源

        /// <summary>
        /// 克隆对象
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="parent">父物体</param>
        /// <returns></returns>
        public GameObject Clone(string path, Transform parent = null)
        {
            if (!m_LoadedAssetDict.ContainsKey(path))
            {
                m_LoadedAssetDict[path] = GetAssetCache<GameObject>(path);
            }
            return Instantiate(m_LoadedAssetDict[path] as GameObject, parent);
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
    }
}