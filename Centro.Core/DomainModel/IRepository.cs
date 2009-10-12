using System.Linq;

namespace Centro.DomainModel
{
    public interface IRepository<TEntityType>
    {
        TEntityType Save(TEntityType entity);
        void Delete(TEntityType entity);
        IQueryable<TEntityType> Linq();
    }
}
