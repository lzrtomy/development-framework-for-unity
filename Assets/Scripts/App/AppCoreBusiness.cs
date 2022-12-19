
using Company.Constants;
using UnityEngine;

namespace Company.NewApp
{
    public class AppCoreBusiness : SimpleUnitySingleton<AppCoreBusiness>
    {
        private void Start()
        {
            Init();

            HotkeyManager.Instance.AddListener<int>(HotkeyEventName.Demo_Hotkey.OnDemoEvent5.ToString(), OnHotKeyEvent);
        }

        /// <summary>
        /// 核心业务逻辑入口
        /// </summary>
        public void Init()
        {
            Debug.Log("[AppCoreBusiness] GameStart");
        }

        private void OnHotKeyEvent(int num)
        {
            Debug.Log("num:" + num);
        }

        private void OnDestroy()
        {
            HotkeyManager.Instance.RemoveListener<int>(HotkeyEventName.Demo_Hotkey.OnDemoEvent5.ToString(), OnHotKeyEvent);
        }

    }
}