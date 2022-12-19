/*

WebSocket请求管理类

*/

using System.Collections.Generic;

namespace Company.WebSocketRequest
{
    public class WebSocketRequestManager : UnitySingleton<WebSocketRequestManager>
    {
        private Dictionary<string, WebSocketRequest> m_WebSocketRequestDict = new Dictionary<string, WebSocketRequest>();
        
        public Dictionary<string, WebSocketRequest> WebSocketRequestDict { get { return m_WebSocketRequestDict; } }
        
        public void Init()
        {

        }

        /// <summary>
        /// 获取WebSocket
        /// </summary>
        /// <param name="webSocketId"></param>
        /// <returns></returns>
        public WebSocketRequest GetWebSocketRequest(string webSocketId)
        {
            WebSocketRequest request = null;
            WebSocketRequestDict.TryGetValue(webSocketId, out request);
            return request;
        }

        /// <summary>
        /// 建立WebSocket链接
        /// 每个WebSocketId只可以创建一个链接
        /// </summary>
        /// <param name="task">WebSocket请求任务</param>
        public void EstablishWebSocketConnection(WebSocketRequestTask task)
        {
            if (task != null && !WebSocketRequestDict.ContainsKey(task.WebSocketId))
            {
                WebSocketRequest request = new WebSocketRequest(task);
                request.Open();

                WebSocketRequestDict[task.WebSocketId] = request;
            }
        }

        /// <summary>
        /// 建立WebSocket链接
        /// 每个WebSocketId只可以创建一个链接
        /// </summary>
        /// <param name="task">WebSocket请求任务</param>
        /// <param name="messageOperator">WebSocket消息操作器</param>
        public void EstablishWebSocketConnection(WebSocketRequestTask task, out IWebSocketMessageOperator messageOperator)
        {
            messageOperator = null;

            if (task != null && !WebSocketRequestDict.ContainsKey(task.WebSocketId))
            {
                WebSocketRequest request = new WebSocketRequest(task);
                request.Open();

                WebSocketRequestDict[task.WebSocketId] = request;
                messageOperator = request;
            }
        }

        /// <summary>
        /// 断开WebSocket连接
        /// </summary>
        /// <param name="webSocketId"></param>
        public void CloseWebSocketConnection(string webSocketId)
        {
            WebSocketRequest request = GetWebSocketRequest(webSocketId);
            if (request != null)
            {
                request.Close();

                WebSocketRequestDict.Remove(webSocketId);
            }
        }

        /// <summary>
        /// 断开全部WebSocket连接
        /// </summary>
        public void CloseAllWebSocketConnections()
        {
            var it = WebSocketRequestDict.GetEnumerator();
            while (it.MoveNext())
            {
                it.Current.Value.Close();
            }

            WebSocketRequestDict.Clear();
        }
    }
}