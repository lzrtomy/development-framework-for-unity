using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Company.NewApp
{
    public class EventManagerBase<U> : SimpleUnitySingleton<U> where U : EventManagerBase<U>
    {
        public delegate void Act();
        public delegate void Act<T>(T t);
        public delegate void Act<T1, T2>(T1 t1, T2 t2);
        public delegate void Act<T1, T2, T3>(T1 t1, T2 t2, T3 t3);
        public delegate void Act<T1, T2, T3, T4>(T1 t1, T2 t2, T3 t3, T4 t4);
        //public delegate void Act<T1, T2, T3, T4, T5>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5);

        protected Dictionary<string, Delegate> m_EventDict;

        protected const string MSG_ERROR_1 = "[{0}] Attempting to add listener with inconsistent signature for event eventName {1}. Current listeners have eventName {2} and listener being added has eventName {3}";
        protected const string MSG_ERROR_2 = "[{0}] Attempting to remove listener for eventName \"{1}\" but Messenger doesn't know about this event eventName.";
        protected const string MSG_ERROR_3 = "[{0}] Attempting to remove listener with for event eventName \"{1}\" but current listener is null.";
        protected const string MSG_ERROR_4 = "[{0}] no such event eventName {1}";


        public virtual void Init()
        {
            m_EventDict = new Dictionary<string, Delegate>();
        }

        #region 事件注册

        public void AddListener(string eventName, Act act)
        {
            if (!m_EventDict.ContainsKey(eventName))
            {
                m_EventDict.Add(eventName, null);
            }

            Delegate d = m_EventDict[eventName];
            if (d != null && d.GetType() != act.GetType())
            {
                Debug.LogError(string.Format(MSG_ERROR_1, this.GetType().Name, eventName, d.GetType().Name, act.GetType().Name));
            }
            else
            {
                m_EventDict[eventName] = (Act)m_EventDict[eventName] + act;
            }
        }
        public void AddListener<T>(string eventName, Act<T> act)
        {
            if (!m_EventDict.ContainsKey(eventName))
            {
                m_EventDict.Add(eventName, null);
            }

            Delegate d = m_EventDict[eventName];
            if (d != null && d.GetType() != act.GetType())
            {
                Debug.LogError(string.Format(MSG_ERROR_1, this.GetType().Name, eventName, d.GetType().Name, act.GetType().Name));
            }
            else
            {
                m_EventDict[eventName] = (Act<T>)m_EventDict[eventName] + act;
            }
        }
        public void AddListener<T1, T2>(string eventName, Act<T1, T2> act)
        {
            if (!m_EventDict.ContainsKey(eventName))
            {
                m_EventDict.Add(eventName, null);
            }

            Delegate d = m_EventDict[eventName];
            if (d != null && d.GetType() != act.GetType())
            {
                Debug.LogError(string.Format(MSG_ERROR_1, this.GetType().Name, eventName, d.GetType().Name, act.GetType().Name));
            }
            else
            {
                m_EventDict[eventName] = (Act<T1, T2>)m_EventDict[eventName] + act;
            }
        }
        public void AddListener<T1, T2, T3>(string eventName, Act<T1, T2, T3> act)
        {
            if (!m_EventDict.ContainsKey(eventName))
            {
                m_EventDict.Add(eventName, null);
            }

            Delegate d = m_EventDict[eventName];
            if (d != null && d.GetType() != act.GetType())
            {
                Debug.LogError(string.Format(MSG_ERROR_1, this.GetType().Name, eventName, d.GetType().Name, act.GetType().Name));
            }
            else
            {
                m_EventDict[eventName] = (Act<T1, T2, T3>)m_EventDict[eventName] + act;
            }
        }
        public void AddListener<T1, T2, T3, T4>(string eventName, Act<T1, T2, T3, T4> act)
        {
            if (!m_EventDict.ContainsKey(eventName))
            {
                m_EventDict.Add(eventName, null);
            }

            Delegate d = m_EventDict[eventName];
            if (d != null && d.GetType() != act.GetType())
            {
                Debug.LogError(string.Format(MSG_ERROR_1, this.GetType().Name, eventName, d.GetType().Name, act.GetType().Name));
            }
            else
            {
                m_EventDict[eventName] = (Act<T1, T2, T3, T4>)m_EventDict[eventName] + act;
            }
        }
        #endregion

        #region 事件注销
        public void RemoveListener(string eventName, Act listenerBeingRemoved)
        {
            if (m_EventDict.ContainsKey(eventName))
            {
                Delegate d = m_EventDict[eventName];

                if (d == null)
                {
                    Debug.LogError(string.Format(MSG_ERROR_3, this.GetType().Name, eventName));
                }
                else if (d.GetType() != listenerBeingRemoved.GetType())
                {
                    Debug.LogError(string.Format(MSG_ERROR_1, this.GetType().Name, eventName, d.GetType().Name, listenerBeingRemoved.GetType().Name));
                }
                else
                {
                    m_EventDict[eventName] = (Act)m_EventDict[eventName] - listenerBeingRemoved;
                    if (m_EventDict[eventName] == null)
                    {
                        m_EventDict.Remove(eventName);
                    }
                }
            }
            else
            {
                //Debug.LogError(string.Format(MSG_ERROR_2, eventName));
            }
        }
        public void RemoveListener<T>(string eventName, Act<T> listenerBeingRemoved)
        {
            if (m_EventDict.ContainsKey(eventName))
            {
                Delegate d = m_EventDict[eventName];

                if (d == null)
                {
                    Debug.LogError(string.Format(MSG_ERROR_3, this.GetType().Name, eventName));
                }
                else if (d.GetType() != listenerBeingRemoved.GetType())
                {
                    Debug.LogError(string.Format(MSG_ERROR_1, this.GetType().Name, eventName, d.GetType().Name, listenerBeingRemoved.GetType().Name));
                }
                else
                {
                    m_EventDict[eventName] = (Act<T>)m_EventDict[eventName] - listenerBeingRemoved;
                    if (m_EventDict[eventName] == null)
                    {
                        m_EventDict.Remove(eventName);
                    }
                }
            }
            else
            {
                //Debug.LogError(string.Format(MSG_ERROR_2, eventName));
            }
        }
        public void RemoveListener<T1, T2>(string eventName, Act<T1, T2> listenerBeingRemoved)
        {
            if (m_EventDict.ContainsKey(eventName))
            {
                Delegate d = m_EventDict[eventName];

                if (d == null)
                {
                    Debug.LogError(string.Format(MSG_ERROR_3, this.GetType().Name, eventName));
                }
                else if (d.GetType() != listenerBeingRemoved.GetType())
                {
                    Debug.LogError(string.Format(MSG_ERROR_1, this.GetType().Name, eventName, d.GetType().Name, listenerBeingRemoved.GetType().Name));
                }
                else
                {
                    m_EventDict[eventName] = (Act<T1, T2>)m_EventDict[eventName] - listenerBeingRemoved;
                    if (m_EventDict[eventName] == null)
                    {
                        m_EventDict.Remove(eventName);
                    }
                }
            }
            else
            {
                //Debug.LogError(string.Format(MSG_ERROR_2, eventName));
            }
        }
        public void RemoveListener<T1, T2, T3>(string eventName, Act<T1, T2, T3> listenerBeingRemoved)
        {
            if (m_EventDict.ContainsKey(eventName))
            {
                Delegate d = m_EventDict[eventName];

                if (d == null)
                {
                    Debug.LogError(string.Format(MSG_ERROR_3, this.GetType().Name, eventName));
                }
                else if (d.GetType() != listenerBeingRemoved.GetType())
                {
                    Debug.LogError(string.Format(MSG_ERROR_1, this.GetType().Name, eventName, d.GetType().Name, listenerBeingRemoved.GetType().Name));
                }
                else
                {
                    m_EventDict[eventName] = (Act<T1, T2, T3>)m_EventDict[eventName] - listenerBeingRemoved;
                    if (m_EventDict[eventName] == null)
                    {
                        m_EventDict.Remove(eventName);
                    }
                }
            }
            else
            {
                //Debug.LogError(string.Format(MSG_ERROR_2, eventName));
            }
        }
        public void RemoveListener<T1, T2, T3, T4>(string eventName, Act<T1, T2, T3, T4> listenerBeingRemoved)
        {
            if (m_EventDict.ContainsKey(eventName))
            {
                Delegate d = m_EventDict[eventName];

                if (d == null)
                {
                    Debug.LogError(string.Format(MSG_ERROR_2, this.GetType().Name, eventName));
                }
                else if (d.GetType() != listenerBeingRemoved.GetType())
                {
                    Debug.LogError(string.Format(MSG_ERROR_1, this.GetType().Name, eventName, d.GetType().Name, listenerBeingRemoved.GetType().Name));
                }
                else
                {
                    m_EventDict[eventName] = (Act<T1, T2, T3, T4>)m_EventDict[eventName] - listenerBeingRemoved;
                    if (m_EventDict[eventName] == null)
                    {
                        m_EventDict.Remove(eventName);
                    }
                }
            }
            else
            {
                Debug.LogError(string.Format(MSG_ERROR_2, this.GetType().Name, eventName));
            }
        }

        public void RemoveAllListener(string eventName)
        {
            if (m_EventDict.ContainsKey(eventName))
            {
                m_EventDict[eventName] = null;
                m_EventDict.Remove(eventName);
            }
            else
            {
                Debug.LogError(string.Format(MSG_ERROR_2, this.GetType().Name, eventName));
            }
        }
        #endregion

        #region 事件传递
        public void Dispatch(string eventName)
        {
            Delegate d;
            if (m_EventDict.TryGetValue(eventName, out d))
            {
                Act callback = d as Act;

                if (callback != null)
                {
                    callback();
                }
                else
                {
                    Debug.LogWarning(string.Format(MSG_ERROR_4, this.GetType().Name, eventName));
                }
            }
        }
        public void Dispatch<T>(string eventName, T param1)
        {
            Delegate d;
            if (m_EventDict.TryGetValue(eventName, out d))
            {
                Act<T> callback = d as Act<T>;

                if (callback != null)
                {
                    callback(param1);
                }
                else
                {
                    Debug.LogError(string.Format(MSG_ERROR_4, this.GetType().Name, eventName));
                }
            }
        }
        public void Dispatch<T1, T2>(string eventName, T1 param1, T2 param2)
        {
            Delegate d;
            if (m_EventDict.TryGetValue(eventName, out d))
            {
                Act<T1, T2> callback = d as Act<T1, T2>;

                if (callback != null)
                {
                    callback(param1, param2);
                }
                else
                {
                    Debug.LogError(string.Format(MSG_ERROR_4, this.GetType().Name, eventName));
                }
            }
        }
        public void Dispatch<T1, T2, T3>(string eventName, T1 param1, T2 param2, T3 param3)
        {
            Delegate d;
            if (m_EventDict.TryGetValue(eventName, out d))
            {
                Act<T1, T2, T3> callback = d as Act<T1, T2, T3>;

                if (callback != null)
                {
                    callback(param1, param2, param3);
                }
                else
                {
                    Debug.LogError(string.Format(MSG_ERROR_4, this.GetType().Name, eventName));
                }
            }
        }
        public void Dispatch<T1, T2, T3, T4>(string eventName, T1 param1, T2 param2, T3 param3, T4 param4)
        {
            Delegate d;
            if (m_EventDict.TryGetValue(eventName, out d))
            {
                Act<T1, T2, T3, T4> callback = d as Act<T1, T2, T3, T4>;

                if (callback != null)
                {
                    callback(param1, param2, param3, param4);
                }
                else
                {
                    Debug.LogError(string.Format(MSG_ERROR_4, this.GetType().Name, eventName));
                }
            }
        }
        #endregion
    }
}