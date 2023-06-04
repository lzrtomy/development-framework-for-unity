using Company.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Company.NewApp
{
    public class AddressablesManager : UnitySingleton<AddressablesManager>
    {
        #region ����ļ�����Դ���첽����
        //KeyΪ���ض���ʱʹ�õ�key��ValueΪ���ض�����첽�����Ļ���
        //���ص�������
        Dictionary<string, AsyncOperationHandle<UnityEngine.Object>> m_AssetHandleDict;
        //���ض������
        Dictionary<string, AsyncOperationHandle<IList<UnityEngine.Object>>> m_AssetListHandleDict;
        #endregion


        private bool m_LogEnabled = true;

        public void Init()
        {
            m_LogEnabled = AppSettings.Instance.LogEnabled;
            InitDicts();
        }

        private void InitDicts()
        {
            m_AssetHandleDict = new Dictionary<string, AsyncOperationHandle<UnityEngine.Object>>();

            m_AssetListHandleDict = new Dictionary<string, AsyncOperationHandle<IList<UnityEngine.Object>>>();
        }

        /// <summary>
        /// ����༭����׿ƽ̨��Addressable�޷��������ز��ʵ�����
        /// </summary>
        /// <param name="go"></param>
        public void RefreshMaterial(GameObject go)
        {
#if UNITY_EDITOR
            if (go)
            {
                Renderer renderer = go.GetComponent<Renderer>();
                RefreshMaterial(renderer);
            }
#endif
        }

        public void RefreshMaterial(Renderer renderer)
        {
#if UNITY_EDITOR
            if (renderer)
            {
                string shaderName = renderer.sharedMaterial.shader.name;
                renderer.sharedMaterial.shader = Shader.Find(shaderName);
            }
#endif
        }

        #region ������Դ

        /// <summary>
        /// Key:
        /// (1)ͨ��Name����Asset(string)
        /// (2)ͨ����ǩ����Asset(string)
        /// </summary>
        /// <param name="key">Address��Label</param>
        public T LoadAsset<T>(string key) where T : UnityEngine.Object
        {
            if (!m_AssetHandleDict.ContainsKey(key))
            {
                AsyncOperationHandle<UnityEngine.Object> handle = Addressables.LoadAssetAsync<UnityEngine.Object>(key);
                m_AssetHandleDict[key] = handle;
                T asset = handle.WaitForCompletion() as T;

                if (asset == null && m_LogEnabled)
                    Debug.Log("[AddressablesManager]  Cannot get asset cache with key: " + key);
                if (asset is GameObject) 
                {
                    RefreshMaterial(asset as GameObject);
                }
                return asset;
            }
            else 
            {
                return m_AssetHandleDict[key].Result as T;
            }
        }

        public void LoadAsset<T>(string key, Action<T> onLoad) where T : UnityEngine.Object
        {
            if (m_AssetHandleDict.ContainsKey(key))
            {
                onLoad?.Invoke(m_AssetHandleDict[key].Result as T);
            }
            else 
            {
                AsyncOperationHandle<UnityEngine.Object> tempHandle = Addressables.LoadAssetAsync<UnityEngine.Object>(key);
                m_AssetHandleDict[key] = tempHandle;

                tempHandle.Completed += handle=>
                {
                    if (handle.Result == null && m_LogEnabled)
                    {
                        Debug.Log("[AddressablesManager]  Cannot get asset cache with key: " + key);
                        onLoad?.Invoke(null);
                        return;
                    }
                    else if (handle.Result is GameObject)
                    {
                        RefreshMaterial(handle.Result as GameObject);
                    }
                    onLoad?.Invoke(handle.Result as T);
                };
            }
        }

        /// <summary>
        /// ͨ�����Key���ض������
        /// </summary>
        /// <param name="keys"></param>
        public IList<T> LoadAssets<T>(List<string> keys) where T : UnityEngine.Object
        {
            string mergeKey = StringTools.ConnectString(keys);
            if (!m_AssetListHandleDict.ContainsKey(mergeKey))
            {
                AsyncOperationHandle<IList<UnityEngine.Object>> handle = Addressables.LoadAssetsAsync<UnityEngine.Object>(
                keys, // Either a single key or a List of keys 
                addressable =>
                {
                    Debug.Log("[AddressableManger] name: " + addressable.name);
                    //Gets called for every loaded asset
                    if (addressable != null && addressable is GameObject)
                    {
                        RefreshMaterial(addressable as GameObject);
                    }
                },
                Addressables.MergeMode.Union, // How to combine multiple labels 
                false);// Whether to fail if any asset fails to load
                m_AssetListHandleDict[mergeKey] = handle;
                IList<UnityEngine.Object> list = handle.WaitForCompletion();

                if ((list == null || list.Count == 0) && m_LogEnabled)
                    Debug.Log("[AddressablesManager]  Cannot get load assets with keys. MergeKey is: " + mergeKey);

                return CollectionTools.ListFormatConversion<UnityEngine.Object, T>(list);
            }
            else 
            {
                return m_AssetListHandleDict[mergeKey].Result as IList<T>;
            }
        }

        #endregion

        #region ʵ������Դ

        public GameObject Instantiate(object key, Transform parent) 
        {
            AsyncOperationHandle<GameObject> handle = Addressables.InstantiateAsync(key, parent);
            return handle.WaitForCompletion();
        }

        #endregion

        #region �ͷ���Դ

        /// <summary>
        /// ͨ��Keyж���ڴ����
        /// </summary>
        /// <param name="key"></param>
        public void Release(string key) 
        {
            if (m_AssetHandleDict.ContainsKey(key))
            {
                Addressables.Release(m_AssetHandleDict[key]);
                m_AssetHandleDict.Remove(key);

                if (m_LogEnabled)
                    Debug.Log("[AddressablesManager] release Asset: " + key.ToString());
            }
        }

        /// <summary>
        /// ͨ�����Keyж�ض������
        /// </summary>
        /// <param name="keys"></param>
        public void Release(List<string> keys)
        {
            string mergeKey = StringTools.ConnectString(keys);

            if (m_AssetListHandleDict.ContainsKey(mergeKey))
            {
                Addressables.Release(m_AssetListHandleDict[mergeKey]);
                m_AssetListHandleDict.Remove(mergeKey);

                if (m_LogEnabled)
                    Debug.Log("[AddressablesManager] release Assets: " + mergeKey.ToString());
            }
        }

        public void ReleaseInstance(GameObject addessable) 
        {
            Addressables.ReleaseInstance(addessable);
        }

        /// <summary>
        /// ж�����м��ص���Դ
        /// </summary>
        public void ReleaseAll()
        {
            foreach (var item in m_AssetHandleDict)
            {
                if (item.Value.Result != null)
                {
                    Addressables.Release(item.Value);
                }
            }
            foreach (var item in m_AssetListHandleDict)
            {
                if (item.Value.Result != null)
                {
                    Addressables.Release(item.Value);
                }
            }

            m_AssetHandleDict = new Dictionary<string, AsyncOperationHandle<UnityEngine.Object>>();
            m_AssetListHandleDict = new Dictionary<string, AsyncOperationHandle<IList<UnityEngine.Object>>>();

            if (m_LogEnabled)
                Debug.Log("[AddressablesManager] release all Asset");
        }

        #endregion

        #region ������Դ



        #endregion
    }
}