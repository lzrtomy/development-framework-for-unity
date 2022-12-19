/*

WebSocket请求任务

*/

#if BEST_HTTP

using BestHTTP.WebSocket;

#endif

namespace Company.WebSocketRequest
{
    public class WebSocketRequestTask
    {
        //WebSocket请求Id
        public string WebSocketId = null;

        //WebSocket请求地址
        public string Url = null;

#if BEST_HTTP

        //成功建立连接时触发的回调
        public OnWebSocketOpenDelegate OnOpen;

        //接收到字符串消息时触发的回调
        public OnWebSocketMessageDelegate OnMessage;

        //接收到二进制消息时触发的回调
        public OnWebSocketBinaryDelegate OnBinary;

        //发生异常断开连接的回调，OnError被回调的时候，则不再触发OnClose的回调
        public OnWebSocketErrorDelegate OnError;

        //断开连接时候触发的回调
        public OnWebSocketClosedDelegate OnClose;

#endif

    }
}