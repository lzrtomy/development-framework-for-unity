
//===================================================
//fileName：Level_General
//备    注：此代码为工具生成 请勿手工修改
//===================================================

using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// Level_General实体数据管理
/// </summary>
public partial class Level_GeneralRawEntityModel : AbstractRawEntityModel<Level_GeneralRawEntityModel, Level_GeneralRawEntity>
{
    /// <summary>
    /// 文件名称
    /// </summary>
    protected override string FileName { get { return "Level_General.data"; } }

    /// <summary>
    /// 创建实体
    /// </summary>
    /// <param name="parse"></param>
    /// <returns></returns>
    protected override Level_GeneralRawEntity MakeEntity(GameEntityParser parse)
    {
        Level_GeneralRawEntity entity = new Level_GeneralRawEntity();
        entity.Id = parse.GetFieldValue("Id").ToInt();
        return entity;
    }
}
