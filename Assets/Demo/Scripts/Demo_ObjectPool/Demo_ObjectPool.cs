using Company.NewApp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Company.DevFramework.Demo
{
    public class Demo_ObjectPool : MonoBehaviour
    {
        public void Start()
        {
            StartCoroutine(IETestObjectPool());
        }

        IEnumerator IETestObjectPool()
        {
            yield return new WaitForSeconds(1);

            List<GameObject> list = new List<GameObject>(); ;
            string key = "Prefabs/Sphere.prefab";
            for (int index = 0; index < 20; index++)
            {
                list.Add(ObjectPool.Instance.Get(key));
                list[index].transform.position += UnityEngine.Random.insideUnitSphere * 3;
                yield return new WaitForSeconds(0.2f);
            }

            for (int index = 0; index < 3; index++)
            {
                int hide = UnityEngine.Random.Range(1, 6);
                for (int indexHide = 0; indexHide < hide; indexHide++)
                {
                    if (list.Count > 0)
                    {
                        GameObject hideGo = list[list.Count - 1];
                        list.Remove(hideGo);
                        ObjectPool.Instance.Return(key, hideGo);
                    }
                    yield return new WaitForSeconds(0.5f);
                }

                yield return new WaitForSeconds(1);

                int show = UnityEngine.Random.Range(1, 6);
                for (int indexShow = 0; indexShow < show; indexShow++)
                {
                    list.Add(ObjectPool.Instance.Get(key));
                    list[index].transform.position += UnityEngine.Random.insideUnitSphere * 5;
                    yield return new WaitForSeconds(0.5f);
                }
            }

            yield return new WaitForSeconds(1.5f);

            for (int index = 0; index < list.Count; index++)
            {
                ObjectPool.Instance.Return(key, list[index]);
                yield return new WaitForSeconds(0.2f);
            }
            list.Clear();
            ObjectPool.Instance.ReleaseAll();
            Debug.Log("[Demo] Object Pool release all");
        }
    }
}