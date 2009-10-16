using Centro.DomainModel;
using NHibernate;

namespace Centro.Data.DomainModel
{
    public abstract class BaseRepository : IRepository
    {
        protected readonly ISession Session;

        protected BaseRepository(ISession session)
        {
            Session = session;
        }
    }
}
