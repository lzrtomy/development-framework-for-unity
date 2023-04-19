using UnityEngine;
using Company.NewApp.Views;
using Company.Constants;

namespace Company.NewApp.Presenters
{
    public class UIExamplePresenter : UIPresenterBase
    {
        [SerializeField] UIExampleView m_UIExampleView;

        public override void Init(params object[] parameters)
        {
            base.Init(parameters);
            m_UIExampleView.Init();
        }

        protected override void AddListeners()
        {
            EventManager.Instance.AddListener<int>(MyEventName.UIGlobal.OnCloseViewById.ToString(), OnClose);
        }

        protected override void RemoveListeners()
        {
            EventManager.Instance.RemoveListener<int>(MyEventName.UIGlobal.OnCloseViewById.ToString(), OnClose);
        }

        protected override void OnClose(int viewId)
        {
            if (m_UIExampleView.ViewId != viewId)
                return;

            CloseViewPrefab(m_UIExampleView);
        }
    }
}