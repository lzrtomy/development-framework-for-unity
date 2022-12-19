using System;

namespace Company.NewApp
{
    /// <summary>
    /// 对象初始化状态
    /// </summary>
    public enum InitState
    {
        NotInit,
        InInit,
        Inited
    }

    /// <summary>
    /// 音效枚举
    /// </summary>
    public enum AudioEvent
    {
        None,
        Demo_BGM,
        Demo_Eff_1,
        Demo_Eff_2
    }

    /// <summary>
    /// 按键输入类型枚举
    /// </summary>
    public enum InputKeyType
    {
        //无
        None,
        //任意按键
        AnyKey,
        //指定按键
        Specify,
        //数字按键
        AlphaNum,
        //字母按键
        Alphabet
    }
}