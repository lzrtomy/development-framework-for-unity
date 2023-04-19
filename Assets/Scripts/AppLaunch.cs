using System.Collections;
using Company.NewApp.Models;
using Company.HttpWebRequest;
using Company.WebSocketRequest;
using UnityEngine;

namespace Company.NewApp
{
    public class AppLaunch : UnitySingleton<AppLaunch>
    {
        private static InitState m_InitState = InitState.NotInit;

        private IEnumerator m_IEInitData;
        private IEnumerator m_IEInitApp;

        private GameObject m_AppManagersNode;
        private GameObject m_AudioManagerNode;
        private GameObject m_PoolsGo;

        public static InitState InitState { get { return m_InitState; } }

        void Start()
        {
            m_InitState = InitState.InInit;

            m_AppManagersNode = CreateNode("AppManagers", 0);
            m_AudioManagerNode = CreateNode("AudioManager", 1);
            m_PoolsGo = CreateNode("Pools", 2);

            SetCultureInfo();
            InitAppSettings();
            m_IEInitData = IEInitData();
            StartCoroutine(m_IEInitData);
        }

        /// <summary>
        /// 在欧洲国家，会使用","作为小数点，使用"."作为千位分隔符，这会导致json解析错误
        /// </summary>
        private void SetCultureInfo()
        {
            System.Globalization.CultureInfo.DefaultThreadCurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Globalization.CultureInfo.DefaultThreadCurrentUICulture = new System.Globalization.CultureInfo("en-US");
        }

        private void InitAppSettings()
        {
            AppSettings.Instance.Init();
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <returns></returns>
        IEnumerator IEInitData()
        {
            m_AppManagersNode.AddComponent<FileManager>().Init();

            m_AppManagersNode.AddComponent<DataModelManager>().Init(()=> 
            {
                m_IEInitApp = IEInitApp();
                StartCoroutine(m_IEInitApp);
            });
            yield break;
        }

        IEnumerator IEInitApp()
        {
            AppAudioSettings.Instance.Init();
            UISettings.Instance.Init();

            #region 数据模型层初始化

            //示例配置表
            DataModelManager.Instance.AddDataModel<ExampleEntityModel>();
            DataModelManager.Instance.AddDataModel<HotkeyEntityModel>();
            DataModelManager.Instance.AddDataModel<LevelEntityModel>();

            #endregion

            yield return null;

            #region 框架初始化

            m_AppManagersNode.AddComponent<UpdateManager>();
            m_AppManagersNode.AddComponent<EventManager>().Init();
            m_AppManagersNode.AddComponent<HotkeyManager>().Init();
            m_AppManagersNode.AddComponent<AddressablesManager>().Init();
            m_AppManagersNode.AddComponent<ResourcesManager>().Init();
            m_AppManagersNode.AddComponent<CameraManager>().Init();
            m_AppManagersNode.AddComponent<UIManager>().Init();
            m_AppManagersNode.AddComponent<HttpRequestManager>().Init();
            m_AppManagersNode.AddComponent<WebSocketRequestManager>().Init();
            m_AppManagersNode.AddComponent<AppManager>().Init();

            m_AudioManagerNode.AddComponent<AudioManager>().Init();

            m_PoolsGo.AddComponent<ObjectPool>().Init();
            m_PoolsGo.AddComponent<CachePool>().Init();

            #endregion

            yield return null;

            #region 检查资源更新

            #endregion

            yield return null;
            
            m_InitState = InitState.Inited;
            
            MainController.Instance?.Init();
            Debug.Log("RuntimePath:" + UnityEngine.AddressableAssets.Addressables.RuntimePath);
            Debug.Log("buildPath:" + UnityEngine.AddressableAssets.Addressables.BuildPath);
            Debug.Log("LibraryPath:" + UnityEngine.AddressableAssets.Addressables.LibraryPath);
            yield break;
        }

        private GameObject CreateNode(string name, int siblingIndex)
        {
            GameObject node = new GameObject();
            node.transform.SetParent(transform);
            node.transform.SetSiblingIndex(siblingIndex);
            node.transform.localPosition = Vector3.zero;
            node.name = name;
            return node;
        }

        private void OnDestroy()
        {
            Debug.Log("Destroy");
        }

    }
}