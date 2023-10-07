using Company.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Company.Tools
{
    /// <summary>
    /// ������Ҫע�������㣺
    /// 1.��ʱ��������ʼ��ʱ����ʱ������ֵ����ǰʱ�������Update����ʱ��ʱ���жϵ�ǰʱ���Ƿ���ڻ�����״��ӳٵ���+ʱ�����ĺͣ��ܱ�֤�״�ִ�м�ʱ�����ӳ�ʱ�䣬������֮���ӳ�ʱ�����0.
    /// 2.ɾ�������Ӷ�ʱ������ֱ����ӣ���ʱ����ļ����У��ڱ���֮ǰ��ӵ�ִ�������ֵ��У���������ɾ����ֹ�Ķ�ʱ����
    /// 3.�ظ���������0����ִ�����޼�ʱ��
    /// </summary>
    public class TimerTool : UnitySingleton<TimerTool>
    {
        private Dictionary<int, TimerTask> m_TimerTasks = new Dictionary<int, TimerTask>();
        private List<TimerTask> m_CacheRemoveList = new List<TimerTask>();
        private List<TimerTask> m_CacheAddList = new List<TimerTask>();
        private int TaskID = 0;
        private bool m_IsPause = false;

        
        private TimerTask m_TempTimerTask = null;

        public void Update()
        {
            if (m_IsPause)
                return;
            //��������������ӵ�ִ�������ֵ�
            for (int index = 0; index < m_CacheAddList.Count; index++)
            {
                m_TimerTasks.Add(m_CacheAddList[index].ID, m_CacheAddList[index]);
            }
            m_CacheAddList.Clear();

            for (int index = 0; index < m_TimerTasks.Count; index++)
            {
                var timerTask = m_TimerTasks.ElementAt(index);
                m_TempTimerTask = timerTask.Value;

                if (m_TempTimerTask.IsCanRemove)
                {
                    continue;
                }
                if (m_TempTimerTask.CurTime < m_TempTimerTask.Duration + m_TempTimerTask.DelayTime)
                {
                    m_TempTimerTask.CurTime += m_TempTimerTask.IgnoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime;
                }
                else
                {
                    m_TempTimerTask.Complete?.Invoke(m_TempTimerTask.Paramt);
                    m_TempTimerTask.CurTime = 0;
                    m_TempTimerTask.DelayTime = 0;
                    m_TempTimerTask.Repeat = m_TempTimerTask.Repeat < 0 ? -1 : (m_TempTimerTask.Repeat - 1);

                    if (m_TempTimerTask.Repeat == 0)
                    {
                        m_TimerTasks.Remove(timerTask.Key);
                        //�Ƴ���ı�ִ�������ֵ䣬ȷ��ִ��ȫ������
                        index -= 1;
                    }
                }
            }
            //�Ƴ���ֹ�������
            for (int index = 0; index < m_CacheRemoveList.Count; index++)
            {
                if (m_TimerTasks.ContainsKey(m_CacheRemoveList[index].ID))
                {
                    m_TimerTasks.Remove(m_CacheRemoveList[index].ID);
                }
            }
            m_CacheRemoveList.Clear();
        }
        /// <summary>
        /// ���ζ�ʱ��(�޲�)
        /// </summary>
        /// <param name="complete">�ص�</param>
        /// <param name="delay">��ʱ���</param>
        /// <returns></returns>
        public int TimingOnce(Action<object> complete, float delay, bool ignoreTimeScale = false)
        {
            return Timing(complete, 1, delay, 0, ignoreTimeScale);
        }
        /// <summary>
        /// ���ζ�ʱ��(�в�)
        /// </summary>
        /// <param name="complete">�ص�</param>
        /// <param name="paramt">����</param>
        /// <param name="delay">��ʱ���</param>
        /// <returns></returns>
        public int TimingOnce(Action<object> complete, object paramt, float delay, bool ignoreTimeScale = false)
        {
            return Timing(complete, paramt, 1, delay, 0, ignoreTimeScale);
        }
        /// <summary>
        /// �ظ���ʱ��(�޲�)
        /// </summary>
        /// <param name="complete">�ص�����</param>
        /// <param name="repeat">�ظ�����</param>
        /// <param name="duration">ʱ����</param>
        /// <param name="delay">��ʱ</param>
        /// <returns></returns>
        public int Timing(Action<object> complete, int repeat, float delay, float duration = 0, bool ignoreTimeScale = false)
        {
            return Timing(complete, null, repeat, delay, duration, ignoreTimeScale);
        }
        /// <summary>
        /// �ظ���ʱ��(�в�)
        /// </summary>
        /// <param name="complete">�ص�</param>
        /// <param name="paramt">�ص�����</param>
        /// <param name="repeat">�ظ�����</param>
        /// <param name="duration">��ʱ���</param>
        /// <param name="delay">��ʼ��ʱ���</param>
        /// <returns></returns>
        public int Timing(Action<object> complete, object paramt, int repeat, float delay, float duration = 0, bool ignoreTimeScale = false)
        {
            TaskID++;
            TimerTask timerTask = new TimerTask();
            timerTask.Complete = complete;
            timerTask.Paramt = paramt;
            timerTask.Repeat = repeat;
            timerTask.IgnoreTimeScale = ignoreTimeScale;
            timerTask.Duration = duration;
            timerTask.DelayTime = delay;
            timerTask.CurTime = duration;
            timerTask.ID = TaskID;
            timerTask.IsCanRemove = false;
            m_CacheAddList.Add(timerTask);
            return TaskID;
        }
        /// <summary>
        /// ֹͣ��ʱ
        /// </summary>
        /// <param name="id"></param>
        public void StopTiming(int id)
        {
            if (!m_TimerTasks.ContainsKey(id))
                return;
            m_TimerTasks[id].IsCanRemove = true;
            m_CacheRemoveList.Add(m_TimerTasks[id]);
        }
        /// <summary>
        /// ��ͣ��ʱ��ģ��
        /// </summary>
        public void PauseTiming()
        {
            m_IsPause = true;
        }
        /// <summary>
        /// ������ʱ��
        /// </summary>
        public void ContinueTiming()
        {
            m_IsPause = false;
        }
    }
    public class TimerTask
    {
        public int ID;
        public Action<object> Complete;
        public object Paramt;
        public bool IgnoreTimeScale;
        public float Duration;
        public float DelayTime;
        public float CurTime;
        public int Repeat;
        public bool IsCanRemove;
    }
}

