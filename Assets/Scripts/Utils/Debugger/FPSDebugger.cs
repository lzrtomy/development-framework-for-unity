using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Company.NewApp;

public class FPSDebugger : MonoBehaviour
{
    //是否记录FPS
    public bool IsRecordFPS;
    //记录FPS的文件名
    public string RecordFileName = "";
    //测量间隔时间
    public float fpsMeasuringDelta = 2.0f;

    private float timePassed;

    //记录等待延迟（掐头）
    public float DelayTime = 0;
    //记录时长
    public float RecordTime = 0;
    //记录时长计时器
    private float RecordTimer = 0;
    private int m_FrameCount = 0;
    private float m_FPS = 0.0f;

    private float allFrame;
    private int allFrameCount = 0;

    GUIStyle m_GUIStyle = new GUIStyle();

    private StreamWriter sw;
    private string tip = "";

    private void Awake()
    {
        m_GUIStyle.normal.background = null;    //这是设置背景填充的
        //bb.normal.textColor = new Color(1.0f, .7f, 0.0f);   //设置字体颜色的
        m_GUIStyle.normal.textColor = Color.green;   //设置字体颜色的
        m_GUIStyle.fontSize = 40;       //当然，这是字体大小
    }

    private void Start()
    {
        //Application.targetFrameRate = 60;
        timePassed = 0.0f;

        if (IsRecordFPS)
        {
#if UNITY_EDITOR
            StartCoroutine(IERecordFPS());
#endif
        }
    }

    private void Update()
    {
        m_FrameCount = m_FrameCount + 1;
        timePassed = timePassed + Time.unscaledDeltaTime;//Time.deltaTime

        if (timePassed > fpsMeasuringDelta)
        {
            m_FPS = m_FrameCount / timePassed;
            allFrame += m_FPS;
            allFrameCount++;
            timePassed = 0.0f;
            m_FrameCount = 0;
        }
    }

    IEnumerator IERecordFPS() 
    {
        tip = "Waiting";
        yield return new WaitForSeconds(DelayTime);
        StartRecordFPS(RecordFileName);
        tip = "Recording";
        sw.WriteLine(RecordFileName);
        while (RecordTimer <= RecordTime) 
        {
            sw.WriteLine(m_FPS);
            yield return new WaitForSeconds(fpsMeasuringDelta);
            RecordTimer += fpsMeasuringDelta;
        }

        sw.Flush();
        sw.Close();
        tip = "Finish";
    }

    private void OnApplicationQuit()
    {
        //ADManager.ClickRecordString(Consts._00031, (allFrame / allFrameCount).ToString());
    }
    
    private void OnGUI()
    {
        //if(!Debug.isDebugBuild)
        //{
        //    return;
        //}

        

        //居中显示FPS
        GUI.Label(new Rect(Screen.width / 2 + 10, 10, Screen.width / 2, 200), "FPS: " + m_FPS + "\r\n" + tip + "  " + RecordTimer, m_GUIStyle);
    }


    private void StartRecordFPS(string name) 
    {
        string directory = FileManager.PersistentDataPath;
        string path = directory + "FPS_" + name + ".xls";
        if (!Directory.Exists(directory)) 
        {
            Directory.CreateDirectory(directory);
        }

        FileStream fs = File.Create(path);
        fs.Close();
        sw = File.AppendText(path);

    }
}
