using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameEntityParser : IDisposable
{
    /// <summary>
    /// 行数
    /// </summary>
    private int _row;

    /// <summary>
    /// 列数
    /// </summary>
    private int _column;

    /// <summary>
    /// 字段名称
    /// </summary>
    private string[] _fieldNames;

    /// <summary>
    /// 字段名称
    /// </summary>
    public string[] FieldNames
    {
        get { return _fieldNames; }
    }

    /// <summary>
    /// 游戏数据
    /// </summary>
    private string[,] _gameData;

    /// <summary>
    /// 当前行号
    /// </summary>
    private int _curRowNum = 3;

    /// <summary>
    /// 字段名称字典
    /// </summary>
    private Dictionary<string, int> _fieldNameDic;

    /// <summary>
    /// 是否结束
    /// </summary>
    public bool Eof
    {
        get
        {
            return _curRowNum == _row;
        }
    }

    /// <summary>
    /// 构造方法
    /// </summary>
    /// <param name="path">配表路径</param>
    public GameEntityParser(string path)
    {
        _fieldNameDic = new Dictionary<string, int>();

        byte[] buffer = null;

        //第一步：读取文件
        using (FileStream fs = new FileStream(path, FileMode.Open))
        {
            buffer = new byte[fs.Length];
            fs.Read(buffer,0,buffer.Length);
        }
        //第二步：解压缩
        buffer = ZlibHelper.DeCompressBytes(buffer);
        //第三步：xor解密
        buffer = SecurityUtil.Xor(buffer);
        //第四步：解析数据到数组
        using (MemoryStreamInfo ms = new MemoryStreamInfo(buffer))
        {
            _row = ms.ReadInt();
            _column = ms.ReadInt();

            _gameData = new string[_row,_column];
            _fieldNames = new string[_column];

            for(int i = 0; i < _row;i++)
            {
                for(int j = 0;j < _column;j++)
                {
                    string str = ms.ReadUTF8String();
                    if(i == 0)
                    {
                        //表示读取的是字段
                        _fieldNames[j] = str;
                        _fieldNameDic[str] = j;
                    }else if(i > 2)
                    {
                        //表示读取的是内容
                        _gameData[i, j] = str;
                    }
                }
            }
        }
    }

    public GameEntityParser(byte[] data)
    {
        _fieldNameDic = new Dictionary<string, int>();

        byte[] buffer = data;

        buffer = ZlibHelper.DeCompressBytes(buffer);
        buffer = SecurityUtil.Xor(buffer);
        using (MemoryStreamInfo ms = new MemoryStreamInfo(buffer))
        {
            _row = ms.ReadInt();
            _column = ms.ReadInt();

            _gameData = new string[_row, _column];
            _fieldNames = new string[_column];

            for (int i = 0; i < _row; i++)
            {
                for (int j = 0; j < _column; j++)
                {
                    string str = ms.ReadUTF8String();
                    if (i == 0)
                    {
                        //表示读取的是字段
                        _fieldNames[j] = str;
                        _fieldNameDic[str] = j;
                    }
                    else if (i > 2)
                    {
                        //表示读取的是内容
                        _gameData[i, j] = str;
                    }
                }
            }
        }
    }

    /// <summary>
    /// 转到下一个
    /// </summary>
    public void Next()
    {
        if (Eof) return;
        _curRowNum++;
    }

    /// <summary>
    /// 获取字段值
    /// </summary>
    /// <param name="fieldName"></param>
    /// <returns></returns>
    public string GetFieldValue(string fieldName)
    {
        try
        {
            if (_curRowNum < 3 || _curRowNum >= _row) return null;
            return _gameData[_curRowNum,_fieldNameDic[fieldName]];
        }catch(Exception ex)
        {
            Debug.LogError("读取生成数据错误 " + ex.Message);
            return null;
        }
    }

    /// <summary>
    /// 释放
    /// </summary>
    public void Dispose()
    {
        _fieldNameDic.Clear();
        _fieldNameDic = null;
        _fieldNames = null;
        _gameData = null;
    }

    

}
