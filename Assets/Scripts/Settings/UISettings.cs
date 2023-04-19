
using System;
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

        //UI�����ֵ�
        private Dictionary<ViewType, ViewInfo> m_UIDataDict = new Dictionary<ViewType, ViewInfo>();
        public Dictionary<ViewType, ViewInfo> UIDataDict { get { return m_UIDataDict; } }

        public void Init()
        {
            AddUIDataToDictionaryFromExcel();
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

        /// <summary>
        /// ͨ��Excel��ȡUI��Ϣ�������ֵ���
        /// </summary>
        private void AddUIDataToDictionaryFromExcel()
        {
            List<UI_GeneralRawEntity> rawEntityList = UI_GeneralRawEntityModel.Instance.GetList();
            for (int i = 0; i < rawEntityList.Count; i++)
            {
                ViewInfo viewInfo = new ViewInfo();
                if (!Enum.TryParse<ViewType>(rawEntityList[i].ViewType, out viewInfo.ViewType))
                {
                    Debug.LogErrorFormat("[UISettings] View type -{0}- is wrong at id -{1}-", rawEntityList[i].ViewType, rawEntityList[i].Id);
                }
                viewInfo.FullPath = rawEntityList[i].FullPath;
                viewInfo.IsWithPresenter = rawEntityList[i].IsWithPresenter == 1;
                viewInfo.IsRefInManager = rawEntityList[i].IsRefInManager == 1;
                viewInfo.Recyclable = rawEntityList[i].Recyclable == 1;
                viewInfo.Preload = rawEntityList[i].Preload == 1;

                UIDataDict.Add(viewInfo.ViewType, viewInfo);
                Debug.Log("[UISettings] view type:" + viewInfo.ViewType);
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

        //�Ƿ���UIManager�й�������
        public bool IsRefInManager = true;

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