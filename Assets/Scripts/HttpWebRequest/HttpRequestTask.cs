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

    //请求名称
    public string RequestName = null;

    //请求类型
    public HttpRequestType RequestType = HttpRequestType.GET;

    //请求Url
    private string m_Url = null;

    //Post请求上传的参数
    private byte[] m_Body = null;

    //请求协程
    private IEnumerator m_IERequest = null;

    //失败重试次数
    private int m_MaxRetryNum = 2;

    //失败重试次数计数器
    private int m_RetryCounter = 0;

    //请求时长
    public double RequestTime = -1;

    //请求错误信息
    public HttpRequestError RequestError = new HttpRequestError();
    
    public IEnumerator IERequest { get { return m_IERequest; } }
    
    public string Url { get{ return m_Url; } }

    public byte[] Body { get { return m_Body; } }

    /// <summary>
    /// 设置请求所需的参数
    /// </summary>
    /// <param name="ieRequest"></param>
    public void SetRequestParams(IEnumerator ieRequest)
    {
        m_IERequest = ieRequest;
    }

    /// <summary>
    /// 设置请求所需的参数
    /// </summary>
    /// <param name="url"></param>
    /// <param name="ieRequest"></param>
    public void SetRequestParams(string url, IEnumerator ieRequest)
    {
        m_Url = url;
        m_IERequest = ieRequest;
    }

    /// <summary>
    /// 设置请求所需的参数
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
    /// 增加重试次数计数
    /// </summary>
    public void AddRetryCount()
    {
        m_RetryCounter++;
    }

    /// <summary>
    /// 是否可以重试
    /// </summary>
    /// <returns></returns>
    public bool IsRetryable()
    {
        return m_RetryCounter <= m_MaxRetryNum;
    }

    /// <summary>
    /// 将此任务添加到Manager中
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
    /// 从Manager中移除此任务
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
