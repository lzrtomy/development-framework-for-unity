using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Company.NewApp.Models;
using Company.NewApp.Entities;

namespace Company.NewApp
{
    public class HotkeyManager : EventManagerBase<HotkeyManager>
    {
        private HotkeyEntityModel m_HotkeyEntityModel;

        private HotkeyEntity m_TempHotkeyEntity;

        public override void Init()
        {
            base.Init();

            m_HotkeyEntityModel = DataModelManager.Instance.GetDataModel<HotkeyEntityModel>();
        }

        void Update()
        {
            if (!string.IsNullOrEmpty(Input.inputString))
            {
                Debug.Log("input:" + Input.inputString);
            }
            if (!CheckTrippleKeyInput())
            {
                if (!CheckDoubleKeyInput())
                {
                    CheckSingleKeyInput();
                }
            }
        }

        /// <summary>
        /// �������������ϵ�����
        /// </summary>
        /// <returns></returns>
        private bool CheckTrippleKeyInput()
        {
            bool isTrippleKeyInput = false;
            for (int index = 0; index < m_HotkeyEntityModel.TrippleHotkeyEntityList.Count; index++)
            {
                m_TempHotkeyEntity = m_HotkeyEntityModel.TrippleHotkeyEntityList[index];

                if (Input.GetKey(m_TempHotkeyEntity.KeyCode1) && Input.GetKey(m_TempHotkeyEntity.KeyCode2))
                {
                    isTrippleKeyInput = true;
                    if (CheckFinalInputKey(m_TempHotkeyEntity.EventName, m_TempHotkeyEntity.InputKeyType3, m_TempHotkeyEntity.KeyCode3))
                    {
                        return true;
                    }
                }
            }
            return isTrippleKeyInput;
        }

        /// <summary>
        /// �������������ϵ�����
        /// </summary>
        /// <returns></returns>
        private bool CheckDoubleKeyInput()
        {
            bool isDoubleKeyInput = false;
            for (int index = 0; index < m_HotkeyEntityModel.DoubleHotkeyEntityList.Count; index++)
            {
                m_TempHotkeyEntity = m_HotkeyEntityModel.DoubleHotkeyEntityList[index];

                if (Input.GetKey(m_TempHotkeyEntity.KeyCode1))
                {
                    isDoubleKeyInput = true;
                    if (CheckFinalInputKey(m_TempHotkeyEntity.EventName, m_TempHotkeyEntity.InputKeyType2, m_TempHotkeyEntity.KeyCode2))
                    {
                        return true;
                    }
                }
            }
            return isDoubleKeyInput;
        }

        /// <summary>
        /// ��ⵥ������������
        /// </summary>
        /// <returns></returns>
        private bool CheckSingleKeyInput()
        {
            for (int index = 0; index < m_HotkeyEntityModel.SingleHotkeyEntityList.Count; index++)
            {
                m_TempHotkeyEntity = m_HotkeyEntityModel.SingleHotkeyEntityList[index];

                if (CheckFinalInputKey(m_TempHotkeyEntity.EventName, m_TempHotkeyEntity.InputKeyType1, m_TempHotkeyEntity.KeyCode1))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// ������һ������İ���
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="type"></param>
        /// <param name="keyCode"></param>
        /// <returns></returns>
        private bool CheckFinalInputKey(string eventName, InputKeyType type, KeyCode keyCode)
        {
            if (type == InputKeyType.AnyKey)
            {
                if (Input.anyKeyDown)
                {
                    Dispatch(eventName);
                    return true;
                }
            }
            if (type == InputKeyType.Specify)
            {
                if (Input.GetKeyDown(keyCode))
                {
                    Dispatch(eventName);
                    return true;
                }
            }
            if (type == InputKeyType.AlphaNum)
            {
                for (int index = (int)KeyCode.Alpha0; index <= (int)KeyCode.Alpha9; index++)
                {
                    if (Input.GetKeyDown((KeyCode)index))
                    {
                        Dispatch<int>(eventName, (index - (int)KeyCode.Alpha0));
                        return true;
                    }
                }
            }
            if (type == InputKeyType.Alphabet)
            {
                for (int index = (int)KeyCode.A; index <= (int)KeyCode.Z; index++)
                {
                    if (Input.GetKeyDown((KeyCode)index))
                    {
                        Dispatch<string>(eventName, ((KeyCode)index).ToString());
                        return true;
                    }
                }
            }
            return false;
        }
    }
}