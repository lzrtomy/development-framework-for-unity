
using UnityEngine;

namespace Company.NewApp
{
    public class AppMemento : MonoBehaviour
    {
        //µ±Ç°¹Ø¿¨
        private static int m_CurrentLevel = 0;

        public static int CurrentLevel
        {
            get { return m_CurrentLevel; }
            set
            {
                LastLevel = m_CurrentLevel;
                m_CurrentLevel = value;
            }
        }

        public static int LastLevel { get; set; }
    }
}
