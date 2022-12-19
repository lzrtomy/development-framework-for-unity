using System.Collections.Generic;

namespace Company.NewApp
{
    public class CachePool : UnitySingleton<CachePool>
    {
        private Dictionary<string, List<object>> m_CachePoolDict;

        public void Init()
        {
            m_CachePoolDict = new Dictionary<string, List<object>>();
        }

        /// <summary>
        /// 从缓存池中获取对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Get<T>() where T : class, new()
        {
            string className = typeof(T).Name;

            if (!m_CachePoolDict.ContainsKey(className))
            {
                m_CachePoolDict[className] = new List<object>();
            }

            if (m_CachePoolDict[className].Count == 0)
            {
                return Create<T>();
            }

            return m_CachePoolDict[className][0] as T;
        }

        private T Create<T>() where T : class, new()
        {
            return new T();
        }

        /// <summary>
        /// 放回缓存池
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        public void Return<T>(T t) where T : class, new()
        {
            string className = typeof(T).Name;

            if (!m_CachePoolDict.ContainsKey(className))
            {
                m_CachePoolDict[className] = new List<object>();
            }

            m_CachePoolDict[className].Add(t);
        }

        /// <summary>
        /// 释放指定类型缓存池
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void Release<T>() where T : class, new()
        {
            string className = typeof(T).Name;

            if (m_CachePoolDict[className] != null)
            {
                m_CachePoolDict[className].Clear();
            }
        }

        /// <summary>
        /// 释放全部类型缓存池
        /// </summary>
        public void ReleaseAll()
        {
            var it = m_CachePoolDict.GetEnumerator();

            while (it.MoveNext())
            {
                it.Current.Value.Clear();
            }

            m_CachePoolDict.Clear();
        }
    }
}