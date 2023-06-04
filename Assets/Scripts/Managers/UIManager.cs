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
        private RectTransform m_CanvasRect;

        private Dictionary<ViewType, List<UIViewBase>> m_ViewDict;

        //客户端页面跳转方法集合
        private Dictionary<string, Func<bool>> m_LinkFunctions;

        private UISettings m_UISettings;

        private bool m_LogEnabled;

        private RectTransform CanvasRect
        {
            get 
            {
                if (!m_CanvasRect) 
                {
                    m_CanvasRect = GameObject.Find("Canvas").GetComponent<RectTransform>();
                }
                return m_CanvasRect;
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
        /// <param name="t"></param>
        /// <returns></returns>
        public bool IsViewExist<T>(ViewType type, out T t) where T : UIViewBase
        {
            return IsViewExist<T>(type, type.ToString(), out t);
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

        public void Open<T>(ViewType type, Action<T> onOpen, params object[] initParams) where T : UIViewBase
        {
            Open<T>(type, type.ToString(), CanvasRect, onOpen, initParams);
        }

        public void Open<T>(ViewType type, params object[] initParams) where T : UIViewBase
        {
            Open<T>(type, type.ToString(), CanvasRect, null, initParams);
        }

        public void Open<T>(ViewType type, RectTransform parent, Action<T> onOpen, params object[] initParams) where T : UIViewBase
        {
            Open<T>(type, type.ToString(), parent, onOpen, initParams);
        }

        public void Open<T>(ViewType type, RectTransform parent, params object[] initParams) where T : UIViewBase
        {
            Open<T>(type, type.ToString(), parent, null, initParams);
        }

        public void Open<T>(ViewType type, string viewName, Transform parent, params object[] initParams) where T : UIViewBase
        {
            Open<T>(type, viewName, parent, null, initParams);
        }

        /// <summary>
        /// 加载并展示UI
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <param name="viewName"></param>
        /// <param name="parent"></param>
        /// <param name="onOpen"></param>
        /// <param name="initParams"></param>
        public void Open<T>(ViewType type, string viewName, Transform parent, Action<T> onOpen, params object[] initParams) where T : UIViewBase
        {
            T t = null;
            if (IsViewExist(type, viewName, out t))
            {
                Debug.LogError(string.Format("[UIManager] Cannot open new {0}, since it has already been existed!", typeof(T).Name));
                onOpen?.Invoke(t);
                return;
            }

            if (m_UISettings.UIDataDict[type].Recyclable)
            {
                GetViewFromPool<T>(type, viewName, parent, onOpen, initParams);
            }
            else
            {
                NewView<T>(type, viewName, parent, onOpen, initParams);
            }
        }

        /// <summary>
        /// 创建view
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private void NewView<T>(ViewType type, string viewName, Transform parent, Action<T> onOpen, params object[] initParams) where T : UIViewBase
        {
            ResourcesManager.Instance.Clone(m_UISettings.UIDataDict[type].FullPath, 
                (GameObject go) =>
                {
                    HandleView<T>(go, type, viewName, parent, onOpen, initParams);
                });
        }

        private void GetViewFromPool<T>(ViewType type, string viewName, Transform parent, Action<T> onOpen, params object[] initParams) where T : UIViewBase
        {
            ObjectPool pool = ObjectPool.Instance;
            string path = m_UISettings.UIDataDict[type].FullPath;
            pool.Preload(path, 
                () =>
                {
                    GameObject go = ObjectPool.Instance.Get(path);
                    HandleView<T>(go, type, viewName, parent, onOpen, initParams);
                });
        }

        private void HandleView<T>(GameObject viewGo, ViewType type, string viewName, Transform parent, Action<T> onOpen, params object[] initParams) where T : UIViewBase 
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
                onOpen?.Invoke(null);
                return;
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
            rect.SetParent(parent);
            rect.SetAsLastSibling();
            AdjustRectTransform(rect);

            onOpen?.Invoke(view);
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

            //四边锚定的适配方式
            if (rect.anchorMin == Vector2.zero && rect.anchorMax == Vector2.one)
            {
                rect.offsetMin = Vector2.zero;
                rect.offsetMax = Vector2.zero;
            }
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