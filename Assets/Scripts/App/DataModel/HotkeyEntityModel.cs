using System;
using System.Collections.Generic;
using UnityEngine;
using Company.NewApp.Entities;
using Company.Constants;

namespace Company.NewApp.Models
{
    public class HotkeyEntityModel : DataModelBase
    {
        private List<HotkeyEntity> m_SingleHotkeyEntityList = new List<HotkeyEntity>();

        private List<HotkeyEntity> m_DoubleHotkeyEntityList = new List<HotkeyEntity>();

        private List<HotkeyEntity> m_TrippleHotkeyEntityList = new List<HotkeyEntity>();


        public List<HotkeyEntity> SingleHotkeyEntityList { get { return m_SingleHotkeyEntityList; } }

        public List<HotkeyEntity> DoubleHotkeyEntityList { get { return m_DoubleHotkeyEntityList; } }

        public List<HotkeyEntity> TrippleHotkeyEntityList { get { return m_TrippleHotkeyEntityList; } }


        public override void Init(params object[] parameters)
        {
            base.Init(parameters);

            //CreateEntityList();
        }

        public void CreateEntityList()
        {
            string csv = FileManager.Instance.ReadTextFromStreamingAssets(Defines.Path.CSV_HOTKEY);
            MyJson.JObject csvHotkey = CSV.LoadCSV("Hotkey", csv).AsDict();

            MyJson.JObject tempDict = null;
            foreach (var itemDict in csvHotkey)
            {
                tempDict = itemDict.Value.AsDict();
                HotkeyEntity entity = new HotkeyEntity();

                entity.Id = itemDict.Key.ToInt();

                entity.EventName = tempDict["EventName"].AsString();

                string key1 = tempDict["Key1"].AsString();
                if (key1 != "" && !Enum.TryParse(key1, out entity.InputKeyType1))
                {
                    entity.InputKeyType1 = InputKeyType.Specify;
                    entity.KeyCode1 = (KeyCode)Enum.Parse(typeof(KeyCode), key1);
                }
                string key2 = tempDict["Key2"].AsString();
                if (key2 != "" && !Enum.TryParse(key2, out entity.InputKeyType2))
                {
                    entity.InputKeyType2 = InputKeyType.Specify;
                    entity.KeyCode2 = (KeyCode)Enum.Parse(typeof(KeyCode), key2);
                }
                string key3 = tempDict["Key3"].AsString();
                if (key3 != "" && !Enum.TryParse(key3, out entity.InputKeyType3))
                {
                    entity.InputKeyType3 = InputKeyType.Specify;
                    entity.KeyCode3 = (KeyCode)Enum.Parse(typeof(KeyCode), key3);
                }

                if (entity.InputKeyType3 != InputKeyType.None)
                {
                    if (entity.InputKeyType1 != InputKeyType.Specify || entity.InputKeyType2 != InputKeyType.Specify)
                        Debug.LogErrorFormat("[HotkeyEntityModel] Error key combination at id:{0}, Key 1 and Key 2 should be both specified", entity.Id);
                    else
                        m_TrippleHotkeyEntityList.Add(entity);
                }
                else if (entity.InputKeyType2 != InputKeyType.None)
                {
                    if (entity.InputKeyType1 != InputKeyType.Specify)
                        Debug.LogErrorFormat("[HotkeyEntityModel] Error key combination at id:{0}, Key 1 should be specified", entity.Id);
                    else
                        m_DoubleHotkeyEntityList.Add(entity);
                }
                else if (entity.InputKeyType1 != InputKeyType.None)
                    m_SingleHotkeyEntityList.Add(entity);

                //Debug.Log("[Hotkey] entity id:" + entity.Id + " inputkeytype1:" + entity.InputKeyType1 + " keycode1:" + entity.KeyCode1);
                //Debug.Log("[Hotkey] entity id:" + entity.Id + " inputkeytype2:" + entity.InputKeyType2 + " keycode2:" + entity.KeyCode2);
                //Debug.Log("[Hotkey] entity id:" + entity.Id + " inputkeytype3:" + entity.InputKeyType3 + " keycode3:" + entity.KeyCode3);
            }


            if (AppSettings.Instance.LogEnabled)
                Debug.LogFormat("[HotkeyEntityModel] Single hotkey entity count:{0}. Double hotkey entity count:{1}. Tripple hotkey entity count:{2}", 
                    SingleHotkeyEntityList.Count,
                    DoubleHotkeyEntityList.Count,
                    TrippleHotkeyEntityList.Count);
        }
    }
}