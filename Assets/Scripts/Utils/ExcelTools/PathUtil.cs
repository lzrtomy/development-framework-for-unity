using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PathUtil
{
    /// <summary>
    /// 获取文件的路径
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string GetDirectoryName(string path)
    {
        return Path.GetDirectoryName(path);
    }

    /// <summary>
    /// 获取文件的后缀名
    /// </summary>
    public static string GetExtension(string path)
    {
        string result = Path.GetExtension(path);
        Debug.Log("[PathUtil] GetExtension: \r\nInput: " + path + " \r\nOutput: " + result);
        return result;
    }

    /// <summary>
    /// 根据路径获取文件的名称，并带有后缀名
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string GetFileName(string path)
    {
        string result = Path.GetFileName(path);
        Debug.Log("[PathUtil] GetFileName: \r\nInput: " + path + " \r\nOutput: " + result);
        return result;
    }

    /// <summary>
    /// 根据路径获取文件的名称
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string GetFileNameWithoutExtension(string path)
    {
        string result = Path.GetFileNameWithoutExtension(path);
        Debug.Log("[PathUtil] GetFileNameWithoutExtension: \r\nInput: " + path + " \r\nOutput: " + result);
        return result;
    }

    /// <summary>
    /// 合并路径
    /// </summary>
    /// <param name="path1"></param>
    /// <param name="path2"></param>
    /// <returns></returns>
    public static string Combine(string path1, string path2)
    {
        string result = Path.Combine(path1, path2);
        Debug.Log("[PathUtil] Combine: \r\nInput 1: " + path1 + " \r\nInput 2: " + path2 + " \r\nOutput: " + result);
        return result;
    }
}
