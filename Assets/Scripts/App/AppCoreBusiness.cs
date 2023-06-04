
using Company.Constants;
using Company.Tools;
using UnityEngine;

namespace Company.NewApp
{
    public class AppCoreBusiness : SimpleUnitySingleton<AppCoreBusiness>
    {
        private void Start()
        {
            Init();
        }

        /// <summary>
        /// 核心业务逻辑入口
        /// </summary>
        public void Init()
        {
            Debug.Log("[AppCoreBusiness] GameStart");

            AddressablesManager.Instance.LoadAsset(AddressablesUtility.AssetPathToName("Prefabs/Sphere.prefab"), (GameObject result) => 
            {
                GameObject go = ResourcesManager.Instance.Clone(result);
                go.transform.position = Vector3.zero;
                Debug.Log("加载球成功");
            });
        }

    }
}