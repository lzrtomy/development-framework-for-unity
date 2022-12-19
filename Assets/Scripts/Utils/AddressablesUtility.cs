using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Company.Tools
{
    public class AddressablesUtility:Singleton<AddressablesUtility>
    {
        /// <summary>
        /// ��ȡ��Դ·���е���Դ���ƣ��������ļ���չ��
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string AssetPathToName(string path)
        {
            string[] sArray = path.Split('/');
            string[] nameArray = sArray[sArray.Length - 1].Split('.');
            return nameArray[0];
        }
    }
}