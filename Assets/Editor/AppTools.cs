using System.Collections;
using System.IO;
using UnityEditor;
using UnityEngine;
using Company.NewApp;

public class AppTools : Editor
{
    /// <summary>
    /// 清除编辑器模式下的PlayerPrefs
    /// </summary>
    [MenuItem("Tools/DeletePlayerPrefs")]
    public static void DeletePlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        Debug.LogWarning("[AppTools] Player Prefs is deleted!");
    }

    /// <summary>
    /// 创建游戏设置Asset文件
    /// </summary>
    [MenuItem("Tools/Create All App Settings Assets")]

    public static void CreateAllGameSettingsAssets()
    {
        CreateAppSettings();
        CreateAppAudioSettings();
        CreateUISettings();
    }

    public static void CreateAppSettings()
    {
        string directory = Application.dataPath + "/Resources/";
        string path = "Assets/Resources/AppSettings.asset";
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
        if (!File.Exists(path))
        {
            AppSettings scriptableObj = CreateInstance<AppSettings>();
            AssetDatabase.CreateAsset(scriptableObj, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.LogWarning("[AppTools] succeed to create AppSettings.asset!");
        }
        else 
        {
            Debug.LogWarning("[AppTools] AppSettings.asset has already existed!");
        }
    }

    /// <summary>
    /// 创建音频设置Asset文件
    /// </summary>
    public static void CreateAppAudioSettings()
    {
        string directory = Application.dataPath + "/Resources/";
        string path = "Assets/Resources/AppAudioSettings.asset";
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
        if (!File.Exists(path))
        {
            AppAudioSettings scriptableObj = CreateInstance<AppAudioSettings>();
            AssetDatabase.CreateAsset(scriptableObj, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.LogWarning("[AppTools] succeed to create AppAudioSettings.asset!");
        }
        else
        {
            Debug.LogWarning("[AppTools] AppAudioSettings.asset has already existed!");
        }
    }

    /// <summary>
    /// 创建UI设置Asset文件
    /// </summary>
    public static void CreateUISettings()
    {
        string directory = Application.dataPath + "/Resources/";
        string path = "Assets/Resources/UISettings.asset";
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
        if (!File.Exists(path))
        {
            UISettings scriptableObj = CreateInstance<UISettings>();
            AssetDatabase.CreateAsset(scriptableObj, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.LogWarning("[AppTools] succeed to create UISettings.asset!");
        }
        else
        {
            Debug.LogWarning("[AppTools] UISettings.asset has already existed!");
        }
    }

    /// <summary>
    /// 统计有效代码行数
    /// </summary>
    [MenuItem("Tools/Code Analysis")]
    private static void PrintTotalLine()
    {
        Debug.Log("============================== Code Analysis Start ==============================");

        string path = "Assets/Scripts";
        string[] fileName = Directory.GetFiles(path, "*.cs", SearchOption.AllDirectories);
        int totalLine = 0;
        for(int index = 1; index <= fileName.Length; index++)
        {
            int nowLine = 0;
            using (StreamReader sr = new StreamReader(fileName[index - 1]))
            {

                string line = sr.ReadLine();
                while (line != null)
                {
                    nowLine++;
                    line = sr.ReadLine();
                }
            }

            Debug.Log(string.Format("Number: {0}/{1}. Code line: {2}. Script name: {3}.", index, fileName.Length, nowLine, fileName[index-1]));

            totalLine += nowLine;
        }

        Debug.Log("============================== Result ==============================");

        Debug.Log(string.Format("Total code line: {0}. Script file quantity: {1}", totalLine, fileName.Length));

        Debug.Log("============================== Code Analysis Complete ==============================");
    }
}
