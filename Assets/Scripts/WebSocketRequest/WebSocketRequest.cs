/*

WebSocket请求类

*/

#if BEST_HTTP

using BestHTTP.WebSocket;

#endif

using Company.Constants;
using System;
using UnityEngine;

namespace Company.WebSocketRequest
{
    public class WebSocketRequest:IWebSocketMessageOperator
    {
        private string m_WebSocketId;
        public string WebSocketId { get { return m_WebSocketId; } }

#if BEST_HTTP

        private WebSocketRequestTask m_Task;
        public WebSocketRequestTask Task { get { return m_Task; } }


        private WebSocket m_WebSocket;
        public WebSocket WebSocket { get { return m_WebSocket; } }

#endif

        public WebSocketRequest(WebSocketRequestTask task)
        {
#if BEST_HTTP

            if (task == null)
            {
                return;
            }

            m_Task = task;
            m_WebSocketId = task.WebSocketId;
            m_WebSocket = new WebSocket(new Uri(task.Url));
            AddListener(task);
            
#else
            Debug.Log("[WebSocketRequestManager] " + Defines.Text.BEST_HTTP_NOT_AVALIABLE);
#endif
        }

        private void AddListener(WebSocketRequestTask task)
        {
#if BEST_HTTP

            WebSocket.OnOpen += task.OnOpen;
            WebSocket.OnMessage += task.OnMessage;
            WebSocket.OnBinary += task.OnBinary;
            WebSocket.OnError += task.OnError;
            WebSocket.OnClosed += task.OnClose;
            
#else
            Debug.LogError("[WebSocketRequestManager] " + Defines.Text.BEST_HTTP_NOT_AVALIABLE);
#endif
        }

        /// <summary>
        /// 开启WebSocket
        /// </summary>
        public void Open()
        {
#if BEST_HTTP

            if (WebSocket == null)
            {
                m_WebSocket = new WebSocket(new Uri(m_Task.Url));
            }
            if (!WebSocket.IsOpen)
            {
                WebSocket.Open();
            }

#else
            Debug.LogError("[WebSocketRequestManager] " + Defines.Text.BEST_HTTP_NOT_AVALIABLE);
#endif
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message">字符串消息</param>
        public void SendMessage(string message)
        {
#if BEST_HTTP

            if (WebSocket != null && WebSocket.IsOpen)
            {
                WebSocket.Send(message);
            }
            else
            {
                Debug.LogErrorFormat("[WebSocketRequest] Cannot send message at websocket id:-{0}-", m_Task?.WebSocketId);
            }

#else
            Debug.LogError("[WebSocketRequestManager] " + Defines.Text.BEST_HTTP_NOT_AVALIABLE);
#endif
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message">字节数组消息</param>
        public void SendMessage(byte[] buffer)
        {
#if BEST_HTTP

            if (WebSocket != null && WebSocket.IsOpen)
            {
                WebSocket.Send(buffer);
            }

#else
            Debug.LogError("[WebSocketRequestManager] " + Defines.Text.BEST_HTTP_NOT_AVALIABLE);
#endif
        }

        /// <summary>
        /// 关闭WebSocket
        /// webSocket实例对象被Close后，该对象便不能再被复用，因而置空
        /// </summary>
        public void Close()
        {
#if BEST_HTTP

            if (WebSocket != null && WebSocket.IsOpen)
            {
                Debug.LogFormat("[WebSocketRequest] Close at web socket id:-{0}-", m_Task?.WebSocketId);
                WebSocket.Close();
                m_WebSocket = null;
            }

#else
            Debug.LogError("[WebSocketRequestManager] " + Defines.Text.BEST_HTTP_NOT_AVALIABLE);
#endif
        }
    }
}