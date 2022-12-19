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
        /// �ӻ�����л�ȡ����
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
        /// �Żػ����
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
        /// �ͷ�ָ�����ͻ����
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
        /// �ͷ�ȫ�����ͻ����
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