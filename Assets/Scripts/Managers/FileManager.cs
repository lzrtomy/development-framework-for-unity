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
        public static string StreamingAssetPath
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
#if UNITY_EDITOR
                string pseudoPath = Application.dataPath + "/MockPersistentData/";
                if (!Directory.Exists(pseudoPath))
                {
                    Directory.CreateDirectory(pseudoPath);
                }
                return pseudoPath;
#endif
                return Application.persistentDataPath + "/";
            }
        }

        private string m_FromDirectory
        {
            get
            {
#if UNITY_EDITOR
                return "file://" + Application.streamingAssetsPath + "/";
#elif UNITY_ANDROID && !UNITY_EDITOR
        return Application.streamingAssetsPath + "/";  //"jar: file://" + Application.dataPath + "!/assets/";
#elif UNITY_IOS && !UNITY_EDITOR
        return "file://" + Application.streamingAssetsPath + "/";
#endif
                return "file://" + Application.streamingAssetsPath + "/";
            }
        }

        public void Init()
        {
            m_LogEnabled = AppSettings.Instance.LogEnabled;
        }

        //---------- FILE NO EXISTS ------------//
        // 从StreamingAsset中复制文件
        public void CopyFileFromStreamingAsset(string file_name, Action onRequestComplete)
        {
            CopyFileFromStreamingAsset(file_name, null, onRequestComplete);
        }

        // 从StreamingAsset中复制文件
        public void CopyFileFromStreamingAsset(string file_name, string folderName, Action onRequestComplete)
        {
            string from_path = m_FromDirectory + file_name;
            string to_path = PersistentDataPath + file_name;

            if (!string.IsNullOrEmpty(folderName))
            {
                from_path = new Uri(Path.Combine(StreamingAssetPath, folderName, "/",file_name)).AbsolutePath;
                to_path = PersistentDataPath + folderName + "/" + file_name;
                if (!Directory.Exists(PersistentDataPath + folderName + "/"))
                {
                    Directory.CreateDirectory(PersistentDataPath + folderName + "/");
                }
            }

            if (m_IECopyFileFromStreamingAsset != null)
            {
                StopCoroutine(m_IECopyFileFromStreamingAsset);
            }
            m_IECopyFileFromStreamingAsset = IECopyFileFromStreamingAsset(from_path, to_path, onRequestComplete);
            StartCoroutine(m_IECopyFileFromStreamingAsset);
        }


        private IEnumerator IECopyFileFromStreamingAsset(string fromPath, string toPath, Action onRequestComplete)
        {
            using (UnityWebRequest uwb = UnityWebRequest.Get(fromPath))
            {
                yield return uwb.SendWebRequest();

                if (!uwb.isHttpError && !uwb.isNetworkError)
                {
                    File.WriteAllBytes(toPath, uwb.downloadHandler.data);
                    onRequestComplete?.Invoke();
                }
                else
                {
                    if (m_LogEnabled)
                        Debug.Log("[FileManager] Failed to copy file from streamingasset path. Error: " + uwb.error);
                }
            }
        }

        /// <summary>
        /// 从StreamingAssets文件夹下读取文本信息
        /// </summary>
        /// <param name="subPath"></param>
        /// <returns></returns>
        public string ReadTextFromStreamingAssets(string subPath)
        {
            using (WWW www = new WWW("file://" + StreamingAssetPath + subPath))
            {
                while (!www.isDone) { }
                if (www.error == null)
                    return www.text;
                else
                    Debug.LogError("[FileManager] ReadTextFromStreamingAssets, error:\r\n" + www.error);
            }
            return null;
        }

        //判断Persistent路径中是否存在指定文件
        public bool PersistentPathExists(string fileName)
        {
            return PersistentPathExists(fileName, null);
        }

        //判断Persistent路径中是否存在指定文件
        public bool PersistentPathExists(string fileName, string folderName)
        {
            string fullPath = PersistentDataPath + fileName;
            if (!string.IsNullOrEmpty(folderName))
            {
                fullPath = PersistentDataPath + folderName + "/" + fileName;
            }
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