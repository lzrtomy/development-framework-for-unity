
using Company.HttpWebRequest;
using Company.Tools;
using System.Text;
using UnityEngine;

namespace Company.DevFramework.Demo
{
    /// <summary>
    /// ����Post�������
    /// </summary>
    /// <param name="onGetComplete">����ɹ��ص�</param>
    /// <param name="onGetFailed">����ʧ�ܻص�</param>
    public class WebPostExample : HttpRequestBase
    {
        public WebPostExample(RequestCompleteHandler<PostExampleEntity> onGetComplete, RequestFailedHandler onGetFailed)
        {
            m_Task = new HttpRequestTask("PostExample", HttpRequestTask.HttpRequestType.POST, 1);
            m_CompleteEvent = onGetComplete;
            m_FailEvent = onGetFailed;
        }

        /// <summary>
        /// ����Post����
        /// </summary>
        public void Post()
        {
            //ָ��url
            string url = "example";

            //����������json
            string bodyJson = CreateJson();

            //�趨Post�������
            m_Task.SetRequestParams(url, Encoding.UTF8.GetBytes(bodyJson), IEPostRequest(m_Task));

            //��������
            UnityTools.Instance.StartIE(m_Task.IERequest);
        }

        /// <summary>
        /// ����Json����
        /// </summary>
        /// <returns></returns>
        private string CreateJson()
        {
            JSONObject root = new JSONObject();

            root.Add("example", "example");

            return root.ToString();
        }

        /// <summary>
        /// ����ʵ�֣�Post����ɹ��Ļص�
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
        /// �������ݶ���
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
