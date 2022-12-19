
using System.Collections.Generic;
using UnityEngine;

namespace Company.NewApp
{
    /// <summary>
    /// 音频设置
    /// </summary>
    [CreateAssetMenu]
    [System.Serializable]
    public class AppAudioSettings : ScriptableObject
    {
        private static AppAudioSettings m_Instance;
        public static AppAudioSettings Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = (AppAudioSettings)UnityEngine.Resources.Load("AppAudioSettings");
                }
                return m_Instance;
            }
        }

        [Header("放在本地的音频文件名，按照*.*格式填写")]
        [SerializeField] private AudioData[] m_AudioDataArray;

        //音频名称字典
        private Dictionary<AudioEvent, AudioData> m_AudioDataDict = new Dictionary<AudioEvent, AudioData>();
        public Dictionary<AudioEvent, AudioData> AudioDataDict { get { return m_AudioDataDict; } }

        public void Init()
        {
            AddAudioDataToDictionary();
        }

        private void AddAudioDataToDictionary()
        {
            for (int index = 0; index < m_AudioDataArray.Length; index++)
            {
                if (!AudioDataDict.ContainsKey(m_AudioDataArray[index].AudioEvent))
                {
                    AudioDataDict.Add(m_AudioDataArray[index].AudioEvent, m_AudioDataArray[index]);
                }
            }
        }
    }

    [System.Serializable]
    public class AudioData
    {
        public AudioEvent AudioEvent;
        public string AudioFullName;
        [Range(0, 1)]
        public float VolumeScale = 1;
        public bool IsPreload = true;
    }
}