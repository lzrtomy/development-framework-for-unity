
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Company.NewApp
{
    /// <summary>
    /// UI设置
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

        //UI名称字典
        private Dictionary<ViewType, ViewInfo> m_UIDataDict = new Dictionary<ViewType, ViewInfo>();
        public Dictionary<ViewType, ViewInfo> UIDataDict { get { return m_UIDataDict; } }

        public void Init()
        {
            AddUIDataToDictionaryFromExcel();
        }

        /// <summary>
        /// 获取UI信息
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
        /// 通过Excel读取UI信息并管理到字典中
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
        //UI类型
        public ViewType ViewType;

        //UI全路径（带扩展名）
        public string FullPath = "";

        //预制体上是否带有Presenter层
        public bool IsWithPresenter = false;

        //是否在UIManager中管理起来
        public bool IsRefInManager = true;

        //关闭时是否回收
        public bool Recyclable = false;

        //是否预加载
        public bool Preload = false;
    }

    /// <summary>
    /// 主要UI的枚举,顺序需要与ViewInfo.xlsx中的UI顺序相对应
    /// </summary>
    public enum ViewType
    {
        None,
        Example,
        UISelectLevel
    }
}