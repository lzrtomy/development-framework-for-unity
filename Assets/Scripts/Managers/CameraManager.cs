using UnityEngine;

public class CameraManager : UnitySingleton<CameraManager>
{
    private const float m_StandardWidth = 1080;
    private const float m_StandardHeight = 1920;
    private Camera m_MainCamera = null;
    private RectTransform m_CanvasTrans = null;

    //1080 * 0.00520833
    private float m_StandardCannvasWidthInSpaceWorld = 10f;
    

    private float m_StandardOrthographicSize = 5;

    public void Init()
    {

    }

    /// <summary>
    /// 校准正交投影大小
    /// </summary>
    public void CalibrateCamera() 
    {
        CalibrateCamera(Vector3.zero);
    }
    
    public void CalibrateCamera(Vector3 newPosition)
    {
        m_MainCamera = Camera.main;
        GameObject canvasGo = GameObject.Find("Canvas");

        if (m_MainCamera && canvasGo)
        {
            m_CanvasTrans = canvasGo.GetComponent<RectTransform>();

            //标准情况下宽高比
            float m_StandardRatio = m_StandardWidth / m_StandardHeight;
            if (Screen.width < 720)
            {
                //计算当前分辨率下Size，公式：标准宽高比 * 标准Size = 当前设备宽高比 * 当前Size
                m_MainCamera.orthographicSize = m_StandardOrthographicSize;
            }
            else
            {
                //计算当前分辨率下Size，公式：标准宽高比 * 标准Size = 当前设备宽高比 * 当前Size
                m_MainCamera.orthographicSize = (m_StandardOrthographicSize * m_StandardRatio) / ((float)Screen.width / Screen.height);
            }

            m_MainCamera.transform.parent.position = newPosition;

        }
    }
}
