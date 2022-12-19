
//===================================================
//fileName：Example_General
//备    注：此代码为工具生成 请勿手工修改
//===================================================

using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// Example_General实体数据管理
/// </summary>
public partial class Example_GeneralRawEntityModel : AbstractRawEntityModel<Example_GeneralRawEntityModel, Example_GeneralRawEntity>
{
    /// <summary>
    /// 文件名称
    /// </summary>
    protected override string FileName { get { return "Example_General.data"; } }

    /// <summary>
    /// 创建实体
    /// </summary>
    /// <param name="parse"></param>
    /// <returns></returns>
    protected override Example_GeneralRawEntity MakeEntity(GameEntityParser parse)
    {
        Example_GeneralRawEntity entity = new Example_GeneralRawEntity();
        entity.Id = parse.GetFieldValue("Id").ToInt();
        entity.StrValue = parse.GetFieldValue("StrValue");
        entity.IntValue = parse.GetFieldValue("IntValue").ToInt();
        entity.FloatValue = parse.GetFieldValue("FloatValue").ToFloat();
        entity.LongValue = parse.GetFieldValue("LongValue").ToLong();
        return entity;
    }
}
