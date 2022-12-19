
using System.Collections.Generic;
using UnityEngine;

namespace Company.NewApp
{

    /// <summary>
    /// 游戏设置
    /// </summary>
    [CreateAssetMenu]
    [System.Serializable]
    public class AppSettings : ScriptableObject
    {
        private static AppSettings m_Instance;
        public static AppSettings Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = (AppSettings)UnityEngine.Resources.Load("AppSettings");
                }
                return m_Instance;
            }
        }

        [Header("是否开启log")]
        [SerializeField] private bool m_LogEnabled = true;
        public bool LogEnabled { get { return m_LogEnabled; } }

        [Header("资源管理设置")]
        [SerializeField] private ResourcesSettings m_Resources;
        public ResourcesSettings Resources { get { return m_Resources; } }

        [Header("摄像机设置")]
        [SerializeField] private CameraSettings m_Camera;
        public CameraSettings Camera { get { return m_Camera; } }

        public void Init()
        {
            Debug.unityLogger.logEnabled = m_LogEnabled;
        }
    }

    [System.Serializable]
    public class ResourcesSettings
    {
        [Header("是否在编辑器平台下使用AssetDatabase模式加载资源")]
        [SerializeField] private bool m_IsUseAssetDatabaseModeInEditor;
        public bool IsUseAssetDatabaseModeInEditor { get { return m_IsUseAssetDatabaseModeInEditor; } }
    }


    [System.Serializable]
    public class CameraSettings
    {
        [Header("是否开启摄像机自适应")]
        [SerializeField] private bool m_IsCameraAdaptationOn = true;
        public bool IsCameraAdaptationOn { get { return m_IsCameraAdaptationOn; } }
    }
}