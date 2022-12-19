using Company.HttpWebRequest;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Company.DevFramework.Demo
{
    public class Demo_HttpWebRequest : MonoBehaviour
    {
        [SerializeField] Button m_BtnGetExample;
        [SerializeField] Button m_BtnPostExample;
        [SerializeField] Button m_BtnStopAllRequests;
        [SerializeField] Text m_TxtResult;

        private void Start()
        {
            m_BtnGetExample.onClick.AddListener(OnClickGetExampleButton);
            m_BtnPostExample.onClick.AddListener(OnClickPostExampleButton);
            m_BtnStopAllRequests.onClick.AddListener(OnClickStopAllRequestsButton);
        }

        #region Example of Get Request

        private void OnClickGetExampleButton()
        {
            SetText("Sending get request...");
            WebGetExample webGetExample = new WebGetExample(OnGetExampleComplete, OnGetExampleFailed);
            webGetExample.Get();
        }

        private void OnGetExampleComplete(GetExampleEntity entity)
        {
            SetText("Get request complete");
        }

        private void OnGetExampleFailed(long code, string message)
        {
            SetText("Get request failed: " + message);
        }

        #endregion

        #region Example of Post Request

        private void OnClickPostExampleButton()
        {
            SetText("Sending post request...");
            WebPostExample webPostExample = new WebPostExample(OnPostExampleComplete, OnPostExampleFailed);
            webPostExample.Post();
        }

        private void OnPostExampleComplete(PostExampleEntity entity)
        {
            SetText("Post request complete");
        }

        private void OnPostExampleFailed(long code, string message)
        {
            SetText("Post request failed: " + message);
        }

        #endregion

        private void OnClickStopAllRequestsButton()
        {
            HttpRequestManager.Instance.StopAllRequestTasks();
            SetText("Stop all requests");
        }

        private void SetText(string text)
        {
            m_TxtResult.text = text;
        }
    }
}