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


        public void Init()
        {
            m_LogEnabled = AppSettings.Instance.LogEnabled;
        }

        #region Read File

        /// <summary>
        /// ��ȡָ��·���µ��ı�
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="onComplete"></param>
        /// <param name="onFailed"></param>
        /// <returns></returns>
        public IEnumerator IEReadFile(Uri uri, Action<string> onComplete, Action onFailed = null)
        {
            using (UnityWebRequest uwb = UnityWebRequest.Get(uri))
            {
                yield return uwb.SendWebRequest();

#if UNITY_2020_1_OR_NEWER
                if (uwb.result == UnityWebRequest.Result.Success)
#else
                if (!uwb.isHttpError && !uwb.isNetworkError)
#endif
                {
                    if (m_LogEnabled)
                        Debug.Log("[FileManager] Read text:" + uwb.error + "\r\nUri:" + uri);
                    onComplete?.Invoke(uwb.downloadHandler.text);
                }
                else
                {
                    if (m_LogEnabled)
                        Debug.LogError("[FileManager] Read text error:" + uwb.error + "\r\nUri:" + uri);
                    onFailed?.Invoke();
                }
            }
            yield break;
        }

        #endregion Read File


        #region Save File

        //���ı����浽StreamingAssetsĿ¼��
        public void SaveTextToStreamingAssets(string subDirectory, string fileName, string text)
        {
            string directory = StreamingAssetsPath + subDirectory;
            SaveFile(directory, fileName, text);
        }

        //���ı����浽persistentdataĿ¼��
        public void SaveTextToPersistentData(string subDirectory, string fileName, string text)
        {
            string directory = PersistentDataPath + subDirectory;
            SaveFile(directory, fileName, text);
        }

        /// <summary>
        /// �����ļ�
        /// </summary>
        /// <param name="directory">�����ļ�����·��</param>
        /// <param name="fileName">�ļ���</param>
        /// <param name="text">�����ļ�����</param>
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