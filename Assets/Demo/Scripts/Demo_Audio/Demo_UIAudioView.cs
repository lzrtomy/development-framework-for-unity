
using UnityEngine;
using Company.NewApp.Views;
using UnityEngine.UI;
using Company.NewApp;
using Company.Constants;

namespace Company.DevFramework.Demo
{
    public class Demo_UIAudioView : UIViewBase
    {
        [SerializeField] Slider m_SliderVolume;
        [SerializeField] Button m_BtnBGM;
        [SerializeField] Button m_BtnEffect01;
        [SerializeField] Button m_BtnEffect02;
        [SerializeField] Text m_TxtVolume;

        [SerializeField] Toggle m_ToggleMute; 

        public override void Init(params object[] paramters)
        {
            m_SliderVolume.onValueChanged.AddListener(OnVolumeSliderValueChanged);
            m_BtnBGM.onClick.AddListener(OnBtnBGMClicked);
            m_BtnEffect01.onClick.AddListener(OnBtnEffect01Clicked);
            m_BtnEffect02.onClick.AddListener(OnBtnEffect02Clicked);
            m_ToggleMute.onValueChanged.AddListener(OnToggleClicked);
        }

        private void OnVolumeSliderValueChanged(float volumn)
        {
            EventManager.Instance.Dispatch<float>(MyEventName.Demo_Auido.OnVolumeSliderValueChanged.ToString(), volumn);
        }

        private void OnBtnBGMClicked()
        {
            EventManager.Instance.Dispatch(MyEventName.Demo_Auido.OnBtnBGMClicked.ToString());
        }

        private void OnBtnEffect01Clicked()
        {
            EventManager.Instance.Dispatch(MyEventName.Demo_Auido.OnBtnEffect01Clicked.ToString());
        }

        private void OnBtnEffect02Clicked()
        {
            EventManager.Instance.Dispatch(MyEventName.Demo_Auido.OnBtnEffect02Clicked.ToString());
        }

        private void OnToggleClicked(bool isOn)
        {
            EventManager.Instance.Dispatch(MyEventName.Demo_Auido.OnMuteToggleClicked.ToString(), isOn);
        }


        public void SetVolumeSliderView(float volumn)
        {
            m_SliderVolume.value = volumn;
        }

        public void SetVolumeTextView(float volumn)
        {
            m_TxtVolume.text = m_SliderVolume.value.ToString();
        }

        public void SetToggleMuteView(bool isOn)
        {
            m_ToggleMute.isOn = isOn;
        }
    }
}