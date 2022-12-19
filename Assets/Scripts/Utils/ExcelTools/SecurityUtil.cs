using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityUtil
{
    /// <summary>
    /// 加密解密的因子
    /// </summary>
    private static readonly byte[] xorScale = new byte[] { 45, 66, 38, 55, 23, 254, 9, 165, 90, 19, 41, 45, 201, 58, 55, 37, 254, 185, 165, 169, 19, 171 };

    /// <summary>
    /// 对byte数组进行异或加密
    /// </summary>
    /// <returns></returns>
    public static byte[] Xor(byte[] buffer)
    {
        int iScaleLen = xorScale.Length;
        for (int i = 0; i < buffer.Length; i++)
        {
            buffer[i] = (byte)(buffer[i] ^ xorScale[i % iScaleLen]);
        }
        return buffer;

    }
}
