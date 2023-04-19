using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Company.NewApp
{
    public class AssetBundleManager : UnitySingleton<AssetBundleManager>
    {
        public T LoadAssets<T>(string path, string key) where T : UnityEngine.Object
        {
            using (UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(path))
            {
                request.SendWebRequest();
                while (!request.isDone) { }

#if UNITY_2020_1_OR_NEWER
                if (request.result == UnityWebRequest.Result.Success)
#else
                if (!uwb.isHttpError && !uwb.isNetworkError)
#endif
                {
                    AssetBundle ab = DownloadHandlerAssetBundle.GetContent(request);
                    T t = ab.LoadAsset<T>(key);
                    return t;
                }
                return null;
            }
        }
    }
}