using UnityEngine;
using UnityEngine.UI;
using Company.Constants;

namespace Company.NewApp.Views
{
    public class UIExampleView : UIViewBase
    {
        [SerializeField] Button m_BtnClose;

        public override void Init(params object[] paramters)
        {
            base.Init(paramters);

            m_BtnClose.onClick.RemoveAllListeners();
            m_BtnClose.onClick.AddListener(OnClickBtnClose);
        }

        private void OnClickBtnClose()
        {
            UIManager.Instance.Close(ViewType, ViewId);
        }

        public override void Close()
        {
            EventManager.Instance.Dispatch<int>(MyEventName.UIGlobal.OnCloseViewById.ToString(), ViewId);
        }
    }
}