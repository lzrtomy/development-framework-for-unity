using Company.NewApp.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Company.NewApp.PersistentData;

namespace Company.NewApp
{
    public class DataModelManager : UnitySingleton<DataModelManager>
    {
        Dictionary<string, DataModelBase> m_DataModelDict = new Dictionary<string, DataModelBase>();
        Dictionary<string, AbstractPersistentData> m_PersistentDataDict = new Dictionary<string, AbstractPersistentData>();

        private bool m_LogEnabled = true;

        public Dictionary<string, DataModelBase> DataModelDict { get { return m_DataModelDict; } }
        public Dictionary<string, AbstractPersistentData> PersistentDataDict { get { return m_PersistentDataDict; } }

        public void Init(Action nextInitStep)
        {
            m_LogEnabled = AppSettings.Instance.LogEnabled;
            StartCoroutine(IELoadRawEntityData(nextInitStep));

        }

        IEnumerator IELoadRawEntityData(Action nextInitSetp) 
        {
            #region 加载表格数据示例

            yield return StartCoroutine(Example_GeneralRawEntityModel.Instance.IELoadData());
            if(m_LogEnabled)
                Debug.Log("[DataModelManager] Example raw entity count: " + Example_GeneralRawEntityModel.Instance.GetList().Count);
            yield return StartCoroutine(UI_GeneralRawEntityModel.Instance.IELoadData());
            if (m_LogEnabled)
                Debug.Log("[DataModelManager] UI raw entity count: " + UI_GeneralRawEntityModel.Instance.GetList().Count);
            yield return StartCoroutine(Level_GeneralRawEntityModel.Instance.IELoadData());

            #endregion


            //TODO 装载游戏数据

            nextInitSetp?.Invoke();
            yield break;
        }

        public T AddDataModel<T>(params object[] parameters) where T : DataModelBase, new()
        {
            string name = typeof(T).FullName;
            if (!m_DataModelDict.ContainsKey(name))
            {
                T t = new T();
                m_DataModelDict[name] = t;
                t.DataModelManager = this;
                t.Init(parameters);
            }
            return m_DataModelDict[name] as T;
        }

        public T GetDataModel<T>(params object[] parameters) where T : DataModelBase, new()
        {
            string name = typeof(T).FullName;
            DataModelBase model = null;
            m_DataModelDict.TryGetValue(name, out model);
            return model as T;
        }

        public void RemoveDataModel<T>() where T : DataModelBase, new()
        {
            string name = typeof(T).FullName;
            if (m_DataModelDict.ContainsKey(name))
            {
                m_DataModelDict.Remove(name);
            }
        }

        public T GetPersistentData<T>() where T : AbstractPersistentData, new()
        {
            string name = typeof(T).FullName;
            AbstractPersistentData data = null;
            m_PersistentDataDict.TryGetValue(name, out data);
            if (data == null)
            {
                data = new T();
                data.ReadData("test-user");
                m_PersistentDataDict[name] = data;
            }

            return data as T;
        }
        
        public void DeleteAllPersistentData()
        {
            var it = m_PersistentDataDict.GetEnumerator();
            while (it.MoveNext())
            {
                it.Current.Value.Delete();
            }
        }

    }
}