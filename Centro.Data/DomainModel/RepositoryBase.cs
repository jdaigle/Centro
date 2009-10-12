using System.Linq;
using Centro.DomainModel;
using NHibernate;
using NHibernate.Linq;

namespace Centro.Data.DomainModel
{
    public abstract class RepositoryBase<TEntityType> : IRepository<TEntityType>
    {
        protected readonly ISession Session;

        protected RepositoryBase(ISession session)
        {
            Session = session;
        }

        public TEntityType Save(TEntityType entity)
        {
            return (TEntityType)Session.SaveOrUpdateCopy(entity);
        }

        public void Delete(TEntityType entity)
        {
            Session.Delete(entity);
        }

        public IQueryable<TEntityType> Linq()
        {
            return Session.Linq<TEntityType>();
        }
    }
}
