using Company.Attributes;
using System.Collections.Generic;
using UnityEngine;

namespace Company.NewApp.Views
{
    public abstract class UIViewBase : MonoBehaviour
    {
        protected GameObject m_Go;
        protected RectTransform m_RectTrans;

        [ReadOnly]
        [Header("[只读]视图名称")]
        [SerializeField] protected string m_ViewName;

        [ReadOnly]
        [Header("[只读]视图ID，用于区分UI")]
        [SerializeField] protected int m_ViewId;

        [ReadOnly]
        [Header("[只读]视图种类，用于确定复用的UI预制体")]
        [SerializeField] protected ViewType m_ViewType = ViewType.None;

        public GameObject GameObject 
        { 
            get 
            {
                if (!m_Go) 
                {
                    m_Go = gameObject;
                }
                return m_Go;
            } 
        }
        public RectTransform RectTransform 
        { 
            get 
            {
                if (!m_RectTrans) 
                {
                    m_RectTrans = GetComponent<RectTransform>();
                }
                return m_RectTrans; 
            }
        }

        public string ViewName { get { return m_ViewName; } }

        public int ViewId { get { return m_ViewId; } }

        public ViewType ViewType { get { return m_ViewType; } }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="paramters"></param>
        public virtual void Init(params object[] paramters)
        {
            m_ViewId = GetInstanceID();
        }

        /// <summary>
        /// 设定视图种类
        /// </summary>
        /// <param name="viewType"></param>
        public virtual void SetViewType(ViewType viewType) { m_ViewType = viewType; }

        /// <summary>
        /// 设定视图名称
        /// </summary>
        /// <param name="viewName"></param>
        public virtual void SetViewName(string viewName) { m_ViewName = viewName; }

        /// <summary>
        /// 展示
        /// </summary>
        public virtual void Open()
        {

        }

        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="o"></param>
        public virtual void Close()
        {
            //TODO EventManager.Instance.Dispatch(myEventName);
        }

        /// <summary>
        /// 设置对象可见性
        /// </summary>
        /// <param name="controlledGos"></param>
        /// <param name="gos"></param>
        protected void SetGosVisible(GameObject[] controlledGos, params GameObject[] gos)
        {
            if (controlledGos != null && controlledGos.Length > 0)
            {
                for (int index = 0; index < controlledGos.Length; index++)
                {
                    controlledGos[index]?.SetActive(false);
                }
            }
            if (gos != null && gos.Length > 0)
            {
                for (int index = 0; index < gos.Length; index++)
                {
                    gos[index]?.SetActive(true);
                }
            }
        }
        protected void SetGosVisible(List<GameObject> controlledGos, params GameObject[] gos)
        {
            if (controlledGos != null && controlledGos.Count > 0)
            {
                for (int index = 0; index < controlledGos.Count; index++)
                {
                    controlledGos[index]?.SetActive(false);
                }
            }
            if (gos != null && gos.Length > 0)
            {
                for (int index = 0; index < gos.Length; index++)
                {
                    gos[index]?.SetActive(true);
                }
            }
        }
    }
}