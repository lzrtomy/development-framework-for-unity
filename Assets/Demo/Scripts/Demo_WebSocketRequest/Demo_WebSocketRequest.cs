
#if BEST_HTTP

using BestHTTP.WebSocket;

#endif

using UnityEngine;
using UnityEngine.UI;
using Company.WebSocketRequest;
using System;
using Company.Constants;

namespace Company.DevFramework.Demo
{
    public class Demo_WebSocketRequest : MonoBehaviour
    {
        [Header("建立WebSocket请求按钮")]
        [SerializeField] Button m_BtnEstablishWebSocket;

        [Header("发送消息")]
        [SerializeField]
        Button m_BtnSendMessage;

        [Header("关闭WebSocket请求按钮")]
        [SerializeField] Button m_BtnCloseWebSocket;

        [Header("提示文字")]
        [SerializeField] Text m_TxtTip;


        private IWebSocketMessageOperator m_WebSocketMessageOperator = null;

        private void Start()
        {
#if BEST_HTTP

            m_BtnEstablishWebSocket.onClick.AddListener(OnEstablishWebSocketRequest);
            m_BtnCloseWebSocket.onClick.AddListener(OnCloseWebSocketRequest);
            m_BtnSendMessage.onClick.AddListener(OnSendMessage);

#else
            m_TxtTip.text = Defines.Text.BEST_HTTP_NOT_AVALIABLE;
            Debug.LogError("[Demo_WebSocketRequest] " + Defines.Text.BEST_HTTP_NOT_AVALIABLE);
#endif
        }

#if BEST_HTTP

        #region Button Functions

        private void OnEstablishWebSocketRequest()
        {
            WebSocketRequestManager.Instance.EstablishWebSocketConnection(new WebSocketRequestTask()
            {
                WebSocketId = "DemoWebSocketRequest",
                Url = "ws://121.40.165.18:8800",
                OnOpen = OnOpen,
                OnMessage = OnMessage,
                OnError = OnError,
                OnClose = OnClose
            }, out m_WebSocketMessageOperator);
        }

        private void OnCloseWebSocketRequest()
        {
            WebSocketRequestManager.Instance.CloseWebSocketConnection("DemoWebSocketRequest");
        }

        private void OnSendMessage()
        {
            m_WebSocketMessageOperator.SendMessage("Hello World!");
            Debug.Log("[DemoWebSocketRequest] Send message:\r\n Hello World");
        }

        #endregion

        #region WebSocketRequest Functions

        private void OnOpen(WebSocket webSocket)
        {
            Debug.Log("[DemoWebSocketRequest] On open!");
        }

        private void OnMessage(WebSocket webSocket, string message)
        {
            Debug.Log("[DemoWebSocketRequest] On message:\r\n" + message);
        }

        private void OnError(WebSocket webSocket, string message)
        {
            Debug.Log("[DemoWebSocketRequest] On error:\r\n" + message);
        }

        private void OnClose(WebSocket webSocket, UInt16 code, string message)
        {
            Debug.Log("[DemoWebSocketRequest] On close:\r\n" + message);
        }

        #endregion

#endif

        private void OnDisable()
        {
            m_BtnCloseWebSocket.onClick.RemoveAllListeners();
            m_BtnEstablishWebSocket.onClick.RemoveAllListeners();
            m_BtnSendMessage.onClick.RemoveAllListeners();
        }
    }
}