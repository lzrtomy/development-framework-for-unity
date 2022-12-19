
namespace Company.NewApp.Entities
{
    public class ViewInfoEntity : AbstractEntity
    {
        //ID，需要与PageType中的序号保持一致
        public int Id = 0;

        //UI名称
        public string Name = "";

        //路径
        public string Path = "";

        //是否可以回收，是则使用对象池加载和回收，否则直接加载和销毁
        public bool Recyclable = false;
    }
}