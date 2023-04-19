
//===================================================
//fileName：UI_General
//备    注：此代码为工具生成 请勿手工修改
//===================================================
using System.Collections;

/// <summary>
/// UI_General实体
/// </summary>
public partial class UI_GeneralRawEntity : AbstractRawEntity
{
    /// <summary>
    /// UI类型
    /// </summary>
    public string ViewType { get; set; }

    /// <summary>
    /// UI全路径（带扩展名）
    /// </summary>
    public string FullPath { get; set; }

    /// <summary>
    /// 预制体上是否带有Presenter层
    /// </summary>
    public int IsWithPresenter { get; set; }

    /// <summary>
    /// 是否在UIManager中管理起来
    /// </summary>
    public int IsRefInManager { get; set; }

    /// <summary>
    /// 关闭时是否回收
    /// </summary>
    public int Recyclable { get; set; }

    /// <summary>
    /// 是否预加载
    /// </summary>
    public int Preload { get; set; }

}
