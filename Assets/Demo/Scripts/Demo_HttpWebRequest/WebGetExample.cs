
using Company.HttpWebRequest;
using Company.Tools;
using UnityEngine;

namespace Company.DevFramework.Demo
{
    public class WebGetExample : HttpRequestBase
    {
        /// <summary>
        /// ����Get�������
        /// </summary>
        /// <param name="onGetComplete">����ɹ��ص�</param>
        /// <param name="onGetFailed">����ʧ�ܻص�</param>
        public WebGetExample(RequestCompleteHandler<GetExampleEntity> onGetComplete, RequestFailedHandler onGetFailed)
        {
            m_Task = new HttpRequestTask("GetExample", HttpRequestTask.HttpRequestType.GET);
            m_CompleteEvent = onGetComplete;
            m_FailEvent = onGetFailed;
        }

        /// <summary>
        /// ����Get����
        /// </summary>
        public void Get()
        {
            //ָ��url
            string url = "example";

            //�趨Post�������
            m_Task.SetRequestParams(url, IEGetRequest(m_Task));

            //��������
            UnityTools.Instance.StartIE(m_Task.IERequest);
        }

        /// <summary>
        /// ����ʵ�֣�Get����ɹ��Ļص�
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
        /// �������ݶ���
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
