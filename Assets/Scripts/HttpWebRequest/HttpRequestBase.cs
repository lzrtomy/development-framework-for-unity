using Company.Tools;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Company.NewApp;


namespace Company.HttpWebRequest
{
    public abstract class HttpRequestBase
    {
        //超时阈值
        protected const int TIME_OUT = 10;

        //请求任务
        protected HttpRequestTask m_Task = null;

        //请求成功回调
        public delegate void RequestCompleteHandler<T>(T result);
        protected Delegate m_CompleteEvent;

        //请求失败回调
        public delegate void RequestFailedHandler(long code, string message);
        protected RequestFailedHandler m_FailEvent;

        protected bool m_LogEnabled = true;

        public HttpRequestTask Task
        {
            get
            {
                return m_Task;
            }
        }

        public HttpRequestBase()
        {
            m_LogEnabled = AppSettings.Instance.LogEnabled;
        }

        /// <summary>
        ///发送 http 的GET请求
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        protected IEnumerator IEGetRequest(HttpRequestTask task)
        {
            if (task == null) { yield break; }

            task.RequestType = HttpRequestTask.HttpRequestType.GET;
            using (UnityWebRequest request = UnityWebRequest.Get(task.Url))
            {
                DateTime startTime = DateTime.UtcNow;
                request.timeout = TIME_OUT;

                yield return request.SendWebRequest();
                string remoteJson = request.downloadHandler.text;

#if UNITY_2020_1_OR_NEWER
                if(request.result != UnityWebRequest.Result.Success)
#else
                if (request.isHttpError || request.isNetworkError)
#endif
                {
                    if (m_LogEnabled)
                        Debug.LogError(string.Format("[WebRequestBase] {0} - GET requst error: {1}\r\nUrl: {2}\r\n{3}", task.RequestName, request.error, task.Url, remoteJson));

                    task.RequestError.Code = request.responseCode;
                    task.RequestError.Message = request.error;
                    OnWebRequestFailed(task);
                }
                else
                {
                    if (m_LogEnabled)
                        Debug.Log(string.Format("[WebRequestBase] {0} - GET requst complete: \r\nUrl: {1}\r\n{2}", task.RequestName, task.Url, remoteJson));

                    DateTime stopTime = DateTime.UtcNow;
                    task.RequestTime = (stopTime - startTime).TotalSeconds;
                    OnWebRequestComplete(task, remoteJson);
                }
            }
        }
        
        /// <summary>
        /// 发送 http 的POST请求
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        protected IEnumerator IEPostRequest(HttpRequestTask task)
        {
            if (task == null) { yield break; }

            task.RequestType = HttpRequestTask.HttpRequestType.POST;
            using (UnityWebRequest request = new UnityWebRequest(task.Url, task.RequestType.ToString()))
            {
                DateTime startTime = DateTime.UtcNow;
                if (task.Body != null && task.Body.Length > 0)
                {
                    request.uploadHandler = new UploadHandlerRaw(task.Body);
                }
                request.SetRequestHeader("Content-Type", "application/json");
                request.downloadHandler = new DownloadHandlerBuffer();
                request.timeout = TIME_OUT;

                yield return request.SendWebRequest();
                string remoteJson = request.downloadHandler.text;

#if UNITY_2020_1_OR_NEWER
                if (request.result != UnityWebRequest.Result.Success)
#else
                if (request.isHttpError || request.isNetworkError)
#endif
                {
                    if (m_LogEnabled)
                        Debug.LogError(string.Format("[PostRequestBase] {0} - Post requst error: {1}\r\nUrl: {2}\r\n{3}", task.RequestName, request.error, task.Url, remoteJson));

                    task.RequestError.Code = request.responseCode;
                    task.RequestError.Message = request.error;
                    OnWebRequestFailed(task);
                }
                else
                {
                    if (m_LogEnabled)
                        Debug.Log(string.Format("[PostRequestBase] {0} - Post requst complete: \r\nUrl: {1}\r\n{2}", task.RequestName, task.Url, remoteJson));

                    DateTime stopTime = DateTime.UtcNow;
                    task.RequestTime = (stopTime - startTime).TotalSeconds;
                    OnWebRequestComplete(task, remoteJson);
                }
            }
        }

        #region Handle Web Request Complete

        /// <summary>
        /// 请求成功
        /// </summary>
        /// <param name="task"></param>
        /// <param name="remoteJson"></param>
        protected abstract void OnWebRequestComplete(HttpRequestTask task, string remoteJson);

        /// <summary>
        /// 发送请求成功的事件
        /// </summary>
        /// <param name="result"></param>
        protected virtual void SendCompleteEvent<T>(T result) 
        {
            RequestCompleteHandler<T> onRequestComplete = m_CompleteEvent as RequestCompleteHandler<T>;
            onRequestComplete?.Invoke(result);

            m_Task.RemoveTaskFromManager();
        }

        #endregion

        #region Handle Web Reqeust Failed

        /// <summary>
        /// 请求失败
        /// </summary>
        /// <param name="task"></param>
        protected virtual void OnWebRequestFailed(HttpRequestTask task)
        {
            task.AddRetryCount();
            if (task.IERequest != null)
            {
                UnityTools.Instance.StopIE(task.IERequest);
            }
            if (task.IsRetryable())
            {
                if (m_Task.RequestType == HttpRequestTask.HttpRequestType.GET)
                {
                    task.SetRequestParams(IEGetRequest(task));
                }
                else if (m_Task.RequestType == HttpRequestTask.HttpRequestType.POST)
                {
                    task.SetRequestParams(IEPostRequest(task));
                }
                UnityTools.Instance.StartIE(task.IERequest);
            }
            else
            {
                SendFailEvent(task.RequestError.Code, task.RequestError.Message);
            }
        }

        /// <summary>
        ///发送请求失败的事件
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        protected virtual void SendFailEvent(long code, string message)
        {
            m_FailEvent?.Invoke(code, message);

            m_Task.RemoveTaskFromManager();
        }

#endregion
        
    }
}