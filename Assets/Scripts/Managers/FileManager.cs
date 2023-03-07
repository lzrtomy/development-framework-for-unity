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
        /// StreamingAssetsĿ¼
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
        /// PersistentDataĿ¼
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

        /// <summary>
        /// ��StreamingAssets�ļ����¶�ȡ�ı���Ϣ
        /// </summary>
        /// <param name="subPath"></param>
        /// <returns></returns>
        public string ReadTextFromStreamingAssets(string subPath)
        {
            using (UnityWebRequest uwb = UnityWebRequest.Get("file://" + StreamingAssetsPath + subPath))
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
                    Debug.LogError("[FileManager] ReadTextFromStreamingAssets, error:\r\n" + uwb.error);
            }
            return null;
        }

        /// <summary>
        /// ��PersistentData�ļ����¶�ȡ�ı���Ϣ
        /// </summary>
        /// <param name="subPath"></param>
        /// <returns></returns>
        public string ReadTextFromPersistentData(string subPath)
        {
            using (UnityWebRequest uwb = UnityWebRequest.Get("file://" + PersistentDataPath + subPath))
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
                    Debug.LogError("[FileManager] ReadTextFromPersistentData, error:\r\n" + uwb.error);
            }
            return null;
        }

        /// <summary>
        /// ���ı����浽StreamingAssetsĿ¼��
        /// </summary>
        /// <param name="subDirectory">StreamingAssets�µ���Ŀ¼</param>
        /// <param name="fileName">�ļ���</param>
        /// <param name="text">��������</param>
        public void SaveTextToStreamingAssets(string subDirectory, string fileName, string text)
        {
            string fullDirectory = StreamingAssetsPath + subDirectory;
            if (!Directory.Exists(fullDirectory))
            {
                Directory.CreateDirectory(fullDirectory);
            }

            string fullPath = fullDirectory + fileName;
            using (StreamWriter sw = new StreamWriter(fullPath))
            {
                sw.Write(text);
                sw.Close();
            }
        }

        /// <summary>
        /// ���ı����浽persistentdataĿ¼��
        /// </summary>
        /// <param name="subDirectory">PersistentData�µ���Ŀ¼</param>
        /// <param name="fileName">�ļ���</param>
        /// <param name="text">��������</param>
        public void SaveTextToPersistentData(string subDirectory, string fileName, string text)
        {
            string fullDirectory = PersistentDataPath + subDirectory;
            if (!Directory.Exists(fullDirectory))
            {
                Directory.CreateDirectory(fullDirectory);
            }

            string fullPath = fullDirectory + fileName;
            using (StreamWriter sw = new StreamWriter(fullPath))
            {
                sw.Write(text);
                sw.Close();
            }
        }

        //�ж�Persistent·�����Ƿ����ָ���ļ�
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