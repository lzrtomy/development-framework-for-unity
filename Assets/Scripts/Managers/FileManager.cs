using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace Company.NewApp
{
    public class FileManager : UnitySingleton<FileManager>
    {
        private bool m_LogEnabled = true;

        private IEnumerator m_IECopyFileFromStreamingAsset;

        /// <summary>
        /// StreamingAssets目录
        /// </summary>
        public static string StreamingAssetsPath
        {
            get
            {
#if UNITY_EDITOR
                return Application.streamingAssetsPath + "/";
#elif UNITY_ANDROID && !UNITY_EDITOR
            return Application.dataPath + "!/assets/";
#elif UNITY_IOS && !UNITY_EDITOR
            return Application.dataPath + "/Raw/";
#elif UNITY_STANDALONE_OSX && !UNITY_EDITOR
            return Application.dataPath + "/Resources/Data/StreamingAssets/";
#endif
                return Application.streamingAssetsPath + "/";
            }
        }

        /// <summary>
        /// PersistentData目录
        /// </summary>
        public static string PersistentDataPath
        {
            get
            {
                return Application.persistentDataPath + "/";
#if UNITY_EDITOR
                string pseudoPath = Application.dataPath + "/MockPersistentData/";
                if (!Directory.Exists(pseudoPath))
                {
                    Directory.CreateDirectory(pseudoPath);
                }
                return pseudoPath;
#endif
            }
        }

        public void Init()
        {
            m_LogEnabled = AppSettings.Instance.LogEnabled;
        }

#region Read File

        //从StreamingAssets文件夹下读取文本信息
        public string ReadTextFromStreamingAssets(string subPath)
        {
            return ReadFile(StreamingAssetsPath + subPath);
        }

        //从PersistentData文件夹下读取文本信息
        public string ReadTextFromPersistentData(string subPath)
        {
            return ReadFile(PersistentDataPath + subPath);
        }

        /// <summary>
        /// 读取指定路径下的文本
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string ReadFile(string path)
        {
            using (UnityWebRequest uwb = UnityWebRequest.Get("file://" + path))
            {
                uwb.SendWebRequest();
                while (!uwb.isDone) { }

#if UNITY_2020_1_OR_NEWER
                if (uwb.result == UnityWebRequest.Result.Success)
#else
                if (!uwb.isHttpError && !uwb.isNetworkError)
#endif
                {
                    return uwb.downloadHandler.text;
                }
                else if (m_LogEnabled)
                    Debug.LogError("[FileManager] Read text error:" + uwb.error + "\r\npath:" + path);
            }
            return null;
        }

        #endregion Read File


        #region Save File

        //将文本保存到StreamingAssets目录下
        public void SaveTextToStreamingAssets(string subDirectory, string fileName, string text)
        {
            string directory = StreamingAssetsPath + subDirectory;
            SaveFile(directory, fileName, text);
        }

        //将文本保存到persistentdata目录下
        public void SaveTextToPersistentData(string subDirectory, string fileName, string text)
        {
            string directory = PersistentDataPath + subDirectory;
            SaveFile(directory, fileName, text);
        }

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="directory">不含文件名的路径</param>
        /// <param name="fileName">文件名</param>
        /// <param name="text">保存文件内容</param>
        public void SaveFile(string directory, string fileName, string text)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string fullPath = directory + fileName;
            using (StreamWriter sw = new StreamWriter(fullPath))
            {
                sw.Write(text);
                sw.Close();
            }
        }

        #endregion Save File


        #region Delete File

        public void DeleteFileInPersistentDataPath(string subPath)
        {
            DeleteFile(PersistentDataPath + subPath);
        }

        public void DeleteFile(string path)
        {
            File.Delete(path);
        }

        #endregion Delete File

        //判断Persistent路径中是否存在指定文件
        public bool PersistentPathExists(string subPath)
        {
            string fullPath = PersistentDataPath + subPath;
            if (File.Exists(fullPath))
            {
                if (m_LogEnabled)
                    Debug.Log("[FileManager] File exist at path: " + fullPath);
                return true;
            }
            if (m_LogEnabled)
                Debug.Log("[FileManager] File not exist at path: " + fullPath);
            return false;
        }

    }
}