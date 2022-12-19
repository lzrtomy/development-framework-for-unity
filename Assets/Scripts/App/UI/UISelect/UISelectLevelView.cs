using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Company.Constants;

namespace Company.NewApp.Views
{
    public class UISelectLevelView : UIViewBase
    {
        [SerializeField] InputField m_LevelInputField;
        [SerializeField] Button m_BtnLoadLevel;
        [SerializeField] Text m_TxtLoadLevelTip;

        public override void Init(params object[] paramters)
        {
            m_BtnLoadLevel.onClick.AddListener(OnClickLoadLevelButton);
        }

        private void OnClickLoadLevelButton() 
        {
            EventManager.Instance.Dispatch(MyEventName.UISelectLevel.OnLoadLevel.ToString(), m_LevelInputField.text);
        }

        public void SetTipView(string text) 
        {
            m_TxtLoadLevelTip.text = text;
        }
    }
}