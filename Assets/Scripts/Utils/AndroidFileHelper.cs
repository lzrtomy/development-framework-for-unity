using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class AndroidFileHelper
{

    //---------- FILE NO EXISTS ------------//
    // 从StreamingAsset中复制文件
    static public void CopyFileFromStreamingAsset(string file_name)
    {
        string from_path;
        if (Application.platform == RuntimePlatform.Android)
            from_path = "jar:file://" + Application.dataPath + "!/assets/" + file_name;
        else
            from_path = Application.streamingAssetsPath + "/" + file_name;

        string to_path = Application.persistentDataPath + "/" + file_name;
        Debug.LogWarning("Copy File To:" + to_path);
        WWW www = new WWW(from_path);
        while (!www.isDone) { }
        if (www.error == null)
            File.WriteAllBytes(to_path, www.bytes);
    }

    // 在PersistentData创建文件
    static public void CreateFileInPersistentData(string file_name)
    {
        string path = Application.persistentDataPath + "/" + file_name;
        Debug.LogWarning("Create File To:" + path);
        if (File.Exists(path)) return;
        FileStream fs = File.Create(path);
        fs.Close();
    }

    //------------ READ FILE START ----------//

    //读取文件
    static public string ReadFileOrCopy(string file_name)
    {

        string path = Application.persistentDataPath + "/" + file_name;
        Debug.Log(path);
        if (!File.Exists(path)) CopyFileFromStreamingAsset(file_name);
        return File.ReadAllText(path);
    }

    //按行读取文件
    static public string[] ReadFileLinesOrCopy(string file_name)
    {
        Debug.Log(Application.persistentDataPath);

        string path = Application.persistentDataPath + "/" + file_name;
        if (!File.Exists(path)) CopyFileFromStreamingAsset(file_name);
        return File.ReadAllLines(path);
    }

    //直接从StreamingAsset中读取文件
    static public string ReadFileFromStreamingAsset(string file_name)
    {
        string from_path;
        if (Application.platform == RuntimePlatform.Android)
            from_path = "jar:file://" + Application.dataPath + "!/assets/" + file_name;
        else
            from_path = Application.streamingAssetsPath + "/" + file_name;

        WWW www = new WWW(from_path);
        while (!www.isDone) { }
        if (www.error == null) return www.text;
        else return www.error;
    }

    //直接从StreamingAsset中按行读取文件
    static public string[] ReadFileLinesFromStreamingAsset(string file_name)
    {
        string str = ReadFileFromStreamingAsset(file_name);
        return str.Split("\n"[0]);
    }

    //读取文件，没有就创建一个空的
    static public string ReadFileOrCreat(string file_name)
    {
        string path = Application.persistentDataPath + "/" + file_name;

        if (!File.Exists(path)) CreateFileInPersistentData(file_name);
        return File.ReadAllText(path);
    }

    //按行读取文件，没有就创建一个空的
    static public string[] ReadFileLinesOrCreat(string file_name)
    {

        string path = Application.persistentDataPath + "/" + file_name;
        Debug.Log(path);
        if (!File.Exists(path)) CreateFileInPersistentData(file_name);
        return File.ReadAllLines(path);
    }
    //------------ WRITE FILE START ----------//

    // 更新文件
    static public void WriteFile(string file_name, string str)
    {
        string path = Application.persistentDataPath + "/" + file_name;
        File.WriteAllText(path, str);
    }

    //按行更新文件
    static public void WriteFileLines(string file_name, string[] str_list)
    {
        string path = Application.persistentDataPath + "/" + file_name;
        File.WriteAllLines(path, str_list);
    }

    //------------ DELETE FILE START --------//
    //删除目标文件
    static public void DeleteFile(string file_name)
    {
        string path = Application.persistentDataPath + "/" + file_name;

        File.Delete(path);
    }

    //清空数据
    static public void EmptyFile(string file_name)
    {
        CreateFileInPersistentData(file_name);
    }

}
