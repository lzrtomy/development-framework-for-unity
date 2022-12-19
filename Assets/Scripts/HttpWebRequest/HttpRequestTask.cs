using Company.HttpWebRequest;
using System.Collections;

public class HttpRequestTask
{
    public class HttpRequestError
    {
        public long Code = -1;
        public string Message = "Empty";
    }

    public enum HttpRequestType
    {
        GET,
        POST
    }

    public HttpRequestTask(HttpRequestType requestType)
    {
        RequestType = requestType;
        AddTaskToManager();
    }

    public HttpRequestTask(string requestName, HttpRequestType requestType)
    {
        RequestName = requestName;
        RequestType = requestType;
        AddTaskToManager();
    }

    public HttpRequestTask(string requestName, HttpRequestType requestType, int maxRetryNum)
    {
        RequestName = requestName;
        RequestType = requestType;
        m_MaxRetryNum = maxRetryNum;
        AddTaskToManager();
    }

    //��������
    public string RequestName = null;

    //��������
    public HttpRequestType RequestType = HttpRequestType.GET;

    //����Url
    private string m_Url = null;

    //Post�����ϴ��Ĳ���
    private byte[] m_Body = null;

    //����Э��
    private IEnumerator m_IERequest = null;

    //ʧ�����Դ���
    private int m_MaxRetryNum = 2;

    //ʧ�����Դ���������
    private int m_RetryCounter = 0;

    //����ʱ��
    public double RequestTime = -1;

    //���������Ϣ
    public HttpRequestError RequestError = new HttpRequestError();
    
    public IEnumerator IERequest { get { return m_IERequest; } }
    
    public string Url { get{ return m_Url; } }

    public byte[] Body { get { return m_Body; } }

    /// <summary>
    /// ������������Ĳ���
    /// </summary>
    /// <param name="ieRequest"></param>
    public void SetRequestParams(IEnumerator ieRequest)
    {
        m_IERequest = ieRequest;
    }

    /// <summary>
    /// ������������Ĳ���
    /// </summary>
    /// <param name="url"></param>
    /// <param name="ieRequest"></param>
    public void SetRequestParams(string url, IEnumerator ieRequest)
    {
        m_Url = url;
        m_IERequest = ieRequest;
    }

    /// <summary>
    /// ������������Ĳ���
    /// </summary>
    /// <param name="url"></param>
    /// <param name="requestBody"></param>
    /// <param name="ieRequest"></param>
    public void SetRequestParams(string url, byte[] requestBody, IEnumerator ieRequest)
    {
        m_Url = url;
        m_Body = requestBody;
        m_IERequest = ieRequest;
    }

    /// <summary>
    /// �������Դ�������
    /// </summary>
    public void AddRetryCount()
    {
        m_RetryCounter++;
    }

    /// <summary>
    /// �Ƿ��������
    /// </summary>
    /// <returns></returns>
    public bool IsRetryable()
    {
        return m_RetryCounter <= m_MaxRetryNum;
    }

    /// <summary>
    /// ����������ӵ�Manager��
    /// </summary>
    public void AddTaskToManager()
    {
        if (RequestType == HttpRequestType.GET)
        {
            HttpRequestManager.Instance.AddGetRequestTask(this);
        }
        else
        {
            HttpRequestManager.Instance.AddPostRequestTask(this);
        }
    }

    /// <summary>
    /// ��Manager���Ƴ�������
    /// </summary>
    public void RemoveTaskFromManager()
    {
        if (RequestType == HttpRequestType.GET)
        {
            HttpRequestManager.Instance.RemoveGetRequestTask(this);
        }
        else
        {
            HttpRequestManager.Instance.RemovePostRequestTask(this);
        }
    }
}
