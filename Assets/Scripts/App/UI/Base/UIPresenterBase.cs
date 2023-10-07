using System.Collections;
using System.Collections.Generic;
using Company.NewApp.Views;
using UnityEngine;

namespace Company.NewApp.Presenters
{
    public abstract class UIPresenterBase : MonoBehaviour
    {
        //Presenter初始化进度
        protected InitState m_InitState = InitState.NotInit;

        public InitState InitState { get { return m_InitState; } }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="parameters"></param>
        public virtual void Init(params object[] parameters)
        {
            AddListeners();
        }

        /// <summary>
        /// 注册事件
        /// </summary>
        protected abstract void AddListeners();

        /// <summary>
        /// 注销事件
        /// </summary>
        protected abstract void RemoveListeners();


        protected IEnumerator IECheckPreloadUIAsset(ViewType type)
        {
            bool isPreload = false;
            WaitUntil waitLoad = new WaitUntil(() => isPreload);

            UIManager.Instance.CheckPreloadUIAsset(type, () => isPreload = true);

            yield return waitLoad;
        }

        protected IEnumerator IECheckPreloadUIAssets(List<ViewType> typeList)
        {
            bool isPreload = false;
            WaitUntil waitLoad = new WaitUntil(() => isPreload);

            UIManager.Instance.CheckPreloadUIAssets(typeList, () => isPreload = true);

            yield return waitLoad;
        }


        /// <summary>
        /// 关闭UI
        /// </summary>
        protected virtual void OnClose(int viewId)
        {
            //TODO 处理关闭UI的逻辑
        }

        /// <summary>
        /// 关闭UI的预制体
        /// </summary>
        /// <param name="view"></param>
        protected void CloseViewPrefab(UIViewBase view)
        {
            if (!view)
                return;

            ViewInfo info = UISettings.Instance.UIDataDict[view.ViewType];
            if (info.Recyclable)
            {
                ObjectPool.Instance.Return(info.FullPath, view.GameObject);
            }
            else
            {
                Destroy(view.GameObject);
            }
            RemoveListeners();
        }
    }
}