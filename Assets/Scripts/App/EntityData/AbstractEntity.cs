
namespace Company.NewApp.Entities
{
    public class AbstractEntity
    {
        public int ID = -1;

        public T Clone<T>() where T : AbstractEntity
        {
            return (T)MemberwiseClone();
        }
    }
}