using Company.NewApp.Views;
using Company.NewApp.Presenters;
using Company.Constants;
using System;
using System.Collections.Generic;
using UnityEngine;
using Company.NewApp.Models;
using Company.Tools;

namespace Company.NewApp
{
    public class UIManager : UnitySingleton<UIManager>
    {
        //每个场景下的Canvas
        private Transform m_RootTrans;

        private Dictionary<ViewType, List<UIViewBase>> m_ViewDict;

        //客户端页面跳转方法集合
        private Dictionary<string, Func<bool>> m_LinkFunctions;

        private UISettings m_UISettings;

        private bool m_LogEnabled;

        private Transform RootTrans 
        {
            get 
            {
                if (!m_RootTrans) 
                {
                    m_RootTrans = GameObject.Find("Canvas").transform;
                }
                return m_RootTrans;
            }
        }

        public void Init()
        {
            m_LogEnabled = AppSettings.Instance.LogEnabled;
            m_UISettings = UISettings.Instance;

            m_ViewDict = new Dictionary<ViewType, List<UIViewBase>>();
            InitLinkFunctionsDict();
        }

        /// <summary>
        /// 判断是否已经创建了指定的UI
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <param name="viewName"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool IsViewExist<T>(ViewType type, string viewName, out T t) where T : UIViewBase
        {
            t = null;
            List<UIViewBase> list = null;
            m_ViewDict.TryGetValue(type, out list);
            if (list != null)
            {
                for (int index = 0; index < list.Count; index++)
                {
                    if (list[index].ViewName == viewName)
                    {
                        t = list[index] as T;
                        return true;
                    }
                }
            }
            return false;
        }

        #region Manage Views

        /// <summary>
        /// 添加view
        /// </summary>
        /// <param name="type"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        private bool AddView(ViewType type, UIViewBase view)
        {
            if (!m_ViewDict.ContainsKey(type))
            {
                m_ViewDict[type] = new List<UIViewBase>();
                m_ViewDict[type].Add(view);
                return true;
            }

            if (!m_ViewDict[type].Contains(view))
            {
                m_ViewDict[type].Add(view);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 移除View
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private bool RemoveView(ViewType type)
        {
            if (!m_ViewDict.ContainsKey(type))
            {
                return true;
            }
            m_ViewDict.Remove(type);
            return true;
        }

        #endregion

        #region 加载并展示UI

        /// <summary>
        /// 加载并展示UI
        /// </summary>
        /// <typeparam name="T">UI类型</typeparam>
        /// <param name="root">父节点</param>
        /// <param name="name">自定义名称</param>
        /// <param name="initParams">UI参数</param>
        /// <returns></returns>
        public T Open<T>(ViewType type, string viewName, Transform root, params object[] initParams) where T : UIViewBase
        {
            T t = null;
            if (IsViewExist(type, viewName, out t))
            {
                Debug.LogError(string.Format("[UIManager] Cannot open new {0}, since it has already been existed!", typeof(T).Name));
                return t;
            }
            if (m_UISettings.UIDataDict[type].Recyclable)
            {
                return GetViewFromPool<T>(type, viewName, root, initParams);
            }
            else
            {
                return NewView<T>(type, viewName, root, initParams);
            }
        }

        /// <summary>
        /// 创建view
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private T NewView<T>(ViewType type, string viewName, Transform root, params object[] initParams) where T : UIViewBase
        {
            GameObject go = ResourcesManager.Instance.Clone(m_UISettings.UIDataDict[type].FullPath);
            return HandleView<T>(go, type, viewName, root, initParams);
        }

        private T GetViewFromPool<T>(ViewType type, string viewName, Transform root, params object[] initParams) where T : UIViewBase 
        {
            GameObject go = ObjectPool.Instance.Get(m_UISettings.UIDataDict[type].FullPath);
            return HandleView<T>(go, type, viewName, root, initParams);
        }

        private T HandleView<T>(GameObject viewGo, ViewType type, string viewName, Transform root, params object[] initParams) where T : UIViewBase 
        {
            ViewInfo viewInfo = m_UISettings.UIDataDict[type];

            T view = viewGo.GetComponent<T>();
            if (view)
            {
                view.SetViewType(type);
                view.SetViewName(viewName);
            }
            else
            {
                return null;
            }

            if (viewInfo.IsWithPresenter)
            {
                UIPresenterBase presenter = viewGo.GetComponent<UIPresenterBase>();
                if (presenter)
                {
                    presenter.Init(initParams);
                }
                else
                {
                    Debug.LogError(string.Format("[UIManager] {0} has wrong config on IsWithPresenter", viewGo.name));
                }
            }
            else
            {
                view.Init(initParams);
                view.Open();
            }

            if (viewInfo.IsRefInManager)
            {
                AddView(type, view);
            }

            //调整UI属性
            RectTransform rect = viewGo.GetComponent<RectTransform>();
            rect.SetParent(root);
            rect.SetAsLastSibling();
            AdjustRectTransform(rect);

            return view;
        }

        /// <summary>
        /// 调整ui的rect
        /// </summary>
        /// <param name="rect"></param>
        private void AdjustRectTransform(RectTransform rect)
        {
            rect.localEulerAngles = Vector3.zero;
            rect.localPosition = Vector3.zero;
            rect.localScale = Vector3.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
        }

        #endregion

        #region 关闭UI

        /// <summary>
        /// 关闭指定ViewType下的全部UI
        /// </summary>
        /// <param name="name">自定义名称</param>
        public void Close(ViewType type)
        {
            List<UIViewBase> tempList = null;
            m_ViewDict.TryGetValue(type, out tempList);

            if (!CollectionTools.IsNullOrEmpty(tempList))
            {
                for (int index = 0; index < tempList.Count; index++)
                {
                    if (tempList[index])
                    {
                        tempList[index].Close();
                    }
                }
                RemoveView(type);
            }

            if (!m_UISettings.GetViewInfo(type).Recyclable)
            {
                ResourcesManager.Instance.Release(m_UISettings.UIDataDict[type].FullPath);
            }
        }

        /// <summary>
        /// 关闭指定ViewType下的指定UI
        /// </summary>
        /// <param name="name">自定义名称</param>
        public void Close(ViewType type, int viewId)
        {
            List<UIViewBase> tempList = null;
            m_ViewDict.TryGetValue(type, out tempList);

            UIViewBase view = null;

            if (!CollectionTools.IsNullOrEmpty(tempList))
            {
                for (int index = 0; index < tempList.Count; index++)
                {
                    view = tempList[index];
                    if (view.ViewId == viewId)
                    {
                        view.Close();
                        tempList.RemoveAt(index);
                        break;
                    }
                }
            }

            if (tempList != null && tempList.Count == 0 && !m_UISettings.GetViewInfo(type).Recyclable)
            {
                ResourcesManager.Instance.Release(m_UISettings.UIDataDict[type].FullPath);
            }
        }

        /// <summary>
        /// 关闭全部通过UIManager打开的UI
        /// </summary>
        public void ClearViews() 
        {
            m_ViewDict.Clear();
        }

        #endregion

        #region UI Skip To

        /// <summary>
        /// 初始化跳转方法集合
        /// </summary>
        private void InitLinkFunctionsDict()
        {
            m_LinkFunctions = new Dictionary<string, Func<bool>>()
            {
                { ClientPos.Example.ToString(), LinkExampleClientPage}
            };
        }

        /// <summary>
        /// UI跳转
        /// </summary>
        public bool LinkPos(LinkType linkType, string linkValue)
        {
            if (linkType == LinkType.ClientPage)
            {
                if (m_LinkFunctions.ContainsKey(linkValue))
                {
                    return m_LinkFunctions[linkValue].Invoke();
                }
            }
            if (linkType == LinkType.ExternalUrl)
            {
                LinkExternalUrl(linkValue);
            }
            return false;
        }

        private bool LinkExampleClientPage()
        {
            if(m_LogEnabled)
                Debug.Log("[UIManager] pretend to go to example client page");

            return true;
        }

        private bool LinkExternalUrl(string linkValue)
        {
            Application.OpenURL(linkValue);
            return true;
        }

        #endregion
    }

    /// <summary>
    /// 跳转类型
    /// </summary>
    public enum LinkType
    {
        ClientPage,             //客户端页面
        ExternalUrl,            //外部url链接
        LocalUrl,               //本地url链接
        Facebook,               //Facebook
    }

    /// <summary>
    /// 跳转位置
    /// </summary>
    public enum ClientPos
    {
        Example
    }
}