
using Company.HttpWebRequest;
using Company.Tools;
using UnityEngine;

namespace Company.DevFramework.Demo
{
    public class WebGetExample : HttpRequestBase
    {
        /// <summary>
        /// 构造Get请求对象
        /// </summary>
        /// <param name="onGetComplete">请求成功回调</param>
        /// <param name="onGetFailed">请求失败回调</param>
        public WebGetExample(RequestCompleteHandler<GetExampleEntity> onGetComplete, RequestFailedHandler onGetFailed)
        {
            m_Task = new HttpRequestTask("GetExample", HttpRequestTask.HttpRequestType.GET);
            m_CompleteEvent = onGetComplete;
            m_FailEvent = onGetFailed;
        }

        /// <summary>
        /// 发起Get请求
        /// </summary>
        public void Get()
        {
            //指定url
            string url = "example";

            //设定Post请求参数
            m_Task.SetRequestParams(url, IEGetRequest(m_Task));

            //发起请求
            UnityTools.Instance.StartIE(m_Task.IERequest);
        }

        /// <summary>
        /// 抽象实现：Get请求成功的回调
        /// </summary>
        /// <param name="task"></param>
        /// <param name="remoteJson"></param>
        protected override void OnWebRequestComplete(HttpRequestTask task, string remoteJson)
        {
            GetExampleEntity entity = null;

            GetExampleResponse response = JsonUtility.FromJson<GetExampleResponse>(remoteJson);
            if (response != null)
            {
                entity = CreateEntity(response);
            }

            SendCompleteEvent(entity);
        }


        #region Create Entity from response object

        /// <summary>
        /// 创建数据对象
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        private GetExampleEntity CreateEntity(GetExampleResponse response)
        {
            GetExampleEntity entity = new GetExampleEntity();

            entity.Json = response.data;

            return entity;
        }

        #endregion
    }
}
