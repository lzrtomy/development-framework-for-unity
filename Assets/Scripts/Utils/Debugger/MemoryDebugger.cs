
using UnityEngine;
using UnityEngine.Profiling;

public class MemoryDebugger : MonoBehaviour
{
    //Memory
    private string sUserMemory;
    private string s;
    public bool OnMemoryGUI;
    private long MonoUsedM;
    private long AllMemory;
    [Range(0, 200)]
    public int MaxMonoUsedM = 50;
    [Range(0, 400)]
    public int MaxAllMemory = 200;

    //测量间隔时间
    public float MemoryMeasuringDelta = 2.0f;
    private float timePassed;

    GUIStyle m_NormalStyle = new GUIStyle();
    GUIStyle m_WarningStyle = new GUIStyle();
    GUIStyle m_ErrorStyle = new GUIStyle();

    private void Awake()
    {
        SetGUIStyle();
    }

    void Update()
    {
        timePassed += Time.deltaTime;
        if (timePassed >= MemoryMeasuringDelta)
        {
            timePassed = 0;
            UpdateUsed();
        }
    }

    private void SetGUIStyle()
    {
        m_NormalStyle.normal.background = null;    //这是设置背景填充的
        m_NormalStyle.normal.textColor = Color.green;   //设置字体颜色的
        m_NormalStyle.fontSize = 40;       //当然，这是字体大小}

        m_WarningStyle.normal.background = null;    //这是设置背景填充的
        m_WarningStyle.normal.textColor = new Color32(255, 162, 0, 255);   //设置字体颜色的
        m_WarningStyle.fontSize = 40;       //当然，这是字体大小}

        m_ErrorStyle.normal.background = null;    //这是设置背景填充的
        m_ErrorStyle.normal.textColor = Color.red;   //设置字体颜色的
        m_ErrorStyle.fontSize = 40;       //当然，这是字体大小}
    }

    void UpdateUsed()
    {
        sUserMemory = "";
        MonoUsedM = Profiler.GetMonoHeapSizeLong() / 1000000;
        AllMemory = Profiler.GetTotalAllocatedMemoryLong() / 1000000;
        sUserMemory += "MonoUsed:" + MonoUsedM + "M" + "\n";
        sUserMemory += "AllMemory:" + AllMemory + "M" + "\n";
        sUserMemory += "UnUsedReserved:" + Profiler.GetTotalUnusedReservedMemoryLong() / 1000000 + "M" + "\n";
        s = "";
        s += " MonoHeap:" + Profiler.GetMonoHeapSizeLong() / 1000 + "k";
        s += " MonoUsed:" + Profiler.GetMonoUsedSizeLong() / 1000 + "k";
        s += " Allocated:" + Profiler.GetTotalAllocatedMemoryLong() / 1000 + "k";
        s += " Reserved:" + Profiler.GetTotalReservedMemoryLong() / 1000 + "k";
        s += " UnusedReserved:" + Profiler.GetTotalUnusedReservedMemoryLong() / 1000 + "k";
        s += " UsedHeap:" + Profiler.usedHeapSizeLong / 1000 + "k";
    }

    void OnGUI()
    {
        if (OnMemoryGUI)
        {
            if (AllMemory >= MaxAllMemory)
            {
                GUI.Label(new Rect(10, 10, Screen.width / 2 - 10, 2000), sUserMemory, m_ErrorStyle);
                return;
            }
            if (MonoUsedM >= MaxMonoUsedM)
            {
                GUI.Label(new Rect(10, 10, Screen.width / 2 - 10, 2000), sUserMemory, m_WarningStyle);
                return;
            }
            GUI.Label(new Rect(10, 10, Screen.width / 2 - 10, 2000), sUserMemory, m_NormalStyle);
        }
    }
}