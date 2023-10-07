using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Company.NewApp.Entities;
using System;

namespace Company.NewApp.Models
{
    public class ExampleEntityModel : DataModelBase
    {
        private Dictionary<int, ExampleEntity> m_ExampleEntityDict;
        public Dictionary<int, ExampleEntity> ExampleEntityDict { get { return m_ExampleEntityDict; } }

        public override void Init(params object[] parameters)
        {
            base.Init(parameters);
            m_ExampleEntityDict = new Dictionary<int, ExampleEntity>();
            CreateExampleEntityLib();

            m_InitState = InitState.Inited;
        }

        /// <summary>
        /// 获取样例数据对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ExampleEntity GetExampleEntity(int id) 
        {
            if (ExampleEntityDict.ContainsKey(id)) 
            {
                return ExampleEntityDict[id];
            }
            return null;
        }

        private void CreateExampleEntityLib() 
        {
            List<Example_GeneralRawEntity> rawEntities = Example_GeneralRawEntityModel.Instance.GetList();
            for (int index = 0; index < rawEntities.Count; index++) 
            {
                ExampleEntity entity = new ExampleEntity();
                entity.Id = rawEntities[index].Id;
                entity.StringValue = rawEntities[index].StrValue;
                entity.IntValue = rawEntities[index].IntValue;
                entity.FLoatValue = rawEntities[index].FloatValue;
                entity.LongValue = rawEntities[index].LongValue;
                m_ExampleEntityDict[entity.Id] = entity;
            }
            if (AppSettings.Instance.LogEnabled)
            {
                Debug.Log("[ExampleModel] example count:" + ExampleEntityDict.Count);
            }
        }
    }
}