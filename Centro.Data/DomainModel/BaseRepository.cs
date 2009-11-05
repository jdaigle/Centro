using NHibernate;

namespace Centro.Data.DomainModel
{
    public abstract class BaseRepository
    {
        protected readonly ISession Session;

        protected BaseRepository(ISession session)
        {
            Session = session;
        }
    }
}
