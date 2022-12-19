using Company.NewApp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Company.DevFramework
{
    public class DemoHub : MonoBehaviour
    {
        [SerializeField] Button m_BtnDemoHotkey;

        [SerializeField] Button m_BtnDemoObjectPool;

        [SerializeField] Button m_BtnDemoUIFramework;

        [SerializeField] Button m_BtnDemoHttpWebRequest;

        [SerializeField] Button m_DemoWebSocketRequest;

        [SerializeField] Button m_DemoAudio;

        private void Start()
        {
            m_BtnDemoHotkey.onClick.AddListener(OnShowDemoHotkey);
            m_BtnDemoObjectPool.onClick.AddListener(OnShowDemoObjectPool);
            m_BtnDemoUIFramework.onClick.AddListener(OnShowDemoUIFramework);
            m_BtnDemoHttpWebRequest.onClick.AddListener(OnShowDemoHttpWebRequest);
            m_DemoWebSocketRequest.onClick.AddListener(OnShowDemoWebSocketRequest);
            m_DemoAudio.onClick.AddListener(OnShowDemoAudio);
        }

        private void OnShowDemoHotkey()
        {
            SceneManager.LoadScene("Demo_Hotkey");
        }

        private void OnShowDemoObjectPool()
        {
            SceneManager.LoadScene("Demo_ObjectPool");
        }

        private void OnShowDemoUIFramework()
        {
            SceneManager.LoadScene("Demo_UIFramework");
        }

        private void OnShowDemoHttpWebRequest()
        {
            SceneManager.LoadScene("Demo_HttpWebRequest");
        }

        private void OnShowDemoWebSocketRequest()
        {
            SceneManager.LoadScene("Demo_WebSocketRequest");
        }

        private void OnShowDemoAudio()
        {
            SceneManager.LoadScene("Demo_Audio");
        }

    }
}