
namespace Company.NewApp.Models
{
    public abstract class DataModelBase
    {
        public DataModelManager DataModelManager;

        public virtual void Init(params object[] parameters) { }
    }
}