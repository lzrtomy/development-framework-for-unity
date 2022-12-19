using Company.Constants;
using Company.NewApp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Company.DevFramework
{
    public class Demo_Hotkey : MonoBehaviour
    {
        [SerializeField] Text m_TxtTip;

        // Start is called before the first frame update
        void Start()
        {
            HotkeyManager.Instance.AddListener<int>(HotkeyEventName.Demo_Hotkey.OnDemoEvent1.ToString(), OnDemoEvent1);
            HotkeyManager.Instance.AddListener<string>(HotkeyEventName.Demo_Hotkey.OnDemoEvent2.ToString(), OnDemoEvent2);
            HotkeyManager.Instance.AddListener<int>(HotkeyEventName.Demo_Hotkey.OnDemoEvent3.ToString(), OnDemoEvent3);
            HotkeyManager.Instance.AddListener<string>(HotkeyEventName.Demo_Hotkey.OnDemoEvent4.ToString(), OnDemoEvent4);
            HotkeyManager.Instance.AddListener(HotkeyEventName.Demo_Hotkey.OnDemoEvent5.ToString(), OnDemoEvent5);
        }

        private void OnDemoEvent1(int num)
        {
            m_TxtTip.text = num.ToString();
        }

        private void OnDemoEvent2(string alphabet)
        {
            m_TxtTip.text = alphabet;
        }

        private void OnDemoEvent3(int num)
        {
            m_TxtTip.text = string.Format("LeftAlt + {0}", num.ToString());
        }

        private void OnDemoEvent4(string alphabet)
        {
            m_TxtTip.text = string.Format("LeftAlt + {0}", alphabet);
        }

        private void OnDemoEvent5()
        {
            m_TxtTip.text = "LeftAlt + Space + AnyKey";
        }

        private void OnDestroy()
        {
            HotkeyManager.Instance.RemoveListener<int>(HotkeyEventName.Demo_Hotkey.OnDemoEvent1.ToString(), OnDemoEvent1);
            HotkeyManager.Instance.RemoveListener<string>(HotkeyEventName.Demo_Hotkey.OnDemoEvent2.ToString(), OnDemoEvent2);
            HotkeyManager.Instance.RemoveListener<int>(HotkeyEventName.Demo_Hotkey.OnDemoEvent3.ToString(), OnDemoEvent3);
            HotkeyManager.Instance.RemoveListener<string>(HotkeyEventName.Demo_Hotkey.OnDemoEvent4.ToString(), OnDemoEvent4);
            HotkeyManager.Instance.RemoveListener(HotkeyEventName.Demo_Hotkey.OnDemoEvent5.ToString(), OnDemoEvent5);
        }
    }
}