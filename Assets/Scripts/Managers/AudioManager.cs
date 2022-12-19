using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Company.Constants;
using Company.Attributes;

namespace Company.NewApp
{
    public enum AudioPlayState
    {
        Stop,
        Play,
        Pause
    }

    public class AudioManager : SimpleUnitySingleton<AudioManager>
    {
        [ReadOnly]
        [Header("[ֻ��]BGM��Ƶ������")]
        [SerializeField] private AudioSource m_BGMAudioSource;

        [ReadOnly]
        [Header("[ֻ��]Ԥ����Ч��Ƶ����������")]
        [SerializeField] int m_PresetEffectAudioManagerNum = 1;

        [ReadOnly]
        [Header("[ֻ��]��Ч��Ƶ������")]
        [SerializeField] private List<AudioSource> m_EffectAduioSourceList;

        //ȫ����Ƶ�ֵ�
        private Dictionary<string, AudioClip> m_AudioClipDict;

        private AppAudioSettings m_AudioSettings;

        //BGM����״̬
        private AudioPlayState m_BGMPlayState = AudioPlayState.Stop;

        //Ч��������״̬
        private AudioPlayState m_EffectsPlayState = AudioPlayState.Stop;

        //����������������0 - 1��
        private float m_BGMVolumeScale = 1;

        //������������0 - 1��
        private float m_BGMVolume = 1;

        //��Ч������0 - 1��
        private float m_EffectVolume = 1;

        public AudioPlayState BGMPlayState { get { return m_BGMPlayState; } }

        public AudioPlayState EffectsPlayState { get { return m_EffectsPlayState; } }


        public void Init()
        {
            m_AudioSettings = AppAudioSettings.Instance;

            m_EffectAduioSourceList = new List<AudioSource>();

            PreloadAudioAssets();

            if (!m_BGMAudioSource)
            {
                m_BGMAudioSource = gameObject.AddComponent<AudioSource>();
                m_BGMAudioSource.playOnAwake = false;
                m_BGMAudioSource.loop = true;
            }

            for (int index = 0; index < m_PresetEffectAudioManagerNum; index++) 
            {
                AudioSource audioSource = gameObject.AddComponent<AudioSource>();
                audioSource.playOnAwake = false;
                audioSource.loop = false;

                m_EffectAduioSourceList.Add(audioSource);
            }
        }

        /// <summary>
        /// Ԥ������Ƶ��Դ
        /// </summary>
        public void PreloadAudioAssets()
        {
            m_AudioClipDict = m_AudioClipDict == null ? new Dictionary<string, AudioClip>() : m_AudioClipDict;

            foreach (var item in m_AudioSettings.AudioDataDict)
            {
                if (item.Value.IsPreload && (!m_AudioClipDict.ContainsKey(item.Value.AudioFullName) || m_AudioClipDict[item.Value.AudioFullName] == null))
                {
                    m_AudioClipDict[item.Value.AudioFullName] = ResourcesManager.Instance.LoadAsset<AudioClip>(Defines.Path.RES_SOUNDS + item.Value.AudioFullName);
                }
            }
        }

        #region ��ȡ��Ƶ�ļ�

        /// <summary>
        /// ��ȡ��Ƶ�ļ�
        /// </summary>
        /// <param name="audioFullName">ȫ����*.*</param>
        /// <returns></returns>
        private AudioClip GetAudioClip(string audioFullName)
        {
            if (!m_AudioClipDict.ContainsKey(audioFullName) || m_AudioClipDict[audioFullName] == null)
            {
                m_AudioClipDict[audioFullName] = ResourcesManager.Instance.LoadAsset<AudioClip>(Defines.Path.RES_SOUNDS + audioFullName);
            }
            if (m_AudioClipDict.ContainsKey(audioFullName))
            {
                return m_AudioClipDict[audioFullName];
            }
            return null;
        }

        /// <summary>
        /// ��ȡ��Ƶ�ļ�
        /// </summary>
        /// <param name="audioEvent"></param>
        /// <returns></returns>
        private AudioClip GetAudioClip(AudioEvent audioEvent) 
        {
            if (m_AudioSettings.AudioDataDict.ContainsKey(audioEvent))
            {
                return GetAudioClip(m_AudioSettings.AudioDataDict[audioEvent].AudioFullName);
            }
            return null;
        }

        #endregion

        #region ������Ƶ�ļ�

        /// <summary>
        /// ���ű�����
        /// </summary>
        public void PlayBGM(AudioEvent audioEvent, bool loop)
        {
            if (m_AudioSettings.AudioDataDict.ContainsKey(audioEvent))
            {
                PlayBGM(GetAudioClip(audioEvent),loop, m_AudioSettings.AudioDataDict[audioEvent].VolumeScale);
            }
        }

        /// <summary>
        /// ���ű�����
        /// </summary>
        /// <param name="fullAudioName">ȫ����*.*</param>
        /// <param name="volumeScale">��ʼ����</param>
        public void PlayBGM(string fullAudioName, bool loop, float volumeScale = 1)
        {
            PlayBGM(GetAudioClip(fullAudioName),loop, volumeScale);
        }
        
        private void PlayBGM(AudioClip audioClip, bool loop, float volumeScale)
        {
            if (audioClip && m_BGMAudioSource.clip != audioClip && !m_BGMAudioSource.isPlaying && m_BGMPlayState != AudioPlayState.Pause)
            {
                m_BGMPlayState = AudioPlayState.Play;
                m_BGMAudioSource.clip = audioClip;
                m_BGMAudioSource.loop = loop;
                m_BGMVolumeScale = volumeScale;
                m_BGMAudioSource.volume = m_BGMVolumeScale * m_BGMVolume;
                m_BGMAudioSource.Play();
            }
        }

        /// <summary>
        /// ����Ч������
        /// </summary>
        /// <param name="audioEvent"></param>
        /// <param name="canOverlay">�Ƿ���Ե��Ӳ���</param>
        public void PlayEffect(AudioEvent audioEvent, bool canOverplay)
        {
            if (m_AudioSettings.AudioDataDict.ContainsKey(audioEvent))
            {
                if (canOverplay)
                {
                    PlayEffectOverlay(GetAudioClip(audioEvent), m_AudioSettings.AudioDataDict[audioEvent].VolumeScale);
                }
                else
                {
                    PlayEffectNotOverlay(GetAudioClip(audioEvent), m_AudioSettings.AudioDataDict[audioEvent].VolumeScale);
                }
            }
        }

        /// <summary>
        /// ����Ч������
        /// </summary>
        /// <param name="fullAudioName">ȫ����*.*</param>
        /// <param name="volumeScale">��ʼ����</param>
        /// <param name="canOverlay">�Ƿ���Ե��Ӳ���</param>
        public void PlayEffect(string fullAudioName, bool canOverplay, float volumeScale = 1)
        {
            if (canOverplay)
            {
                PlayEffectOverlay(GetAudioClip(fullAudioName), volumeScale);
            }
            else
            {
                PlayEffectNotOverlay(GetAudioClip(fullAudioName), volumeScale);
            }
        }

        private void PlayEffectOverlay(AudioClip audioClip, float volumeScale) 
        {
            if (audioClip)
            {
                AudioSource audioSource = null;

                audioSource = m_EffectAduioSourceList.Count > 0 ? m_EffectAduioSourceList[0] : audioSource;

                if (!audioSource)
                {
                    audioSource = gameObject.AddComponent<AudioSource>();
                    m_EffectAduioSourceList.Add(audioSource);
                    audioSource.playOnAwake = false;
                    audioSource.loop = false;
                }

                //��ǰ״̬�ǿ��Բ��ŲŲ���
                if (EffectsPlayState != AudioPlayState.Pause)
                {
                    audioSource.volume = volumeScale * m_EffectVolume;
                    audioSource.PlayOneShot(audioClip);
                }
            }
        }

        private void PlayEffectNotOverlay(AudioClip audioClip, float volumeScale)
        {
            StartCoroutine(IEPlayEffectNotOverlay(audioClip, audioClip.length, volumeScale));
        }

        private IEnumerator IEPlayEffectNotOverlay(AudioClip audioClip, float duration, float volumeScale)
        {
            if (audioClip)
            {
                AudioSource audioSource = null;

                for (int index = 0; index < m_EffectAduioSourceList.Count; index++)
                {
                    if (m_EffectAduioSourceList[index].clip == audioClip && m_EffectAduioSourceList[index].isPlaying)
                    {
                        yield break;
                    }
                }

                for (int index = 0; index < m_EffectAduioSourceList.Count; index++)
                {
                    if (!m_EffectAduioSourceList[index].isPlaying)
                    {
                        audioSource = m_EffectAduioSourceList[index];
                        break;
                    }
                }

                if (!audioSource)
                {
                    audioSource = gameObject.AddComponent<AudioSource>();
                    m_EffectAduioSourceList.Add(audioSource);
                    audioSource.playOnAwake = false;
                    audioSource.loop = false;
                }


                //��ǰ״̬�ǿ��Բ��ŲŲ���
                if (EffectsPlayState != AudioPlayState.Pause)
                {
                    audioSource.volume = volumeScale * m_EffectVolume;
                    audioSource.clip = audioClip;
                    audioSource.Play();
                    yield return new WaitForSeconds(duration);
                    audioSource.Stop();
                    audioSource.clip = null;
                }
            }
            yield break;
        }

        #endregion

        #region ��ͣ����

        /// <summary>
        /// ��ͣ����ȫ������
        /// </summary>
        public void PauseAll()
        {
            PauseBGM();
            PauseEffect();
        }
        
        public void PauseBGM() 
        {
            m_BGMPlayState = AudioPlayState.Pause;
            m_BGMAudioSource.Pause();
        }
        
        public void PauseEffect()
        {
            m_EffectsPlayState = AudioPlayState.Pause;
            for (int index = 0; index < m_EffectAduioSourceList.Count; index++)
            {
                m_EffectAduioSourceList[index].Pause();
            }
        }

        #endregion

        #region ֹͣ����

        /// <summary>
        /// ֹͣ����ȫ������
        /// </summary>
        public void StopAll()
        {
            StopBGM();
            StopEffects();
        }

        public void StopBGM() 
        {
            m_BGMPlayState = AudioPlayState.Stop;
            m_BGMAudioSource.Stop();
        }
        
        public void StopEffects()
        {
            m_EffectsPlayState = AudioPlayState.Stop;
            for (int index = 0; index < m_EffectAduioSourceList.Count; index++)
            {
                m_EffectAduioSourceList[index].Stop();
            }
        }

        #endregion

        #region ��������

        /// <summary>
        /// ������������
        /// </summary>
        /// <param name="volume"></param>
        public void SetVolume(float volume) 
        {
            SetBGMVolume(volume);
            SetEffectsVolume(volume);
        }

        public void SetBGMVolume(float volume)
        {
            m_BGMVolume = volume;
            m_BGMAudioSource.volume = m_BGMVolumeScale * m_BGMVolume;
        }

        public void SetEffectsVolume(float volume)
        {
            m_EffectVolume = volume;
        }

        #endregion

        #region �ָ���������

        /// <summary>
        /// �ָ�����ȫ������
        /// </summary>
        public void ResumePlayAll()
        {
            ResumePlayBGM();
            ResumePlayEffects();
        }
        
        public void ResumePlayBGM()
        {
            if (BGMPlayState == AudioPlayState.Pause)
            {
                m_BGMPlayState = AudioPlayState.Play;
                if (m_BGMAudioSource.clip != null && !m_BGMAudioSource.isPlaying)
                {
                    m_BGMAudioSource.Play();
                }
            }
        }

        public void ResumePlayEffects()
        {
            if (EffectsPlayState == AudioPlayState.Pause)
            {
                m_EffectsPlayState = AudioPlayState.Play;
                for (int index = 0; index < m_EffectAduioSourceList.Count; index++)
                {
                    if (m_BGMAudioSource.clip != null && !m_BGMAudioSource.isPlaying)
                    {
                        m_EffectAduioSourceList[index].Play();
                    }
                }
            }
        }

        #endregion
    }
}