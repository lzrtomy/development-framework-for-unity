using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Company.Tools 
{
    public class MathTools
    {
        /// <summary>
        /// 字符串是否为整数值
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsIntNumberic(string text)
        {
            if (string.IsNullOrEmpty(text))
                return false;

            try
            {
                int num = Convert.ToInt32(text);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 字符串是否为浮点数值
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsFloatNumeric(string text)
        {
            if (string.IsNullOrEmpty(text))
                return false;

            try
            {
                float num = float.Parse(text);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 在最大值限制范围内做加法
        /// </summary>
        /// <param name="left">加号左</param>
        /// <param name="right">加号右</param>
        /// <param name="max">最大值</param>
        /// <returns></returns>
        public static int SafeAdd(int left, int right, int max = int.MaxValue)
        {
            int sum = left + right;
            return Mathf.Clamp(sum, int.MinValue, max);
        }
        public static float SafeAdd(float left, float right, float max = float.MaxValue)
        {
            float sum = left + right;
            return Mathf.Clamp(sum, float.MinValue, max);
        }

        /// <summary>
        /// 在最小值限制范围内做减法
        /// </summary>
        /// <param name="left">减号左</param>
        /// <param name="right">减号右</param>
        /// <param name="max">最小值</param>
        /// <returns></returns>
        public static int SafeReduce(int left, int right, int min = int.MinValue)
        {
            int diff = left - right;
            return Mathf.Clamp(diff, min, int.MaxValue);
        }
        public static float SafeReduce(float left, float right, float min = float.MinValue)
        {
            float diff = left - right;
            return Mathf.Clamp(diff, min, float.MaxValue);
        }

        /// <summary>
        /// 在双端限制范围内求乘积
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static float SafeMultiply(float left, float right, float min = float.MinValue, float max = float.MaxValue)
        {
            float product = left * right;
            return Mathf.Clamp(product, min, max);
        }

        public static float SafeDivide(float left, float right, float min = float.MinValue, float max = float.MaxValue)
        {
            float division = left / right;
            return Mathf.Clamp(division, min, max);
        }

        /// <summary>
        /// 在双端值限制范围内求百分比
        /// </summary>
        /// <param name="left">分子</param>
        /// <param name="right">分母</param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static float SafePercentage(float left, float right, float min = float.MinValue, float max = float.MaxValue)
        {
            if (right == 0)
            {
                return 0;
            }
            float percentage = left / right * 100;
            return Mathf.Clamp(percentage, min, max);
        }

        /// <summary>
        /// Vector3投影为水平面上的Vector2
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public static Vector2 ToVector2_Horizontal(Vector3 position)
        {
            return new Vector2(position.x, position.z);
        }

        /// <summary>
        /// 向量模方
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static float DistanceSquare(Vector2 a, Vector2 b)
        {
            return (float)(Math.Pow(b.x - a.x, 2) + Math.Pow(b.y - a.y, 2));
        }
        public static float DistanceSquare(Vector3 a, Vector3 b)
        {
            return (float)(Math.Pow(b.x - a.x, 2) + Math.Pow(b.y - a.y, 2) + Math.Pow(b.z - a.z, 2));
        }
        public static float DistanceSquare_Horizontal(Vector3 a, Vector3 b)
        {
            return (float)(Math.Pow(b.x - a.x, 2) + Math.Pow(b.z - a.z, 2));
        }

        /// <summary>
        /// 水平距离
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static float Distance_Horizontal(Vector3 a, Vector3 b)
        {
            return Vector2.Distance(ToVector2_Horizontal(a), ToVector2_Horizontal(b));
        }

        /// <summary>
        /// 向量叉乘
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static float VectorCross(Vector2 a, Vector2 b)
        {
            return a.x * a.y - b.x * b.y;
        }
        public static float VectorCross_Horizontal(Vector3 a, Vector3 b)
        {
            return a.x * a.z - b.x * b.z;
        }

        /// <summary>
        /// 计算两个向量在水平面上的投影的夹角(顺时针方向，有正负)
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static float Angle_Horizontal(Vector3 from, Vector3 to)
        {
            from.y = 0;
            to.y = 0;
            float angle = Vector3.Angle(from, to);
            Vector3 normal = Vector3.Cross(from, to);
            return angle * Mathf.Sign(Vector3.Dot(normal, Vector3.up));
        }

        /// <summary>
        /// 一元一次方程y值
        /// </summary>
        /// <param name="k">斜率</param>
        /// <param name="b">纵截距</param>
        /// <param name="x">x</param>
        /// <returns></returns>
        public static float GetLinearFunctionValue(float k, float b, float x)
        {
            return k * x + b;
        }
    }

    public class CollectionTools
    {
        public static bool ListAdd<T>(IList<T> list, T element, bool checkDuplicates = true)
        {
            if (list == null)
                list = new List<T>();

            if (!checkDuplicates)
            {
                list.Add(element);
                return true;
            }
            if (checkDuplicates && !list.Contains(element))
            {
                list.Add(element);
                return true;
            }
            return false;
        }

        public static bool ListRemove<T>(IList<T> list, T element)
        {
            if (list != null && list.Count > 0)
            {
                if (list.Contains(element))
                {
                    list.Remove(element);
                    return true;
                }
            }
            return false;
        }

        public static bool ListRemoveAt<T>(IList<T> list, int index)
        {
            if (list != null && list.Count > index)
            {
                list.RemoveAt(index);
                return true;
            }
            return false;
        }

        public static IList<V> ListFormatConversion<U,V>(IList<U> list) where U : UnityEngine.Object where V: UnityEngine.Object
        {
            IList<V> result = new List<V>();
            for (int index = 0; index < list.Count; index++) 
            {
                result.Add(list[index] as V);
            }
            return result;
        }

        public static V[] ArrayFormatConversion<U, V>(U[] array) where U : UnityEngine.Object where V : UnityEngine.Object 
        {
            V[] result = new V[array.Length];
            for (int index = 0; index < array.Length; index++) 
            {
                result[index] = array[index] as V;
            }
            return result;
        }

        public static bool IsNullOrEmpty<T>(T[] array)
        {
            if (array == null || array.Length == 0)
                return true;
            return false;
        }

        public static bool IsNullOrEmpty<T>(List<T> list)
        {
            if (list == null || list.Count == 0)
                return true;
            return false;
        }
    }

    public class RandomTools 
    {

        # region 从权重数组中随机选取指定数量的下标结果

        /// <summary>
        /// 从权重数组中随机选取指定数量的下标结果
        /// </summary>
        /// <param name="weightArray">权重数组</param>
        /// <param name="quantity">选取结果数量</param>
        /// <param name="weightSum">权重和，不输入则会根据权重数组计算一遍</param>
        /// <returns></returns>
        public static int[] GetRandomIndexes(int[] weightArray, int quantity, int weightSum = -1)
        {
            //随抽取次数发生变化的权重集临时数组
            List<int> tempWeightList = new List<int>();
            for (int index = 0; index < weightArray.Length; index++)
            {
                tempWeightList.Add(index);
            }

            //随抽取次数发生变化的结果集临时数组
            List<int> tempIndexList = new List<int>();
            for (int index = 0; index < weightArray.Length; index++)
            {
                tempIndexList.Add(index);
            }

            //抽取结果数组
            int[] result = new int[quantity];

            if (weightSum < 0)
            {
                weightSum = WeightSum(tempWeightList);
            }

            for (int index = 0; index < quantity; index++)
            {
                //构建权重范围数组
                int[] weightRangeArray = CreateWeightTerraceArray(weightArray);

                //生成随机数
                float random = UnityEngine.Random.Range(0, (float)weightSum);

                //随机到的元素index
                int randomIndex = GetIndexByValue(random, weightRangeArray);

                if (randomIndex >= 0)
                {
                    result[index] = tempIndexList[randomIndex];

                    if (randomIndex >= 0)
                    {
                        tempWeightList.RemoveAt(randomIndex);
                        tempIndexList.RemoveAt(randomIndex);
                        weightSum = WeightSum(tempWeightList);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 构建权重阶梯数组
        /// </summary>
        /// <param name="weightArray"></param>
        /// <returns></returns>
        private static int[] CreateWeightTerraceArray(int[] weightArray)
        {
            int[] weightRangeArray = new int[weightArray.Length];
            weightRangeArray[0] = weightArray[0];
            for (int index = 1; index < weightRangeArray.Length; index++)
            {
                weightRangeArray[index] = weightRangeArray[index - 1] + weightArray[index];
            }
            return weightRangeArray;
        }

        /// <summary>
        /// 求权重和
        /// </summary>
        /// <param name="weightList"></param>
        /// <returns></returns>
        private static int WeightSum(List<int> weightList)
        {
            int sum = 0;
            for (int index = 0; index < weightList.Count; index++)
            {
                sum += weightList[index];
            }
            return sum;
        }

        /// <summary>
        /// 求随机数对应的区间index
        /// </summary>
        /// <param name="value"></param>
        /// <param name="array"></param>
        /// <returns></returns>
        private static int GetIndexByValue(float value, int[] array)
        {
            if (value < array[0])
            {
                return 0;
            }
            for (int index = 1; index < array.Length; index++)
            {
                if (value >= array[index - 1] && value < array[index])
                {
                    return index;
                }
            }
            if (value >= array[array.Length - 1])
            {
                return array.Length - 1;
            }

            return -1;
        }

        #endregion

        /// <summary>
        /// 圆边缘随机选点(二位空间)
        /// </summary>
        /// <param name="center"></param>
        /// <param name="maxRadiu"></param>
        /// <returns></returns>
        public static Vector2 GetRandomPointAroundCircle(Vector2 center, float maxRadiu)
        {
            float radians = Mathf.PI * UnityEngine.Random.Range(0, 360) / 180;
            float x = center.x + maxRadiu * Mathf.Sin(radians);
            float y = center.y + maxRadiu * Mathf.Cos(radians);
            return new Vector2(x, y);
        }

        /// <summary>
        /// 圆边缘随机选点(三维空间水平面)
        /// </summary>
        /// <param name="center"></param>
        /// <param name="maxRadiu"></param>
        /// <returns></returns>
        public static Vector3 GetRandomPointAroundCircle_Horizontal(Vector3 center, float maxRadiu)
        {
            float radians = Mathf.PI * UnityEngine.Random.Range(0, 360) / 180;
            float x = center.x + maxRadiu * Mathf.Sin(radians);
            float z = center.z + maxRadiu * Mathf.Cos(radians);
            return new Vector3(x,center.y, z);
        }

        /// <summary>
        /// 圆内随机选点(三维空间水平面)
        /// </summary>
        /// <param name="center"></param>
        /// <param name="maxRadiu"></param>
        /// <returns></returns>
        public static Vector3 GetRandomPointInCircle_Horizontal(Vector3 center, float maxRadiu)
        {
            float radius = UnityEngine.Random.Range(0, maxRadiu);
            float radians = Mathf.PI * UnityEngine.Random.Range(0, 360) / 180;
            float x = center.x + radius * Mathf.Sin(radians);
            float z = center.z + radius * Mathf.Cos(radians);
            return new Vector3(x, center.y, z);
        }
    }

    public class ParseTools
    {
        /// <summary>
        /// 将字符串解析为字典
        /// 格式：1,1;2,2;3,3
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Dictionary<int, int> ArrayToDict(string str)
        {
            Dictionary<int, int> dict = new Dictionary<int, int>();

            string[] dictArray = str.Split(';');
            for (int index = 0; index < dictArray.Length; index++)
            {
                string[] itemArray = dictArray[index].Split(',');
                if (MathTools.IsIntNumberic(itemArray[0]) && MathTools.IsIntNumberic(itemArray[1]))
                {
                    dict[Convert.ToInt32(itemArray[0])] = Convert.ToInt32(itemArray[1]);
                }
            }

            return dict;
        }

        /// <summary>
        /// 清理字符串数组中的无效字符串
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static List<string> CleanStrArrayToList(string[] array)
        {
            List<string> list = new List<string>();
            for (int index = 0; index < array.Length; index++)
            {
                if (!string.IsNullOrEmpty(array[index]))
                {
                    list.Add(array[index]);
                }
            }
            return list;
        }
        public static string[] CleanStrArrayToArray(string[] array)
        {
            return CleanStrArrayToList(array).ToArray();
        }
    }

    public class EditorTools 
    {

    }

    public class DebugTools
    {
        public static bool LogEnabled = true;

        public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration)
        {
            if (!LogEnabled)
                return;

            Debug.DrawLine(start, end, color, duration);
        }
    }

    public class StringTools 
    {
        public static string ConnectString(List<string> stringList) 
        {
            StringBuilder sb = new StringBuilder();
            for (int index = 0; index < stringList.Count; index++) 
            {
                sb.Append(stringList[index]);
            }
            return sb.ToString();
        }

        public static string ConnectString(string[] stringArray)
        {
            StringBuilder sb = new StringBuilder();
            for (int index = 0; index < stringArray.Length; index++)
            {
                sb.Append(stringArray[index]);
            }
            return sb.ToString();
        }
    }
}
