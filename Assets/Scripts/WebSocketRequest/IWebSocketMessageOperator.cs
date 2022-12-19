/*

对WebSocket连接进行消息操作的接口

*/

namespace Company.WebSocketRequest
{
    public interface IWebSocketMessageOperator
    {
        /// <summary>
        /// 发送字符串消息
        /// </summary>
        /// <param name="message"></param>
        void SendMessage(string message);

        /// <summary>
        /// 发送字节数组消息
        /// </summary>
        /// <param name="buffer"></param>
        void SendMessage(byte[] buffer);
    }
}