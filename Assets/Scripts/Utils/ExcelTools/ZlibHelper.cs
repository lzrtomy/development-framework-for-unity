using ComponentAce.Compression.Libs.zlib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ZlibHelper
{
    /// <summary>
    /// 对原始字节数组进行zlib压缩，得到处理结果字节数组
    /// </summary>
    /// <param name="orgByte">需要被压缩的原始数据</param>
    /// <param name="compressRate">压缩率，有默认值</param>
    /// <returns></returns>
    public static byte[] CompressBytes(byte[] orgBytes,int compressRate = zlibConst.Z_BEST_SPEED)
    {
        if (orgBytes == null) return null;

        using (MemoryStream orgStream = new MemoryStream(orgBytes))
        {
            using (MemoryStream compressedStream = new MemoryStream())
            {
                using (ZOutputStream outZStream = new ZOutputStream(compressedStream,compressRate))
                {
                    try
                    {
                        CopyStream(orgStream, outZStream);
                        //重要，否则结果数据不完整
                        outZStream.finish();
                        //执行到这里，compressedStream就是压缩后的数据
                        if (compressedStream == null) return null;

                        return compressedStream.ToArray();
                    }
                    catch(Exception ex)
                    {
                        Debug.LogError("压缩数据出错 " + ex.Message);
                        return null;
                    }
                }
            }
        }
    }

    /// <summary>
    /// 对经过Zlip压缩的数据，进行解压缩
    /// </summary>
    /// <param name="compressedBytes">被压缩的byte数组</param>
    /// <returns></returns>
    public static byte[] DeCompressBytes(byte[] compressedBytes)
    {
        if (compressedBytes == null) return null;

        using (MemoryStream compressedStream = new MemoryStream(compressedBytes))
        {
            using (MemoryStream orgStream = new MemoryStream())
            {
                using (ZOutputStream outZStream = new ZOutputStream(orgStream))
                {
                    try
                    {
                        CopyStream(compressedStream,outZStream);
                        //重要，确保数据完整
                        outZStream.finish();
                        //orgStream就是解压后的数据
                        if (orgStream == null) return null;
                        return orgStream.ToArray();
                    }
                    catch(Exception ex)
                    {
                        Debug.LogError("解压缩出错 " + ex.Message);
                        return null;
                    }
                }
            }
        }
    }

    /// <summary>
    /// 压缩字符串
    /// </summary>
    /// <returns></returns>
    public static string CompressString(string sourceString,int compressRate = zlibConst.Z_DEFAULT_COMPRESSION)
    {
        byte[] byteSource = System.Text.Encoding.UTF8.GetBytes(sourceString);
        byte[] byteCompress = CompressBytes(byteSource,compressRate);
        if(byteCompress != null)
        {
            return Convert.ToBase64String(byteCompress);
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// 解压字符串
    /// </summary>
    /// <param name="sourceString">需要被解压的字符串</param>
    /// <returns></returns>
    public static string DecompressString(string sourceString)
    {
        byte[] byteSource = Convert.FromBase64String(sourceString);
        byte[] byteDecompress = DeCompressBytes(byteSource);
        if(byteDecompress != null)
        {
            return System.Text.Encoding.UTF8.GetString(byteDecompress);
        }
        else
        {
            return null;
        }
    }


    /// <summary>
    /// 拷贝流
    /// </summary>
    /// <param name="input"></param>
    /// <param name="output"></param>
    private static void CopyStream(Stream input,Stream output)
    {
        byte[] buffer = new byte[2000];
        int len;
        while ((len = input.Read(buffer, 0, 2000)) > 0)
        {
            output.Write(buffer, 0, len);
        }
        output.Flush();
    }

    /// <summary>
    /// 将解压缩的二进制数据转换为字符串
    /// </summary>
    /// <returns></returns>
    public static string GetStringByByteArr(byte[] zipData)
    {
        return System.Text.Encoding.UTF8.GetString(zipData);
    }
}
