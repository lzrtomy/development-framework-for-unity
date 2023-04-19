
//===================================================
//fileName：UI_General
//备    注：此代码为工具生成 请勿手工修改
//===================================================

using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// UI_General实体数据管理
/// </summary>
public partial class UI_GeneralRawEntityModel : AbstractRawEntityModel<UI_GeneralRawEntityModel, UI_GeneralRawEntity>
{
    /// <summary>
    /// 文件名称
    /// </summary>
    protected override string FileName { get { return "UI_General.data"; } }

    /// <summary>
    /// 创建实体
    /// </summary>
    /// <param name="parse"></param>
    /// <returns></returns>
    protected override UI_GeneralRawEntity MakeEntity(GameEntityParser parse)
    {
        UI_GeneralRawEntity entity = new UI_GeneralRawEntity();
        entity.Id = parse.GetFieldValue("Id").ToInt();
        entity.ViewType = parse.GetFieldValue("ViewType");
        entity.FullPath = parse.GetFieldValue("FullPath");
        entity.IsWithPresenter = parse.GetFieldValue("IsWithPresenter").ToInt();
        entity.IsRefInManager = parse.GetFieldValue("IsRefInManager").ToInt();
        entity.Recyclable = parse.GetFieldValue("Recyclable").ToInt();
        entity.Preload = parse.GetFieldValue("Preload").ToInt();
        return entity;
    }
}
