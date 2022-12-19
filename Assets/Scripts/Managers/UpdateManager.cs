using Company.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Company.NewApp
{
    [System.Serializable]
    public class UpdateExecutor
    {
        public delegate void Callback();

        public int Interval;
        public int Counter;
        public Callback OnUpdate;
    }

    public class UpdateManager : SimpleUnitySingleton<UpdateManager>
    {
        [Header(" «∑Ò‘›Õ£")]
        public bool IsPaused = false;

        [ReadOnly]
        [SerializeField] private List<UpdateExecutor> m_UpdateExecutorList = new List<UpdateExecutor>();

        [ReadOnly]
        [SerializeField] private List<UpdateExecutor> m_FixedUpdateExecutorList = new List<UpdateExecutor>();

        [ReadOnly]
        [SerializeField] private List<UpdateExecutor> m_LateUpdateExecutorList = new List<UpdateExecutor>();

        private UpdateExecutor m_TempUpdateExecutor = null;
        

        #region ====================== Fixed Update ======================

        private void FixedUpdate()
        {
            DoUpdate(m_FixedUpdateExecutorList);
        }

        public void AddFixedUpdate(UpdateExecutor.Callback onFixedUpdate, int interval)
        {
            AddUpdateExecutor(m_FixedUpdateExecutorList, onFixedUpdate, interval);
        }

        public void RemoveFixedUpdate(UpdateExecutor.Callback onFixedUpdate, int interval)
        {
            RemoveUpdateExecutor(m_FixedUpdateExecutorList, onFixedUpdate, interval);
        }

        public void ClearFixedUpdate()
        {
            m_FixedUpdateExecutorList.Clear();
        }

        #endregion ====================== Fixed Update ======================


        #region ====================== Update ======================

        private void Update()
        {
            DoUpdate(m_UpdateExecutorList);
        }

        public void AddUpdate(UpdateExecutor.Callback onUpdate, int interval)
        {
            AddUpdateExecutor(m_UpdateExecutorList, onUpdate, interval);
        }

        public void RemoveUpdate(UpdateExecutor.Callback onUpdate, int interval)
        {
            RemoveUpdateExecutor(m_UpdateExecutorList, onUpdate, interval);
        }

        public void ClearUpdate()
        {
            m_UpdateExecutorList.Clear();
        }

        #endregion ====================== Update ======================
        

        #region ====================== Late Update ======================

        private void LateUpdate()
        {
            DoUpdate(m_LateUpdateExecutorList);
        }

        public void AddLateUpdate(UpdateExecutor.Callback onLateUpdate, int interval)
        {
            AddUpdateExecutor(m_LateUpdateExecutorList, onLateUpdate, interval);
        }

        public void RemoveLateUpdate(UpdateExecutor.Callback onLateUpdate, int interval)
        {
            RemoveUpdateExecutor(m_LateUpdateExecutorList, onLateUpdate, interval);
        }

        public void ClearLateUpdate()
        {
            m_LateUpdateExecutorList.Clear();
        }

        #endregion ====================== Late Update ======================


        private void AddUpdateExecutor(List<UpdateExecutor> list, UpdateExecutor.Callback onUpdate, int interval)
        {
            interval = interval >= 0 ? interval : 0;
            for (int index = 0; index < list.Count; index++)
            {
                if (list[index].Interval == interval)
                {
                    list[index].OnUpdate += onUpdate;
                    return;
                }
            }

            UpdateExecutor executor = new UpdateExecutor()
            {
                Interval = interval,
                Counter = 0
            };
            executor.OnUpdate += onUpdate;
            list.Add(executor);
        }

        private void RemoveUpdateExecutor(List<UpdateExecutor> list, UpdateExecutor.Callback onUpdate, int interval)
        {
            interval = interval >= 0 ? interval : 0;
            for (int index = 0; index < list.Count; index++)
            {
                if (list[index].Interval == interval)
                {
                    list[index].OnUpdate -= onUpdate;
                    if (list[index].OnUpdate == null)
                    {
                        list[index].Counter = 0;
                    }
                    return;
                }
            }
        }
        
        private void DoUpdate(List<UpdateExecutor> list)
        {
            if (IsPaused)
                return;

            for (int index = 0; index < list.Count; index++)
            {
                m_TempUpdateExecutor = list[index];
                if (m_TempUpdateExecutor.OnUpdate != null)
                {
                    m_TempUpdateExecutor.Counter++;
                    if (m_TempUpdateExecutor.Counter > m_TempUpdateExecutor.Interval)
                    {
                        m_TempUpdateExecutor.Counter = 0;
                        m_TempUpdateExecutor.OnUpdate.Invoke();
                    }
                }
            }
        }

        public void ClearAll()
        {
            ClearUpdate();
            ClearFixedUpdate();
            ClearLateUpdate();
        }
    }
}