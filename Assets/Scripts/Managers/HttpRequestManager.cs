using Company.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Company.NewApp;

namespace Company.HttpWebRequest
{
    public class HttpRequestManager : UnitySingleton<HttpRequestManager>
    {
        private List<HttpRequestTask> m_ControlledPostTaskList;

        private List<HttpRequestTask> m_ControlledGetTaskList;

        private bool m_LogEnabled = true;

        public void Init()
        {
            m_LogEnabled = AppSettings.Instance.LogEnabled;
            m_ControlledPostTaskList = new List<HttpRequestTask>();
            m_ControlledGetTaskList = new List<HttpRequestTask>();
        }

        public void AddPostRequestTask(HttpRequestTask task)
        {
            CollectionTools.ListAdd(m_ControlledPostTaskList, task);

            if(m_LogEnabled)
                Debug.Log("[HttpRequestManager] Post task count: " + m_ControlledPostTaskList.Count);
        }

        public void RemovePostRequestTask(HttpRequestTask task)
        {
            m_ControlledPostTaskList.Remove(task);

            if (m_LogEnabled)
                Debug.Log("[HttpRequestManager] Post task count: " + m_ControlledPostTaskList.Count);
        }

        public void AddGetRequestTask(HttpRequestTask task)
        {
            CollectionTools.ListAdd(m_ControlledGetTaskList, task);

            if (m_LogEnabled)
                Debug.Log("[HttpRequestManager] Get task count: " + m_ControlledGetTaskList.Count);
        }

        public void RemoveGetRequestTask(HttpRequestTask task)
        {
            m_ControlledGetTaskList.Remove(task);

            if (m_LogEnabled)
                Debug.Log("[HttpRequestManager] Get task count: " + m_ControlledGetTaskList.Count);
        }

        public void StopAllRequestTasks()
        {
            //for(int index = 0; index < m_ControlledPostTaskList.Count; index++)
            //{
            //    MintTools.Instance.StopIE(m_ControlledPostTaskList[index].IERequest);
            //}
            //m_ControlledPostTaskList.Clear();

            for (int index = 0; index < m_ControlledGetTaskList.Count; index++)
            {
                UnityTools.Instance.StopIE(m_ControlledGetTaskList[index].IERequest);
            }
            m_ControlledGetTaskList.Clear();
        }
    }
}