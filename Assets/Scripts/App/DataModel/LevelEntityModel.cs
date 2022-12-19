using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Company.NewApp.Entities;

namespace Company.NewApp.Models
{
    public class LevelEntityModel : DataModelBase
    {
        private List<LevelEntity> m_LevelEntityList = new List<LevelEntity>();

        public List<LevelEntity> LevelEntityList { get { return m_LevelEntityList; } }

        public override void Init(params object[] parameters)
        {
            CreateLevelEntityList();
        }

        /// <summary>
        /// ��ȡ�ؿ����ݶ���
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public LevelEntity GetLevelEntity(int Id)
        {
            if (LevelEntityList.Count >= Id)
            {
                return LevelEntityList[Id - 1];
            }
            return null;
        }

        /// <summary>
        /// ���ɹؿ����ݶ���List
        /// </summary>
        private void CreateLevelEntityList()
        {
            List<Level_GeneralRawEntity> rawEntityList = Level_GeneralRawEntityModel.Instance.GetList();
            for (int index = 0; index < rawEntityList.Count; index++)
            {
                LevelEntity entity = new LevelEntity();
                entity.ID = rawEntityList[index].Id;

                //TODO �����Զ���ؿ�����

                LevelEntityList.Add(entity);
            }

            if (AppSettings.Instance.LogEnabled)
                Debug.Log("[LevelModel] level count:" + LevelEntityList.Count);
        }
    }
}