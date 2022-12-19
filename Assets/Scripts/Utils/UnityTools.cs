using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Company.Tools
{
    public class UnityTools : UnitySingleton<UnityTools>
    {
        private Dictionary<string, IEnumerator> _ieDict = new Dictionary<string, IEnumerator>();
        public int CoroutineCount = 0;
        private Vector2 _piviot = new Vector2(0.5f, 0.5f);


        // Note that Color32 and Color implictly convert to each other. You may pass a Color object to this method without first casting it.
        public string ColorToHex(Color32 color)
        {
            string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
            return hex;
        }

        public Color HexToColor(string hex)
        {
            hex = hex.Replace("0x", "");
            hex = hex.Replace("#", "");
            byte a = 255;
            byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            if (hex.Length == 8)
            {
                a = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
            }
            return new Color32(r, g, b, a);
        }

        public IEnumerator LateExec(float latency, Action lateAction)
        {
            string guid = GetGUID();
            IEnumerator ieLateExec = IELateExec(latency, lateAction, guid);
            _ieDict.Add(guid, ieLateExec);
            CoroutineCount = _ieDict.Keys.Count;
            StartCoroutine(ieLateExec);
            return ieLateExec;
        }

        private IEnumerator IELateExec(float latency, Action lateAction, string guid)
        {
            int temp = 0;
            yield return new WaitForSeconds(latency);
            lateAction?.Invoke();
            RemoveDict(guid, _ieDict);
            CoroutineCount = _ieDict.Keys.Count;
        }

        public IEnumerator LoopExec(float latency, Action loopAction, bool isLateExec = false)
        {
            string guid = GetGUID();
            IEnumerator ieLoopExec = IELoopExec(latency, loopAction, guid, isLateExec);
            _ieDict.Add(guid, ieLoopExec);
            CoroutineCount = _ieDict.Keys.Count;
            StartCoroutine(ieLoopExec);
            return ieLoopExec;
        }

        private IEnumerator IELoopExec(float latency, Action loopAction, string guid, bool isLateExec = false)
        {
            WaitForSeconds wfs = new WaitForSeconds(latency);
            if (!isLateExec)
            {
                loopAction?.Invoke();
            }
            while (true)
            {
                yield return wfs;
                loopAction?.Invoke();
            }
            RemoveDict(guid, _ieDict);
            CoroutineCount = _ieDict.Keys.Count;
        }

        public void StopExec(ref IEnumerator ienumerator)
        {
            if (ienumerator != null)
            {
                string guid = string.Empty;
                foreach (string key in _ieDict.Keys)
                {
                    if (_ieDict[key].Equals(ienumerator))
                    {
                        guid = key;
                        break;
                    }
                }
                if (!string.IsNullOrEmpty(guid))
                {
                    _ieDict.Remove(guid);
                    CoroutineCount = _ieDict.Keys.Count;
                }
                StopCoroutine(ienumerator);
                ienumerator = null;
            }
        }

        public void StopExec(IEnumerator ienumerator)
        {
            if (ienumerator != null)
            {
                string guid = string.Empty;
                foreach (string key in _ieDict.Keys)
                {
                    if (_ieDict[key].Equals(ienumerator))
                    {
                        guid = key;
                        break;
                    }
                }
                if (!string.IsNullOrEmpty(guid))
                {
                    _ieDict.Remove(guid);
                    CoroutineCount = _ieDict.Keys.Count;
                }
                StopCoroutine(ienumerator);
            }
        }

        public void StartExec(IEnumerator ienumerator)
        {
            string guid = GetGUID();
            _ieDict.Add(guid, ienumerator);
            CoroutineCount = _ieDict.Keys.Count;
            StartCoroutine(ienumerator);
        }

        public void StopAllExec()
        {
            _ieDict.Clear();
            CoroutineCount = _ieDict.Keys.Count;
            StopAllCoroutines();
        }

        public void RemoveDict<T, V>(T key, Dictionary<T, V> dict)
        {
            if (dict.ContainsKey(key))
            {
                dict.Remove(key);
            }
        }

        public void StartIE(IEnumerator ienumerator)
        {
            if (ienumerator != null)
            {
                StartCoroutine(ienumerator);
            }
        }

        public void StopIE(IEnumerator ienumerator)
        {
            if (ienumerator != null)
            {
                StopCoroutine(ienumerator);
            }
        }

        public void StopIEs(params IEnumerator[] ienumerators)
        {
            if (ienumerators.Length > 0)
            {
                for (int index = 0; index < ienumerators.Length; index++)
                {
                    StopIE(ienumerators[index]);
                }
            }
        }

        public void StopIEs(List<IEnumerator> ienumerators)
        {
            if (ienumerators.Count > 0)
            {
                for (int index = 0; index < ienumerators.Count; index++)
                {
                    StopIE(ienumerators[index]);
                }
            }
        }

        public string GetGUID()
        {
            return Guid.NewGuid().ToString();
        }

        public Sprite Texture2Sprite(Texture texture)
        {
            Sprite sprite = null;
            if (texture != null)
            {
                sprite = Sprite.Create((Texture2D)texture, new Rect(0.0f, 0.0f, texture.width, texture.height), _piviot, 100.0f);
            }
            return sprite;
        }

        public int GetSecondsFromNow(DateTime destDateTime)
        {
            var lastTime = destDateTime.Subtract(DateTime.Now);
            return Mathf.CeilToInt((float)lastTime.TotalSeconds);
        }

        public int GetSecondsTillNow(DateTime fromDateTime)
        {
            var lastTime = DateTime.Now.Subtract(fromDateTime);
            return Mathf.CeilToInt((float)lastTime.TotalSeconds);
        }


        /// <summary>
        /// 计时器
        /// </summary>
        /// <param name="countDownTime">倒计时时长</param>
        /// <param name="Interval">倒计时检查间隔</param>
        /// <param name="finalCallBack">倒计时结束回调</param>
        /// <param name="intervalCallBack">倒计时检查回调</param>
        /// <returns></returns>
        public IEnumerator IETimer(float countDownTime, Action finalCallBack, float interval = -1, Action intervalCallBack = null) 
        {
            interval = interval == -1 ? countDownTime : interval;
            while (countDownTime > 0) 
            {
                countDownTime -= interval;
                yield return new WaitForSeconds(interval);
                intervalCallBack?.Invoke();
            }
            finalCallBack?.Invoke();

            yield break;
        }

        /// <summary>
        /// 限定FPS
        /// </summary>
        /// <param name="platform"></param>
        /// <param name="fps"></param>
        public void SetTargetFrameRate(RuntimePlatform platform, int fps)
        {
            if (Application.platform == platform)
            {
                Application.targetFrameRate = fps;
            }
        }

    }

    public class TsfmYComparer : IComparer<Transform>
    {
        public int Compare(Transform left, Transform right)
        {
            if (left != null && right != null)
            {
                if (left.position.y > right.position.y)
                    return 1;
                else if (left.position.y == right.position.y)
                    return 0;
                else
                    return -1;
            }
            return 0;
        }
    }

    
}
