
using System.Collections.Generic;
using UnityEngine;

namespace Company.NewApp
{
    /// <summary>
    /// UI����
    /// </summary>
    [CreateAssetMenu]
    [System.Serializable]
    public class UISettings : ScriptableObject
    {
        private static UISettings m_Instance;
        public static UISettings Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = (UISettings)UnityEngine.Resources.Load("UISettings");
                }
                return m_Instance;
            }
        }

        [Header("���ڱ��ص�UIԤ������Ϣ��ȫ·��Ҫд���ļ���չ��")]
        [SerializeField] private ViewInfo[] m_UIDataArray;

        //UI�����ֵ�
        private Dictionary<ViewType, ViewInfo> m_UIDataDict = new Dictionary<ViewType, ViewInfo>();
        public Dictionary<ViewType, ViewInfo> UIDataDict { get { return m_UIDataDict; } }

        public void Init()
        {
            AddUIDataToDictionary();
        }

        /// <summary>
        /// ��ȡUI��Ϣ
        /// </summary>
        /// <param name="viewType"></param>
        /// <returns></returns>
        public ViewInfo GetViewInfo(ViewType viewType)
        {
            ViewInfo viewInfo = null;
            UIDataDict.TryGetValue(viewType, out viewInfo);
            return viewInfo;
        }

        private void AddUIDataToDictionary()
        {
            for (int index = 0; index < m_UIDataArray.Length; index++)
            {
                if (!UIDataDict.ContainsKey(m_UIDataArray[index].ViewType))
                {
                    UIDataDict.Add(m_UIDataArray[index].ViewType, m_UIDataArray[index]);
                }
            }
        }
    }

    [System.Serializable]
    public class ViewInfo
    {
        //UI����
        public ViewType ViewType;

        //UIȫ·��������չ����
        public string FullPath = "";

        //Ԥ�������Ƿ����Presenter��
        public bool IsWithPresenter = false;

        //�ر�ʱ�Ƿ����
        public bool Recyclable = false;

        //�Ƿ�Ԥ����
        public bool Preload = false;
    }

    /// <summary>
    /// ��ҪUI��ö��,˳����Ҫ��ViewInfo.xlsx�е�UI˳�����Ӧ
    /// </summary>
    public enum ViewType
    {
        None,
        Example,
        UISelectLevel
    }
}