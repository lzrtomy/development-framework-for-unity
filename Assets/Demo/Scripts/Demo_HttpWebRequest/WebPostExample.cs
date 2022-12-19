
using Company.HttpWebRequest;
using Company.Tools;
using System.Text;
using UnityEngine;

namespace Company.DevFramework.Demo
{
    /// <summary>
    /// 构造Post请求对象
    /// </summary>
    /// <param name="onGetComplete">请求成功回调</param>
    /// <param name="onGetFailed">请求失败回调</param>
    public class WebPostExample : HttpRequestBase
    {
        public WebPostExample(RequestCompleteHandler<PostExampleEntity> onGetComplete, RequestFailedHandler onGetFailed)
        {
            m_Task = new HttpRequestTask("PostExample", HttpRequestTask.HttpRequestType.POST, 1);
            m_CompleteEvent = onGetComplete;
            m_FailEvent = onGetFailed;
        }

        /// <summary>
        /// 发起Post请求
        /// </summary>
        public void Post()
        {
            //指定url
            string url = "example";

            //生成请求体json
            string bodyJson = CreateJson();

            //设定Post请求参数
            m_Task.SetRequestParams(url, Encoding.UTF8.GetBytes(bodyJson), IEPostRequest(m_Task));

            //发起请求
            UnityTools.Instance.StartIE(m_Task.IERequest);
        }

        /// <summary>
        /// 生成Json对象
        /// </summary>
        /// <returns></returns>
        private string CreateJson()
        {
            JSONObject root = new JSONObject();

            root.Add("example", "example");

            return root.ToString();
        }

        /// <summary>
        /// 抽象实现：Post请求成功的回调
        /// </summary>
        /// <param name="task"></param>
        /// <param name="remoteJson"></param>
        protected override void OnWebRequestComplete(HttpRequestTask task, string remoteJson)
        {
            PostExampleEntity entity = null;

            PostExampleResponse response = JsonUtility.FromJson<PostExampleResponse>(remoteJson);
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
        private PostExampleEntity CreateEntity(PostExampleResponse response)
        {
            PostExampleEntity entity = new PostExampleEntity();

            entity.Json = response.data;

            return entity;
        }

        #endregion
    }
}
