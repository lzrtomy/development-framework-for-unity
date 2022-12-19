
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

        [Header("放在本地的UI预制体信息，全路径要写到文件扩展名")]
        [SerializeField] private ViewInfo[] m_UIDataArray;

        //UI名称字典
        private Dictionary<ViewType, ViewInfo> m_UIDataDict = new Dictionary<ViewType, ViewInfo>();
        public Dictionary<ViewType, ViewInfo> UIDataDict { get { return m_UIDataDict; } }

        public void Init()
        {
            AddUIDataToDictionary();
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
        //UI类型
        public ViewType ViewType;

        //UI全路径（带扩展名）
        public string FullPath = "";

        //预制体上是否带有Presenter层
        public bool IsWithPresenter = false;

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