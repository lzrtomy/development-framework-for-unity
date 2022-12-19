using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Company.NewApp.Views;
using Company.Tools;
using Company.Constants;
using Company.NewApp.Models;
using Company.NewApp.Entities;

namespace Company.NewApp.Presenters
{
    public class UISelectLevelPresenter : UIPresenterBase
    {
        [SerializeField] UISelectLevelView m_UISelectLevelView;

        public override void Init(params object[] parameters)
        {
            base.Init();

            m_UISelectLevelView.Init();
            m_UISelectLevelView.SetTipView("");
        }

        protected override void AddListeners()
        {
            EventManager.Instance.AddListener<string>(MyEventName.UISelectLevel.OnLoadLevel.ToString(), OnLoadLevel);
        }

        protected override void RemoveListeners()
        {
            EventManager.Instance.RemoveListener<string>(MyEventName.UISelectLevel.OnLoadLevel.ToString(), OnLoadLevel);
        }

        private void OnLoadLevel(string text) 
        {
            if (MathTools.IsIntNumberic(text))
            {
                int inputLevel = text.ToInt();
                m_UISelectLevelView.SetTipView("");

                bool result = AppManager.Instance.LoadLevel(inputLevel);
                if(!result)
                    m_UISelectLevelView.SetTipView("The input level does not exist in the Level.xlsx");
            }
            else
                m_UISelectLevelView.SetTipView("Please input integer!");
        }
    }
}