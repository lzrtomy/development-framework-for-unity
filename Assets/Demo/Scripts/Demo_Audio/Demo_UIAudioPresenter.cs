using UnityEngine;
using Company.NewApp.Presenters;
using Company.NewApp;
using Company.Constants;

namespace Company.DevFramework.Demo
{
    public class Demo_UIAudioPresenter : UIPresenterBase
    {
        [SerializeField] Demo_UIAudioView m_UIAudioView;
        private AudioManager m_AudioManager;
        private float m_CachedVolume = 1;

        private void Start()
        {
            //Invoke("InitDemo", 1);
            InitDemo();
        }

        private void InitDemo()
        {
            Init();

        }

        public override void Init(params object[] parameters)
        {
            base.Init(parameters);
            m_UIAudioView.Init();
            m_AudioManager = AudioManager.Instance;

            OnVolumeChanged(1);
            m_UIAudioView.SetVolumeSliderView(1);
            OnSetMute(false);
        }

        protected override void AddListeners()
        {
            EventManager.Instance.AddListener<float>(MyEventName.Demo_Auido.OnVolumeSliderValueChanged.ToString(), OnVolumeChanged);
            EventManager.Instance.AddListener(MyEventName.Demo_Auido.OnBtnBGMClicked.ToString(), OnPlayBGM);
            EventManager.Instance.AddListener(MyEventName.Demo_Auido.OnBtnEffect01Clicked.ToString(), OnPlayEffect01);
            EventManager.Instance.AddListener(MyEventName.Demo_Auido.OnBtnEffect02Clicked.ToString(), OnPlayEffect02);
            EventManager.Instance.AddListener<bool>(MyEventName.Demo_Auido.OnMuteToggleClicked.ToString(), OnSetMute);
        }

        protected override void RemoveListeners()
        {
            EventManager.Instance.RemoveListener<float>(MyEventName.Demo_Auido.OnVolumeSliderValueChanged.ToString(), OnVolumeChanged);
            EventManager.Instance.RemoveListener(MyEventName.Demo_Auido.OnBtnBGMClicked.ToString(), OnPlayBGM);
            EventManager.Instance.RemoveListener(MyEventName.Demo_Auido.OnBtnEffect01Clicked.ToString(), OnPlayEffect01);
            EventManager.Instance.RemoveListener(MyEventName.Demo_Auido.OnBtnEffect02Clicked.ToString(), OnPlayEffect02);
            EventManager.Instance.RemoveListener<bool>(MyEventName.Demo_Auido.OnMuteToggleClicked.ToString(), OnSetMute);
        }

        private void OnVolumeChanged(float volume)
        {
            m_CachedVolume = volume;
            m_AudioManager.SetVolume(volume);

            m_UIAudioView.SetToggleMuteView(volume <= 0);
        }

        private void OnPlayBGM()
        {
            m_AudioManager.PlayBGM(AudioEvent.Demo_BGM, true);
        }

        private void OnPlayEffect01()
        {
            m_AudioManager.PlayEffect(AudioEvent.Demo_Eff_1, true);
        }

        private void OnPlayEffect02()
        {
            m_AudioManager.PlayEffect(AudioEvent.Demo_Eff_2, false);
        }

        private void OnSetMute(bool mute)
        {
            m_UIAudioView.SetVolumeSliderView(mute?0:m_CachedVolume);
            m_UIAudioView.SetToggleMuteView(mute);
        }
    }
}