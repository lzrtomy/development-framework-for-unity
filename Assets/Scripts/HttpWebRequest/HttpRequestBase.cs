using Company.Tools;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Company.NewApp;
using System.Collections.Generic;

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
        public delegate void RequestCompleteHandler<T1, T2>(T1 result1, T2 result2);
        public delegate void RequestCompleteHandler<T1, T2, T3>(T1 result1, T2 result2, T3 result3);
        protected Delegate m_CompleteEvent;

        //请求失败回调
        public delegate void RequestFailedHandler(long code, string message);
        protected RequestFailedHandler m_FailEvent;

        protected bool m_LogEnabled = true;

        protected Dictionary<string, string> m_RequestHeaderDict = new Dictionary<string, string>();

        private const string GET_COMPLETE = "[{0}] {1} - GET requst complete: \r\nUrl: {2}\r\n{3}";
        private const string GET_ERROR = "[{0}] {1} - GET requst error: {2}\r\nUrl: {3}\r\n{4}";
        private const string POST_COMPLETE = "[{0}] {1} - Post requst complete: \r\nUrl: {2}\r\n{3}";
        private const string POST_ERROR = "[{0}] {1} - Post requst error: {2}\r\nUrl: {3}\r\n{4}";
        private const string EMPTY_COMP_EVENT = "[{0}] Empty complete event!";
        private const string FAIL_SEND_COMP_EVENT = "[{0}] Failed to send complete event, since event type is wrong!";

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

                SetRequestHeader(request, m_RequestHeaderDict);
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
                        Debug.LogError(string.Format(GET_ERROR, GetType().Name, task.RequestName, request.error, task.Url, remoteJson));

                    task.RequestError.Code = request.responseCode;
                    task.RequestError.Message = request.error;
                    OnWebRequestFailed(task);
                }
                else
                {
                    if (m_LogEnabled)
                        Debug.Log(string.Format(GET_COMPLETE, GetType().Name, task.RequestName, task.Url, remoteJson));

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

                SetRequestHeader(request, m_RequestHeaderDict);
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
                        Debug.LogError(string.Format(POST_ERROR, GetType().Name, task.RequestName, request.error, task.Url, remoteJson));

                    task.RequestError.Code = request.responseCode;
                    task.RequestError.Message = request.error;
                    OnWebRequestFailed(task);
                }
                else
                {
                    if (m_LogEnabled)
                        Debug.Log(string.Format(POST_COMPLETE, GetType().Name, task.RequestName, task.Url, remoteJson));

                    DateTime stopTime = DateTime.UtcNow;
                    task.RequestTime = (stopTime - startTime).TotalSeconds;
                    OnWebRequestComplete(task, remoteJson);
                }
            }
        }

        private void SetRequestHeader(UnityWebRequest request, Dictionary<string, string> dict)
        {
            if (dict.Count == 0)
                return;

            var it = dict.GetEnumerator();
            while (it.MoveNext())
            {
                request.SetRequestHeader(it.Current.Key, it.Current.Value);
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
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        protected virtual void SendCompleteEvent<T>(T result) 
        {
            if(m_CompleteEvent == null)
            {
                Debug.LogErrorFormat(EMPTY_COMP_EVENT, GetType().Name);
                return;
            }

            if (m_CompleteEvent is RequestCompleteHandler<T>)
            {
                RequestCompleteHandler<T> onRequestComplete = m_CompleteEvent as RequestCompleteHandler<T>;
                onRequestComplete?.Invoke(result);
            }
            else
                Debug.LogErrorFormat(FAIL_SEND_COMP_EVENT, GetType().Name);
            m_Task.RemoveTaskFromManager();
        }

        /// <summary>
        /// 发送请求成功的事件
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="result1"></param>
        /// <param name="result2"></param>
        protected virtual void SendCompleteEvent<T1, T2>(T1 result1, T2 result2)
        {
            if (m_CompleteEvent == null)
            {
                Debug.LogErrorFormat(EMPTY_COMP_EVENT, GetType().Name);
                return;
            }

            if (m_CompleteEvent is RequestCompleteHandler<T1, T2>)
            {
                RequestCompleteHandler<T1, T2> onRequestComplete = m_CompleteEvent as RequestCompleteHandler<T1, T2>;
                onRequestComplete?.Invoke(result1, result2);
            }
            else
                Debug.LogErrorFormat(FAIL_SEND_COMP_EVENT, GetType().Name);
            m_Task.RemoveTaskFromManager();
        }

        /// <summary>
        /// 发送请求成功的事件
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <param name="result1"></param>
        /// <param name="result2"></param>
        /// <param name="result3"></param>
        protected virtual void SendCompleteEvent<T1, T2, T3>(T1 result1, T2 result2, T3 result3)
        {
            if (m_CompleteEvent == null)
            {
                Debug.LogErrorFormat(EMPTY_COMP_EVENT, GetType().Name);
                return;
            }

            if (m_CompleteEvent is RequestCompleteHandler<T1, T2, T3>)
            {
                RequestCompleteHandler<T1, T2, T3> onRequestComplete = m_CompleteEvent as RequestCompleteHandler<T1, T2, T3>;
                onRequestComplete?.Invoke(result1, result2, result3);
            }
            else
                Debug.LogErrorFormat(FAIL_SEND_COMP_EVENT, GetType().Name);
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