using Company.Attributes;
using Company.NewApp;
using Company.NewApp.Presenters;
using Company.NewApp.Views;
using UnityEngine;

namespace Company.DevFramework.Demo
{
    public class Demo_UIFramework : MonoBehaviour
    {
        [SerializeField] RectTransform m_CanvasRect;

        [ReadOnly]
        [SerializeField] UIExampleView m_UIExampleView = null;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ShowView();
            }
        }

        private void ShowView()
        {
            if (!UIManager.Instance.IsViewExist(ViewType.Example, "UIExample", out m_UIExampleView))
            {
                m_UIExampleView = UIManager.Instance.Open<UIExampleView>(ViewType.Example, "UIExample", m_CanvasRect);
            }
        }
    }
}