using Company.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Company.Tools
{
    /// <summary>
    /// 读者需要注意两个点：
    /// 1.计时任务对象初始化时，将时间间隔赋值给当前时间变量，Update更新时间时，判断当前时间是否大于或等于首次延迟调用+时间间隔的和，能保证首次执行计时按照延迟时间，并在这之后将延迟时间等于0.
    /// 2.删除和增加定时器不能直接添加，暂时缓存的集合中，在遍历之前添加到执行任务字典中，遍历结束删除终止的定时器。
    /// 3.重复次数等于0，则执行无限计时。
    /// </summary>
    public class TimerTool : UnitySingleton<TimerTool>
    {
        private Dictionary<int, TimerTask> m_TimerTasks = new Dictionary<int, TimerTask>();
        private List<TimerTask> m_CacheRemoveList = new List<TimerTask>();
        private List<TimerTask> m_CacheAddList = new List<TimerTask>();
        private int TaskID = 0;
        private bool m_IsPause = false;
        public void Update()
        {
            if (m_IsPause)
                return;
            //缓存增加任务添加到执行任务字典
            for (int index = 0; index < m_CacheAddList.Count; index++)
            {
                m_TimerTasks.Add(m_CacheAddList[index].ID, m_CacheAddList[index]);
            }
            m_CacheAddList.Clear();

            for (int index = 0; index < m_TimerTasks.Count; index++)
            {
                var timerTask = m_TimerTasks.ElementAt(index);
                if (timerTask.Value.IsCanRemove)
                {
                    continue;
                }
                if (timerTask.Value.CurTime < timerTask.Value.Duration + timerTask.Value.DelayTime)
                {
                    timerTask.Value.CurTime += Time.deltaTime;
                }
                else
                {
                    timerTask.Value.Complete?.Invoke(timerTask.Value.Paramt);
                    timerTask.Value.CurTime = 0;
                    timerTask.Value.DelayTime = 0;
                    timerTask.Value.Repeat--;

                    if (timerTask.Value.Repeat == 0)
                    {
                        m_TimerTasks.Remove(timerTask.Key);
                        //移除会改变执行任务字典，确保执行全部任务
                        index -= 1;
                    }
                }
            }
            //移除终止任务对象
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
        /// 单次定时器(无参)
        /// </summary>
        /// <param name="complete">回调</param>
        /// <param name="delay">延时间隔</param>
        /// <returns></returns>
        public int TimingOnce(Action<object> complete, float delay)
        {
            return Timing(complete, 1, delay);
        }
        /// <summary>
        /// 单次定时器(有参)
        /// </summary>
        /// <param name="complete">回调</param>
        /// <param name="paramt">参数</param>
        /// <param name="delay">延时间隔</param>
        /// <returns></returns>
        public int TimingOnce(Action<object> complete, object paramt, float delay)
        {
            return Timing(complete, paramt, 1, delay);
        }
        /// <summary>
        /// 重复定时器(无参)
        /// </summary>
        /// <param name="complete">回调函数</param>
        /// <param name="repeat">重复次数</param>
        /// <param name="duration">时间间隔</param>
        /// <param name="delay">延时</param>
        /// <returns></returns>
        public int Timing(Action<object> complete, int repeat, float delay, float duration = 0)
        {
            return Timing(complete, null, repeat, delay, duration);
        }
        /// <summary>
        /// 重复定时器(有参)
        /// </summary>
        /// <param name="complete">回调</param>
        /// <param name="paramt">回调参数</param>
        /// <param name="repeat">重复次数</param>
        /// <param name="duration">计时间隔</param>
        /// <param name="delay">开始延时间隔</param>
        /// <returns></returns>
        public int Timing(Action<object> complete, object paramt, int repeat, float delay, float duration = 0)
        {
            TaskID++;
            TimerTask timerTask = new TimerTask();
            timerTask.Complete = complete;
            timerTask.Paramt = paramt;
            timerTask.Repeat = repeat;
            timerTask.Duration = duration;
            timerTask.DelayTime = delay;
            timerTask.CurTime = duration;
            timerTask.ID = TaskID;
            timerTask.IsCanRemove = false;
            m_CacheAddList.Add(timerTask);
            return TaskID;
        }
        /// <summary>
        /// 停止计时
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
        /// 暂停定时器模块
        /// </summary>
        public void PauseTiming()
        {
            m_IsPause = true;
        }
        /// <summary>
        /// 继续定时器
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
        public float Duration;
        public float DelayTime;
        public float CurTime;
        public int Repeat;
        public bool IsCanRemove;
    }
}

