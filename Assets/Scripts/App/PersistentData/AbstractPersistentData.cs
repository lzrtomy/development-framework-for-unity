using UnityEngine;

namespace Company.NewApp.PersistentData
{
    public abstract class AbstractPersistentData
    {
        public abstract void ReadData(string userId);

        public abstract void Save();

        public abstract void Delete();
    }

    [System.Serializable]
    public abstract class AbstractPersistentData<T> : AbstractPersistentData where T : AbstractPersistentData<T>, new()
    {
        protected static T m_Data = null;

        public string UserId = "";

        protected bool m_LogEnabled = true;

        protected string m_ClassName = "";

        protected string m_DataFieldName = "";

        public override void ReadData(string userId)
        {
            if (m_Data != null)
                return;

            UserId = userId;
            m_LogEnabled = AppSettings.Instance.LogEnabled;
            m_ClassName = typeof(T).FullName;
            m_DataFieldName = UserId + "_" + m_ClassName;

            string json = PlayerPrefs.GetString(m_DataFieldName, "");
            if (json != "")
            {
                JsonUtility.FromJsonOverwrite(json, this);

                if (m_LogEnabled)
                    Debug.LogFormat("[{0}] Load presistent data:\r\n{1}", m_ClassName, json);
            }
            else
            {
                if (m_LogEnabled)
                    Debug.LogFormat("[{0}] No persistent data to load.", m_ClassName);
            }
        }

        public override void Save()
        {
            string data = JsonUtility.ToJson(this);
            PlayerPrefs.SetString(m_DataFieldName, data);

            if (AppSettings.Instance.LogEnabled)
                Debug.LogFormat("[{0}] Save {1}:\r\n{2}", m_ClassName, m_DataFieldName, data);
        }

        public override void Delete()
        {
            PlayerPrefs.DeleteKey(m_DataFieldName);
        }
    }
}
